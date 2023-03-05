using System;
using System.Runtime.Serialization;

namespace Propeus.Modulo.Core.Exceptions
{
    /// <summary>
    /// Excecao para quando for encontrado mais de um modulo de mesmo nome
    /// </summary>
    [Serializable]
    public class TipoModuloAmbiguoException : Exception
    {
        /// <summary>
        /// Construtor padrao
        /// </summary>
        /// <param name="message">Mensagem do erro</param>
        public TipoModuloAmbiguoException(string message) : base(message) { }
    }
}