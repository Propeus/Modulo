using System;
using System.Runtime.Serialization;

namespace Propeus.Modulo.Abstrato.Exceptions
{
    /// <summary>
    /// Excecao para quando o modulo nao possuir nenhum construtor publico
    /// </summary>
    [Serializable]
    public class ModuloConstrutorAusenteException : ModuloException
    {

        /// <summary>
        /// Construtor padrao
        /// </summary>
        /// <param name="message">Mensagem do erro</param>
        public ModuloConstrutorAusenteException(string message) : base(message)
        {
        }


    }
}