using Microsoft.VisualStudio.TestTools.UnitTesting;
using Propeus.Modulo.Util.Thread;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Propeus.Modulo.Util.Teste.Thread
{
    [TestClass]
    public class LimitedConcurrencyLevelTaskSchedulerTeste
    {
        [TestMethod]
        public void ObterTipoParametrosTesteSucesso()
        {
            // Create a scheduler that uses two threads.
            LimitedConcurrencyLevelTaskScheduler lcts = new LimitedConcurrencyLevelTaskScheduler(2);
            List<Task> tasks = new List<Task>();

            // Create a TaskFactory and pass it our custom scheduler.
            TaskFactory factory = new TaskFactory(lcts);
            CancellationTokenSource cts = new CancellationTokenSource();

            // Use our factory to run a set of tasks.
            object lockObj = new object();
            int outputItem = 0;

            for (int tCtr = 0; tCtr <= 4; tCtr++)
            {
                int iteration = tCtr;
                Task t = factory.StartNew(() =>
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        lock (lockObj)
                        {
                            Console.Write("{0} in task t-{1} on thread {2}   ",
                                          i, iteration, System.Threading.Thread.CurrentThread.ManagedThreadId);
                            outputItem++;
                            if (outputItem % 3 == 0)
                            {
                                Console.WriteLine();
                            }
                        }
                    }
                }, cts.Token);
                tasks.Add(t);
            }
            // Use it to run a second set of tasks.
            for (int tCtr = 0; tCtr <= 4; tCtr++)
            {
                int iteration = tCtr;
                Task t1 = factory.StartNew(() =>
                {
                    for (int outer = 0; outer <= 10; outer++)
                    {
                        for (int i = 0x21; i <= 0x7E; i++)
                        {
                            lock (lockObj)
                            {
                                Console.Write("'{0}' in task t1-{1} on thread {2}   ",
                                              Convert.ToChar(i), iteration, System.Threading.Thread.CurrentThread.ManagedThreadId);
                                outputItem++;
                                if (outputItem % 3 == 0)
                                {
                                    Console.WriteLine();
                                }
                            }
                        }
                    }
                }, cts.Token);
                tasks.Add(t1);
            }

            // Wait for the tasks to complete before displaying a completion message.
            Task.WaitAll(tasks.ToArray());
            cts.Dispose();
            Console.WriteLine("\n\nSuccessful completion.");
        }
    }
}