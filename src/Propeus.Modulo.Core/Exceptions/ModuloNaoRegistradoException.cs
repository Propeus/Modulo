using System;
using System.Runtime.Serialization;

namespace Propeus.Modulo.Core.Exceptions
{
    /// <summary>
    /// Excecao para quando o modulo for criado fora do Gerenciador
    /// </summary>
    [Serializable]
    public class ModuloNaoRegistradoException : Exception
    {
        /// <summary>
        /// Construtor padrao
        /// </summary>
        /// <param name="message">Mensagem do erro</param>
        public ModuloNaoRegistradoException(string message) : base(message) { }
    }
}