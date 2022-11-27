using Propeus.Modulo.Util.Listas;
using Propeus.Modulo.Util.Objetos;
using Propeus.Modulo.Util.Properties;
using System;
using System.ComponentModel;
using System.Linq;

namespace Propeus.Modulo.Util.Enumerados
{
    /// <summary>
    /// Classe de ajuda para <see cref="Enum"/>
    /// </summary>
    public static partial class Helper
    {
        /// <summary>
        /// Concatena um array de enum em um único Enum
        /// </summary>
        /// <typeparam name="TEnum"><see cref="Enum"/> qualquer.</typeparam>
        /// <param name="enum">Array de <see cref="Enum"/></param>
        /// <returns></returns>
        /// <exception cref="InvalidCastException">O tipo não é <see cref="Enum"/></exception>
        /// <exception cref="ArgumentException">Argumento <paramref name="enum"/> vazio ou nulo</exception>
        public static TEnum ConcatenarEnum<TEnum>(this TEnum[] @enum)
        {
            if (@enum.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(@enum), Resources.ARGUMENTO_NULO_OU_VAZIO);
            }

            if (@enum.Any(x => x.GetType().IsEnum))
            {
                string enums = string.Join(",", @enum);
                return (TEnum)Enum.Parse(typeof(TEnum), enums);
            }
            else
            {
                throw new ArgumentException(string.Format(Resources.ARGUMENTO_NAO_E_DO_TIPO, @enum.First().GetType().Name), nameof(@enum));
            }
        }

        /// <summary>
        /// Obtém a descrição do enum
        /// </summary>
        /// <typeparam name="TEnum"><see cref="Enum"/> a ser obtido a descrição</typeparam>
        /// <param name="enum">Valor do <see cref="Enum"/> que será obtido a descrição </param>
        /// <returns></returns>
        /// <exception cref="InvalidEnumArgumentException"><see cref="Enum"/> sem descrição</exception>
        /// <exception cref="ArgumentException">Argumento <paramref name="enum"/> nulo</exception>
        public static string ObterDescricaoEnum<TEnum>(this TEnum @enum)
        {
            if (@enum.IsNull())
            {
                throw new ArgumentNullException(nameof(@enum), Resources.ARGUMENTO_NULO_OU_VAZIO);
            }

            if (@enum.GetType().GetField(@enum.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), true).Any())
            {
                return @enum.GetType().GetField(@enum.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), true).First().As<DescriptionAttribute>().Description;
            }
            else
            {
                throw new InvalidEnumArgumentException(Resources.ENUM_SEM_DESCRICAO);
            }
        }
    }
}