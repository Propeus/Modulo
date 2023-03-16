using System;

namespace Propeus.Modulo.Abstrato.Exceptions
{
    /// <summary>
    /// Excecao generica
    /// </summary>
    public class ModuloException : Exception
    {

        /// <summary>
        /// Construtor padrao
        /// </summary>
        /// <param name="message">Mensagem do erro</param>
        public ModuloException(string message) : base(message)
        {
        }


    }
}