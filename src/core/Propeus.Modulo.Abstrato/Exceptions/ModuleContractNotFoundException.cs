using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

using Propeus.Modulo.Abstrato.Attributes;

namespace Propeus.Modulo.Abstrato.Exceptions
{

    /// <summary>
    /// Excecao para quando a interface de contrato nao possui o atributo <see cref="ModuleContractAttribute"/>
    /// </summary>
    [Serializable]
    public class ModuleContractNotFoundException : ModuleException
    {
        ///<inheritdoc/>
        public ModuleContractNotFoundException(string message) : base(message)
        {
        }

        ///<inheritdoc/>
        [ExcludeFromCodeCoverage(Justification = "Impelementacao do Serializable Pattern")]
        protected ModuleContractNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

}