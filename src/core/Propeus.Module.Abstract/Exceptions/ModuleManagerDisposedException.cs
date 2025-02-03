using System.Diagnostics.CodeAnalysis;
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
        /// Exceção para quando é acionado alguma ação no <see cref="IModuleManager"/> após a chamada do método <see cref="IDisposable.Dispose"/>
        /// </summary>
        public ModuleManagerDisposedException() : base(Constantes.ERRO_GERENCIADOR_DESCARTADO)
        {
        }

        ///<inheritdoc/>
        [ExcludeFromCodeCoverage(Justification = Constantes.EXECEPTION_CODE_COVERAGE_JUSTIFICATION)]
        protected ModuleManagerDisposedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}