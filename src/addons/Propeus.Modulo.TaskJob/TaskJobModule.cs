using System.Collections.Concurrent;
using System.Text;

using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Attributes;
using Propeus.Modulo.Util.Thread;

namespace Propeus.Modulo.Taskjob
{
    /// <summary>
    /// Modulo para gerenciar tarefas
    /// </summary>
    [Module]
    public class TaskJobModule : BaseModule
    {
        private enum EstadoRunner
        {
            Criado,
            Executando,
            Aguardando,
            EmFila,
            Finalizado
        }

        private class Runner : IDisposable
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

            public void Run(TaskFactory _taskFactory, CancellationToken cancellationToken)
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

        private readonly ConcurrentDictionary<string, Runner> _runners;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly TaskFactory _taskFactory;

        public TaskJobModule(int threads = 2) : base(true)
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
                runner.Run(_taskFactory, _cancellationTokenSource.Token);
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
                {
                    Task.WaitAll(_runners.Select(x => x.Value.Task).ToArray());
                }
                else
                {
                    Task.WaitAll(_runners.Select(x => x.Value.Task).ToArray(), timeSpan.Value);
                }
            }
            catch (TaskCanceledException)
            {
                //Ignora exceção
            }
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (KeyValuePair<string, Runner> item in _runners)
            {
                stringBuilder.Append(item.Key).Append(": ").AppendLine(item.Value.Estado.ToString().ToUpper());
            }
            return stringBuilder.ToString();
        }

        public string ToStringView()
        {
            Dictionary<string, StringBuilder> grupos = new Dictionary<string, StringBuilder>
            {
                { "Global", new StringBuilder() }
            };
            grupos.First().Value.AppendLine("Global");

            foreach (KeyValuePair<string, Runner> item in _runners)
            {
                string[] nme_group = item.Key.Split("::");
                if (nme_group.Length > 1)
                {
                    if (grupos.TryGetValue(nme_group[0], out var targetValue))
                    {
                        targetValue.Append('\t').Append("- ").Append(nme_group[1]).Append(": ").AppendLine(item.Value.Estado.ToString().ToUpper());
                    }
                    else
                    {
                        var stringBuilder = new StringBuilder();
                        stringBuilder.AppendLine(nme_group[0]);
                        stringBuilder.Append('\t').Append("- ").Append(nme_group[1]).Append(": ").AppendLine(item.Value.Estado.ToString().ToUpper());
                        grupos.Add(nme_group[0], stringBuilder);
                    }
                }
                else
                {
                    grupos.First().Value.Append('\t').Append("- ").Append(nme_group[0]).Append(": ").AppendLine(item.Value.Estado.ToString().ToUpper());
                }
            }

            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, StringBuilder> item in grupos)
            {
                sb.Append(item.Value);
            }
            return sb.ToString();
        }

        #region IDisposable

        protected override void Dispose(bool disposing)
        {
            if (!disposedValue && disposing)
            {
                _cancellationTokenSource.Cancel();
                _runners.Clear();
            }

            base.Dispose(disposing);
        }
        #endregion
    }
}
