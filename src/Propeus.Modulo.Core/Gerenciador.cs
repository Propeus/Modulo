using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Abstrato.Exceptions;
using Propeus.Modulo.Abstrato.Helpers;
using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Abstrato.Proveders;
using Propeus.Modulo.Util;
using Propeus.Modulo.Util.Thread;


namespace Propeus.Modulo.Core
{
    /// <summary>
    /// Controlador de modulos
    /// </summary>
    public sealed class Gerenciador : GerenciadorBase
    {
        /// <summary>
        /// Inicializa o gerenciador de modulos
        /// </summary>
        private Gerenciador()
        {
            DataInicio = DateTime.Now;

            Console.CancelKeyPress += Console_CancelKeyPress;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
        }

        private readonly CancellationTokenSource _cancellationToken = new();
        private static Gerenciador _atual;


        ///<inheritdoc/>
        public static IGerenciador Atual
        {
            get
            {
                if (_atual is null || _atual.Estado == Estado.Desligado || _atual.disposedValue)
                {
                    _atual = new Gerenciador();
                }

                return _atual;
            }
        }

        ///<inheritdoc/>
        public override DateTime DataInicio { get; }

        ///<inheritdoc/>
        public override T Criar<T>()
        {
            return (T)Criar(typeof(T));
        }
        ///<inheritdoc/>
        public override IModulo Criar(string nomeModulo)
        {
            return base.Criar(nomeModulo);
        }
        ///<inheritdoc/>
        public override IModulo Criar(Type modulo)
        {
            if (modulo is null)
            {
                throw new ArgumentNullException(nameof(modulo));
            }
            modulo = ResolverContrato(modulo);
            return base.Criar(modulo);
        }

        ///<inheritdoc/>
        public override bool Existe(Type modulo)
        {
            if (modulo is null)
            {
                throw new ArgumentNullException(nameof(modulo));
            }

            try
            {
                modulo = ResolverContrato(modulo);
                return base.Existe(modulo);
            }
            catch (TipoModuloNaoEncontradoException)
            {
                return false;
            }

        }
        ///<inheritdoc/>
        public override bool Existe(IModulo modulo)
        {
            return modulo is null ? throw new ArgumentNullException(nameof(modulo)) : Existe(modulo.Id);
        }
        ///<inheritdoc/>
        public override bool Existe(string id)
        {
            return string.IsNullOrEmpty(id) ? throw new ArgumentNullException(nameof(id)) : base.Existe(id);
        }
        ///<inheritdoc/>
        public override IModulo Obter(Type modulo)
        {
            modulo = ResolverContrato(modulo);
            return base.Obter(modulo);
        }
        ///<inheritdoc/>
        public override T Obter<T>()
        {
            return (T)Obter(typeof(T));
        }
        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<exception cref="ModuloNaoEncontradoException">Instancia do modulo nao foi inicializado</exception>
        public override IModulo Obter(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id), string.Format(Constantes.ERRO_ARGUMENTO_NULO_OU_VAZIO, nameof(id)));
            }

            if (!Existe(id))
            {
                throw new ModuloNaoEncontradoException(string.Format(Constantes.ERRO_MODULO_ID_NAO_ENCONTRADO, id));
            }

            return base.Obter(id);
        }
        ///<inheritdoc/>
        public override void Remover<T>(T modulo)
        {
            base.Remover<T>(modulo);
        }
        ///<inheritdoc/>
        public override void Remover(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException($"'{nameof(id)}' não pode ser nulo nem vazio.", nameof(id));
            }

            base.Remover(id);

        }
        ///<inheritdoc/>
        public override void RemoverTodos()
        {
            base.RemoverTodos();
        }

        ///<inheritdoc/>
        public override T Reciclar<T>(T modulo)
        {
            return (T)Reciclar(modulo.Id);
        }
        ///<inheritdoc/>
        public override IModulo Reciclar(string id)
        {
            return base.Reciclar(id);
        }


        ///<inheritdoc/>
        public override async Task ManterVivoAsync()
        {
            await Task.Run(async () =>
              {
                  while (!_cancellationToken.IsCancellationRequested)
                  {
                      await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(false);
                  }

              }, _cancellationToken.Token).ConfigureAwait(false);
        }

        ///<inheritdoc/>
        public override IEnumerable<IModulo> Listar()
        {
            return base.Listar();
        }

        ///<inheritdoc/>
        private void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Dispose();
            e.Cancel = true;
        }
        ///<inheritdoc/>
        private void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            Atual.Dispose();
        }

        ///<inheritdoc/>
        private static Type ResolverContrato(Type modulo)
        {
            if (!modulo.IsInterface && !modulo.IsClass)
            {
                throw new TipoModuloInvalidoException(Constantes.ERRO_TIPO_INVALIDO);
            }

            if (modulo.IsInterface)
            {
                ModuloContratoAttribute attr = modulo.ObterModuloContratoAtributo();

                if (attr is null)
                {
                    throw new ModuloContratoNaoEncontratoException(Constantes.ERRO_TIPO_NAO_MARCADO);
                }


                modulo = attr.Tipo ?? TypeProvider.Provider.Get(attr.Nome);
                if (modulo is null)
                {
                    throw new TipoModuloNaoEncontradoException(string.Format(Constantes.ERRO_MODULO_NAO_ENCONTRADO, attr.Nome));
                }

            }

            if (modulo.IsClass)
            {
                if (!modulo.Is<IModulo>())
                {
                    throw new TipoModuloInvalidoException(Constantes.ERRO_TIPO_NAO_HERDADO);
                }

                if (modulo.ObterModuloAtributo() is null)
                {
                    throw new TipoModuloInvalidoException(Constantes.ERRO_TIPO_NAO_MARCADO);
                }
            }

            return modulo;
        }


        ///<inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {                
                _cancellationToken.Cancel();
                _cancellationToken.Dispose();
            }
            base.Dispose(disposing);

        }
        ///<inheritdoc/>
        public override string ToString()
        {
            StringBuilder stringBuilder = new(base.ToString());
            _ = stringBuilder.Append("Ultima atualização: ").Append(UltimaAtualizacao).AppendLine();
            _ = stringBuilder.Append("Modulos inicializados: ").Append(ModulosInicializados).AppendLine();
            return stringBuilder.ToString();

        }


    }
}
