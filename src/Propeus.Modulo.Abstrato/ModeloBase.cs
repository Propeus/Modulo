using System;
using System.Text;

using Propeus.Modulo.Abstrato.Interfaces;

namespace Propeus.Modulo.Abstrato
{
    /// <summary>
    /// Classe com o modelo base para o projeto
    /// </summary>
    public abstract class ModeloBase : IBaseModelo
    {

        /// <summary>
        /// Inicia um modelo basico
        /// </summary>
        protected ModeloBase()
        {
            Nome = GetType().Name;
            Estado = Estado.Inicializado;
            Id = Guid.NewGuid().ToString();
            IdManifesto = this.GetType().Assembly.ManifestModule.ModuleVersionId.ToString();
        }

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
        public Estado Estado { get; set; }
        ///<inheritdoc/>
        public string Nome { get; protected set; }
        ///<inheritdoc/>
        public string Id { get; }
        ///<inheritdoc/>
        public string IdManifesto { get; }

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

            return sb.ToString();
        }

        #region IDisposable Support
        /// <summary>
        /// Para detectar chamadas redundantes
        /// </summary>
        protected bool disposedValue = false; 

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
                }

                disposedValue = true;
            }
        }

        ///<inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
