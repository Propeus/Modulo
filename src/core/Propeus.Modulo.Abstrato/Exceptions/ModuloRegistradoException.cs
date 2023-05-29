using System;
using System.Runtime.Serialization;

namespace Propeus.Modulo.Abstrato.Exceptions
{
    /// <summary>
    /// Excecao para quando o mesmo modulo for registrado mais de uma unica vez
    /// </summary>
    [Serializable]
    public class ModuloRegistradoException : Exception
    {
        /// <summary>
        /// Construtor padrao
        /// </summary>
        /// <param name="message">Mensagem do erro</param>
        public ModuloRegistradoException(string message) : base(message) { }
    }
}