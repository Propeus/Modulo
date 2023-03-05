using System;
using System.Runtime.Serialization;

namespace Propeus.Modulo.Core.Exceptions
{
    /// <summary>
    /// Excecao para quando o modulo nao seguir os padroes de implementacao
    /// </summary>
    [Serializable]
    public class TipoModuloInvalidoException : Exception
    {
        /// <summary>
        /// Construtor padrao
        /// </summary>
        /// <param name="message">Mensagem do erro</param>
        public TipoModuloInvalidoException(string message) : base(message) { }
    }
}