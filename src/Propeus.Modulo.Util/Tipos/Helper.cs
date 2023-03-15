using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Propeus.Modulo.Util
{

    /// <summary>
    /// Classe de ajuda para o tipo <see cref="Type"/>
    /// </summary>
    public static partial class Helper
    {
        /// <summary>
        /// Verifica se o tipo é herdado do tipo passado no parametro <paramref name="comparacao"/>
        /// </summary>
        /// <param name="obj">Tipo a ser verificado</param>
        /// <param name="comparacao">Tipo a ser comparado</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        /// <exception cref="ArgumentException">Argumento invalido</exception>
        public static bool Herdado(this Type obj, Type comparacao)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
            if (obj.IsVoid())
            {
                throw new ArgumentException();
            }

            if (comparacao is null)
            {
                throw new ArgumentNullException(nameof(comparacao));
            }
            if (comparacao.IsVoid())
            {
                throw new ArgumentException();
            }

            bool r = false;
            if (comparacao.IsClass)
            {
                Type type = obj;
                List<Type> heranca = new();
                while (type != null)
                {
                    heranca.Add(type);
                    type = type.BaseType;
                }
                r = heranca.Any(t => t.Name == comparacao.Name);
                return r;
            }
            else if (comparacao.IsInterface)
            {
                Type type = obj;
                List<Type> heranca = new();
                while (type != null)
                {
                    heranca.AddRange(type.GetInterfaces());
                    type = type.BaseType;
                }
                r = heranca.Contains(comparacao);
                return r;
            }
            return r;
        }

        /// <summary>
        /// Verifica se o tipo é herdado do tipo passado no parametro <typeparamref name="T"/>
        /// </summary>
        /// <param name="type">Tipo a ser verificado</param>
        /// <typeparam name="T">Tipo a ser comparado</typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        /// <exception cref="ArgumentException">Argumento invalido</exception>
        public static bool Herdado<T>(this Type type)
        {
            return Herdado(type, typeof(T));
        }

        /// <summary>
        /// Verifica se o tipo <paramref name="obj"/> é igual ou herdado de <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Tipo a ser comparado</typeparam>
        /// <param name="obj">Tipo a ser validado</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        /// <exception cref="ArgumentException">Argumento invalido</exception>
        public static bool Is<T>(this Type obj)
        {
            return obj.Is(typeof(T));
        }

        /// <summary>
        ///  Verifica se o tipo do parâmetro <paramref name="obj"/> é igual ou herdado de <paramref name="comparacao"/>
        /// </summary>
        /// <param name="obj">Tipo a ser validado</param>
        /// <param name="comparacao">Tipo a ser comparado</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        /// <exception cref="ArgumentException">Argumento invalido</exception>
        public static bool Is(this Type obj, Type comparacao)
        {
            return obj is null
                ? throw new ArgumentNullException(nameof(obj))
                : comparacao is null
                ? throw new ArgumentNullException(nameof(comparacao))
                : obj.IsPrimitive || comparacao.IsPrimitive ? obj == comparacao : obj == comparacao || obj.Herdado(comparacao);
        }

        /// <summary>
        /// Verifica se o tipo é void
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsVoid(this Type type)
        {
            return type == typeof(void);
        }

        /// <summary>
        /// Obtem o valor padrão do tipo passado no parametro <paramref name="obj"/>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object Default(this Type obj)
        {
            if (obj.IsClass || obj.IsInterface)
            {
                return null;
            }
            else if (obj.IsEnum)
            {
                return Enum.GetValues(obj).GetValue(0);
            }
            else if (obj.IsPrimitive || obj.IsValueType)
            {
                return Activator.CreateInstance(obj);
            }

            throw new InvalidCastException();
        }

        /// <summary>
        /// Obtem as interfaces do tipo selecionado
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        public static IEnumerable<Type> ObterInterfaces(this Type obj)
        {
            return obj is null ? throw new ArgumentNullException(nameof(obj)) : (IEnumerable<Type>)obj.GetInterfaces();
        }

    }
}