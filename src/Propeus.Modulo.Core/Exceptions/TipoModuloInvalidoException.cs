using System;
using System.Runtime.Serialization;

namespace Propeus.Modulo.Core.Exceptions
{
    [Serializable]
    internal class TipoModuloInvalidoException : Exception
    {
        public TipoModuloInvalidoException()
        {
        }

        public TipoModuloInvalidoException(string message) : base(message)
        {
        }

        public TipoModuloInvalidoException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TipoModuloInvalidoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}