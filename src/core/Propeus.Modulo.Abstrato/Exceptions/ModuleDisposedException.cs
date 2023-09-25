using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

using Propeus.Module.Abstract.Interfaces;

namespace Propeus.Module.Abstract.Exceptions
{
    /// <summary>
    /// Exceção para quando o modulo é descartado pelo <see cref="IDisposable.Dispose"/> ou quando o <see cref="GC"/> coleta o objeto
    /// </summary>
    [Serializable]
    public class ModuleDisposedException : ModuleException
    {
        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="module">Instancia do modulo</param>
        public ModuleDisposedException(IModule module) : base(string.Format(Constantes.ERRO_MODULO_ID_DESCARTADO, module.Id)) { }

        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="idModule">Id do modulo</param>
        public ModuleDisposedException(string idModule) : base(string.Format(Constantes.ERRO_MODULO_ID_DESCARTADO, idModule)) { }

        ///<inheritdoc/>
        [ExcludeFromCodeCoverage(Justification = Constantes.EXECEPTION_CODE_COVERAGE_JUSTIFICATION)]
        protected ModuleDisposedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}