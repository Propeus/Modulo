namespace Propeus.Module.Abstract.Interfaces
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
        /// Os eventos sao o CRUD (Criar, Reiniciar, Atualizar ou Remover) do gerenciador
        /// </remarks>
        DateTime LastUpdate { get; }

        /// <summary>
        /// Indica a quantidade de modulos inicializados pelo gerenciador
        /// </summary>
        /// <value>Quantidade de módulos instanciados</value>
        int InitializedModules { get; }



        /// <summary>
        /// Cria uma nova instancia do moduleType <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Qualquer tipo herdado de <see cref="IModule"/></typeparam>
        /// <param name="args">Qualquer argumento necessário para o moduleType </param>
        /// <returns></returns>
        T CreateModule<T>(object[]? args = null) where T : IModule;
        /// <summary>
        /// Cria uma nova instancia do moduleType usando o tipo do parametro <paramref name="moduleType"/>
        /// </summary>
        /// <param name="moduleType">ModuleType do moduleType</param>
        /// <param name="args">Qualquer argumento necessário para o moduleType </param>
        /// <returns><see cref="IModule"/></returns>
        IModule CreateModule(Type moduleType, object[]? args = null);
        /// <summary>
        /// Cria uma nova instancia do moduleType buscando o tipo pelo nome
        /// </summary>
        /// <param name="moduleName">ModuleName do moduleType</param>
        /// <param name="args">Qualquer argumento necessário para o moduleType </param>
        /// <returns><see cref="IModule"/></returns>
        IModule CreateModule(string moduleName, object[]? args = null);

        /// <summary>
        /// Remove um modulo pelo seu ID
        /// </summary>
        /// <param name="idModule">Identificação unica do modulo</param>
        void RemoveModule(string idModule);
        /// <summary>
        /// Remove um modulo pelo objeto
        /// </summary>
        /// <typeparam name="T">Qualquer tipo herdado de <see cref="IModule"/></typeparam>
        /// <param name="moduleInstance">Qualquer instancia de modulo herdado de <see cref="IModule"/></param>
        void RemoveModule<T>(T moduleInstance) where T : IModule;

        /// <summary>
        /// Obtem a instancia do modulo pelo tipo
        /// </summary>
        /// Caso exista mais de uma instancia do mesmo tipo, o primeiro modulo sempre será retornado
        /// <remarks>
        /// </remarks>
        /// <typeparam name="T">Qualquer tipo herdado de <see cref="IModule"/></typeparam>
        /// <returns>A instancia do modulo</returns>
        T GetModule<T>() where T : IModule;
        /// <summary>
        /// Obtem a instancia do modulo pelo tipo
        /// </summary>
        /// <remarks>
        /// Caso exista mais de uma instancia do mesmo tipo, o primeiro modulo sempre será retornado
        /// </remarks>
        /// <param name="moduleType">Qualquer tipo herdado de <see cref="IModule"/></param>
        /// <returns>A instancia do modulo</returns>
        IModule GetModule(Type moduleType);
        /// <summary>
        /// Obtem a instancia do modulo pelo id
        /// </summary>
        /// <param name="idModule">Identificação unica do modulo</param>
        /// <returns>A instancia do modulo</returns>
        IModule GetModule(string idModule);

        /// <summary>
        /// Verifica se a instancia do modulo existe no genrenciador
        /// </summary>
        /// <param name="moduleInstance">A instancia do modulo</param>
        /// <returns>Retorna <see langword="true"/> caso exista, caso contrario, <see langword="false"/></returns>
        bool ExistsModule(IModule moduleInstance);
        /// <summary>
        /// Verifica se existe alguma instancia do tipo no gerenciador
        /// </summary>
        /// <param name="moduleType">Tipo do modulo a ser verificado</param>
        /// <returns>Retorna <see langword="true"/> caso exista, caso contrario, <see langword="false"/></returns>
        bool ExistsModule(Type moduleType);
        /// <summary>
        /// Verifica se existe alguma instancia com o id informando no gerenciador
        /// </summary>
        /// <param name="idModule">Identificação unica do modulo</param>
        /// <returns>Retorna <see langword="true"/> caso exista, caso contrario, <see langword="false"/></returns>
        bool ExistsModule(string idModule);

        /// <summary>
        /// Destroi o modulo antigo e retorna um novo do mesmo tipo
        /// </summary>
        /// <typeparam name="T">Qualquer tipo herdado de <see cref="IModule"/></typeparam>
        /// <param name="moduleInstance">Instancia do modulo a ser reciclado</param>
        /// <returns>Nova instancia do modulo</returns>
        T RecycleModule<T>(T moduleInstance) where T : IModule;
        /// <summary>
        /// Destroi o modulo antigo e retorna um novo do mesmo tipo
        /// </summary>
        /// <param name="idModule">Identificação unica do modulo a ser reciclado</param>
        /// <returns>Nova instancia do modulo</returns>
        IModule RecycleModule(string idModule);

        /// <summary>
        /// Lista todos os modulos disponiveis
        /// </summary>
        /// <returns>Lista de modulos ativos</returns>
        IEnumerable<IModule> ListAllModules();


        /// <summary>
        /// Mantem um modulo vivo mesmo não possuindo referencia alguma
        /// </summary>
        /// <param name="moduleInstance">Modulo a ser mantido vivo</param>
        /// <returns>Retorna uma tarefa</returns>
        void KeepAliveModule(IModule moduleInstance);



    }

    /// <summary>
    /// Modelo para monitorar chamadas de um gerenciador
    /// </summary>
    public interface IModuleManagerHealthCheck : IBaseModel
    {
        /// <summary>
        /// Eventos para monitoramento do Gerenciador
        /// </summary>
        event Action<ModuleManagerHealthCheckEvent>? ModuleManagerEvent;
        /// <summary>
        /// Eventos para monitoramento de modulos
        /// </summary>
        event Action? ModuleRemoved;
        /// <summary>
        /// Eventos para monitoramento de modulos
        /// </summary>
        event Action<IModule>? ModuleCreated, ModuleRecycled;
    }

    /// <summary>
    /// Tipos de eventos do Gerenciador
    /// </summary>
    public enum ModuleManagerHealthCheckEvent
    {
        /// <summary>
        /// Qualquer outro evento nao listado
        /// </summary>
        Any = 0,
        /// <summary>
        /// Modulo criado
        /// </summary>
        Created,
        /// <summary>
        /// Modulo removido
        /// </summary>
        Removed,
        /// <summary>
        /// Modulo reciclado
        /// </summary>
        Recycled,
        /// <summary>
        /// Modulo listado
        /// </summary>
        Listed,
        /// <summary>
        /// Modulo obtido
        /// </summary>
        Obtained,
        /// <summary>
        /// Modulo existente
        /// </summary>
        Existed


    }
}
