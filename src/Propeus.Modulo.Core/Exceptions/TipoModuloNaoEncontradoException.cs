using System;
using System.Runtime.Serialization;

namespace Propeus.Modulo.Core.Exceptions
{
    [Serializable]
    internal class TipoModuloNaoEncontradoException : Exception
    {
        public TipoModuloNaoEncontradoException()
        {
        }

        public TipoModuloNaoEncontradoException(string message) : base(message)
        {
        }

        public TipoModuloNaoEncontradoException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TipoModuloNaoEncontradoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}