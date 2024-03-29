﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Propeus.Modulo.Util.Objetos;

using static System.Collections.Specialized.BitVector32;

namespace Propeus.Modulo.Util.Thread
{
    public class TaskJob : IDisposable
    {

        static TaskJob _task;
        public static TaskJob GetTasker(int workers = 2)
        {
            if (_task == null || _task.disposedValue)
                _task = new TaskJob(workers);
            return _task;
        }

        enum EstadoRunner
        {
            Criado,
            Executando,
            Aguardando,
            EmFila,
            Finalizado
        }

        class Runner : IDisposable
        {

            public Runner()
            {
                CancelationToken = new CancellationTokenSource();
            }

            public Action<object> Action;
            public string Name;
            public TimeSpan? Period;
            public CancellationTokenSource CancelationToken { get; internal set; }

            public Task Task { get; internal set; }
            public EstadoRunner Estado { get; internal set; }

            public void Run(CancellationToken cancellationToken, TaskFactory _taskFactory)
            {
                Task = _taskFactory.StartNew(Action, cancellationToken, CancelationToken.Token);
            }

            private bool disposedValue;

            protected virtual void Dispose(bool disposing)
            {
                if (!disposedValue)
                {
                    if (disposing)
                    {
                        CancelationToken.Cancel();
                        CancelationToken.Dispose();
                        CancelationToken = null;
                    }


                    disposedValue = true;
                }
            }


            public void Dispose()
            {
                // Não altere este código. Coloque o código de limpeza no método 'Dispose(bool disposing)'
                Dispose(disposing: true);
                GC.SuppressFinalize(this);
            }
        }

        ConcurrentDictionary<string, Runner> _runners;
        CancellationTokenSource _cancellationTokenSource;
        TaskFactory _taskFactory;

        private TaskJob(int threads = 2)
        {
            _runners = new ConcurrentDictionary<string, Runner>();
            _cancellationTokenSource = new CancellationTokenSource();
            _taskFactory = new TaskFactory(new LimitedConcurrencyLevelTaskScheduler(threads));
        }

        public void RegisterJob(Action<object> action, string nomeJob = null, TimeSpan? period = null)
        {
            nomeJob ??= Guid.NewGuid().ToString();
            Runner runner = new Runner()
            {
                Name = nomeJob ?? Guid.NewGuid().ToString(),
                Period = period,
                Estado = EstadoRunner.Criado
            };
            if (period is null)
            {
                runner.Action = (cancelationToken) =>
                {
                    _runners.TryRemove(nomeJob, out _);
                    runner.Estado = EstadoRunner.Executando;
                    action.Invoke(cancelationToken);
                };
            }
            else
            {
                runner.Action = (ct) =>
                {
                    CancellationToken cancelationToken = (CancellationToken)ct;

                    if (!cancelationToken.IsCancellationRequested)
                    {
                        runner.Estado = EstadoRunner.Executando;
                        action.Invoke(cancelationToken);
                        runner.Estado = EstadoRunner.Aguardando;
                        try
                        {
                            Task.Delay(period.Value).Wait();
                            runner.Estado = EstadoRunner.EmFila;
                            runner.Task = _taskFactory.StartNew(runner.Action, cancelationToken, runner.CancelationToken.Token);
                        }
                        catch (TaskCanceledException)
                        {
                            runner.Estado = EstadoRunner.Finalizado;
                        }
                        
                    }
                    else
                    {
                        runner.Estado = EstadoRunner.Finalizado;
                    }

                };
            }

            if (_runners.TryAdd(nomeJob, runner))
            {
                runner.Run(_cancellationTokenSource.Token, _taskFactory);
            }
            else
            {
                runner.Dispose();
            }
        }
        public void UnregisterJob(string nomeJob)
        {
            if (_runners.TryRemove(nomeJob, out Runner runner))
            {
                runner.Dispose();
            }
        }
        public void WaitAll(TimeSpan? timeSpan = null)
        {
            try
            {
                if (timeSpan == null)
                    Task.WaitAll(_runners.Select(x => x.Value.Task).ToArray());
                else
                    Task.WaitAll(_runners.Select(x => x.Value.Task).ToArray(), timeSpan.Value);
            }
            catch (TaskCanceledException)
            {

            }
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in _runners)
            {
                stringBuilder.Append(item.Key).Append(": ").AppendLine(item.Value.Estado.ToString().ToUpper());
            }
            return stringBuilder.ToString();
        }

        public string ToStringView()
        {
            Dictionary<string, StringBuilder> grupos = new Dictionary<string, StringBuilder>();
            grupos.Add("Global", new StringBuilder());
            grupos.First().Value.AppendLine("Global");

            foreach (var item in _runners)
            {
                var nme_group = item.Key.Split("::");
                if (nme_group.Length > 1)
                {
                    if (grupos.ContainsKey(nme_group[0]))
                    {
                        grupos[nme_group[0]].Append('\t').Append("- ").Append(nme_group[1]).Append(": ").AppendLine(item.Value.Estado.ToString().ToUpper());
                    }
                    else
                    {
                        grupos.Add(nme_group[0], new StringBuilder());
                        grupos[nme_group[0]].AppendLine(nme_group[0]);
                        grupos[nme_group[0]].Append('\t').Append("- ").Append(nme_group[1]).Append(": ").AppendLine(item.Value.Estado.ToString().ToUpper());
                    }
                }
                else
                {
                    grupos.First().Value.Append('\t').Append("- ").Append(nme_group[0]).Append(": ").AppendLine(item.Value.Estado.ToString().ToUpper());
                }
            }

            StringBuilder sb = new StringBuilder();
            foreach (var item in grupos)
            {
                sb.Append(item.Value.ToString());
            }
            return sb.ToString();
        }

        #region IDisposable
        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _cancellationTokenSource.Cancel();
                    WaitAll();
                    _runners.Clear();
                }


                disposedValue = true;
            }
        }



        public void Dispose()
        {
            // Não altere este código. Coloque o código de limpeza no método 'Dispose(bool disposing)'
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }


}
