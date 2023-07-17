using System;
using System.Runtime.Serialization;

namespace Propeus.Modulo.Abstrato.Exceptions
{
    /// <summary>
    /// Excecao para quando o modulo nao seguir os padroes de implementacao
    /// </summary>
    [Serializable]
    public class ModuleTypeInvalidException : ModuleException
    {
        ///<inheritdoc/>
        public ModuleTypeInvalidException(string message) : base(message) { }

        ///<inheritdoc/>
        protected ModuleTypeInvalidException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}