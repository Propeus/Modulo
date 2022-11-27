using Propeus.Modulo.Util.Listas;
using Propeus.Modulo.Util.Objetos;
using Propeus.Modulo.Util.Properties;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;

namespace Propeus.Modulo.Util.Vetores
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
            if (obj.IsNull())
            {
                throw new ArgumentNullException(nameof(obj), Resources.ARGUMENTO_NULO);
            }

            return (TObjeto)obj.Deserializar(typeof(TObjeto));
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
            MemoryStream memorystreamd = new MemoryStream(obj);
            BinaryFormatter bfd = new BinaryFormatter();
            object objeto = Convert.ChangeType(bfd.Deserialize(memorystreamd), type);
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
                throw new ArgumentNullException(nameof(bytes), Resources.ARGUMENTO_NULO_OU_VAZIO);
            }

            using MD5 md5 = new MD5CryptoServiceProvider();
            StringBuilder stringBuilder = new StringBuilder();
            md5.ComputeHash(bytes).Select(c => stringBuilder.Append(c.ToString("x2")));
            return stringBuilder.ToString();
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