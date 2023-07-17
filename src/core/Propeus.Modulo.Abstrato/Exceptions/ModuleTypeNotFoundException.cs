using System;
using System.Runtime.Serialization;

namespace Propeus.Modulo.Abstrato.Exceptions
{
    /// <summary>
    /// Excecao para quando o tipo do modulo informado nao for encontrado no Assembly
    /// </summary>
    [Serializable]
    public class ModuleTypeNotFoundException : ModuleException
    {
        ///<inheritdoc/>
        public ModuleTypeNotFoundException(string message) : base(message) { }

        ///<inheritdoc/>
        protected ModuleTypeNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}