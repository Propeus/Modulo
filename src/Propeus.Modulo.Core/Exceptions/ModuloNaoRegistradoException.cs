using System;
using System.Runtime.Serialization;

namespace Propeus.Modulo.Core.Exceptions
{
    [Serializable]
    public class ModuloNaoRegistradoException : Exception
    {
        public ModuloNaoRegistradoException()
        {
        }

        public ModuloNaoRegistradoException(string message) : base(message)
        {
        }

        public ModuloNaoRegistradoException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ModuloNaoRegistradoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}