using System.Text;

using Propeus.Module.Abstract.Interfaces;

namespace Propeus.Module.Abstract
{
    /// <summary>
    /// Classe com o modelo base para o projeto
    /// </summary>
    public abstract class BaseModel : IBaseModel
    {

        /// <summary>
        /// Inicia um modelo basico
        /// </summary>
        protected BaseModel()
        {
            Name = GetType().Name;
            State = State.Created;
            Id = Guid.NewGuid().ToString();
            ManifestId = GetType().Assembly.ManifestModule.ModuleVersionId.ToString();
        }

        ///<inheritdoc/>
        public virtual string Version
        {
            get
            {
                Version ver = GetType().Assembly.GetName().Version;
                return $"{ver.Major}.{ver.Minor}.{ver.Build}";
            }
        }


        ///<inheritdoc/>
        public State State { get; set; }
        ///<inheritdoc/>
        public string Name { get; protected set; }
        ///<inheritdoc/>
        public string Id { get; }
        ///<inheritdoc/>
        public string ManifestId { get; }

        /// <summary>
        /// Exibe informações basicas sobre o modelo
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new();

            _ = sb.Append("ModuleName: ").Append(Name).AppendLine();
            _ = sb.Append("State: ").Append(State).AppendLine();
            _ = sb.Append("Id: ").Append(Id).AppendLine();
            _ = sb.Append("Version: ").Append(Version).AppendLine();

            return sb.ToString();
        }

        #region IDisposable Support
        /// <summary>
        /// Para detectar chamadas redundantes
        /// </summary>
        protected bool disposedValue = false;

        /// <summary>
        /// Libera os objetos deste modelo e altera o estado dele para <see cref="State.Off"/>
        /// </summary>
        /// <param name="disposing">Indica se deve alterar o estado do objeto para <see cref="State.Off"/></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    State = State.Off;
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
