using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Dinamico.Contracts;
using Propeus.Modulo.Dinamico.Modules;

namespace Propeus.Modulo.Dinamico
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
        public static IModuleManager CreateModuleManager(IModuleManager moduleManagerCore)
        {
            ModuleManager gen = moduleManagerCore.CreateModule<ModuleManager>();
            gen.KeepAliveModuleAsync(gen.CreateModule<ModuleWatcherModule>()).Wait();
            return gen;
        }
    }
}
