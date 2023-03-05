using System;
using System.Runtime.Serialization;

namespace Propeus.Modulo.Core.Exceptions
{
    /// <summary>
    /// Excecao para quando o modulo é descartado pelo <see cref="IDisposable.Dispose"/> ou quando o <see cref="GC"/> coleta o objeto
    /// </summary>
    [Serializable]
    public class ModuloDescartadoException : Exception
    {
        /// <summary>
        /// Construtor padrao
        /// </summary>
        /// <param name="message">Mensagem do erro</param>
        public ModuloDescartadoException(string message) : base(message) { }
    }
}