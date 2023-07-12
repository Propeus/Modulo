using System;
using System.Runtime.Serialization;

namespace Propeus.Modulo.Abstrato.Exceptions
{
    /// <summary>
    /// Excecao para quando for encontrado mais de um modulo de mesmo nome
    /// </summary>
    [Serializable]
    public class ModuleTypeAmbiguousException : ModuleException
    {
        /// <summary>
        /// Construtor padrao
        /// </summary>
        /// <param name="message">Mensagem do erro</param>
        public ModuleTypeAmbiguousException(string message) : base(message) { }
    }
}