﻿using System;
using System.Collections.Generic;
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
        public static IEnumerable<Type> ObterTipos(this string nomeTipo)
        {
            foreach (Assembly item in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type typeAssembly in item.GetTypes())
                {
                    if (typeAssembly.Name.Equals(nomeTipo, StringComparison.CurrentCultureIgnoreCase))
                    {
                        yield return typeAssembly;
                    }

                    if (typeAssembly.FullName.Equals(nomeTipo, StringComparison.CurrentCultureIgnoreCase))
                    {
                        yield return typeAssembly;
                    }
                }

            }

        }

    }
}