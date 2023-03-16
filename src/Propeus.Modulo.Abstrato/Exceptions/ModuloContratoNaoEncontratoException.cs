using System;

using Propeus.Modulo.Abstrato.Atributos;

namespace Propeus.Modulo.Abstrato.Exceptions
{
    /// <summary>
    /// Excecao para quando a interface de contrato nao possui o atributo <see cref="ModuloContratoAttribute"/>
    /// </summary>
    [Serializable]
    public class ModuloContratoNaoEncontratoException : Exception
    {
        /// <summary>
        /// Construtor padrao
        /// </summary>
        /// <param name="message">Mensagem do erro</param>
        public ModuloContratoNaoEncontratoException(string message) : base(message)
        {
        }


    }
}