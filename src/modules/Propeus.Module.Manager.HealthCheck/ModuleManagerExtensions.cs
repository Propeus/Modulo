using System;

using Propeus.Module.Abstract.Interfaces;

namespace Propeus.Module.Manager.HealthCheck
{
    /// <summary>
    /// Classe de extensão para o gerenciador HealthCheck
    /// </summary>
    public static partial class ModuleManagerExtensions
    {

        /// <summary>
        /// Cria uma nova instancia do gereciador HealthCheck
        /// </summary>
        /// <returns></returns>
        public static IModuleManager CreateModuleManagerHealthCheck(this IModuleManager moduleManagerCore)
        {
            var gen = moduleManagerCore.CreateModule<ModuleManagerHealthCheck>();
            return gen;
        }
    }
}
