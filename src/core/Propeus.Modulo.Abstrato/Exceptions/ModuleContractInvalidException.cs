using System;

using Propeus.Modulo.Abstrato.Attributes;

namespace Propeus.Modulo.Abstrato.Exceptions
{
#pragma warning disable S3925 // "ISerializable" should be implemented correctly
    /// <summary>
    /// Excecao para quando a interface de contrato possui o atributo <see cref="ModuleContractInvalidException"/> invalido
    /// </summary>
    public class ModuleContractInvalidException : ModuleException
    {
        /// <summary>
        /// Construtor padrao
        /// </summary>
        /// <param name="message">Mensagem do erro</param>
        public ModuleContractInvalidException(string message) : base(message)
        {
        }
    }
#pragma warning restore S3925 // "ISerializable" should be implemented correctly
}