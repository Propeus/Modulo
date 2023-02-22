using System.Reflection;

using static Propeus.Modulo.Abstrato.Constantes;

namespace Propeus.Modulo.Abstrato.Util
{
    /// <summary>
    /// Classe de ajuda para tipos genericos
    /// </summary>
    public static partial class Helper
    {
        /// <summary>
        /// Cria uma nova instacia de <typeparamref name="TPara"/> e copia as propriedades de <typeparamref name="TDe"/>
        /// </summary>
        /// <typeparam name="TPara">Tipo da saida</typeparam>
        /// <typeparam name="TDe">Tipo de entrada</typeparam>
        /// <param name="de">Instancia do objeto de entrada</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        public static TPara CopiarPropriedades<TDe, TPara>(this TDe de)
            where TPara : class
            where TDe : class
        {
            if (de.IsNull())
            {
                throw new ArgumentNullException(nameof(de), ARGUMENTO_NULO);
            }

            TPara saida = Activator.CreateInstance<TPara>();
            IDictionary<PropertyInfo, Type> entradaPropriedades = de.ObterPropriedadeInfoType();
            IDictionary<PropertyInfo, Type> saidaPropriedades = saida.ObterPropriedadeInfoType();

            foreach (KeyValuePair<PropertyInfo, Type> psaida in saidaPropriedades)
            {
                foreach (KeyValuePair<PropertyInfo, Type> pentrada in entradaPropriedades)
                {
                    if (psaida.Key.Name == pentrada.Key.Name && psaida.Value == pentrada.Value)
                    {
                        psaida.Key.SetValue(saida, de.ObterValorPropriedade(pentrada.Key));
                    }
                }
            }

            return saida;
        }

        /// <summary>
        /// Obtem todas as propriedades e tipo de um objeto
        /// </summary>
        /// <typeparam name="T">Tipo do objeto</typeparam>
        /// <param name="obj">Instancia do objeto</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        public static IDictionary<PropertyInfo, Type> ObterPropriedadeInfoType<T>(this T obj) where T : class
        {
            if (obj.IsNull())
            {
                throw new ArgumentNullException(nameof(obj), ARGUMENTO_NULO);
            }

            IDictionary<PropertyInfo, Type> dic = new Dictionary<PropertyInfo, Type>();
            PropertyInfo[] props = obj.GetType().GetProperties();
            foreach (PropertyInfo propriedade in props)
            {
                dic.Add(propriedade, propriedade.PropertyType);
            }

            return dic;
        }

        /// <summary>
        /// Obtem o valor de uma proprieddade de uma classe.
        /// </summary>
        /// <typeparam name="TClase">Tipo da classe</typeparam>
        /// <param name="obj">Objeto do tipo class</param>
        /// <param name="property">Propriedade que será obtido o valor</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        public static object? ObterValorPropriedade<TClase>(this TClase obj, PropertyInfo property)
        {
            return obj.IsNull()
                ? throw new ArgumentNullException(nameof(obj), ARGUMENTO_NULO)
                : property.IsNull() ? throw new ArgumentNullException(nameof(property), ARGUMENTO_NULO) : property.GetValue(obj);
        }

        /// <summary>
        /// Insere um valor em uma propriedade
        /// </summary>
        /// <typeparam name="TClase">Tipo da classe</typeparam>
        /// <param name="obj">Classe onde será inserido o valor</param>
        /// <param name="property">Propriedade onde será inserido o valor</param>
        /// <param name="valor">Valor a ser inserido</param>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="TargetException"></exception>
        /// <exception cref="MethodAccessException"></exception>
        /// <exception cref="TargetInvocationException"></exception>
        public static void InserirValorPropriedade<TClase>(this TClase obj, PropertyInfo property, object valor)
        {
            if (obj.IsNull())
            {
                throw new ArgumentNullException(nameof(obj), ARGUMENTO_NULO);
            }
            if (property.IsNull())
            {
                throw new ArgumentNullException(nameof(property), ARGUMENTO_NULO);
            }

            property.SetValue(obj, valor);
        }

        /// <summary>
        /// Verifica se a classe é nula
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNull<T>(this T obj) where T : class
        {
            return obj == default(T);
        }

        /// <summary>
        /// Verifica se o objeto não é nulo
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNotNull<T>(this T obj) where T : class
        {
            return !obj.IsNull();
        }

        /// <summary>
        /// Verifica se existe o procedimento informado
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        public static bool ExisteMetodo<T>(this T obj, Action action)
        {
            return obj.IsNull()
                ? throw new ArgumentNullException(nameof(obj), ARGUMENTO_NULO)
                : action.IsNull() ? throw new ArgumentNullException(nameof(action), ARGUMENTO_NULO) : action.Method.Name.ExisteMetodo<T>();
        }
    }
}