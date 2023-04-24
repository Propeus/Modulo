using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;

using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Abstrato.Modulos;
using Propeus.Modulo.Abstrato.Proveders;

namespace Propeus.Modulo.WorkerService
{
    /// <summary>
    /// Modulo para inicializar um <see cref="BackgroundService"/>
    /// </summary>
    [Modulo]
    public abstract class BackgroundServiceModulo : ModuloBase, IHostedService
    {

        ///<inheritdoc/>
        protected BackgroundServiceModulo(bool instanciaUnica = false) : base(instanciaUnica)
        {

        }


        private Task _executeTask;
        private CancellationTokenSource _stoppingCts;

        /// <summary>
        /// Inicializa o servico
        /// </summary>
        public void CriarInstancia()
        {
            StartAsync(new CancellationTokenSource().Token);
        }

        /// <summary>
        /// Gets the Task that executes the background operation.
        /// </summary>
        /// <remarks>
        /// Will return null if the background operation hasn't started.
        /// </remarks>
        public virtual Task ExecuteTask => _executeTask;


        /// <summary>
        /// This method is called when the <see cref="Microsoft.Extensions.Hosting.IHostedService"/> starts.
        /// The implementation should return a task that represents the lifetime of the long running operation(s) being performed.
        /// </summary>
        /// <remarks>
        /// See Worker Services in .NET for implementation guidelines.
        /// </remarks>
        /// <param name="stoppingToken">Triggered when <see cref="Microsoft.Extensions.Hosting.IHostedService.StopAsync(System.Threading.CancellationToken)"/> </param>
        /// <returns>A System.Threading.Tasks.Task that represents the long running operations.</returns>
        protected abstract Task ExecuteAsync(CancellationToken stoppingToken);

        /// <summary>
        /// Triggered when the application host is ready to start the service.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns></returns>
        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            _stoppingCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _executeTask = ExecuteAsync(_stoppingCts.Token);
            if (_executeTask.IsCompleted)
            {
                return _executeTask;
            }

            Estado = Estado.Inicializado;
            return Task.CompletedTask;
        }

        /// <summary>
        ///  Triggered when the application host is performing a graceful shutdown.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
        /// <returns></returns>
        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_executeTask != null)
            {
                try
                {
                    _stoppingCts.Cancel();
                }
                finally
                {
                    await Task.WhenAny(_executeTask, Task.Delay(-1, cancellationToken)).ConfigureAwait(continueOnCapturedContext: false);
                }
            }

            Estado = Estado.Desligado;
        }

        ///<inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            _stoppingCts?.Cancel();
            base.Dispose(disposing);
        }


    }
}