using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

using Propeus.Module.Abstract.Attributes;

namespace Propeus.Module.Abstract.Exceptions
{

    /// <summary>
    /// Exceção para quando a interface de contrato nao possui o atributo <see cref="ModuleContractAttribute"/>
    /// </summary>
    [Serializable]
    public class ModuleContractNotFoundException : ModuleException
    {
        /// <summary>
        /// Exceção para quando a interface de contrato nao possui o atributo <see cref="ModuleContractAttribute"/>
        /// </summary>
        /// <param name="contractType">Tipo da interface de contrato</param>
        public ModuleContractNotFoundException(Type contractType) : base(string.Format(Constantes.ERRO_ATRIBUTO_MODULO_CONTRATO_OMISSO, contractType.FullName))
        {
        }

        ///<inheritdoc/>
        [ExcludeFromCodeCoverage(Justification = Constantes.EXECEPTION_CODE_COVERAGE_JUSTIFICATION)]
        protected ModuleContractNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

}