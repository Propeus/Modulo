using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Propeus.Modulo.Util.Thread
{
    /// <summary>
    /// Gerenciador de <see cref="Task"/>
    /// </summary>
    public class TaskJob : IDisposable
    {
        private const string NOME_JOB_COLETOR = "__COLETOR_TAKS_COMPLETADOS";
        private readonly LimitedConcurrencyLevelTaskScheduler _scheduler;
        private readonly TaskFactory _taskFactory;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly ConcurrentDictionary<string, Task> _tasks;
        private readonly ConcurrentDictionary<string, CancellationTokenSource> _tasksTokenSource;


        /// <summary>
        /// Indica quantas taks estao sendo executados no momento
        /// </summary>
        public int EmExecucao => _tasks.Where(x => x.Key != NOME_JOB_COLETOR).Count(x => x.Value.Status == TaskStatus.Running);
        /// <summary>
        /// Indica quantas tasks ja foram executados
        /// </summary>
        /// <remarks>
        /// Caso a task <see cref="NOME_JOB_COLETOR"/> esteja em execucao, o numero de tasks completados sera igual a propriedade <see cref="EmExecucao"/>, 
        /// pois esta task remove as tasks que ja foram executadas
        /// </remarks>
        public int Completado => _tasks.Where(x => x.Key != NOME_JOB_COLETOR).Count(x => x.Value.Status == TaskStatus.RanToCompletion);
        /// <summary>
        /// Numero de tasks que aguardam a execucao
        /// </summary>
        public int Aguardando => _tasks.Where(x => x.Key != NOME_JOB_COLETOR).Count(x => x.Value.Status == TaskStatus.WaitingToRun);

        /// <summary>
        /// Inicializa o gerenciador com uma quantidade de taks que podem ser executados ao mesmo tempo
        /// </summary>
        /// <param name="threads">Numereo de taks que podem ser executado ao mesmo tempo. Por padrao este valor e 2</param>
        public TaskJob(int threads = 2)
        {

            _scheduler = new LimitedConcurrencyLevelTaskScheduler(threads);
            _taskFactory = new TaskFactory(_scheduler);
            _cancellationTokenSource = new CancellationTokenSource();
            _tasks = new ConcurrentDictionary<string, Task>();
            _tasksTokenSource = new ConcurrentDictionary<string, CancellationTokenSource>();

            _ = AddTask((state) =>
            {
                foreach (KeyValuePair<string, Task> item in _tasks)
                {
                    if (item.Value.IsCompleted || item.Value.IsCanceled || item.Value.IsCompletedSuccessfully || item.Value.IsFaulted)
                    {
                        _ = _tasks.TryRemove(item);
                        _ = _tasksTokenSource.TryRemove(item.Key, out _);
                    }
                }

            }, TimeSpan.FromSeconds(3), NOME_JOB_COLETOR);
        }

        /// <summary>
        /// Aguarda todas as tasks serem executadas
        /// </summary>
        /// <remarks>
        /// Esta espera ignora a task de limpeza '<see cref="NOME_JOB_COLETOR"/>'
        /// </remarks>
        /// <returns>Retorna uma <see cref="Task"/> que sera concluida quando todas as tarafas forem finalizadas</returns>
        public Task WaitAll()
        {
            Task[] arrTasks = _tasks.Where(x => x.Key != NOME_JOB_COLETOR).Select(x => x.Value).ToArray();
            return Task.WhenAll(arrTasks);
        }

        /// <summary>
        /// Indica se todas as tasks foram concluidas
        /// </summary>
        /// <remarks>
        /// Esta funcao ignora a task de limpeza '<see cref="NOME_JOB_COLETOR"/>'
        /// </remarks>
        /// <returns>Retorna <see langword="true"/> caso todas as tasks tenham sido concluidas, caso contrario retorna <see langword="false"/></returns>
        public bool IsCompleted()
        {
            return _tasks.Where(x => x.Key != NOME_JOB_COLETOR).All(x => x.Value.Status == TaskStatus.RanToCompletion);
        }

        /// <summary>
        /// Adiciona uma nova task ao gerenciador
        /// </summary>
        /// <param name="action">Procedimento que ira ser execurado</param>
        /// <param name="period">Periodo de intervalo entre as execucoes</param>
        /// <param name="nomeJob">Nome da task</param>
        /// <returns>Retorna a task</returns>
        public Task AddTask(Action<object> action, TimeSpan? period = null, string nomeJob = null)
        {
            if (string.IsNullOrEmpty(nomeJob))
            {
                nomeJob = Guid.NewGuid().ToString();
            }

            CancellationTokenSource cts = new();
            if (period.HasValue)
            {
                Task task = _taskFactory.StartNew((innerState) =>
                {
                    CancellationTokenSource cancellationTokenSource = (CancellationTokenSource)innerState;


                    while (!cancellationTokenSource.IsCancellationRequested)
                    {
                        action.Invoke(innerState);
                        try
                        {
                            Task.Delay(period.Value, cancellationTokenSource.Token).Wait(cancellationTokenSource.Token);
                        }
                        catch
                        {
                            //Continua o jogo e foda-se
                        }

                    }


                }, cts, _cancellationTokenSource.Token);
                _ = _tasks.TryAdd(nomeJob, task);
                _ = _tasksTokenSource.TryAdd(nomeJob, cts);
                return task;
            }
            else
            {
                Task task = _taskFactory.StartNew(action.Invoke, cts, _cancellationTokenSource.Token);
                _ = _tasks.TryAdd(nomeJob, task);
                _ = _tasksTokenSource.TryAdd(nomeJob, cts);
                return task;
            }

        }

        private bool disposedValue;

        /// <summary>
        /// Retorna o resumo das tasks em execucao, completados e em espera, alem da lista de tasks em execucao
        /// </summary>
        /// <returns></returns>
        public string ToStringRunning()
        {
            StringBuilder sb = new();

            _ = sb.Append("Jobs em espera: ").AppendLine(Aguardando.ToString());
            _ = sb.Append("Jobs em execucao: ").AppendLine(EmExecucao.ToString());
            _ = sb.Append("Jobs finalizados: ").AppendLine(Completado.ToString());
            _ = sb.AppendLine("=== JOBS === ");
            foreach (KeyValuePair<string, Task> item in _tasks.Where(x => x.Value.Status == TaskStatus.Running))
            {
                _ = sb.Append("Job: ").Append(item.Key).Append(" - ").AppendLine(item.Value.Status.ToString());
            }

            return sb.ToString();
        }

        /// <summary>
        /// Retorna a lista de tasks e seu estado atual
        /// </summary>
        /// <returns>Retorna a lista de tasks e seu estado atual</returns>
        public override string ToString()
        {
            StringBuilder sb = new();

            foreach (KeyValuePair<string, Task> item in _tasks)
            {
                _ = sb.Append("Job: ").Append(item.Key).Append(" - ").AppendLine(item.Value.Status.ToString());
            }

            return sb.ToString();
        }

        /// <summary>
        /// Cancela todas as tasks e limpa a lista
        /// </summary>
        /// <param name="disposing">Indica se deve realizar o dispose nos objetos gerenciados</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _cancellationTokenSource.Dispose();
                    _tasks.Clear();

                    foreach (KeyValuePair<string, CancellationTokenSource> task in _tasksTokenSource)
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

        /// <summary>
        /// Cancela todas as tasks e limpa a lista
        /// </summary>
        public void Dispose()
        {
            // Não altere este código. Coloque o código de limpeza no método 'Dispose(bool disposing)'
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
