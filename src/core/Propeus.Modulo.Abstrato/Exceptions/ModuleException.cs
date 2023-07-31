using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Propeus.Modulo.Abstrato.Exceptions
{
    /// <summary>
    /// Exceção genérica
    /// </summary>
    [Serializable]
    public class ModuleException : Exception
    {

        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="message">Mensagem do erro</param>
        public ModuleException(string message) : base(message)
        {
        }

        /// <summary>
        /// Construtor para serialização
        /// </summary>
        /// <param name="info">Informação da serialização</param>
        /// <param name="context">Contexto</param>
        [ExcludeFromCodeCoverage(Justification = Constantes.EXECEPTION_CODE_COVERAGE_JUSTIFICATION)]
        protected ModuleException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}