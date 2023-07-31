using System;

using Propeus.Modulo.Abstrato.Interfaces;

namespace Propeus.Modulo.Core
{
    /// <summary>
    /// Metodos de extensao para criar gerenciadores
    /// </summary>
    public static partial class ModuleManagerExtensions
    {
        /// <summary>
        /// Cria uma nova instancia do gereciador 
        /// </summary>
        /// <returns></returns>
        public static IModuleManager CreateModuleManager()
        {
            ModuleManager gen = new ModuleManager();
            return gen;
        }
        /// <summary>
        /// Cria ou obtem um modulo existente
        /// </summary>
        /// <typeparam name="TModule">Tipo do modulo</typeparam>
        /// <param name="moduleManager">Gerenciador de modulos</param>
        /// <returns></returns>
        public static TModule CreateOrGetModule<TModule>(this IModuleManager moduleManager) where TModule : IModule
        {
            if (!moduleManager.ExistsModule(typeof(TModule)))
            {
                return moduleManager.CreateModule<TModule>();
            }
            else
            {
                return moduleManager.GetModule<TModule>();
            }
        }




    }
}
