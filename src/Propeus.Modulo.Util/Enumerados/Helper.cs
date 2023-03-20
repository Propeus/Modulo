using System;
using System.Collections.Generic;
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
        /// Converte o enum para o tipo informado usando o attributo <see cref="DescriptionAttribute"/> parao obter o novo valor
        /// </summary>
        /// <typeparam name="TAntigoEnum">Tipo do enum atual</typeparam>
        /// <typeparam name="TNovoEnum">Tipo do novo enum</typeparam>
        /// <param name="enum">Array de enum antigo</param>
        /// <returns>Array de enum novos</returns>
        /// <exception cref="InvalidCastException">O nome do novo enum nao existe</exception>
        public static TNovoEnum[] ParseEnum<TAntigoEnum, TNovoEnum>(this TAntigoEnum[] @enum) where TNovoEnum : struct, Enum
            where TAntigoEnum : struct, Enum
        {
            TNovoEnum[] vnEnums = Enum.GetValues<TNovoEnum>();
            TNovoEnum[] nEnums = new TNovoEnum[@enum.Length];

            for (int i = 0; i < @enum.Length; i++)
            {
                bool convertido = false;
                foreach (TNovoEnum vnEnum in vnEnums)
                {
                    string dsc_vnEnum = vnEnum.ObterDescricaoEnum();
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

        /// <summary>
        /// Converte uma cadeia de enums em um array
        /// </summary>
        /// <remarks>
        /// Este metodo so funciona para enums concatenados por ','
        /// </remarks>
        /// <typeparam name="TEnum">Tipo do enum que sera dividido</typeparam>
        /// <param name="enum">Valor do enum a ser dividido</param>
        /// <returns>Array de enum</returns>
        public static TEnum[] DividirEnum<TEnum>(this TEnum @enum) where TEnum : struct
        {
            string[] sEnum = @enum.ToString().Split(',');
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
        /// <remarks>
        /// Este metodo realiza a funcao do metodo <see cref="DividirEnum{TEnum}(TEnum)"/> de forma reversa, 
        /// ou seja, ele concatena todos os valores utilizando ','
        /// </remarks>
        /// <typeparam name="TEnum"><see cref="Enum"/> qualquer.</typeparam>
        /// <param name="enum">Array de <see cref="Enum"/></param>
        /// <returns>O enum concatenado</returns>
        /// <exception cref="InvalidCastException">O tipo não é <see cref="Enum"/></exception>
        /// <exception cref="ArgumentException">Argumento <paramref name="enum"/> vazio ou nulo</exception>
        public static TEnum ConcatenarEnum<TEnum>(this TEnum[] @enum)
        {
            if (@enum is null || @enum.Length == 0)
            {
                throw new ArgumentNullException(nameof(@enum));
            }

            if (@enum.Any(x => x.GetType().IsEnum))
            {
                string enums = string.Join(",", @enum);
                TEnum vlr = (TEnum)Enum.Parse(typeof(TEnum), enums);
                return vlr;
            }
            else
            {
                throw new ArgumentException(nameof(@enum));
            }
        }

        /// <summary>
        /// Obtém a descrição do enum
        /// </summary>
        /// <typeparam name="TEnum"><see cref="Enum"/> a ser obtido a descrição</typeparam>
        /// <param name="enum">Valor do <see cref="Enum"/> que será obtido a descrição </param>
        /// <returns>Descricao do enum</returns>
        /// <exception cref="InvalidEnumArgumentException"><see cref="Enum"/> sem descrição</exception>
        /// <exception cref="ArgumentException">Argumento <paramref name="enum"/> nulo</exception>
        public static string ObterDescricaoEnum<TEnum>(this TEnum @enum)
        {
            if (@enum is null)
            {
                throw new ArgumentNullException(nameof(@enum));
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