using System;
using System.Runtime.Serialization;

namespace Propeus.Modulo.Core.Exceptions
{
    public class ModuloException : Exception
    {
        public ModuloException()
        {
        }

        public ModuloException(string message) : base(message)
        {
        }

        public ModuloException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ModuloException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}