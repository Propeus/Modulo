using System;

using Propeus.Modulo.Abstrato.Atributos;

namespace Propeus.Modulo.Abstrato.Exceptions
{
#pragma warning disable S3925 // "ISerializable" should be implemented correctly
    /// <summary>
    /// Excecao para quando a interface de contrato nao possui o atributo <see cref="ModuleContractAttribute"/>
    /// </summary>
    public class ModuleContractNotFoundException : ModuleException
    {
        /// <summary>
        /// Construtor padrao
        /// </summary>
        /// <param name="message">Mensagem do erro</param>
        public ModuleContractNotFoundException(string message) : base(message)
        {
        }
    }
#pragma warning restore S3925 // "ISerializable" should be implemented correctly
}