using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Linq;

using static Propeus.Modulo.Abstrato.Constante;

namespace Propeus.Modulo.Abstrato.Util
{
    /// <summary>
    /// Classe de ajuda para <see cref="Enum"/>
    /// </summary>
    public static partial class Helper
    {



        public static TNovoEnum[] ParseEnum<TAntigoEnum, TNovoEnum>(this TAntigoEnum[] @enum) where TNovoEnum : struct, Enum
            where TAntigoEnum : struct, Enum
        {
            var vnEnums = Enum.GetValues<TNovoEnum>();
            var nEnums = new TNovoEnum[@enum.Length];

            for (int i = 0; i < @enum.Length; i++)
            {
                bool convertido = false;
                foreach (var vnEnum in vnEnums)
                {
                    var dsc_vnEnum = vnEnum.ObterDescricaoEnum();
                    if (dsc_vnEnum is not null && dsc_vnEnum.Equals(@enum[i].ToString().Trim(), StringComparison.CurrentCultureIgnoreCase))
                    {
                        convertido = true;
                        nEnums[i] = vnEnum;
                    }
                }
                if (!convertido)
                {
                    throw new InvalidCastException(string.Format("Nao foi possivel converter o valor '{0}' para o tipo '{1}'", @enum[i].ToString(), typeof(TNovoEnum).Name));
                }
            }

            return nEnums;

        }

        public static TNovoEnum ParseEnum<TAntigoEnum, TNovoEnum>(this TAntigoEnum @enum) where TNovoEnum : struct, Enum
            where TAntigoEnum : struct, Enum
        {

            var vnEnums = Enum.GetValues<TNovoEnum>();

            foreach (var vnEnum in vnEnums)
            {
                var dsc_vnEnum = vnEnum.ObterDescricaoEnum();
                if (dsc_vnEnum is not null && dsc_vnEnum.Equals(@enum.ToString().Trim(), StringComparison.CurrentCultureIgnoreCase))
                {
                    return vnEnum;
                }
            }

            throw new InvalidCastException(string.Format("Nao foi possivel converter o valor '{0}' para o tipo '{1}'", @enum.ToString(), typeof(TNovoEnum).Name));

        }

        public static TEnum[] DividirEnum<TEnum>(this TEnum @enum) where TEnum : struct
        {
            var sEnum = @enum.ToString().Split(',');
            TEnum[] renums = new TEnum[sEnum.Length];

            for (int i = 0; i < sEnum.Length; i++)
            {
                renums[i] = Enum.Parse<TEnum>(sEnum[i]);
            }

            return renums;

        }

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
                var vlr = (TEnum)Enum.Parse(typeof(TEnum), enums);
                return vlr;
            }
            else
            {
                throw new ArgumentException(string.Format(ARGUMENTO_NAO_E_DO_TIPO, @enum.First().GetType().Name), nameof(@enum));
            }
        }
        /// <summary>
        /// Concatena um array de enum em um único Enum
        /// </summary>
        /// <typeparam name="TEnum"><see cref="Enum"/> qualquer.</typeparam>
        /// <param name="enum">Array de <see cref="Enum"/></param>
        /// <returns></returns>
        /// <exception cref="InvalidCastException">O tipo não é <see cref="Enum"/></exception>
        /// <exception cref="ArgumentException">Argumento <paramref name="enum"/> vazio ou nulo</exception>
        public static TEnum ConcatenarEnum<TEnum>(this IEnumerable<TEnum> @enum)
        {
            if (@enum is null || @enum.Count() == 0)
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
        public static string ObterDescricaoEnum(this Enum @enum)
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

        /// <summary>
        /// Obtém a descrição do enum
        /// </summary>
        /// <typeparam name="TEnum"><see cref="Enum"/> a ser obtido a descrição</typeparam>
        /// <param name="enum">Valor do <see cref="Enum"/> que será obtido a descrição </param>
        /// <returns></returns>
        /// <exception cref="InvalidEnumArgumentException"><see cref="Enum"/> sem descrição</exception>
        /// <exception cref="ArgumentException">Argumento <paramref name="enum"/> nulo</exception>
        public static string? ObterDescricaoEnum<TEnum>(this TEnum @enum)
        {
            if (@enum is null)
            {
                throw new ArgumentNullException(nameof(@enum), ARGUMENTO_NULO_OU_VAZIO);
            }

            DescriptionAttribute[] attr = @enum.GetType().GetField(@enum.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), true) as DescriptionAttribute[] ?? Array.Empty<DescriptionAttribute>();

            if (attr.Any())
            {
                string data = attr.First().Description;
                return data;
            }
            else
            {
                return null;
            }
        }
    }
}