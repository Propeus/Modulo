﻿namespace Propeus.Modulo.Abstrato.Exceptions
{
    /// <summary>
    /// Excecao para quando o tipo do modulo informado nao for encontrado no Assembly
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3925:\"ISerializable\" should be implemented correctly", Justification = "<Pendente>")]
    public class ModuleTypeNotFoundException : ModuleException
    {
        /// <summary>
        /// Construtor padrao
        /// </summary>
        /// <param name="message">Mensagem do erro</param>
        public ModuleTypeNotFoundException(string message) : base(message) { }
    }
}