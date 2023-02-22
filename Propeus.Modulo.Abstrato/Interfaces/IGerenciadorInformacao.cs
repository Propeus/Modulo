﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Propeus.Modulo.Abstrato.Interfaces
{

    /// <summary>
    /// Modelo base para criação de gerenciadores
    /// </summary>
    public interface IGerenciadorInformacao
    {
       

        /// <summary>
        /// Obtem o <see cref="IModuloTipo"/> de <typeparamref name="T"/> caso exista
        /// <para>Caso exista mais de uma instancia do mesmo tipo, o primeiro <see cref="IModuloTipo"/> sempre será retornado</para>
        /// </summary>
        /// <typeparam name="T">Qualquer tipo herdado de <see cref="IModulo"/></typeparam>
        /// <returns><see cref="IModuloTipo"/></returns>
        IModuloTipo ObterInfo<T>() where T : IModulo;
        /// <summary>
        /// Obtem o <see cref="IModuloTipo"/> de <paramref name="type"/> caso exista
        /// <para>Caso exista mais de uma instancia do mesmo tipo, o primeiro <see cref="IModuloTipo"/> sempre será retornado</para>
        /// </summary>
        /// <param name="type">Qualquer tipo herdado de <see cref="IModulo"/></param>
        /// <returns><see cref="IModuloTipo"/></returns>
        IModuloTipo ObterInfo(Type type);
        /// <summary>
        /// Obtem  o <see cref="IModuloTipo"/> do modulo pelo id
        /// </summary>
        /// <param name="id">Identificação unica do modulo </param>
        /// <returns><see cref="IModuloTipo"/></returns>
        IModuloTipo ObterInfo(string id);

    }
}
