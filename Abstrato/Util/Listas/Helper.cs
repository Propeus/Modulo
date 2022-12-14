using System;
using System.Collections.Generic;
using System.Linq;

using static Propeus.Modulo.Abstrato.Constante;

namespace Propeus.Modulo.Abstrato.Util
{
    /// <summary>
    /// Classe de ajuda para listas
    /// </summary>
    public static partial class Helper
    {
        /// <summary>
        /// Obtém os itens em comum entre duas listas.
        /// </summary>
        /// <typeparam name="T">Tipo da lista</typeparam>
        /// <param name="esquerda">Lista da esquerda</param>
        /// <param name="direita">Lista da direita</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        public static IEnumerable<T> Join<T>(this IEnumerable<T> esquerda, IEnumerable<T> direita)
        {
            if (esquerda.IsNull())
            {
                throw new ArgumentNullException(nameof(esquerda), ARGUMENTO_NULO);
            }

            if (direita.IsNull())
            {
                throw new ArgumentNullException(nameof(direita), ARGUMENTO_NULO);
            }

            ICollection<T> join = new LinkedList<T>();
            foreach (T ia in esquerda)
            {
                foreach (T ib in direita)
                {
                    if (ia.Equals(ib) && !join.Contains(ia))
                    {
                        join.Add(ia);
                        break;
                    }
                }
            }
            return join;
        }

        /// <summary>
        /// Verifica se uma lista contem os parâmetros passados ou se são herdados.
        /// </summary>
        /// <typeparam name="T">Tipo do parametro a ser verificado</typeparam>
        /// <param name="obj">Lista do tipo <typeparamref name="T"/></param>
        /// <param name="params">Parametros do tipo <typeparamref name="T"/></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        public static bool ContainsAll<T>(this IEnumerable<T> obj, params T[] @params)
        {
            if (obj.IsNull())
            {
                throw new ArgumentNullException(nameof(obj), ARGUMENTO_NULO);
            }

            bool result = true;
            foreach (T item in @params)
            {
                result = obj.Contains(item);

                if (!result && typeof(T).Is<Type>())
                {
                    result = obj.Any(x => ((Type)(object)item).Herdado((Type)(object)x));
                }

                if (!result)
                {
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Verifica se a lista esta vazia
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        public static bool IsEmpty<T>(this IEnumerable<T> obj)
        {
            if (obj.IsNull())
            {
                throw new ArgumentNullException(nameof(obj), ARGUMENTO_NULO);
            }

            return !obj.Any();
        }

        /// <summary>
        /// Verifica se a lista não esta vazia
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo ou vazio</exception>
        public static bool IsNotEmpty<T>(this IEnumerable<T> obj)
        {
            if (obj.IsNull())
            {
                throw new ArgumentNullException(nameof(obj), ARGUMENTO_NULO);
            }

            return obj.Any();
        }

        /// <summary>
        /// Verifica se a lista é nula ou vazia
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> obj)
        {
            return obj is null || obj.IsEmpty();
        }

        /// <summary>
        /// Verifica se a lista não é nula ou vazia
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> obj)
        {
            return !obj.IsNullOrEmpty();
        }
    }
}