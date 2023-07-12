using System;

namespace Propeus.Modulo.Abstrato.Exceptions
{
    /// <summary>
    /// Excecao para tentativa de criacao de um novo modulo de instancia unica
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3925:\"ISerializable\" should be implemented correctly", Justification = "<Pendente>")]
    public class ModuleSingleInstanceException : ModuleException
    {
        /// <summary>
        /// Construtor padrao
        /// </summary>
        /// <param name="message">Mensagem do erro</param>
        public ModuleSingleInstanceException(string message) : base(message) { }
    }
}