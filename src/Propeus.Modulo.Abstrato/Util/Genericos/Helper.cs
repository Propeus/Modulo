using System;
using System.Collections.Generic;
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
        /// Obtem todas as propriedades e tipo de um objeto
        /// </summary>
        /// <typeparam name="T">Tipo do objeto</typeparam>
        /// <param name="obj">Instancia do objeto</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        public static IDictionary<PropertyInfo, Type> ObterPropriedadeInfoType<T>(this T obj) where T : class
        {
            if (obj is null)
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
        public static object ObterValorPropriedade<TClase>(this TClase obj, PropertyInfo property)
        {
            return obj is null
                ? throw new ArgumentNullException(nameof(obj), ARGUMENTO_NULO)
                : property is null ? throw new ArgumentNullException(nameof(property), ARGUMENTO_NULO) : property.GetValue(obj);
        }
    }
}