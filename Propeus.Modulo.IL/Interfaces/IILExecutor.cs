using System;
using System.Collections.Generic;
using System.Text;

namespace Propeus.Modulo.IL.Interfaces
{
    /// <summary>
    /// Interface para execução de função
    /// </summary>
    internal interface IILExecutor
    {
        /// <summary>
        /// Executa a montagem do código IL
        /// </summary>
        public void Executar();

        /// <summary>
        /// Inidica se o objeto ja foi executado
        /// </summary>
        public bool Executado { get; }
    }
}