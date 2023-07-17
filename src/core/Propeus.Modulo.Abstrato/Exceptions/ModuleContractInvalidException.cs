using System;
using System.Runtime.Serialization;

namespace Propeus.Modulo.Abstrato.Exceptions
{

    /// <summary>
    /// Excecao para quando a interface de contrato possui o atributo <see cref="ModuleContractInvalidException"/> invalido
    /// </summary>
    [Serializable]
    public class ModuleContractInvalidException : ModuleException
    {
        ///<inheritdoc/>
        public ModuleContractInvalidException(string message) : base(message)
        {
        }

        ///<inheritdoc/>
        protected ModuleContractInvalidException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

}