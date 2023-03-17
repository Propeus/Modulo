using System;
using System.Reflection;

using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Util;

namespace Propeus.Modulo.Abstrato.Helpers
{
    /// <summary>
    /// Classe estatica para ajuda
    /// </summary>
    public static partial class Helper
    {

        /// <summary>
        /// Obtem o atributo <see cref="ModuloAttribute"/> de um objeto
        /// </summary>
        /// <param name="objeto">Qualquer objeto do tipo <see cref="Type"/></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Argumeto obj vazio ou nulo</exception>
        /// <exception cref="InvalidOperationException"><see cref="ModuloAttribute"/> não encontrado</exception>
        public static ModuloAttribute ObterModuloAtributo(this Type objeto)
        {
           return objeto.GetCustomAttribute<ModuloAttribute>();
        }



        /// <summary>
        /// Obtem o atributo <see cref="ModuloContratoAttribute"/> de um objeto
        /// </summary>
        /// <param name="objeto">Qualquer objeto do tipo <see cref="Type"/></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Argumeto obj vazio ou nulo</exception>
        /// <exception cref="InvalidOperationException"><see cref="ModuloContratoAttribute"/> não encontrado</exception>
        public static ModuloContratoAttribute ObterModuloContratoAtributo(this Type objeto)
        {
            return objeto.GetCustomAttribute<ModuloContratoAttribute>();
        }

    }
}
