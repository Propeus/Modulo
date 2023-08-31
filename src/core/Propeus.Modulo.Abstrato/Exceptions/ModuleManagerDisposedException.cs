using System;
using System.Runtime.Serialization;

using Propeus.Module.Abstract.Interfaces;

namespace Propeus.Module.Abstract.Exceptions
{
    /// <summary>
    /// Exceção para quando é acionado alguma ação no <see cref="IModuleManager"/> após a chamada do método <see cref="IDisposable.Dispose"/>
    /// </summary>
    [Serializable]
    public class ModuleManagerDisposedException : ModuleException
    {
        /// <summary>
        /// Construtor padrão
        /// </summary>
        public ModuleManagerDisposedException() : base("O Gerenciador atual foi descartado.")
        {
        }

        ///<inheritdoc/>
        protected ModuleManagerDisposedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}