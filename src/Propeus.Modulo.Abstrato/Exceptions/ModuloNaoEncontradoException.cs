using System;
using System.Runtime.Serialization;

namespace Propeus.Modulo.Abstrato.Exceptions
{
    /// <summary>
    /// Excecao para quando o modulo informado nao foi encontrado
    /// </summary>
    [Serializable]
    public class ModuloNaoEncontradoException : Exception
    {
        /// <summary>
        /// Construtor padrao
        /// </summary>
        /// <param name="message">Mensagem do erro</param>
        public ModuloNaoEncontradoException(string message) : base(message) { }
    }
}