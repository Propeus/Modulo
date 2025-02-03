using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Propeus.Module.Abstract.Exceptions
{
    /// <summary>
    /// Exceção para quando há mais de um modulo valido e de mesmo nome
    /// </summary>
    [Serializable]
    public class ModuleAmbiguousException : ModuleException
    {

        /// <summary>
        /// Exceção para quando há mais de um modulo valido e de mesmo nome
        /// </summary>
        public ModuleAmbiguousException(IEnumerable<Type> modules) : base(string.Format(Constantes.ERRO_MODULO_AMBIGUO, modules.First().Name))
        {
            Data.Add("Modulos duplicados", modules);
        }

        ///<inheritdoc/>
        [ExcludeFromCodeCoverage(Justification = Constantes.EXECEPTION_CODE_COVERAGE_JUSTIFICATION)]
        protected ModuleAmbiguousException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
