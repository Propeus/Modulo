using Propeus.Module.Abstract.Interfaces;
using Propeus.Module.Watcher.Modules;

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
            Action<Type>? arg1 = gen.ModuleManager_OnLoadModule, arg2 = gen.ModuleManager_OnReloadModule;
            var module = gen.CreateModule<ModuleWatcherModule>(new object[] { arg1, arg2 });
            gen.KeepAliveModule(module);
            return gen;
        }
    }
}
