using System;
using System.Runtime.Serialization;

namespace Propeus.Modulo.Core.Exceptions
{
    [Serializable]
    public class ModuloNaoEncontradoException : Exception
    {
        public ModuloNaoEncontradoException()
        {
        }

        public ModuloNaoEncontradoException(string message) : base(message)
        {
        }

        public ModuloNaoEncontradoException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ModuloNaoEncontradoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}