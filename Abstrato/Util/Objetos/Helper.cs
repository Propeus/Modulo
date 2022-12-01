using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.InteropServices;

using static Propeus.Modulo.Abstrato.Constante;

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
            if (obj.IsNull())
            {
                throw new ArgumentNullException(nameof(obj), ARGUMENTO_NULO);
            }

           var size = Marshal.SizeOf(obj);
           var bytes = new byte[size];
           var ptr = Marshal.AllocHGlobal(size);
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
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj), ARGUMENTO_NULO_OU_VAZIO);
            }

            if (comparacao is null)
            {
                throw new ArgumentNullException(nameof(comparacao), ARGUMENTO_NULO);
            }

            if (obj is Type)
            {
                return Helper.Herdado((obj as Type), comparacao);
            }
            else
            {
                return Helper.Herdado(obj.GetType(), comparacao);
            }
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
        /// Verifica se o objeto é uma lista
        /// </summary>
        /// <remarks>https://stackoverflow.com/questions/17190204/check-if-object-is-dictionary-or-list</remarks>
        /// <param name="obj">Objeto a ser verificado</param>
        /// <returns></returns>
        public static bool IsList(this object obj)
        {
            if (obj.IsNullOrDefault())
            {
                throw new ArgumentNullException(nameof(obj), ARGUMENTO_NULO);
            }

            Type tObj = obj.GetType();

            return obj is IList && tObj.IsGenericType && tObj.GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
        }

        /// <summary>
        /// Verifica se o objeto é um dicionario
        /// </summary>
        /// <remarks>https://stackoverflow.com/questions/17190204/check-if-object-is-dictionary-or-list</remarks>
        /// <param name="obj">Objeto a ser verificado</param>
        /// <returns></returns>
        public static bool IsDictionary(this object obj)
        {
            if (obj.IsNullOrDefault())
            {
                throw new ArgumentNullException(nameof(obj), ARGUMENTO_NULO);
            }

            Type tObj = obj.GetType();

            return obj is IDictionary && tObj.IsGenericType && tObj.GetGenericTypeDefinition().IsAssignableFrom(typeof(Dictionary<,>));
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
        /// Verifica se o objeto é struct, caso seja valida somente se é nulo
        /// </summary>
        /// <param name="obj">Qualquer objeto</param>
        /// <returns></returns>
        public static bool IsStructOrNull(this object obj)
        {
            return obj.IsNull() || obj.IsStruct();
        }

        /// <summary>
        /// Verifica se o objeto é do tipo <typeparamref name="T" /></summary>
        /// <typeparam name="T">Tipo a ser validado</typeparam>
        /// <param name="obj">Objeto a ser verificado</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        public static bool Is<T>(this object obj)
        {
            if (!obj.IsStruct() && obj.IsNullOrDefault())
            {
                throw new ArgumentNullException(nameof(obj), ARGUMENTO_NULO);
            }

            return obj is Type ? obj.Is(typeof(T)) : Helper.Is(obj.GetType(), typeof(T));
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
            if (obj.IsNullOrDefault())
            {
                throw new ArgumentNullException(nameof(obj), ARGUMENTO_NULO);
            }

            return Helper.Is(obj.GetType(), comparacao);
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
            if (obj.IsNull())
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

            throw new InvalidCastException(string.Format(TIPO_NAO_CONVERTIDO, obj.GetType().Name, para.Name));
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
#pragma warning disable CA1031 // Do not catch general exception types
            catch
            {
                result = padrao;
                //Em todos os casos ignora erro e retorna o valor padrão
            }
#pragma warning restore CA1031 // Do not catch general exception types

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
            return obj.IsNull() || obj.IsDefault();
        }

        /// <summary>
        /// Verifica se o objeto possui o valor padrão
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        public static bool IsDefault(this object obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj), ARGUMENTO_NULO);
            }

            return !(obj is Type) ? obj.Equals(obj.GetType().Default()) : obj.Equals(((Type)obj).Default());
        }

        /// <summary>
        /// Verifica se o objeto não é nulo
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNotNull(this object obj)
        {
            return !obj.IsNull();
        }

        /// <summary>
        /// Verifica se o objeto é nulo
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNull(this object obj)
        {
            return obj is null;
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

        /// <summary>
        /// Obtem as interfaces do objeto selecionado
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        public static IEnumerable<Type> ObterInterfaces(this object obj)
        {
            if (obj.IsNull())
            {
                throw new ArgumentNullException(nameof(obj), ARGUMENTO_NULO);
            }

            return Helper.ObterInterfaces(obj.GetType());
        }

        /// <summary>
        /// Obtem todos os metodos do objeto que possuem o mesmo nome
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="nome"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo ou vazio</exception>
        public static IEnumerable<MethodInfo> ObterMetodos(this object obj, string nome)
        {
            if (obj.IsNull())
            {
                throw new ArgumentNullException(nameof(obj), ARGUMENTO_NULO);
            }
            if (nome.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(nome), ARGUMENTO_NULO_OU_VAZIO);
            }

            return obj.ObterMetodos().Where(m => m.Name == nome);
        }

        /// <summary>
        /// Obtem todos os metodos do objeto.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        public static IEnumerable<MethodInfo> ObterMetodos(this object obj)
        {
            if (obj.IsNull())
            {
                throw new ArgumentNullException(nameof(obj), ARGUMENTO_NULO);
            }
            return obj.GetType().GetMethods().Where(x => !x.Name.Contains("get_") && !x.Name.Contains("set_"));
        }

        /// <summary>
        /// Obtem o metodo que possua o mesmo nome, quantidade e tipo de parametros
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="nome"></param>
        /// <param name="params"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo ou vazio</exception>
        public static MethodInfo ObterMetodo(this object obj, string nome, params Type[] @params)
        {
            if (obj.IsNull())
            {
                throw new ArgumentNullException(nameof(obj), ARGUMENTO_NULO);
            }
            if (nome.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(nome), ARGUMENTO_NULO_OU_VAZIO);
            }

            if (obj.Is<Type>())
            {
                return Helper.ObterMetodo(obj.To<Type>(), nome, @params);
            }
            else
            {
                return Helper.ObterMetodo(obj.GetType(), nome, @params);
            }
        }

        /// <summary>
        /// Realiza uma chamada a um metodo
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="nome"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo ou vazio</exception>
        public static object InvocarMetodo(this object obj, string nome, params object[] args)
        {
            if (obj.IsNull())
            {
                throw new ArgumentNullException(nameof(obj), ARGUMENTO_NULO);
            }
            if (nome.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(nome), ARGUMENTO_NULO_OU_VAZIO);
            }

            MethodInfo metodo = obj.GetType().GetMethods().Where(x => x.Name == nome).Aggregate((m1, m2) => { return m1.GetParameters().Count() > m2.GetParameters().Count() ? m1 : m2; });
            return metodo.Invoke(obj, args);
        }

        /// <summary>
        /// Obtem os parametros de um metodo
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="nome"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo ou vazio</exception>
        public static Type[] ObterParametrosMetodo(this object obj, string nome)
        {
            if (obj.IsNull())
            {
                throw new ArgumentNullException(nameof(obj), ARGUMENTO_NULO);
            }
            if (nome.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(nome), ARGUMENTO_NULO_OU_VAZIO);
            }

            MethodInfo metodo = obj.GetType().GetMethods().Where(x => x.Name == nome).Aggregate((m1, m2) => { return m1.GetParameters().Count() > m2.GetParameters().Count() ? m1 : m2; });
            return metodo.GetParameters().Select(t => t.ParameterType).ToArray();
        }

        /// <summary>
        /// Verifica se existe o metodo no objeto passado no parametro <paramref name="obj"/>
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="nome"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo ou vazio</exception>
        public static bool ExisteMetodo(this object obj, string nome)
        {
            if (obj.IsNull())
            {
                throw new ArgumentNullException(nameof(obj), ARGUMENTO_NULO);
            }
            if (nome.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(nome), ARGUMENTO_NULO_OU_VAZIO);
            }

            return obj.GetType().GetMethods().Any(x => x.Name == nome);
        }
    }
}