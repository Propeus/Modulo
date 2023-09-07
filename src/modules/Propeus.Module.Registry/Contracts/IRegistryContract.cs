using Propeus.Module.Abstract.Interfaces;

namespace Propeus.Module.Registry.Contracts
{
    public interface IRegistryContract : IModule
    {
        /// <summary>
        /// Numero de modulos inicialzados
        /// </summary>
        int InitializedModules { get; }

        /// <summary>
        /// Verifica se existe o modulo pelo id informado
        /// </summary>
        /// <param name="IdModule">Id do modulo</param>
        /// <returns>Verdadeiro caso o modulo exista, caso contrario falso</returns>
        bool ExistsModule(string IdModule);
        /// <summary>
        /// Obtem todos os modulos registrados e seus dados
        /// </summary>
        /// <returns></returns>
        IEnumerable<IModuleInfo> GetAllModulesInformation();
        /// <summary>
        /// Obtem um modulo pelo id
        /// </summary>
        /// <param name="IdModule">Id do modulo</param>
        /// <returns>Retorna as informações dos modulos</returns>
        IModuleInfo GetModuleInformation(string IdModule);
        /// <summary>
        /// Adiciona um novo modulo no registry
        /// </summary>
        /// <param name="module">Instancia do modulo</param>
        /// <returns></returns>
        IModuleInfo RegisterModule(IModule module);
        /// <summary>
        /// Remove um modulo do registry
        /// </summary>
        /// <param name="idModule">Id do modulo a ser removido</param>
        void UnregisterModule(string idModule);
    }
}
