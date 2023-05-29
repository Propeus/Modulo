using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Propeus.Modulo.Util.Thread
{
    /// <summary>
    /// https://docs.microsoft.com/pt-br/dotnet/api/system.threading._tasks.taskscheduler?view=netcore-3.1
    /// </summary>
    public class LimitedConcurrencyLevelTaskScheduler : TaskScheduler
    {
        // Indicates whether the current thread is processing work items.
        private bool _currentThreadIsProcessingItems;

        // The list of _tasks to be executed
        private readonly LinkedList<Task> _tasks = new(); // protected by lock(_tasks)

        // The maximum concurrency level allowed by this _scheduler.
        private readonly int _maxDegreeOfParallelism;

        // Indicates whether the _scheduler is currently processing work items.
        private int _delegatesQueuedOrRunning = 0;

        /// <summary>
        /// Creates a new instance with the specified degree of parallelism.
        /// </summary>
        /// <param name="maxDegreeOfParallelism"></param>
        public LimitedConcurrencyLevelTaskScheduler(int maxDegreeOfParallelism)
        {
            if (maxDegreeOfParallelism < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(maxDegreeOfParallelism));
            }

            _maxDegreeOfParallelism = maxDegreeOfParallelism;
        }

        /// <summary>
        /// Queues a task to the _scheduler.
        /// </summary>
        /// <param name="task"></param>
        protected sealed override void QueueTask(Task task)
        {
            // Add the task to the list of _tasks to be processed.  If there aren't enough
            // delegates currently queued or running to process _tasks, schedule another.
            lock (_tasks)
            {
                _ = _tasks.AddLast(task);
                if (_delegatesQueuedOrRunning < _maxDegreeOfParallelism)
                {
                    ++_delegatesQueuedOrRunning;
                    NotifyThreadPoolOfPendingWork();
                }
            }
        }

        // Inform the ThreadPool that there's work to be executed for this _scheduler.
        private void NotifyThreadPoolOfPendingWork()
        {
            _ = ThreadPool.UnsafeQueueUserWorkItem(_ =>
            {
                // Note that the current thread is now processing work items.
                // This is necessary to enable inlining of _tasks into this thread.
                _currentThreadIsProcessingItems = true;
                try
                {
                    // Process all available items in the queue.
                    while (true)
                    {
                        Task item;
                        lock (_tasks)
                        {
                            // When there are no more items to be processed,
                            // note that we're done processing, and get out.
                            if (_tasks.Count == 0)
                            {
                                --_delegatesQueuedOrRunning;
                                break;
                            }

                            // Get the next item from the queue
                            item = _tasks.First.Value;
                            _tasks.RemoveFirst();
                        }

                        // Execute the task we pulled out of the queue
                        _ = TryExecuteTask(item);
                    }
                }
                // We're done processing items on the current thread
                finally { _currentThreadIsProcessingItems = false; }
            }, null);
        }

        /// <summary>
        /// Attempts to execute the specified task on the current thread.
        /// </summary>
        /// <param name="task"></param>
        /// <param name="taskWasPreviouslyQueued"></param>
        /// <returns></returns>
        protected sealed override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            // If this thread isn't already processing a task, we don't support inlining
            if (!_currentThreadIsProcessingItems)
            {
                return false;
            }

            // If the task was previously queued, remove it from the queue
            if (taskWasPreviouslyQueued)
            {
                // Try to run the task.
                return TryDequeue(task) && TryExecuteTask(task);
            }
            else
            {
                return TryExecuteTask(task);
            }
        }

        /// <summary>
        /// Attempt to remove a previously scheduled task from the _scheduler.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        protected sealed override bool TryDequeue(Task task)
        {
            lock (_tasks)
            {
                return _tasks.Remove(task);
            }
        }

        /// <summary>
        /// Gets the maximum concurrency level supported by this _scheduler.
        /// </summary>
        public sealed override int MaximumConcurrencyLevel => _maxDegreeOfParallelism;

        /// <summary>
        /// Gets an enumerable of the _tasks currently scheduled on this _scheduler.
        /// </summary>
        /// <returns></returns>
        protected sealed override IEnumerable<Task> GetScheduledTasks()
        {
            bool lockTaken = false;
            try
            {
                Monitor.TryEnter(_tasks, ref lockTaken);
                return lockTaken ? (IEnumerable<Task>)_tasks : throw new NotSupportedException();
            }
            finally
            {
                if (lockTaken)
                {
                    Monitor.Exit(_tasks);
                }
            }
        }
    }
}