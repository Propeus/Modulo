using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Propeus.Modulo.Abstrato.Exceptions
{
    /// <summary>
    /// Excecao generica
    /// </summary>
    [Serializable]
    public class ModuleException : Exception
    {

        /// <summary>
        /// Construtor padrao
        /// </summary>
        /// <param name="message">Mensagem do erro</param>
        public ModuleException(string message) : base(message)
        {
        }

        /// <summary>
        /// Construtor para serializacao
        /// </summary>
        /// <param name="info">Informacao da serializacao</param>
        /// <param name="context">Contexto</param>
        [ExcludeFromCodeCoverage(Justification = "Impelementacao do Serializable Pattern")]
        protected ModuleException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}