using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Propeus.Module.Abstract.Exceptions
{

    /// <summary>
    /// Exceção para quando a interface de contrato possui o atributo <see cref="ModuleContractInvalidException"/> invalido
    /// </summary>
    [Serializable]
    public class ModuleContractInvalidException : ModuleException
    {
        /// <summary>
        /// Exceção para quando a interface de contrato possui o atributo <see cref="ModuleContractInvalidException"/> invalido
        /// </summary>
        public ModuleContractInvalidException() : base(Constantes.ERRO_ATRIBUTO_MODULO_CONTRATO_INVALIDO)
        {
        }

        ///<inheritdoc/>
        [ExcludeFromCodeCoverage(Justification = Constantes.EXECEPTION_CODE_COVERAGE_JUSTIFICATION)]
        protected ModuleContractInvalidException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    /// <summary>
    /// Exceção para quando a interface de contrato não foi mapeado para o modulo alvo
    /// </summary>
    [Serializable]
    public class ModuleContractNotMapedException : ModuleException
    {
        /// <summary>
        /// Exceção para quando a interface de contrato não foi mapeado para o modulo alvo
        /// </summary>
        public ModuleContractNotMapedException() : base(Constantes.ERRO_MODULO_CONTRATO_NAO_MAPEADO)
        {
        }

        ///<inheritdoc/>
        [ExcludeFromCodeCoverage(Justification = Constantes.EXECEPTION_CODE_COVERAGE_JUSTIFICATION)]
        protected ModuleContractNotMapedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

}