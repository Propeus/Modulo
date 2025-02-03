namespace Propeus.Module.Abstract.Attributes
{

    /**
     * MINI MANUAL
     * 1 - A propriedade "AutoStartable" indica ao gerenciador que o modulo deve ser inicializado sem a necessidade da ação de um usuário/sistema.
     * 1.1 - Caso a propriedade "AutoStartable" seja "true", implicitamente a propriedade "KeepAlive" se torna true também.
     * 2 - A propriedade "AutoUpdate" permite que um modulo seja reciclado quando houver mudança em seu arquivo.
     * 3 - A propriedade "Singleton" indica se o modulo pode ter mais de uma instancia ou não
     * 4 - A propriedade "KeepAlive" indica se o gerenciador deve manter uma referencia forte com o modulo atual
     * 4.1 - Caso seja definido como false, o G.C. do .NET poderá remover quando for necessário.
     * 4.2 - Esta propriedade é ativado implicitamente quando a propriedade "AutoStartable" é ativado, pois não foi solicitado pelo o usuário/sistema diretamente.
     * 5 - A propriedade "Description" exibe a descrição do modulo quando for chamado pelo método ".ToString()"
     * 
     * **/

    /// <summary>
    /// Identificador de extremidade de um modulo
    /// </summary>
    /// <remarks>
    /// Serve para marcar uma classe como um modulo funcional
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ModuleAttribute : Attribute
    {

        /// <summary>
        /// Define uma descrição sobre o modulo
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// Informa que o tipo deve ser de instancia unica ou não
        /// </summary>
        /// <value>Por padrão é <see langword="false"/></value>
        public bool Singleton { get; set; } = false;
        /// <summary>
        /// Indica se o modulo deve ser mantido vivo ou não em caso de ausência de referencia
        /// </summary>
        /// <remarks>
        /// Caso o modulo não seja uma job escalavel, evite utilizar esta funcionalidade sem atribuir o valor <see langword="true"/> na propriedade <see cref="Singleton"/>
        /// </remarks>
        /// <example>
        /// <code>
        /// [ModuleAttribute(Description="Um modulo executando como uma tarefa em segundo plano",Keepalive=true,Singleton=true)]
        /// public interface IModuleWorkerServiceSingleton : IModule
        /// {
        /// 
        /// }
        /// </code>
        /// <code>
        /// [ModuleAttribute(Description="Um modulo executando como uma tarefa em segundo plano",Keepalive=true,Singleton=true)]
        /// public interface IModuleWorkerService : IModule
        /// {
        /// 
        /// }
        /// </code>
        /// </example>
        /// <value>Por padrão é <see langword="false"/></value>
        public bool KeepAlive { get; set; } = false;
        /// <summary>
        /// Indica se o modulo é auto inicializavel
        /// </summary>
        /// <remarks>
        /// Por padrão o valor é <see langword="false"/>
        /// </remarks>
        /// <value>
        /// <see langword="true"/> para caso seja auto inicializavel, caso contrario <see langword="false"/>
        /// </value>
        public bool AutoStartable { get; set; } = false;
        /// <summary>
        /// Indica se deve ser recriado todas as instancias do modulo, caso a DLL seja alterada
        /// </summary>
        /// <remarks>
        /// Dependendo do gerenciador, a instancia do objeto se mantem e seu apontamento é trocado.
        /// </remarks>
        /// <value>
        /// <see langword="true"/> para caso seja auto atualizavel, caso contrario <see langword="false"/>
        /// </value>
        public bool AutoUpdate { get; set; } = false;
    }
}
