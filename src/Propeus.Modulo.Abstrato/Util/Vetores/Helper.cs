using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Cryptography;

using static Propeus.Modulo.Abstrato.Constantes;

namespace Propeus.Modulo.Abstrato.Util
{
    /// <summary>
    /// Classe de ajuda para vetores
    /// </summary>
    public static partial class Helper
    {
        /// <summary>
        /// Deserializa o array de bytes ao estado original.
        /// </summary>
        /// <typeparam name="TObjeto"></typeparam>
        /// <param name="obj">Array de bytes</param>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        /// <exception cref="SerializationException">Objeto não serializavel</exception>
        public static TObjeto Deserializar<TObjeto>(this byte[] obj)
        {
            return obj.IsNull() ? throw new ArgumentNullException(nameof(obj), ARGUMENTO_NULO) : (TObjeto)obj.Deserializar(typeof(TObjeto));
        }

        /// <summary>
        /// Deserializa um array de <see cref="byte"/>
        /// </summary>
        /// <param name="obj">Array de <see cref="byte"/> a serem convertidos</param>
        /// <param name="type">Tipo a ser convertido</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        /// <exception cref="SerializationException">Objeto não serializavel</exception>
        public static object Deserializar(this byte[] obj, Type type)
        {
            int size = Marshal.SizeOf(obj);
            nint ptr = Marshal.AllocHGlobal(size);
            Marshal.Copy(obj, 0, ptr, size);
            object? objeto = Marshal.PtrToStructure(ptr, type);
            Marshal.FreeHGlobal(ptr);
            return objeto;
        }

        /// <summary>
        /// Obtem o hash de um array de bytes
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string Hash(this byte[] bytes)
        {
            if (bytes.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(bytes), ARGUMENTO_NULO_OU_VAZIO);
            }
            byte[] md5Hash = MD5.HashData(bytes);
            IEnumerable<string> md5String = md5Hash.Select(c => c.ToString("x2"));

            return string.Concat(md5String);
        }

        /// <summary>
        /// Converte uma string em um array de bytes
        /// </summary>
        /// <param name="obj">Uma string qualquer</param>
        /// <returns></returns>
        public static byte[] ToBytes(this string obj)
        {
            return System.Text.Encoding.Default.GetBytes(obj);
        }


    }
}