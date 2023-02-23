using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

using static Propeus.Modulo.Abstrato.Constantes;

namespace Propeus.Modulo.Abstrato.Util
{
    /// <summary>
    /// Classe de ajuda para tipos variados
    /// </summary>
    public static partial class Helper
    {
        /// <summary>
        /// Serializa um objeto em array de <see cref="byte" /></summary>
        /// <param name="obj">Qualuer objeto do tipo <see cref="object" /></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        /// <exception cref="SerializationException">Objeto não serializavel</exception>
        public static byte[] Serializar(this object obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj), ARGUMENTO_NULO);
            }

            if (obj.Is<string>())
            {
                return obj.ToString().ToArrayByte();
            }

            int size = Marshal.SizeOf(obj);
            byte[] bytes = new byte[size];
            nint ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(obj, ptr, false);
            Marshal.Copy(ptr, bytes, 0, size);
            Marshal.FreeHGlobal(ptr);

            return bytes;
        }

        /// <summary>
        /// Verifica se o objeto é herdado do tipo passado no parametro <paramref name="comparacao" /></summary>
        /// <param name="obj">Classe a ser verificado</param>
        /// <param name="comparacao">Tipo a ser comparado</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        /// <exception cref="ArgumentException">Argumento invalido</exception>
        public static bool Herdado(this object obj, Type comparacao)
        {
            return obj is null
                ? throw new ArgumentNullException(nameof(obj), ARGUMENTO_NULO_OU_VAZIO)
                : comparacao is null
                ? throw new ArgumentNullException(nameof(comparacao), ARGUMENTO_NULO)
                : obj is Type ? Helper.Herdado(obj as Type, comparacao) : Helper.Herdado(obj.GetType(), comparacao);
        }

        /// <summary>
        /// Verifica se o objeto é herdado do tipo passado no tipo <typeparamref name="T" /></summary>
        /// <param name="obj">Classe a ser verificado</param>
        /// <typeparam name="T">Tipo a ser comparado</typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        /// <exception cref="ArgumentException">Argumento invalido</exception>
        public static bool Herdado<T>(this object obj)
        {
            return Herdado(obj, typeof(T));
        }

        /// <summary>
        /// Verifica se o objeto é struct
        /// </summary>
        /// <param name="obj">Qualquer objeto a ser analisado</param>
        /// <returns></returns>
        public static bool IsStruct(this object obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj), ARGUMENTO_NULO);
            }
            Type type = obj.GetType();
            return type.IsValueType && !type.IsPrimitive;
        }

        /// <summary>
        /// Verifica se o objeto é do tipo <typeparamref name="T" /></summary>
        /// <typeparam name="T">Tipo a ser validado</typeparam>
        /// <param name="obj">Objeto a ser verificado</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        public static bool Is<T>(this object obj)
        {
            return !obj.IsStruct() && obj.IsNullOrDefault()
                ? throw new ArgumentNullException(nameof(obj), ARGUMENTO_NULO)
                : obj is Type ? obj.Is(typeof(T)) : Helper.Is(obj.GetType(), typeof(T));
        }

        /// <summary>
        /// Verifica se o objeto <paramref name="obj" /> é igual ou herdado de <paramref name="comparacao" /></summary>
        /// <param name="comparacao">Tipo a ser validado</param>
        /// <param name="obj">Objeto a ser verificado</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        /// <exception cref="ArgumentException">Argumento invalido</exception>
        public static bool Is(this object obj, Type comparacao)
        {
            return obj.IsNullOrDefault() ? throw new ArgumentNullException(nameof(obj), ARGUMENTO_NULO) : Helper.Is(obj.GetType(), comparacao);
        }

        /// <summary>
        /// Converte qualquer objeto para o tipo desejado
        /// </summary>
        /// <typeparam name="T">Tipo a ser convertido</typeparam>
        /// <param name="obj">Objeto a ser convertido</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        /// <exception cref="ArgumentException">Argumento invalido</exception>
        /// <exception cref="OverflowException"></exception>
        public static T To<T>(this object obj)
        {
            return (T)obj.To(typeof(T));
        }

        /// <summary>
        /// Converte qualquer objeto para o tipo desejado
        /// </summary>
        /// <param name="para">Tipo a ser convertido</param>
        /// <param name="obj">Objeto a ser convertido</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        /// <exception cref="ArgumentException">Argumento invalido</exception>
        /// <exception cref="OverflowException"></exception>
        public static object To(this object obj, Type para)
        {
            //Verifica se o objeto é nulo
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj), ARGUMENTO_NULO);
            }
            if (obj.Is<Type>())
            {
                throw new InvalidCastException(string.Format(PARAMETRO_NAO_CONVERTIDO, nameof(obj), para.Name));
            }

            if (para.IsNullOrDefault())
            {
                throw new ArgumentNullException(nameof(para), ARGUMENTO_NULO);
            }

            //Evita redundancia de conversão
            if (obj.GetType() == para)
            {
                return obj;
            }

            if (obj.Herdado(para))
            {
                return obj;
            }

            if (para.IsEnum)
            {
                return Enum.Parse(para, obj.ToString());
            }
            else if (para.IsPrimitive)
            {
                return Convert.ChangeType(obj, para);
            }
            else if (para.IsClass && obj.GetType().IsPrimitive)
            {
                return Convert.ChangeType(obj, para);
            }

            MethodInfo box_explicito_implicito = obj.GetType().GetMethod("op_Explicit", new[] { obj.GetType() })
                ?? obj.GetType().GetMethod("op_Implicit", new[] { obj.GetType() });


            return box_explicito_implicito?.ReturnType == para
                ? box_explicito_implicito.Invoke(obj, new object[] { obj })
                : throw new InvalidCastException(string.Format(TIPO_NAO_CONVERTIDO, obj.GetType().Name, para.Name));
        }

        /// <summary>
        /// Tenta converter o parâmetro <paramref name="obj" /> em <typeparamref name="T" />, caso não consiga será retornado o valor padrão de <typeparamref name="T" /> ou o valor passado no parametro <paramref name="padrao" /></summary>
        /// <typeparam name="T">Tipo a ser convertido</typeparam>
        /// <param name="obj">Objeto a ser convertido em <typeparamref name="T" /></param>
        /// <param name="padrao">Valor padrão em caso de erro.</param>
        /// <returns>
        ///   <typeparamref name="T" /> ou <paramref name="padrao" /></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        /// <exception cref="ArgumentException">Argumento invalido</exception>
        public static T As<T>(this object obj, T padrao = default)
        {
            return (T)obj.As(typeof(T), padrao);
        }

        /// <summary>
        /// Tenta converter o <paramref name="obj" /> em <paramref name="como" />, caso não consiga será retornado nulo ou o valor passado no parametro <paramref name="padrao" /></summary>
        /// <param name="como">Tipo a ser convertido</param>
        /// <param name="obj">Objeto a ser convertido no tipo do parametro <paramref name="como" /></param>
        /// <param name="padrao">Valor padrão em caso de erro.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        /// <exception cref="ArgumentException">Argumento invalido</exception>
        public static object As(this object obj, Type como, object padrao = default)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj), ARGUMENTO_NULO);
            }

            if (como.IsNullOrDefault())
            {
                throw new ArgumentNullException(nameof(como), ARGUMENTO_NULO);
            }

            object result;
            try
            {
                result = obj.To(como);
            }
            catch
            {
                result = padrao;
                //Em todos os casos ignora erro e retorna o valor padrão
            }

            return result;
        }

        /// <summary>
        /// Verifica se o objeto é nulo ou possui um valor padrão
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        public static bool IsNullOrDefault(this object obj)
        {
            return obj is null || obj.IsDefault();
        }

        /// <summary>
        /// Verifica se o objeto possui o valor padrão
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        public static bool IsDefault(this object obj)
        {
            return obj is null
                ? throw new ArgumentNullException(nameof(obj), ARGUMENTO_NULO)
                : obj is not Type ? obj.Equals(obj.GetType().Default()) : obj.Equals(((Type)obj).Default());
        }

        /// <summary>
        /// Obtem o hash de um objeto serializavel
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="SerializationException"></exception>
        public static string Hash(this object obj)
        {
            return Helper.Hash(obj.Serializar());
        }      
    }
}