using System;
using System.Text;

using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Helpers;
using Propeus.Modulo.Abstrato.Interfaces;

namespace Propeus.Modulo.Core
{

    /// <summary>
    /// Informa detalhes sobre o modulo instanciado
    /// </summary>
    internal class ModuloTipo : BaseModel, IModuleType
    {

        /// <summary>
        /// Inicializa o objeto informando a instancia do modulo
        /// </summary>
        /// <param name="modulo">Instancia do modulo</param>
        /// <exception cref="ArgumentNullException">Module nao pode ser nulo</exception>
        public ModuloTipo(IModule modulo)
        {
            WeakReference = new WeakReference(modulo);
            IsSingleInstance = modulo.GetType().GetModuleAttribute().Singleton;
            IdModule = modulo.Id;
            Version = modulo.Version;
            Name = modulo.Name;
        }

        /// <summary>
        /// Version do modulo
        /// </summary>
        public override string Version { get; }
        ///<inheritdoc/>
        public bool IsCollected => WeakReference is null || !WeakReference.IsAlive;
        ///<inheritdoc/>
        public bool IsDeleted => IsCollected || (WeakReference.Target as IModule)?.State == State.Off;
        ///<inheritdoc/>
        public bool IsKeepAlive => _moduleKeepAlive is not null;
        ///<inheritdoc/>
        public WeakReference WeakReference { get; protected set; }
        ///<inheritdoc/>
        public IModule Module => WeakReference.Target as IModule;
        ///<inheritdoc/>
        public Type ModuleType => WeakReference?.Target?.GetType();

        ///<inheritdoc/>
        public bool IsSingleInstance { get; }
        ///<inheritdoc/>
        public string IdModule { get; }


        public IModule _moduleKeepAlive;
        ///<inheritdoc/>
        public void KeepAliveModule(bool keepAlive)
        {
            if (IsDeleted)
            {
                return;
            }

            if (keepAlive)
            {
                _moduleKeepAlive = Module;
            }
            else
            {
                _moduleKeepAlive = null;
            }
        }

        /// <summary>
        /// Exibe informacoes detalhadas sobre o modulo e seu estado no .NET
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new(base.ToString());

            if (Module != null)
            {
                _ = sb.Append(Module).AppendLine();
            }
            _ = sb.Append("IsCollected pelo G.C.: ").Append(IsCollected).AppendLine();
            _ = sb.Append("Objeto eliminado: ").Append(IsDeleted).AppendLine();


            return sb.ToString();
        }

        ///<inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            if (!IsDeleted)
            {
                KeepAliveModule(false);
                Module.Dispose();
            }
            WeakReference = null;
            base.Dispose(disposing);
        }


    }
}
