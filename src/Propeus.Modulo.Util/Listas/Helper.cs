using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Propeus.Modulo.Util.Objetos;
using Propeus.Modulo.Util.Reflections;

namespace Propeus.Modulo.Util.Listas
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

            ICollection<T> join = new HashSet<T>();
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
        /// Junta as duas listas sem repitir os objetos.
        /// </summary>
        /// <remarks>
        /// No dicionario, esquerda retorna como true e direita como false
        /// </remarks>
        /// <param name="esquerda">Lista da esquerda</param>
        /// <param name="direita">Lista da direita</param>
        /// <returns>Retorna os valores das duas listas sem repitir os valores</returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        public static IDictionary<MethodInfo, bool> FullJoinDictionaryMethodInfo(this IEnumerable<MethodInfo> esquerda, IEnumerable<MethodInfo> direita)
        {
            if (esquerda is null)
            {
                throw new ArgumentNullException(nameof(esquerda));
            }

            if (direita is null)
            {
                return esquerda.ToDictionary(k => k, v => false);
            }

            IDictionary<MethodInfo, bool> join = new Dictionary<MethodInfo, bool>();
            IDictionary<string, MethodInfo> joinKV = new Dictionary<string, MethodInfo>();

            foreach (MethodInfo ia in esquerda)
            {
                if (!joinKV.ContainsKey(ia.HashMetodoMethodInfo()))
                {
                    join.Add(ia, true);
                    joinKV.Add(ia.HashMetodoMethodInfo(), ia);
                }
            }
            foreach (MethodInfo ib in direita)
            {
                if (!joinKV.ContainsKey(ib.HashMetodoMethodInfo()))
                {
                    join.Add(ib, false);
                    joinKV.Add(ib.HashMetodoMethodInfo(), ib);
                }
            }

            joinKV.Clear();

            return join;
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