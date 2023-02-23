using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using static Propeus.Modulo.Abstrato.Constantes;

namespace Propeus.Modulo.Abstrato.Util
{
    /// <summary>
    /// Classe de ajuda para <see cref="Attribute"/>
    /// </summary>
    public static partial class Helper
    {
        

        /// <summary>
        /// Obtem um atributo a partir de um tipo especifico.
        /// </summary>
        /// <typeparam name="T">Tipo do atributo a ser procurado</typeparam>
        /// <param name="obj">Qualquer objeto do tipo <see cref="Type"/></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Argumento <paramref name="obj"/> nulo</exception>
        /// <exception cref="InvalidOperationException">Atributo <typeparamref name="T"/> não encontrado</exception>
        /// <exception cref="InvalidOperationException">Tipo do parâmetro <paramref name="obj"/> herdado da classe <see cref="Attribute"/></exception>
        public static T ObterAtributo<T>(this Type obj) where T : Attribute
        {
            if (obj is null)
            {
                throw new ArgumentException(ARGUMENTO_NULO, nameof(obj));
            }

            if (obj.Herdado<Attribute>())
            {
                throw new InvalidOperationException(PARAMETRO_ATRIBUTO_INVALIDO);
            }

            object result = obj.GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == typeof(T));

            return result is null
                ? throw new InvalidOperationException(string.Format(ATRIBUTO_NAO_ENCONTRADO, typeof(T).Name, nameof(obj)))
                : result.To<T>();
        }

        /// <summary>
        /// Obtém um atributo a partir de um objeto especifico.
        /// </summary>
        /// <typeparam name="T">Tipo do atributo a ser procurado</typeparam>
        /// <param name="obj">Qualquer objeto do tipo <see cref="object"/></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Argumento <paramref name="obj"/> nulo</exception>
        /// <exception cref="InvalidOperationException">Atributo <typeparamref name="T"/> não encontrado</exception>
        /// <exception cref="InvalidOperationException">Tipo do parâmetro <paramref name="obj"/> herdado da classe <see cref="Attribute"/></exception>
        public static T ObterAtributo<T>(this object obj) where T : Attribute
        {
            return obj is null
                ? throw new ArgumentException(ARGUMENTO_NULO, nameof(obj))
                : !obj.Is<Type>() ? obj.GetType().ObterAtributo<T>() : obj.As<Type>().ObterAtributo<T>();
        }

        /// <summary>
        /// Verifica se o <paramref name="obj"/> possui o atributo
        /// </summary>
        /// <typeparam name="T">Tipo do atributo a ser procurado</typeparam>
        /// <param name="obj">Qualquer objeto do tipo <see cref="Type"/></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Argumento <paramref name="obj"/> nulo</exception>
        public static bool PossuiAtributo<T>(this Type obj) where T : Attribute
        {
            return obj is null
                ? throw new ArgumentNullException(ARGUMENTO_NULO, nameof(obj))
                : obj.Herdado<Attribute>()
                ? throw new InvalidOperationException(PARAMETRO_ATRIBUTO_INVALIDO)
                : obj.GetCustomAttributes(true).Any(x => x.GetType() == typeof(T));
        }
    }
}