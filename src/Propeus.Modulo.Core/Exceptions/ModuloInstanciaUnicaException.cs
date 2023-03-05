using System;
using System.Runtime.Serialization;

namespace Propeus.Modulo.Core.Exceptions
{
    [Serializable]
    internal class ModuloInstanciaUnicaException : Exception
    {
        public ModuloInstanciaUnicaException()
        {
        }

        public ModuloInstanciaUnicaException(string message) : base(message)
        {
        }

        public ModuloInstanciaUnicaException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ModuloInstanciaUnicaException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}