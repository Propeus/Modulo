using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace Propeus.Modulo.Util
{
    /// <summary>
    /// Classe de ajuda para string
    /// </summary>
    public static partial class Helper
    {
        /// <summary>
        /// Converte uma <see cref="string"/> em um array de bytes
        /// </summary>
        /// <param name="obj">Qualquer objeto do tipo <see cref="string"/></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        public static byte[] ToArrayByte(this string obj)
        {
            if (string.IsNullOrEmpty(obj))
            {
                throw new ArgumentNullException(nameof(obj));
            }

            byte[] arr = new byte[obj.Length];

            for (int i = 0; i < obj.Length; i++)
            {
                arr[i] = (byte)obj[i];
            }

            return arr;
        }

        /// <summary>
        /// Obtem o tipo pelo nome 
        /// </summary>
        /// <param name="nomeTipo">Nome do tipo</param>
        /// <returns>Retorna o <see cref="Type"/></returns>
        public static Type ObterTipo(this string nomeTipo)
        {
            //TODO:Otimizar este metodo, muito pessado em caso de muitos modulos
            foreach (var item in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type type = item.GetTypes().SingleOrDefault(x => x.Name.Equals(nomeTipo, StringComparison.CurrentCultureIgnoreCase));
                if (type != null)
                {
                    return type;
                }
            }

            return null;
        }

        /// <summary>
        /// Obtem o tipo pelo nome 
        /// </summary>
        /// <param name="nomeTipo">Nome do tipo</param>
        /// <returns>Retorna o <see cref="Type"/></returns>
        public static IEnumerable<Type> ObterTipos(this string nomeTipo)
        {
            foreach (var item in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var typeAssembly in item.GetTypes())
                {
                    if (typeAssembly.Name.Equals(nomeTipo, StringComparison.CurrentCultureIgnoreCase))
                        yield return typeAssembly;

                    if (typeAssembly.FullName.Equals(nomeTipo, StringComparison.CurrentCultureIgnoreCase))
                        yield return typeAssembly;
                }

            }

        }

    }
}