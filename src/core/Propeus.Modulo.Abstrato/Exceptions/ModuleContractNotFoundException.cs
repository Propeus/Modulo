using System;
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
        protected ModuleContractNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

}