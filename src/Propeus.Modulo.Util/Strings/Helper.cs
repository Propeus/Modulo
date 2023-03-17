using System;
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
    }
}