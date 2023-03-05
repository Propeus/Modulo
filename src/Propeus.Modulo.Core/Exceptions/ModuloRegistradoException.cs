using System;
using System.Runtime.Serialization;

namespace Propeus.Modulo.Core.Exceptions
{
    [Serializable]
    public class ModuloRegistradoException : Exception
    {
        public ModuloRegistradoException()
        {
        }

        public ModuloRegistradoException(string message) : base(message)
        {
        }

        public ModuloRegistradoException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ModuloRegistradoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}