namespace Propeus.Module.Abstract.Interfaces
{
    /// <summary>
    /// Informa detalhes sobre o modulo instanciado
    /// </summary>
    public interface IModuleInformation : IBaseModel
    {
        /// <summary>
        /// Informa se o modulo foi coletado pelo <see cref="GC"/>
        /// </summary>
        /// <remarks>
        /// Esta propriedade só será verdadeira se o <see cref="GC"/> tiver coletado o objeto
        /// </remarks>
        bool IsCollected { get; }
        /// <summary>
        /// Informa se o modulo foi eliminado da aplicação
        /// </summary>
        /// <remarks>
        /// Esta propriedade só será verdadeira se o modulo foi coletado OU se sua instancia indica o modulo esta com o estado <see cref="State.Off"/>
        /// </remarks>
        bool IsDeleted { get; }
        /// <summary>
        /// Indica se o modulo foi definido para se manter vivo
        /// </summary>
        bool IsKeepAlive { get; }

        /// <summary>
        /// Informa o id gerado para o modulo instanciado
        /// </summary>
        string IdModule { get; }
        /// <summary>
        /// Informa se o modulo é instancia unica
        /// </summary>
        bool IsSingleInstance { get; }
        /// <summary>
        /// Instancia do modulo
        /// </summary>
        IModule Module { get; }
        /// <summary>
        /// ModuleType do modulo
        /// </summary>
        Type ModuleType { get; }
        /// <summary>
        /// Referencia fraca da instancia do modulo
        /// </summary>
        WeakReference WeakReference { get; }

        /// <summary>
        /// Lista de modulos temporarios
        /// </summary>
        /// <remarks>
        /// Esta lista é utilizado para armazenar informações de modulos de instancia unica que necessita ser injetado em novos modulos com novos contratos.
        /// </remarks>
        IList<IModuleInformation> ModulesTemporary { get; }

        /// <summary>
        /// Define se o modulo atual deve ser mantido vivo ou nao
        /// </summary>
        /// <param name="keepAlive">Define se o modulo deve ser mantido vivo</param>
        void KeepAliveModule(bool keepAlive);
    }
}
