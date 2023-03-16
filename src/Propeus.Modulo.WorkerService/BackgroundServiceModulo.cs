using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;

using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Abstrato.Interfaces;

namespace Propeus.Modulo.WorkerService
{
    /// <summary>
    /// Modulo para inicializar um <see cref="BackgroundService"/>
    /// </summary>
    [Modulo]
    public abstract class BackgroundServiceModulo : BackgroundService, IModulo
    {
        private readonly CancellationTokenSource cancellationTokenSource;
        private readonly CancellationTokenSource cancellationTokenSourceCancelKeyPress;
        private readonly Task _worker;

        /// <summary>
        /// Inicializa um modulo
        /// </summary>
        /// <param name="gerenciador">Gerenciador que irá controlar o modulo</param>
        /// <param name="instanciaUnica">Informa se a instancia é unica ou multipla</param>
        protected BackgroundServiceModulo(IGerenciador gerenciador, bool instanciaUnica = false)
        {
            Nome = GetType().Name;
            Estado = Estado.Inicializado;
            Id = Guid.NewGuid().ToString();

            Gerenciador = gerenciador ?? throw new ArgumentNullException(nameof(gerenciador));
            InstanciaUnica = instanciaUnica;
            if (gerenciador is IGerenciadorRegistro)
            {
                (gerenciador as IGerenciadorRegistro).Registrar(this);
            }
            else
            {
                throw new ArgumentException(string.Format(Constantes.GERENCIADOR_INVALIDO, nameof(gerenciador)), nameof(gerenciador));
            }

            cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSourceCancelKeyPress = new CancellationTokenSource();

            System.Console.CancelKeyPress += Console_CancelKeyPress;

            _worker = StartAsync(cancellationTokenSource.Token);
            if (_worker.IsFaulted)
            {
                Estado = Estado.Erro;
                Erros = _worker.Exception;
                _worker = null;
            }

        }

        private void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            _ = StopAsync(cancellationTokenSourceCancelKeyPress.Token);
        }

        /// <summary>
        /// Gerenciador que está manipulando o modulo
        /// </summary>
        public IGerenciador Gerenciador { get; }
        ///<inheritdoc/>
        public bool InstanciaUnica { get; }
        ///<inheritdoc/>
        public virtual string Versao
        {
            get
            {
                Version ver = GetType().Assembly.GetName().Version;
                return $"{ver.Major}.{ver.Minor}.{ver.Build}";
            }
        }
        ///<inheritdoc/>
        public Estado Estado { get; private set; }
        /// <summary>
        /// Caso o estado seja <see cref="Estado.Erro"/>, a excecao será adicionado aqui
        /// </summary>
        public AggregateException Erros { get; }

        ///<inheritdoc/>
        public string Nome { get; }
        ///<inheritdoc/>
        public string Id { get; }

        /// <summary>
        /// Exibe informações basicas sobre o modelo
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new();

            _ = sb.Append("Nome: ").Append(Nome).AppendLine();
            _ = sb.Append("Estado: ").Append(Estado).AppendLine();
            _ = sb.Append("Id: ").Append(Id).AppendLine();
            _ = sb.Append("Versao: ").Append(Versao).AppendLine();
            _ = sb.AppendLine($"Instancia Unica: {InstanciaUnica}");


            return sb.ToString();
        }

        #region IDisposable Support
        private bool disposedValue = false; // Para detectar chamadas redundantes

        /// <summary>
        /// Libera os objetos deste modelo e altera o estado dele para <see cref="Estado.Desligado"/>
        /// </summary>
        /// <param name="disposing">Indica se deve alterar o estado do objeto para <see cref="Estado.Desligado"/></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Estado = Estado.Desligado;
                    _worker?.Wait(cancellationTokenSourceCancelKeyPress.Token);
                    cancellationTokenSource.Cancel();
                }

                disposedValue = true;
            }
        }

        ///<inheritdoc/>
        public override void Dispose()
        {
            // Não altere este código. Coloque o código de limpeza em DisposeMethod(bool disposing) acima.
            Dispose(true);
        }
        #endregion
    }
}