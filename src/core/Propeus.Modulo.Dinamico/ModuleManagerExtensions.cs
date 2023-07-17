using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Dinamico.Modules;

namespace Propeus.Modulo.Dinamico
{
    public static class ModuleManagerExtensions
    {
        //TODO: Testar o core e o dinamico

        /// <summary>
        /// Cria uma nova instancia do gereciador com alguns modulos adicionais
        /// </summary>
        /// <returns></returns>
        public static IModuleManager CreateModuleManagerDefault(IModuleManager moduleManagerCore)
        {
            var gen = new ModuleManager(moduleManagerCore);
            gen.KeepAliveModuleAsync(gen.CreateModule<QueueMessageModule>());
            gen.KeepAliveModuleAsync(gen.CreateModule<ModuleWatcherModule>());
            return gen;
        }
        /// <summary>
        /// Cria uma nova instancia do gereciador 
        /// </summary>
        /// <returns></returns>
        public static IModuleManager CreateModuleManager(IModuleManager moduleManagerCore)
        {
            var gen = new ModuleManager(moduleManagerCore);
            return gen;
        }
    }
}
