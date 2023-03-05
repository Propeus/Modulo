using System;
using System.Runtime.Serialization;

namespace Propeus.Modulo.Core.Exceptions
{
    [Serializable]
    public class ModuloDescartadoException : Exception
    {
        public ModuloDescartadoException()
        {
        }

        public ModuloDescartadoException(string message) : base(message)
        {
        }

        public ModuloDescartadoException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ModuloDescartadoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}