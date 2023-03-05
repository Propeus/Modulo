using System;
using System.Runtime.Serialization;

namespace Propeus.Modulo.Core.Exceptions
{
    [Serializable]
    internal class ModuloConstrutorAusenteException : ModuloException
    {
        public ModuloConstrutorAusenteException()
        {
        }

        public ModuloConstrutorAusenteException(string message) : base(message)
        {
        }

        public ModuloConstrutorAusenteException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ModuloConstrutorAusenteException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}