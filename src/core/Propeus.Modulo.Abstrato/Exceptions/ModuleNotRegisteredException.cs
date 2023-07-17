using System;
using System.Runtime.Serialization;

namespace Propeus.Modulo.Abstrato.Exceptions
{
    /// <summary>
    /// Excecao para quando o modulo for criado fora do Gerenciador
    /// </summary>
    [Serializable]
    public class ModuleNotRegisteredException : ModuleException
    {
        ///<inheritdoc/>
        public ModuleNotRegisteredException(string message) : base(message) { }

        ///<inheritdoc/>
        protected ModuleNotRegisteredException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}