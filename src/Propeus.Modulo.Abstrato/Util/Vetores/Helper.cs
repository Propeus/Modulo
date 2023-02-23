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
    }
}