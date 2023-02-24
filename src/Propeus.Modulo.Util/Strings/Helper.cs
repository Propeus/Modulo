using System;
using System.Reflection;

using static Propeus.Modulo.Compartilhado.Constantes;

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
            if (obj.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(obj), ARGUMENTO_NULO_OU_VAZIO);
            }

            byte[] arr = new byte[obj.Length];

            for (int i = 0; i < obj.Length; i++)
            {
                arr[i] = (byte)obj[i];
            }

            return arr;
        }

        /// <summary>
        /// Verifica se a string esta vazia ou nula
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// Obtem o tipo pelo nome 
        /// </summary>
        /// <param name="nomeTipo">Nome do tipo</param>
        /// <returns>Retorna o <see cref="Type"/></returns>
        public static Type ObterTipo(this string nomeTipo)
        {
           return Assembly.GetExecutingAssembly().GetType(nomeTipo);
        }

    }
}