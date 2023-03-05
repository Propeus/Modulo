using System;
using System.Runtime.Serialization;

namespace Propeus.Modulo.Core.Exceptions
{
    /// <summary>
    /// Excecao para tentativa de criacao de um novo modulo de instancia unica
    /// </summary>
    [Serializable]
    public class ModuloInstanciaUnicaException : Exception
    {
        /// <summary>
        /// Construtor padrao
        /// </summary>
        /// <param name="message">Mensagem do erro</param>
        public ModuloInstanciaUnicaException(string message) : base(message) { }
    }
}