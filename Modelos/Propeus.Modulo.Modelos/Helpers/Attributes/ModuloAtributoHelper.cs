using Propeus.Modulo.Modelos.Atributos;
using Propeus.Modulo.Util.Atributos;
using System;
using System.Linq;
using System.Reflection;

namespace Propeus.Modulo.Modelos.Helpers.Attributes
{
    /// <summary>
    /// Classe estatica para ajuda
    /// </summary>
    public static partial class Helper
    {
        /// <summary>
        /// Obtem o atributo <see cref="ModuloAttribute"/> de um objeto
        /// </summary>
        /// <param name="objeto">Qualquer objeto do tipo <see cref="object"/></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Argumeto obj vazio ou nulo</exception>
        /// <exception cref="InvalidOperationException"><see cref="ModuloAttribute"/> não encontrado</exception>
        public static ModuloAttribute ObterModuloAtributo(this object objeto)
        {
            return objeto.ObterAtributo<ModuloAttribute>();
        }
        /// <summary>
        /// Obtem o atributo <see cref="ModuloAttribute"/> de um objeto
        /// </summary>
        /// <param name="objeto">Qualquer objeto do tipo <see cref="Type"/></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Argumeto obj vazio ou nulo</exception>
        /// <exception cref="InvalidOperationException"><see cref="ModuloAttribute"/> não encontrado</exception>
        public static ModuloAttribute ObterModuloAtributo(this Type objeto)
        {
            return objeto.ObterAtributo<ModuloAttribute>();
        }

        /// <summary>
        /// Obtem o atributo <see cref="ModuloContratoAttribute"/> de um objeto
        /// </summary>
        /// <param name="objeto">Qualquer objeto do tipo <see cref="object"/></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Argumeto obj vazio ou nulo</exception>
        /// <exception cref="InvalidOperationException"><see cref="ModuloContratoAttribute"/> não encontrado</exception>
        public static ModuloContratoAttribute ObterModuloContratoAtributo(this object objeto)
        {
            return objeto.ObterAtributo<ModuloContratoAttribute>();
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
            return objeto.ObterAtributo<ModuloContratoAttribute>();
        }
        /// <summary>
        /// Obtem o <see cref="Type"/> do modulo utilizando o atributo <see cref="ModuloContratoAttribute"/>
        /// </summary>
        /// <param name="objeto">Tipo do objeto com o atributo <see cref="ModuloContratoAttribute"/></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Argumeto obj vazio ou nulo</exception>
        /// <exception cref="InvalidOperationException">Contrato</exception>
        public static Type ObterTipoPorModuloContratoAtributo(this Type objeto)
        {
            string nome = objeto.ObterAtributo<ModuloContratoAttribute>().Nome;
            return Assembly.GetAssembly(objeto).GetTypes().FirstOrDefault(m => m.FullName == nome || m.Name == nome);

        }

    }
}
