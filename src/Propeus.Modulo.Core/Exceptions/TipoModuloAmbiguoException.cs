using System;
using System.Runtime.Serialization;

namespace Propeus.Modulo.Core.Exceptions
{
    [Serializable]
    public class TipoModuloAmbiguoException : Exception
    {
        public TipoModuloAmbiguoException()
        {
        }

        public TipoModuloAmbiguoException(string message) : base(message)
        {
        }

        public TipoModuloAmbiguoException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TipoModuloAmbiguoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}