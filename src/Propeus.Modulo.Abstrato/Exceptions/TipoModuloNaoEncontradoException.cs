using System;

namespace Propeus.Modulo.Abstrato.Exceptions
{
    /// <summary>
    /// Excecao para quando o tipo do modulo informado nao for encontrado no Assembly
    /// </summary>
    [Serializable]
    public class TipoModuloNaoEncontradoException : Exception
    {
        /// <summary>
        /// Construtor padrao
        /// </summary>
        /// <param name="message">Mensagem do erro</param>
        public TipoModuloNaoEncontradoException(string message) : base(message) { }
    }
}