using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Propeus.Modulo.Abstrato.Interfaces
{

    /// <summary>
    /// Modelo base para criação de gerenciadores
    /// </summary>
    public interface IModuleManager : IBaseModel
    {
        /// <summary>
        /// Retorna data e hora que o gerenciador iniciou
        /// </summary>
        DateTime StartDate { get; }
        /// <summary>
        /// Data e hora do ultimo evento realizado no gerenciador
        /// </summary>
        /// <remarks>
        /// Os eventos sao o CRUD (CreateModule, Reiniciar, Atualizar ou RemoveModule) do genreciador
        /// </remarks>
        DateTime LastUpdate { get; }

        /// <summary>
        /// Indica a quantidade de modulos inicializados pelo gerenciador
        /// </summary>
        int InitializedModules { get; }

        /// <summary>
        /// Cria uma nova instancia do moduleType <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Qualquer tipo herdado de <see cref="IModule"/></typeparam>
        /// <returns><typeparamref name="T"/></returns>
        T CreateModule<T>() where T : IModule;
        /// <summary>
        /// Cria uma nova instancia do moduleType usando o tipo do parametro <paramref name="moduleType"/>
        /// </summary>
        /// <param name="moduleType">ModuleType do moduleType</param>
        /// <returns><see cref="IModule"/></returns>
        IModule CreateModule(Type moduleType);
        /// <summary>
        /// Cria uma nova instancia do moduleType buscando o tipo pelo nome
        /// </summary>
        /// <param name="moduleName">ModuleName do moduleType</param>
        /// <returns><see cref="IModule"/></returns>
        IModule CreateModule(string moduleName);

        /// <summary>
        /// RemoveModule um moduleType pelo seu ID
        /// </summary>
        /// <param name="idModule">Identificação unica do moduleType </param>
        void RemoveModule(string idModule);
        /// <summary>
        /// RemoveModule o moduleType instanciado
        /// </summary>
        /// <typeparam name="T">Qualquer tipo herdado de <see cref="IModule"/></typeparam>
        /// <param name="moduleInstance">Qualquer moduleType herdado de <see cref="IModule"/></param>
        void RemoveModule<T>(T moduleInstance) where T : IModule;
        /// <summary>
        /// RemoveModule todos os modulos
        /// </summary>
        void RemoveAllModules();

        /// <summary>
        /// Obtem a instancia de <typeparamref name="T"/> caso exista
        /// <para>Caso exista mais de uma instancia do mesmo tipo, o primeiro moduleType sempre será retornado</para>
        /// </summary>
        /// <typeparam name="T">Qualquer tipo herdado de <see cref="IModule"/></typeparam>
        /// <returns><typeparamref name="T"/></returns>
        T GetModule<T>() where T : IModule;
        /// <summary>
        /// Obtem a instancia de <paramref name="moduleType"/> caso exista
        /// <para>Caso exista mais de uma instancia do mesmo tipo, o primeiro moduleType sempre será retornado</para>
        /// </summary>
        /// <param name="moduleType">Qualquer tipo herdado de IModule</param>
        /// <returns><see cref="IModule"/></returns>
        IModule GetModule(Type moduleType);
        /// <summary>
        /// Obtem a instancia do moduleType pelo idModule
        /// </summary>
        /// <param name="idModule">Identificação unica do moduleType </param>
        /// <returns><see cref="IModule"/></returns>
        IModule GetModule(string idModule);

        /// <summary>
        /// Verifica se a instancia do moduleType existe no genrenciador
        /// </summary>
        /// <param name="moduleInstance">A instancia do moduleType</param>
        /// <returns><see cref="bool"/></returns>
        bool ExistsModule(IModule moduleInstance);
        /// <summary>
        /// Verifica se existe alguma instancia do tipo no gerenciador
        /// </summary>
        /// <param name="moduleType">ModuleType da instancia do moduleType a ser verificado</param>
        /// <returns><see cref="bool"/></returns>
        bool ExistsModule(Type moduleType);
        /// <summary>
        /// Verifica se existe alguma instancia com o idModule no gerenciador
        /// </summary>
        /// <param name="idModule">Identificação unica do moduleType</param>
        /// <returns><see cref="bool"/></returns>
        bool ExistsModule(string idModule);

        /// <summary>
        /// Realiza uma reciclagem do moduleType 
        /// </summary>
        /// <typeparam name="T">Qualquer tipo herdado de <see cref="IModule"/></typeparam>
        /// <param name="moduleInstance"></param>
        /// <returns><typeparamref name="T"/></returns>
        T RecycleModule<T>(T moduleInstance) where T : IModule;
        /// <summary>
        /// Realiza uma reciclagem do moduleType 
        /// </summary>
        /// <param name="idModule">Identificação unica do moduleType</param>
        /// <returns><see cref="bool"/></returns>
        IModule RecycleModule(string idModule);

        /// <summary>
        /// Lista todos os modulos
        /// </summary>
        /// <returns><see cref="IEnumerable{IModulo}"/></returns>
        IEnumerable<IModule> ListAllModules();

        /// <summary>
        /// Mantem o gerenciador vivo durante o uso da aplicação
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        Task KeepAliveAsync();
        Task KeepAliveModuleAsync(IModule moduleInstance);
        


    }
}
