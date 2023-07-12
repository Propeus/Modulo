using System;
using System.Runtime.Serialization;

namespace Propeus.Modulo.Abstrato.Exceptions
{
    /// <summary>
    /// Excecao para quando o modulo nao possuir nenhum construtor publico
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3925:\"ISerializable\" should be implemented correctly", Justification = "<Pendente>")]
    public class ModuleBuilderAbsentException : ModuleException
    {

        /// <summary>
        /// Construtor padrao
        /// </summary>
        /// <param name="message">Mensagem do erro</param>
        public ModuleBuilderAbsentException(string message) : base(message)
        {
        }
    }
}