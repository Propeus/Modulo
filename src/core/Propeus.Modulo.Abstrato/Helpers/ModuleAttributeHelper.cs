using System;
using System.Reflection;

using Propeus.Module.Abstract.Attributes;

namespace Propeus.Module.Abstract.Helpers
{
    /// <summary>
    /// Classe estatica para ajuda
    /// </summary>
    public static partial class Helper
    {

        /// <summary>
        /// Obtem o atributo <see cref="ModuleAttribute"/> de um typeModule
        /// </summary>
        /// <param name="typeModule">Qualquer typeModule do tipo <see cref="Type"/></param>
        /// <returns>Retorna o atributo ou <see langword="null"/></returns>
        /// <exception cref="ArgumentException">Argumeto typeModule vazio ou nulo</exception>
        /// <exception cref="InvalidOperationException"><see cref="ModuleAttribute"/> não encontrado</exception>
        public static ModuleAttribute GetModuleAttribute(this Type typeModule)
        {
            return typeModule.GetCustomAttribute<ModuleAttribute>();
        }



        /// <summary>
        /// Obtem o atributo <see cref="ModuleContractAttribute"/> de um typeModule
        /// </summary>
        /// <param name="typeModule">Qualquer typeModule do tipo <see cref="Type"/></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Argumeto typeModule vazio ou nulo</exception>
        /// <exception cref="InvalidOperationException"><see cref="ModuleContractAttribute"/> não encontrado</exception>
        public static ModuleContractAttribute GetAttributeContractModule(this Type typeModule)
        {
            return typeModule.GetCustomAttribute<ModuleContractAttribute>();
        }

    }
}
