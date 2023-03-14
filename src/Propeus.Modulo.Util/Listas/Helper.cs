using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Propeus.Modulo.Util
{
    /// <summary>
    /// Classe de ajuda para listas
    /// </summary>
    public static partial class Helper
    {
        /// <summary>
        /// Salva o conteudo do <see cref="HashSet{T}"/> dentro de um arquivo de texto
        /// </summary>
        /// <param name="filePath">Caminho do arquivo de texto</param>
        /// <param name="filePaths">Lista caminhos de arquivos a serem salvos</param>
        public static void SaveFilePathsToFile(this string filePath, HashSet<string> filePaths)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (string path in filePaths)
                {
                    writer.Write(path);
                    writer.Write('\t');
                }
            }
        }

        /// <summary>
        /// Carrega a lista de arquivos para o <see cref="HashSet{T}"/>
        /// </summary>
        /// <param name="filePath">Caminho do arquivo</param>
        /// <param name="blockSize">Tamanho do buffer</param>
        /// <returns>Retorna a lista de <see cref="HashSet{T}"/></returns>
        public static HashSet<string> LoadFilePathsAsHashSet(this string filePath, int blockSize = 4096)
        {
            HashSet<string> filePathHashSet = new HashSet<string>();

            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader reader = new StreamReader(fileStream))
            {
                char[] buffer = new char[blockSize];
                int bytesRead;

                while ((bytesRead = reader.ReadBlock(buffer, 0, blockSize)) > 0)
                {
                    string block = new string(buffer, 0, bytesRead);
                    string[] paths = block.Split('\t');

                    foreach (string path in paths)
                    {
                        if (string.IsNullOrEmpty(path)) continue;

                        filePathHashSet.Add(path);
                    }
                }
            }

            return filePathHashSet;
        }

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
        /// <typeparam name="T">Tipo da lista</typeparam>
        /// <param name="esquerda">Lista da esquerda</param>
        /// <param name="direita">Lista da direita</param>
        /// <returns>Retorna os valores das duas listas sem repitir os valores</returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        public static IDictionary<T, bool> FullJoinDictionary<T>(this IEnumerable<T> esquerda, IEnumerable<T> direita)
        {
            if (esquerda is null)
            {
                throw new ArgumentNullException(nameof(esquerda));
            }

            if (direita is null)
            {
                return esquerda.ToDictionary((k => k), (v => false));
            }

            IDictionary<T, bool> join = new Dictionary<T, bool>();
            foreach (T ia in esquerda)
            {
                    join.Add(ia, true);
                }
            foreach (T ib in direita)
            {
                if (!join.ContainsKey(ib))
                {
                    join.Add(ib, false);
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
                return esquerda.ToDictionary((k => k), (v => false));
            }

            IDictionary<MethodInfo, bool> join = new Dictionary<MethodInfo, bool>();
            IDictionary<string, MethodInfo> joinKV = new Dictionary<string, MethodInfo>();

            foreach (MethodInfo ia in esquerda)
            {
                if (!joinKV.ContainsKey(ia.HashMetodo()))
                {
                    join.Add(ia, true);
                    joinKV.Add(ia.HashMetodo(), ia);
                }
            }
            foreach (MethodInfo ib in direita)
            {
                if (!joinKV.ContainsKey(ib.HashMetodo()))
                {
                    join.Add(ib, false);
                    joinKV.Add(ib.HashMetodo(), ib);
                }
            }

            joinKV.Clear();

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