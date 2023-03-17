using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Propeus.Modulo.Abstrato.Exceptions;
using Propeus.Modulo.Abstrato.Interfaces;

namespace Propeus.Modulo.Abstrato.Proveders
{
    /// <summary>
    /// Provedor de instancias
    /// </summary>
    /// <remarks>
    /// Com grandes poderes ha grandes merdas
    /// </remarks>
    public static class InstanciaProvider
    {
        //K:Id | V:modulo
        static ConcurrentDictionary<string, IModuloTipo> modules = new ConcurrentDictionary<string, IModuloTipo>();
        //K:id
        static HashSet<string> cache = new HashSet<string>();

        /// <summary>
        /// Verifica se o tipo informado esta em cache
        /// </summary>
        /// <param name="name">Nome do modulo</param>
        /// <returns><see langword="true"/> caso exista</returns>
        public static bool HasCache(string name)
        {
            return cache.Contains(name);
        }

        /// <summary>
        /// Verifica se existe algum modulo pelo nome
        /// </summary>
        /// <param name="name">Nome do modulo</param>
        /// <returns><see langword="true"/> caso exista</returns>
        public static bool HasByName(string name)
        {
            return cache.Contains(name) || modules.Values.Any(x => x.TipoModulo.Name == name);
        }

        /// <summary>
        /// Verifica se existe algum modulo com o id informado
        /// </summary>
        /// <param name="id">Id do modulo</param>
        /// <returns><see langword="true"/> caso exista</returns>
        public static bool HasById(string id)
        {
            return cache.Contains(id) || modules.ContainsKey(id);
        }

        /// <summary>
        /// Obtem o modulo pelo id informado
        /// </summary>
        /// <param name="id">Id do modulo</param>
        /// <returns><see cref="IModuloTipo"/> caso exista senao retorna <see langword="null"/></returns>
        public static IModuloTipo GetById(string id)
        {
            modules.TryGetValue(id, out var tipo);
            return tipo;
        }

        /// <summary>
        /// Obtem o modulo pelo nome do modulo
        /// </summary>
        /// <param name="name">Nome do modulo</param>
        /// <returns><see cref="IModuloTipo"/> caso exista senao retorna <see langword="null"/></returns>
        public static IModuloTipo GetByName(string name)
        {
            return modules.Values.FirstOrDefault(x => x.TipoModulo.Name == name && !x.Coletado);
        }

        /// <summary>
        /// Retorna todos os modulos ativos
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IModuloTipo> Get()
        {
            return modules.Where(x => !x.Value.Coletado).Select(x => x.Value);
        }

        /// <summary>
        /// Registra um novo modulo no provider
        /// </summary>
        /// <param name="modulo">Instancia do modulo</param>
        /// <exception cref="InvalidOperationException">Caso ja exista um registro de um mesmo modulo marcado como instancia unica</exception>
        public static void Register(IModulo modulo)
        {
            if (!cache.Contains(modulo.Nome) && modulo.InstanciaUnica)
            {
                cache.Add(modulo.Nome);
            }
            else if (modulo.InstanciaUnica)
            {
                throw new InvalidOperationException("Modulo ja registrado");
            }

            if (!modules.ContainsKey(modulo.Id))
                modules.TryAdd(modulo.Id, new ModuloTipo(modulo));

        }

        /// <summary>
        /// Cria uma nova instancia do modulo
        /// </summary>
        /// <param name="type">Tipo do modulo</param>
        /// <param name="args">Argumentos para o construtor do modulo</param>
        /// <returns>Instancia do modulo</returns>
        /// <exception cref="ModuloInstanciaUnicaException">Caso ja exista um mesmo tipo como instancia unica</exception>
        public static IModulo Create(Type type, object[] args)
        {
            if (!cache.Contains(type.Name))
                return (IModulo)Activator.CreateInstance(type, args);
            else
                throw new ModuloInstanciaUnicaException("Ja existe um modulo de mesmo nome");
        }
        /// <summary>
        /// Desliga e remove o modulo do provedor
        /// </summary>
        /// <param name="modulo">Instancia do modulo</param>
        /// <returns>Retorna <see langword="true"/> caso tenha sido removido com sucesso, caso contrario <see langword="false"/></returns>
        public static bool Remove(IModulo modulo)
        {
            if (modules.TryRemove(modulo.Id, out IModuloTipo target) && !target.Coletado)
            {
                target.Dispose();
                cache.Remove(modulo.Nome);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Desliga e remove o modulo pelo id
        /// </summary>
        /// <param name="id">Id do modulo em execucao</param>
        /// <returns>Retorna <see langword="true"/> caso tenha sido removido com sucesso, caso contrario <see langword="false"/></returns>
        public static bool RemoveById(string id)
        {
            if (modules.TryRemove(id, out IModuloTipo target) && !target.Coletado)
            {
                cache.Remove((target.WeakReference.Target as IModulo).Nome);
                target.Dispose();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Desliga e remove todos os modulos do provedor.
        /// </summary>
        /// <remarks>
        /// Use com sabedoria
        /// </remarks>
        public static void Flush()
        {
            foreach (var item in modules.Where(x => !x.Value.Coletado))
            {
                item.Value.Dispose();
            }

            modules.Clear();
            cache.Clear();
        }
    }
}
