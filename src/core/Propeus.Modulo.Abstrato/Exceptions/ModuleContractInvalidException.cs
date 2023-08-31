using System;
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
        public ModuleContractInvalidException() : base("O atributo deve possuir um nome ou tipo do modulo")
        {
        }

        ///<inheritdoc/>
        [ExcludeFromCodeCoverage(Justification = Constantes.EXECEPTION_CODE_COVERAGE_JUSTIFICATION)]
        protected ModuleContractInvalidException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

}