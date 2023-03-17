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
    internal class InstanciaProvider : IDisposable
    {
        static InstanciaProvider _provider;
        public static InstanciaProvider Provider
        {
            get
            {
                if (_provider is null || _provider.disposedValue)
                {
                    _provider = new InstanciaProvider();
                }

                return _provider;
            }
        }

        //K:Id | V:modulo
        ConcurrentDictionary<string, IModuloTipo> modules = new ConcurrentDictionary<string, IModuloTipo>();
        //K:id
        HashSet<string> cache = new HashSet<string>();

        public int ModulosRegistrados => modules.Count;
        public int ModulosAtivos => Get().Count();

        /// <summary>
        /// Verifica se o tipo informado esta em cache
        /// </summary>
        /// <param name="name">Nome do modulo</param>
        /// <returns><see langword="true"/> caso exista</returns>
        public bool HasCache(string name)
        {
            return cache.Contains(name);
        }

        /// <summary>
        /// Verifica se existe algum modulo pelo nome
        /// </summary>
        /// <param name="name">Nome do modulo</param>
        /// <returns><see langword="true"/> caso exista</returns>
        public bool HasByName(string name)
        {
            return cache.Contains(name) || modules.Values.Any(x => x.TipoModulo.Name == name);
        }

        /// <summary>
        /// Verifica se existe algum modulo com o id informado
        /// </summary>
        /// <param name="id">Id do modulo</param>
        /// <returns><see langword="true"/> caso exista</returns>
        public bool HasById(string id)
        {
            return cache.Contains(id) || modules.ContainsKey(id);
        }

        /// <summary>
        /// Obtem o modulo pelo id informado
        /// </summary>
        /// <param name="id">Id do modulo</param>
        /// <returns><see cref="IModuloTipo"/> caso exista senao retorna <see langword="null"/></returns>
        public IModuloTipo GetById(string id)
        {
            modules.TryGetValue(id, out var tipo);
            return tipo;
        }

        /// <summary>
        /// Obtem o modulo pelo nome do modulo
        /// </summary>
        /// <param name="name">Nome do modulo</param>
        /// <returns><see cref="IModuloTipo"/> caso exista senao retorna <see langword="null"/></returns>
        public IModuloTipo GetByName(string name)
        {
            return modules.Values.FirstOrDefault(x => x.TipoModulo.Name == name && !x.Coletado);
        }

        /// <summary>
        /// Retorna todos os modulos ativos
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IModuloTipo> Get()
        {
            return modules.Where(x => !x.Value.Elimindado).Select(x => x.Value);
        }

        /// <summary>
        /// Registra um novo modulo no provider
        /// </summary>
        /// <param name="modulo">Instancia do modulo</param>
        /// <exception cref="InvalidOperationException">Caso ja exista um registro de um mesmo modulo marcado como instancia unica</exception>
        public void Register(IModulo modulo)
        {
            if (!cache.Contains(modulo.Nome) && modulo.InstanciaUnica)
            {
                cache.Add(modulo.Nome);
            }
            else if (modulo.InstanciaUnica)
            {
                var ex = new InvalidOperationException("Modulo ja registrado");
                this.NotificarErro($"Modulo {modulo.Nome} ja possui uma instancia ativa", ex);
                throw ex;
            }

            if (!modules.ContainsKey(modulo.Id))
            {
                modules.TryAdd(modulo.Id, new ModuloTipo(modulo));
                this.NotificarInformacao($"Modulo {modulo.Id} registrado");
            }
        }

        /// <summary>
        /// Cria uma nova instancia do modulo
        /// </summary>
        /// <param name="type">Tipo do modulo</param>
        /// <param name="args">Argumentos para o construtor do modulo</param>
        /// <returns>Instancia do modulo</returns>
        /// <exception cref="ModuloInstanciaUnicaException">Caso ja exista um mesmo tipo como instancia unica</exception>
        public IModulo Create(Type type, object[] args)
        {
            if (!cache.Contains(type.Name))
            {

                var modulo = (IModulo)Activator.CreateInstance(type, args);
                this.NotificarInformacao($"Modulo {modulo.Id} criado com sucesso");
                return modulo;
            }
            else
            {
                this.NotificarInformacao("Ja existe uma instancia do modulo em execucao");
                throw new ModuloInstanciaUnicaException("Ja existe um modulo de mesmo nome");
            }
        }
        /// <summary>
        /// Desliga e remove o modulo do provedor
        /// </summary>
        /// <param name="modulo">Instancia do modulo</param>
        /// <returns>Retorna <see langword="true"/> caso tenha sido removido com sucesso, caso contrario <see langword="false"/></returns>
        public bool Remove(IModulo modulo)
        {
            if (modules.TryRemove(modulo.Id, out IModuloTipo target) && !target.Coletado)
            {
                target.Dispose();
                cache.Remove(modulo.Nome);
                this.NotificarInformacao($"Modulo {modulo.Nome}::{modulo.Id} removido com sucesso");
                return true;
            }
            this.NotificarAviso($"Modulo {modulo.Nome}::{modulo.Id} nao foi encontrado no provedor");
            return false;
        }
        /// <summary>
        /// Desliga e remove o modulo pelo id
        /// </summary>
        /// <param name="id">Id do modulo em execucao</param>
        /// <returns>Retorna <see langword="true"/> caso tenha sido removido com sucesso, caso contrario <see langword="false"/></returns>
        public bool RemoveById(string id)
        {
            if (modules.TryRemove(id, out IModuloTipo target) && !target.Elimindado)
            {
                cache.Remove(target.Modulo.Nome);
                target.Dispose();
                this.NotificarInformacao($"Modulo {target.Nome}::{target.Id} removido com sucesso");
                return true;
            }
            this.NotificarAviso($"Modulo {id} nao foi encontrado no provedor");
            return false;
        }

        /// <summary>
        /// Desliga e remove todos os modulos do provedor.
        /// </summary>
        /// <remarks>
        /// Use com sabedoria
        /// </remarks>
        public void Flush()
        {
            foreach (var item in modules.Where(x => !x.Value.Coletado))
            {
                this.NotificarInformacao($"Modulo {item.Value.Modulo.Nome}::{item.Value.Modulo.Id} removido com sucesso");
                item.Value.Dispose();
            }

            modules.Clear();
            cache.Clear();
        }

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Flush();
                    modules.Clear();
                    cache.Clear();
                }

                modules = null;
                cache = null;
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Não altere este código. Coloque o código de limpeza no método 'Dispose(bool disposing)'
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
