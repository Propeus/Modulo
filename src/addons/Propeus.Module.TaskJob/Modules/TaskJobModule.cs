using System.Collections.Concurrent;
using System.Text;

using Propeus.Module.Abstract;
using Propeus.Module.Abstract.Attributes;
using Propeus.Module.Taskjob.Contracts;
using Propeus.Module.Utils.Thread;

using static Propeus.Module.Taskjob.Modules.TaskJobModule;



namespace Propeus.Module.Taskjob.Modules
{



    /// <summary>
    /// Modulo para gerenciar tarefas
    /// </summary>
    [Module(Description = "Modulo para gerenciamento de tarefas em paralelo", AutoUpdate = false, AutoStartable = false, Singleton = true, KeepAlive = true)]
    public class TaskJobModule : BaseModule, ITaskJobContract
    {
        private class Runner : IDisposable
        {

            public Runner()
            {
                CancellationToken = new CancellationTokenSource();
            }

            public Action<object?> Action;
            public BagAction? Bag;
            public string Name;
            public TimeSpan? Period;
            public CancellationTokenSource CancellationToken { get; internal set; }

            public Task Task { get; internal set; }
            public EstadoRunner Estado { get; internal set; }

            public void Run(TaskFactory _taskFactory, CancellationToken cancellationToken)
            {
                if (Bag == null)
                {
                    Bag = new BagAction();
                    Bag.Add("CancelationToken", CancellationToken.Token);
                }
                Task = _taskFactory.StartNew(Action, CancellationToken.Token, cancellationToken);
            }

            private bool disposedValue;

            protected virtual void Dispose(bool disposing)
            {
                if (!disposedValue)
                {
                    if (disposing)
                    {
                        CancellationToken.Cancel();
                        CancellationToken.Dispose();
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
      
        private enum EstadoRunner
        {
            Criado,
            Executando,
            Aguardando,
            EmFila,
            Finalizado
        }


        /// <summary>
        /// Classe para adicionar itens que devem ser enviados para a action
        /// </summary>
        public class BagAction
        {
            Dictionary<string, object> Bag;

            /// <summary>
            /// Classe para adicionar itens que devem ser enviados para a action
            /// </summary>
            public BagAction()
            {
                Bag = new Dictionary<string, object>();
            }

            /// <summary>
            /// Adiciona um novo item a bolsa
            /// </summary>
            /// <param name="name">Chave do item</param>
            /// <param name="value">Valor do item</param>
            public void Add(string name, object value)
            {
                Bag.Add(name, value);
            }

            /// <summary>
            /// Obtem um item da bolsa
            /// </summary>
            /// <param name="name">Chave do item</param>
            /// <returns>Valor do item</returns>
            public object GetBag(string name)
            {
                return Bag[name];
            }
        }

        private readonly ConcurrentDictionary<string, Runner> _runners;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly TaskFactory _taskFactory;

        public TaskJobModule(int threads = 2) : base()
        {
            _runners = new ConcurrentDictionary<string, Runner>();
            _cancellationTokenSource = new CancellationTokenSource();
            _taskFactory = new TaskFactory(new LimitedConcurrencyLevelTaskScheduler(threads));
        }

        /// <summary>
        /// Registra um job recorrente
        /// </summary>
        /// <param name="action">Um metodo que tem como parametro o <see cref="BagAction"/></param>
        /// <param name="period">Intervalo de tempo entre as execuções do job</param>
        /// <param name="bag">Intens que podem ser necessarios para a action</param>
        /// <param name="nomeJob">Nome do job</param>
        public void RegisterRecurringJob(Action<BagAction> action, TimeSpan period, BagAction? bag = null, string? nomeJob = null)
        {
            Runner runner = new Runner()
            {
                Name = nomeJob ?? Guid.NewGuid().ToString(),
                Period = period,
                Estado = EstadoRunner.Criado,
                Bag = bag
            };
            runner.Action = (object? ct) =>
            {
                if (ct == null)
                {
                    throw new NotImplementedException();
                }
                CancellationToken cancelationToken = (CancellationToken)ct;

                if (!cancelationToken.IsCancellationRequested)
                {
                    runner.Estado = EstadoRunner.Executando;
                    action.Invoke(runner.Bag);
                    try
                    {
                        runner.Estado = EstadoRunner.Aguardando;
                        Task.Delay(period).Wait(cancelationToken);

                        runner.Estado = EstadoRunner.EmFila;
                        runner.Task = _taskFactory.StartNew(runner.Action, cancelationToken, runner.CancellationToken.Token);
                    }
                    catch (TaskCanceledException)
                    {
                        runner.Estado = EstadoRunner.Finalizado;
                    }
                    catch (OperationCanceledException)
                    {
                        runner.Estado = EstadoRunner.Finalizado;
                    }

                }
                else
                {
                    runner.Estado = EstadoRunner.Finalizado;
                }

            };
            if (_runners.TryAdd(runner.Name, runner))
            {
                runner.Run(_taskFactory, _cancellationTokenSource.Token);
            }
            else
            {
                runner.Dispose();
            }
        }

        /// <summary>
        /// Registra um job
        /// </summary>
        /// <param name="action">Um metodo que tem como parametro o <see cref="BagAction"/></param>
        /// <param name="bag">Intens que podem ser necessarios para a action</param>
        /// <param name="nomeJob">Nome do job</param>
        public void RegisterJob(Action<BagAction> action, BagAction? bag = null, string? nomeJob = null)
        {
            Runner runner = new Runner()
            {
                Name = nomeJob ?? Guid.NewGuid().ToString(),
                Estado = EstadoRunner.Criado,
                Bag = bag
            };

            runner.Action = (cancelationToken) =>
            {
                if (_runners.TryRemove(runner.Name, out _))
                {
                    runner.Estado = EstadoRunner.Executando;
                    action.Invoke(runner.Bag);
                }
            };


            if (_runners.TryAdd(runner.Name, runner))
            {
                runner.Run(_taskFactory, _cancellationTokenSource.Token);
            }
            else
            {
                runner.Dispose();
            }
        }
        /// <summary>
        /// Remove um job da lista de execução
        /// </summary>
        /// <param name="nomeJob">Nome do job</param>
        public void UnregisterJob(string nomeJob)
        {
            if (_runners.TryRemove(nomeJob, out Runner? runner))
            {
                runner.Dispose();
            }
        }
        /// <summary>
        /// Espera que todos os jobs tenham sidos executados
        /// </summary>
        /// <param name="timeout">Tempo de espera</param>
        public void WaitAll(TimeSpan? timeout = null)
        {
            try
            {
                if (timeout == null)
                {
                    Task.WaitAll(_runners.Select(x => x.Value.Task).ToArray());
                }
                else
                {
                    Task.WaitAll(_runners.Select(x => x.Value.Task).ToArray(), timeout.Value);
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
