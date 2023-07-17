using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Core.Modules;

namespace Propeus.Modulo.Core
{
    /// <summary>
    /// Metodos de extensao para criar gerenciadores
    /// </summary>
    public static class ModuleManagerCoreExtensions
    {
        /// <summary>
        /// Cria uma nova instancia do gereciador com alguns modulos adicionais
        /// </summary>
        /// <returns></returns>
        public static IModuleManager CreateModuleManagerDefault()
        {
            ModuleManagerCore gen = new ModuleManagerCore();
            TaskJobModule taskJobModule = gen.CreateModule<TaskJobModule>();
            gen.KeepAliveModuleAsync(taskJobModule).Wait();
            gen.RegisterTaskJobs(taskJobModule);
            return gen;
        }
        /// <summary>
        /// Cria uma nova instancia do gereciador 
        /// </summary>
        /// <returns></returns>
        public static IModuleManager CreateModuleManager()
        {
            ModuleManagerCore gen = new ModuleManagerCore();
            return gen;
        }
    }
}
