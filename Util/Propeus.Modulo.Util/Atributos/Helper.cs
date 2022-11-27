using Propeus.Modulo.Util.Listas;
using Propeus.Modulo.Util.Objetos;
using Propeus.Modulo.Util.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Propeus.Modulo.Util.Atributos
{
    /// <summary>
    /// Classe de ajuda para <see cref="Attribute"/>
    /// </summary>
    public static partial class Helper
    {
        /// <summary>
        /// Obtém um atributo a partir de uma lista de atributos
        /// </summary>
        /// <typeparam name="T">Tipo do atributo a ser procurado</typeparam>
        /// <param name="obj">Lista de atributos</param>
        /// <returns>T</returns>
        /// <exception cref="ArgumentException">Argumento <paramref name="obj"/> vazio ou nulo</exception>
        /// <exception cref="InvalidOperationException">Atributo <typeparamref name="T"/> não encontrado</exception>
        public static T ObterAtributo<T>(this IEnumerable<CustomAttributeData> obj) where T : Attribute
        {
            if (obj.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(obj), Resources.ARGUMENTO_NULO_OU_VAZIO);
            }

            CustomAttributeData result = obj.FirstOrDefault(x => x.AttributeType == typeof(T));

            if (result is null)
            {
                throw new InvalidOperationException(string.Format(Resources.Culture, Resources.ATRIBUTO_NAO_ENCONTRADO, typeof(T).Name, nameof(obj)));
            }

            return Activator.CreateInstance(result.AttributeType).To<T>();
        }

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
                throw new ArgumentException(Resources.ARGUMENTO_NULO, nameof(obj));
            }

            if (obj.Herdado<Attribute>())
            {
                throw new InvalidOperationException(Resources.PARAMETRO_ATRIBUTO_INVALIDO);
            }

            object result = obj.GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == typeof(T));

            if (result is null)
            {
                throw new InvalidOperationException(string.Format(Resources.Culture, Resources.ATRIBUTO_NAO_ENCONTRADO, nameof(T), nameof(obj)));
            }

            return result.To<T>();
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
            if (obj is null)
            {
                throw new ArgumentException(Resources.ARGUMENTO_NULO, nameof(obj));
            }

            if (!obj.Is<Type>())
            {
                return obj.GetType().ObterAtributo<T>();
            }
            else
            {
                return obj.As<Type>().ObterAtributo<T>();
            }
        }

        /// <summary>
        /// Obtém uma lista de atributos a partir de um tipo especifico.
        /// </summary>
        /// <typeparam name="T">Tipo do atributo a ser procurado</typeparam>
        /// <param name="obj">Qualquer objeto do tipo <see cref="Type"/></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Argumento <paramref name="obj"/> nulo</exception>
        /// <exception cref="InvalidOperationException">Tipo do parâmetro <paramref name="obj"/> herdado da classe <see cref="Attribute"/></exception>
        /// <exception cref="InvalidOperationException">Atributo <typeparamref name="T"/> não encontrado</exception>
        public static IEnumerable<T> ObterAtributos<T>(this Type obj) where T : Attribute
        {
            if (obj is null)
            {
                throw new ArgumentNullException(Resources.ARGUMENTO_NULO, nameof(obj));
            }

            if (obj.Herdado<Attribute>())
            {
                throw new InvalidOperationException(Resources.PARAMETRO_ATRIBUTO_INVALIDO);
            }

            IEnumerable<object> results = obj.GetCustomAttributes(true).Where(x => x.GetType() == typeof(T));

            if (results.IsNullOrEmpty())
            {
                throw new InvalidOperationException(string.Format(Resources.Culture, Resources.ATRIBUTO_NAO_ENCONTRADO, nameof(T), nameof(obj)));
            }

            return results.Select(x => x.To<T>());
        }

        /// <summary>
        /// Verifica se o <paramref name="obj"/> possui o atributo
        /// </summary>
        /// <typeparam name="T">Tipo do atributo a ser procurado</typeparam>
        /// <param name="obj">Qualquer objeto do tipo <see cref="object"/></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Argumento <paramref name="obj"/> nulo</exception>
        /// <exception cref="InvalidOperationException">Tipo do parâmetro <paramref name="obj"/> herdado da classe <see cref="Attribute"/></exception>
        public static bool PossuiAtributo<T>(this object obj) where T : Attribute
        {
            if (!obj.Is<Type>())
            {
                return obj.GetType().PossuiAtributo<T>();
            }
            else
            {
                return obj.As<Type>().PossuiAtributo<T>();
            }
        }

        /// <summary>
        /// Verifica se o <see cref="PropertyInfo"/> possui o atributo
        /// </summary>
        /// <typeparam name="T">Tipo do atributo a ser procurado</typeparam>
        /// <param name="obj">Qualquer objeto do tipo <see cref="PropertyInfo"/></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Argumento <paramref name="obj"/> nulo</exception>
        public static bool PossuiAtributo<T>(this PropertyInfo obj) where T : Attribute
        {
            if (obj is null)
            {
                throw new ArgumentNullException(Resources.ARGUMENTO_NULO, nameof(obj));
            }

            return obj.GetCustomAttributes(true).Any(x => x.GetType() == typeof(T));
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
            if (obj is null)
            {
                throw new ArgumentNullException(Resources.ARGUMENTO_NULO, nameof(obj));
            }

            if (obj.Herdado<Attribute>())
            {
                throw new InvalidOperationException(Resources.PARAMETRO_ATRIBUTO_INVALIDO);
            }

            return obj.GetCustomAttributes(true).Any(x => x.GetType() == typeof(T));
        }
    }
}