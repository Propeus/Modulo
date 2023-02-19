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
                throw new ArgumentNullException(nameof(obj), ARGUMENTO_NULO_OU_VAZIO);
            }

            CustomAttributeData result = obj.FirstOrDefault(x => x.AttributeType == typeof(T));

            return result is null
                ? throw new InvalidOperationException(string.Format(ATRIBUTO_NAO_ENCONTRADO, typeof(T).Name, nameof(obj)))
                : Activator.CreateInstance(result.AttributeType).To<T>();
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
                throw new ArgumentException(ARGUMENTO_NULO, nameof(obj));
            }

            if (obj.Herdado<Attribute>())
            {
                throw new InvalidOperationException(PARAMETRO_ATRIBUTO_INVALIDO);
            }

            object result = obj.GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == typeof(T));

            return result is null
                ? throw new InvalidOperationException(string.Format(ATRIBUTO_NAO_ENCONTRADO, nameof(T), nameof(obj)))
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
                throw new ArgumentNullException(ARGUMENTO_NULO, nameof(obj));
            }

            if (obj.Herdado<Attribute>())
            {
                throw new InvalidOperationException(PARAMETRO_ATRIBUTO_INVALIDO);
            }

            IEnumerable<object> results = obj.GetCustomAttributes(true).Where(x => x.GetType() == typeof(T));

            return results.IsNullOrEmpty()
                ? throw new InvalidOperationException(string.Format(ATRIBUTO_NAO_ENCONTRADO, nameof(T), nameof(obj)))
                : results.Select(x => x.To<T>());
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
            return !obj.Is<Type>() ? obj.GetType().PossuiAtributo<T>() : obj.As<Type>().PossuiAtributo<T>();
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
            return obj is null
                ? throw new ArgumentNullException(ARGUMENTO_NULO, nameof(obj))
                : obj.GetCustomAttributes(true).Any(x => x.GetType() == typeof(T));
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