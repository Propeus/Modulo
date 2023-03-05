using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;



namespace Propeus.Modulo.Util
{
    /// <summary>
    /// Classe de ajuda para listas
    /// </summary>
    public static partial class Helper
    {

        /// <summary>
        /// Junta as duas listas sem repitir os objetos.
        /// </summary>
        /// <typeparam name="T">Tipo da lista</typeparam>
        /// <param name="esquerda">Lista da esquerda</param>
        /// <param name="direita">Lista da direita</param>
        /// <returns>Retorna os valores das duas listas sem repitir os valores</returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        public static IEnumerable<T> FullJoin<T>(this IEnumerable<T> esquerda, IEnumerable<T> direita)
        {
            if (esquerda is null)
            {
                throw new ArgumentNullException(nameof(esquerda));
            }

            if (direita is null)
            {
                return esquerda;
            }

            ICollection<T> join = new LinkedList<T>();
            foreach (T ia in esquerda)
            {
                join.Add(ia);
            }
            foreach (T ib in direita)
            {
                if (!join.Contains(ib))
                {
                    join.Add(ib);
                }
            }
            return join;
        }
        /// <summary>
        /// Obtém os itens em comum entre duas listas.
        /// </summary>
        /// <typeparam name="T">Tipo da lista</typeparam>
        /// <param name="esquerda">Lista da esquerda</param>
        /// <param name="direita">Lista da direita</param>
        /// <returns>Retorna os valores em commum das duas listas</returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        public static IEnumerable<T> Join<T>(this IEnumerable<T> esquerda, IEnumerable<T> direita)
        {
            if (esquerda is null)
            {
                throw new ArgumentNullException(nameof(esquerda));
            }

            if (direita is null)
            {
                return esquerda;
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
        /// Indica se todos os valores passados em <paramref name="params"/> estao contido em <paramref name="obj"/>
        /// </summary>
        /// <typeparam name="T">Tipo do parametro a ser verificado</typeparam>
        /// <param name="obj">Lista do tipo <typeparamref name="T"/></param>
        /// <param name="params">Parametros do tipo <typeparamref name="T"/></param>
        /// <returns>Retorna <see langword="true"/> caso todos os valores estejam contidos em <paramref name="obj"/>. Caso contrario retorna <see langword="false"/></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        public static bool ContainsAll<T>(this IEnumerable<T> obj, params T[] @params)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
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
        /// Converte uma lista de objetos para uma lista do tipo especificado
        /// </summary>
        /// <typeparam name="TSaida">Tipo a ser convertido</typeparam>
        /// <param name="obj">Lista de objetos a ser convertido</param>
        /// <returns>Lista de objeto convertido</returns>
        public static IEnumerable<TSaida> Converter<TSaida>(this IEnumerable obj)
        {
            foreach (object entrada in obj)
            {
                yield return entrada.To<TSaida>();
            }
        }
    }
}