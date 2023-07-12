using System;

namespace Propeus.Modulo.Abstrato.Exceptions
{
    /// <summary>
    /// Excecao para quando o modulo nao seguir os padroes de implementacao
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3925:\"ISerializable\" should be implemented correctly", Justification = "<Pendente>")]
    public class ModuleTypeInvalidException : ModuleException
    {
        /// <summary>
        /// Construtor padrao
        /// </summary>
        /// <param name="message">Mensagem do erro</param>
        public ModuleTypeInvalidException(string message) : base(message) { }
    }
}