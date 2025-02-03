using System.Text;

using Propeus.Module.Abstract.Helpers;
using Propeus.Module.Abstract.Interfaces;

namespace Propeus.Module.Abstract
{
    /// <summary>
    /// Classe base para o modulo
    /// </summary>
    public abstract class BaseModule : BaseModel, IModule
    {
        /// <summary>
        /// Inicializa um modulo
        /// </summary>
        protected BaseModule() : base()
        {
            Name = GetType().Name;
        }

        ///<inheritdoc/>
        public virtual void ConfigureModule()
        {
            State = State.Ready;
        }
        ///<inheritdoc/>
        public virtual void Launch()
        {
            State = State.Running;
        }

        /// <summary>
        /// Exibe informações básicas sobre o modulo
        /// </summary>
        /// <returns>Informações basicas do modulo atual</returns>
        public override string ToString()
        {
            StringBuilder sb = new(base.ToString());

            var attr = GetType().GetModuleAttribute();
            string? moduleDescription = attr is not null ? attr.Description : string.Empty;

            if (moduleDescription is not null)
            {
                _ = sb.AppendLine($"Descrição: {moduleDescription}");
            }

            return sb.ToString();
        }

    }
}
