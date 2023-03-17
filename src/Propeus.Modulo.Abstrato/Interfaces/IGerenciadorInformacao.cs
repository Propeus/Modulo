using System;

namespace Propeus.Modulo.Abstrato.Interfaces
{

    /// <summary>
    /// Modelo base para obter informações dos modulos
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
        /// Obtem o <see cref="IModuloTipo"/> de <paramref name="modulo"/> caso exista
        /// <para>Caso exista mais de uma instancia do mesmo tipo, o primeiro <see cref="IModuloTipo"/> sempre será retornado</para>
        /// </summary>
        /// <param name="modulo">Qualquer tipo herdado de <see cref="IModulo"/></param>
        /// <returns><see cref="IModuloTipo"/></returns>
        IModuloTipo ObterInfo(Type modulo);
        /// <summary>
        /// Obtem  o <see cref="IModuloTipo"/> do modulo pelo id
        /// </summary>
        /// <param name="id">Identificação unica do modulo </param>
        /// <returns><see cref="IModuloTipo"/></returns>
        IModuloTipo ObterInfo(string id);

    }
}
