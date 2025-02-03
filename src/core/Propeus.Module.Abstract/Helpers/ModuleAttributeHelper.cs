using System.Reflection;

using Propeus.Module.Abstract.Attributes;
using Propeus.Module.Abstract.Interfaces;

namespace Propeus.Module.Abstract.Helpers
{
    /// <summary>
    /// Classe estática para ajuda
    /// </summary>
    public static partial class Helper
    {

        /// <summary>
        /// Obtem o atributo <see cref="ModuleAttribute"/> de um tipo de modulo
        /// </summary>
        /// <param name="typeModule">Qualquer modulo do tipo <see cref="IModule"/></param>
        /// <returns>Retorna o atributo ou <see langword="null"/></returns>
        /// <exception cref="ArgumentException">Argumeto <paramref name="typeModule"/> vazio ou nulo</exception>

        public static ModuleAttribute? GetModuleAttribute(this Type typeModule)
        {
            return typeModule.GetCustomAttribute<ModuleAttribute>();
        }



        /// <summary>
        /// Obtem o atributo <see cref="ModuleContractAttribute"/> de um tipo de modulo
        /// </summary>
        /// <param name="typeModule">Qualquer modulo do tipo <see cref="IModule"/></param>
        /// <returns>Retorna o atributo ou <see langword="null"/></returns>
        /// <exception cref="ArgumentException">Argumeto typeModule vazio ou nulo</exception>
        public static ModuleContractAttribute? GetAttributeContractModule(this Type typeModule)
        {
            return typeModule.GetCustomAttribute<ModuleContractAttribute>();
        }

    }
}
