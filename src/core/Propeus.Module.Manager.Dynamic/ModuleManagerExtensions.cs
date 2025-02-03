using Propeus.Module.Abstract.Interfaces;
using Propeus.Module.WatcherDynamicModule.Contracts;

namespace Propeus.Module.Manager.Dynamic
{
    /// <summary>
    /// Classe de extensão para o gerenciador dinâmico
    /// </summary>
    public static partial class ModuleManagerExtensions
    {

        /// <summary>
        /// Cria uma nova instancia do gereciador 
        /// </summary>
        /// <returns></returns>
        public static IModuleManager CreateModuleManager(this IModuleManager moduleManagerCore)
        {
            ModuleManager gen = moduleManagerCore.CreateModule<ModuleManager>();
            var module = moduleManagerCore.CreateModule<IModuleWatcherContract>();
            gen.KeepAliveModule(module);
            return gen;
        }
    }
}
