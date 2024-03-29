﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Propeus.Modulo.Abstrato.Interfaces
{

   
    /// <summary>
    /// Interface para criar instancias de modulo passando argumentos
    /// </summary>
    public interface IGerenciadorArgumentos : IGerenciador
    {
        /// <summary>
        /// Cria uma nova instancia do modulo <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Qualquer tipo herdado de <see cref="IModulo"/></typeparam>
        /// <param name="args">Qualquer argumento necessário para o modulo </param>
        /// <returns></returns>
        T Criar<T>(object[] args) where T : IModulo;
        /// <summary>
        /// Cria uma nova instancia do modulo usando o tipo do parametro <paramref name="modulo"/>
        /// </summary>
        /// <param name="modulo">Tipo do modulo</param>
        /// <param name="args">Qualquer argumento necessário para o modulo </param>
        /// <returns><see cref="IModulo"/></returns>
        IModulo Criar(Type modulo, object[] args);
        /// <summary>
        /// Cria uma nova instancia do modulo buscando o tipo pelo nome
        /// </summary>
        /// <param name="nomeModulo">Nome do modulo</param>
        /// <param name="args">Qualquer argumento necessário para o modulo </param>
        /// <returns><see cref="IModulo"/></returns>
        IModulo Criar(string nomeModulo, object[] args);
    }

    /// <summary>
    /// Modelo base para criação de gerenciadores
    /// </summary>
    public interface IGerenciador : IBaseModelo
    {
        /// <summary>
        /// Retorna data e hora que o gerenciador iniciou
        /// </summary>
        DateTime DataInicio { get; }
        /// <summary>
        /// Data e hora do ultimo evento realizado no gerenciador
        /// </summary>
        /// <remarks>
        /// Os eventos sao o CRUD (Criar, Reiniciar, Atualizar ou Remover) do genreciador
        /// </remarks>
        DateTime UltimaAtualizacao { get; }

        /// <summary>
        /// Indica a quantidade de modulos inicializados pelo gerenciador
        /// </summary>
        int ModulosInicializados { get; }

        /// <summary>
        /// Cria uma nova instancia do modulo <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Qualquer tipo herdado de <see cref="IModulo"/></typeparam>
        /// <returns><typeparamref name="T"/></returns>
        T Criar<T>() where T : IModulo;
        /// <summary>
        /// Cria uma nova instancia do modulo usando o tipo do parametro <paramref name="modulo"/>
        /// </summary>
        /// <param name="modulo">Tipo do modulo</param>
        /// <returns><see cref="IModulo"/></returns>
        IModulo Criar(Type modulo);
        /// <summary>
        /// Cria uma nova instancia do modulo buscando o tipo pelo nome
        /// </summary>
        /// <param name="nomeModulo">Nome do modulo</param>
        /// <returns><see cref="IModulo"/></returns>
        IModulo Criar(string nomeModulo);

        /// <summary>
        /// Remove um modulo pelo seu ID
        /// </summary>
        /// <param name="id">Identificação unica do modulo </param>
        void Remover(string id);
        /// <summary>
        /// Remove o modulo instanciado
        /// </summary>
        /// <typeparam name="T">Qualquer tipo herdado de <see cref="IModulo"/></typeparam>
        /// <param name="modulo">Qualquer modulo herdado de <see cref="IModulo"/></param>
        void Remover<T>(T modulo) where T : IModulo;
        /// <summary>
        /// Remove todos os modulos
        /// </summary>
        void RemoverTodos();

        /// <summary>
        /// Obtem a instancia de <typeparamref name="T"/> caso exista
        /// <para>Caso exista mais de uma instancia do mesmo tipo, o primeiro modulo sempre será retornado</para>
        /// </summary>
        /// <typeparam name="T">Qualquer tipo herdado de <see cref="IModulo"/></typeparam>
        /// <returns><typeparamref name="T"/></returns>
        T Obter<T>() where T : IModulo;
        /// <summary>
        /// Obtem a instancia de <paramref name="modulo"/> caso exista
        /// <para>Caso exista mais de uma instancia do mesmo tipo, o primeiro modulo sempre será retornado</para>
        /// </summary>
        /// <param name="modulo">Qualquer tipo herdado de IModulo</param>
        /// <returns><see cref="IModulo"/></returns>
        IModulo Obter(Type modulo);
        /// <summary>
        /// Obtem a instancia do modulo pelo id
        /// </summary>
        /// <param name="id">Identificação unica do modulo </param>
        /// <returns><see cref="IModulo"/></returns>
        IModulo Obter(string id);

        /// <summary>
        /// Verifica se a instancia do modulo existe no genrenciador
        /// </summary>
        /// <param name="modulo">A instancia do modulo</param>
        /// <returns><see cref="bool"/></returns>
        bool Existe(IModulo modulo);
        /// <summary>
        /// Verifica se existe alguma instancia do tipo no gerenciador
        /// </summary>
        /// <param name="modulo">Tipo da instancia do modulo a ser verificado</param>
        /// <returns><see cref="bool"/></returns>
        bool Existe(Type modulo);
        /// <summary>
        /// Verifica se existe alguma instancia com o id no gerenciador
        /// </summary>
        /// <param name="id">Identificação unica do modulo</param>
        /// <returns><see cref="bool"/></returns>
        bool Existe(string id);

        /// <summary>
        /// Realiza uma reciclagem do modulo 
        /// </summary>
        /// <typeparam name="T">Qualquer tipo herdado de <see cref="IModulo"/></typeparam>
        /// <param name="modulo"></param>
        /// <returns><typeparamref name="T"/></returns>
        T Reciclar<T>(T modulo) where T : IModulo;
        /// <summary>
        /// Realiza uma reciclagem do modulo 
        /// </summary>
        /// <param name="id">Identificação unica do modulo</param>
        /// <returns><see cref="bool"/></returns>
        IModulo Reciclar(string id);

        /// <summary>
        /// Lista todos os modulos
        /// </summary>
        /// <returns><see cref="IEnumerable{IModulo}"/></returns>
        IEnumerable<IModulo> Listar();

        /// <summary>
        /// Mantem o gerenciador vivo durante o uso da aplicação
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        Task ManterVivoAsync();


    }
}
