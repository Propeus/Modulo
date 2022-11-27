using System;
using System.ComponentModel;
using System.Linq;

using static Propeus.Modulo.Abstrato.Constante;

namespace PPropeus.Modulo.Abstrato.Util
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
            if (@enum is null || @enum.Length == 0)
            {
                throw new ArgumentNullException(nameof(@enum), ARGUMENTO_NULO_OU_VAZIO);
            }

            if (@enum.Any(x => x.GetType().IsEnum))
            {
                string enums = string.Join(",", @enum);
                return (TEnum)Enum.Parse(typeof(TEnum), enums);
            }
            else
            {
                throw new ArgumentException(string.Format(ARGUMENTO_NAO_E_DO_TIPO, @enum.First().GetType().Name), nameof(@enum));
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
            if (@enum is null)
            {
                throw new ArgumentNullException(nameof(@enum), ARGUMENTO_NULO_OU_VAZIO);
            }

            if (@enum.GetType().GetField(@enum.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), true).Any())
            {
                string data = (@enum.GetType().GetField(@enum.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), true).First() as DescriptionAttribute)?.Description;
                return data;
            }
            else
            {
                throw new InvalidEnumArgumentException(ENUM_SEM_DESCRICAO);
            }
        }
    }
}