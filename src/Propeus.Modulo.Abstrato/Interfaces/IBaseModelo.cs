﻿using System;

namespace Propeus.Modulo.Abstrato.Interfaces
{
    /// <summary>
    /// Modelo base para todos os objetos do projeto
    /// </summary>
    public interface IBaseModelo : IDisposable
    {

        /// <summary>
        /// Versao do modelo
        /// </summary>
        string Versao { get; }
        /// <summary>
        /// Representa o estado do objeto.
        /// </summary>
        Estado Estado { get; }
        /// <summary>
        /// Representação amigavel do ojeto. 
        /// <para>
        /// Caso seja nulo o nome da classe herdado será informado na propriedade.
        /// </para>
        /// </summary>
        string Nome { get; }
        /// <summary>
        /// Representação alfanumerica e unica do objeto.
        /// </summary>
        string Id { get; }
        /// <summary>
        /// <see cref="Guid"/> do <see cref="System.Reflection.Assembly" /> atual
        /// </summary>
        string IdManifesto { get; }
    }
}
