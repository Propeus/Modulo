using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Propeus.Modulo.Util.Vetores
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
            if (bytes is null || !bytes.Any())
            {
                throw new ArgumentNullException(nameof(bytes));
            }
            byte[] md5Hash = MD5.HashData(bytes);
            IEnumerable<string> md5String = md5Hash.Select(c => c.ToString("x2"));

            return string.Concat(md5String);
        }

      
    }
}