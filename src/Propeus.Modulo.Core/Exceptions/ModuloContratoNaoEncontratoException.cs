using System;
using System.Runtime.Serialization;

namespace Propeus.Modulo.Core.Exceptions
{
    [Serializable]
    internal class ModuloContratoNaoEncontratoException : Exception
    {
        public ModuloContratoNaoEncontratoException()
        {
        }

        public ModuloContratoNaoEncontratoException(string message) : base(message)
        {
        }

        public ModuloContratoNaoEncontratoException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ModuloContratoNaoEncontratoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}