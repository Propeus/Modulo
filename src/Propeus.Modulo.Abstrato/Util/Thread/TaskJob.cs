using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Propeus.Modulo.Abstrato.Util.Thread
{
    public class TaskJob : IDisposable
    {
        private const string NOME_JOB_COLETOR = "_coletor_taks_completados";
        private LimitedConcurrencyLevelTaskScheduler _scheduler;
        private TaskFactory _taskFactory;
        private CancellationTokenSource _cancellationTokenSource;
        private ConcurrentDictionary<string, Task> _tasks;
        private ConcurrentDictionary<string, CancellationTokenSource> _tasksTokenSource;

            
        public int EmExecucao => _tasks.Where(x => x.Key != NOME_JOB_COLETOR).Count(x => x.Value.Status == TaskStatus.Running);
        public int Completado => _tasks.Where(x => x.Key != NOME_JOB_COLETOR).Count(x => x.Value.Status == TaskStatus.RanToCompletion);
        public int Aguardando => _tasks.Where(x => x.Key != NOME_JOB_COLETOR).Count(x => x.Value.Status == TaskStatus.WaitingToRun);

        public TaskJob(int threads = 2)
        {

            _scheduler = new LimitedConcurrencyLevelTaskScheduler(threads);
            _taskFactory = new TaskFactory(_scheduler);
            _cancellationTokenSource = new CancellationTokenSource();
            _tasks = new ConcurrentDictionary<string, Task>();
            _tasksTokenSource = new ConcurrentDictionary<string, CancellationTokenSource>();

            AddJob((state) =>
            {
                foreach (var item in _tasks)
                {
                    if (item.Value.IsCompleted || item.Value.IsCanceled || item.Value.IsCompletedSuccessfully || item.Value.IsFaulted)
                    {
                        _tasks.TryRemove(item);
                        _tasksTokenSource.TryRemove(item.Key, out _);
                    }
                }

            }, TimeSpan.FromSeconds(3), NOME_JOB_COLETOR);
        }

        public Task WaitAll()
        {
            var arrTasks = _tasks.Where(x => x.Key != NOME_JOB_COLETOR).Select(x => x.Value).ToArray();
            return Task.WhenAll(arrTasks);
        }

        public bool IsCompleted()
        {
            return _tasks.Where(x => x.Key != NOME_JOB_COLETOR).All(x => x.Value.Status == TaskStatus.RanToCompletion);
        }

        public Task AddJob(Action<object?> action, TimeSpan period, string nomeJob = null)
        {
            if (string.IsNullOrEmpty(nomeJob))
            {
                nomeJob = Guid.NewGuid().ToString();
            }

            var cts = new CancellationTokenSource();
            var task = _taskFactory.StartNew((innerState) =>
            {
                CancellationTokenSource cancellationTokenSource = (CancellationTokenSource)innerState;


                while (!cancellationTokenSource.IsCancellationRequested)
                {
                    action.Invoke(innerState);
                    try
                    {
                        Task.Delay(period, cancellationTokenSource.Token).Wait(cancellationTokenSource.Token);
                    }
                    catch 
                    {
                        //Continua o jogo e foda-se
                    }

                }


            }, cts, _cancellationTokenSource.Token);
            _tasks.TryAdd(nomeJob, task);
            _tasksTokenSource.TryAdd(nomeJob, cts);
            return task;
        }

        private bool disposedValue;

        public string ToStringRunning()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Jobs em espera: ").AppendLine(Aguardando.ToString());
            sb.Append("Jobs em execucao: ").AppendLine(EmExecucao.ToString());
            sb.Append("Jobs finalizados: ").AppendLine(Completado.ToString());
            sb.AppendLine("=== JOBS === ");
            foreach (var item in _tasks.Where(x => x.Value.Status == TaskStatus.Running))
            {
                sb.Append("Job: ").Append(item.Key).Append(" - ").AppendLine(item.Value.Status.ToString());
            }

            return sb.ToString();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var item in _tasks)
            {
                sb.Append("Job: ").Append(item.Key).Append(" - ").AppendLine(item.Value.Status.ToString());
            }

            return sb.ToString();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _cancellationTokenSource.Dispose();
                    _tasks.Clear();

                    foreach (var task in _tasksTokenSource)
                    {
                        task.Value.Dispose();
                    }
                    _tasksTokenSource.Clear();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~CronTask()
        // {
        //     // Não altere este código. Coloque o código de limpeza no método 'Dispose(bool disposing)'
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Não altere este código. Coloque o código de limpeza no método 'Dispose(bool disposing)'
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
