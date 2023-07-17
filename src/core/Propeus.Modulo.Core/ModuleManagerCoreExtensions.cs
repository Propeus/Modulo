using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Core.Modules;

namespace Propeus.Modulo.Core
{
    public static class ModuleManagerCoreExtensions
    {
        /// <summary>
        /// Cria uma nova instancia do gereciador com alguns modulos adicionais
        /// </summary>
        /// <returns></returns>
        public static IModuleManager CreateModuleManagerDefault()
        {
            var gen = new ModuleManagerCore();
            var taskJobModule = gen.CreateModule<TaskJobModule>();
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
            var gen = new ModuleManagerCore();
            return gen;
        }
    }
}
