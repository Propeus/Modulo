using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Util;

namespace Propeus.Modulo.Abstrato.Proveders
{
    /// <summary>
    /// Informacoes do tipo
    /// </summary>
    class TypeInfo : IDisposable
    {
        private readonly Dictionary<string, WeakReference<Type>> contratos;

        public TypeInfo(Type type)
        {
            contratos = new Dictionary<string, WeakReference<Type>>();
            Referencia = new WeakReference<Type>(type);
            QuantidadeReferencia = 0;
            Criado = DateTime.Now;
            UltimaModificacao = default;

        }

        public WeakReference<Type> Referencia { get; private set; }
        public int QuantidadeReferencia { get; private set; }
        public DateTime Criado { get; }
        public DateTime? UltimaModificacao { get; private set; }

        public void AdicionarContrato(Type contrato)
        {
            if (!contratos.ContainsKey(contrato.FullName) && contrato.IsInterface && contrato.PossuiAtributo<ModuloContratoAttribute>())
            {
                contratos.Add(contrato.FullName, new WeakReference<Type>(contrato));
            }
        }

        public IEnumerable<Type> ObterContratos()
        {
            foreach (WeakReference<Type> item in contratos.Values)
            {
                if (item.TryGetTarget(out Type target))
                {
                    yield return target;
                }
            }
        }

        public void AtualizarReferencia(Type type)
        {
            Referencia.SetTarget(type);
            UltimaModificacao = DateTime.Now;
            QuantidadeReferencia = 0;
        }

        public Type GetReference()
        {
            if (Referencia.TryGetTarget(out Type target))
            {
                QuantidadeReferencia++;
                return target;
            }
            return null;
        }

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.Referencia = null;
                    this.QuantidadeReferencia = -1;
                    this.UltimaModificacao = DateTime.Now;
                    contratos.Clear();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

    /// <summary>
    /// Gerecia todas os tipos da aplicacao
    /// </summary>
    public class TypeProvider : IDisposable
    {
        static TypeProvider _provider;
        /// <summary>
        /// Obtem um provedor de tipos existente ou cria um novo
        /// </summary>
        public static TypeProvider Provider
        {
            get
            {
                if (_provider is null || _provider.disposedValue)
                {
                    _provider = new TypeProvider();
                }

                return _provider;
            }
        }

        private ConcurrentDictionary<string, TypeInfo> Types = new();
        TypeProvider()
        {
            string dir = Directory.GetCurrentDirectory();

            IEnumerable<Type> ls = AppDomain.CurrentDomain
               .GetAssemblies()
               .Where(x => x.Location.Contains(dir))//Filtrar para obter dll de um determinado diretorio
               .SelectMany(x => x.GetTypes())
               .Where(x => x.GetCustomAttribute<ModuloAttribute>() != null);

            foreach (Type item in ls)
            {
                _ = Types.TryAdd(item.Name, new TypeInfo(item));
            }
        }

        /// <summary>
        /// Obtem todos os tipos que estao marcados com o atributo <see cref="ModuloAutoInicializavelAttribute"/>
        /// </summary>
        /// <returns>Lista de tipos auto inicializavel</returns>
        public IEnumerable<Type> ObterAutoInicializavel()
        {
            foreach (KeyValuePair<string, TypeInfo> item in Types)
            {
                if (item.Value.Referencia.TryGetTarget(out Type target) && target.PossuiAtributo<ModuloAutoInicializavelAttribute>())
                {
                    yield return target;
                }
            }
        }
        /// <summary>
        /// Obtem todos os contratos pelo nome do tipo informado
        /// </summary>
        /// <param name="name">Nome do tipo que sera obtido os contratos</param>
        /// <returns>Lista de contratos</returns>
        public IEnumerable<Type> ObterContratos(string name)
        {
            return Types.ContainsKey(name) ? Types[name].ObterContratos() : Array.Empty<Type>();
        }

        /// <summary>
        /// Obtem todos os contratos pelo tipo informado
        /// </summary>
        /// <param name="tipo">Tipo que sera obtido os contratos</param>
        /// <returns>Lista de contratos</returns>
        public IEnumerable<Type> ObterContratos(Type tipo)
        {
            return Types.ContainsKey(tipo.Name) ? Types[tipo.Name].ObterContratos() : Array.Empty<Type>();
        }

        /// <summary>
        /// Adiciona uma nova interface de contrato no tipo informado
        /// </summary>
        /// <param name="name">Nome do tipo que ira receber o contrato</param>
        /// <param name="contrato">Interface de contrato</param>
        public void AdicionarCntrato(string name, Type contrato)
        {
            if (Types.ContainsKey(name))
            {
                Types[name].AdicionarContrato(contrato);
            }
        }

        /// <summary>
        /// Adiciona uma nova interface de contrato no tipo informado
        /// </summary>
        /// <param name="tipo">Tipo que ira receber o contrato</param>
        /// <param name="contrato">Interface de contrato</param>
        public void AdicionarContrato(Type tipo, Type contrato)
        {
            if (Types.ContainsKey(tipo.Name))
            {
                Types[tipo.Name].AdicionarContrato(contrato);
            }
        }

        /// <summary>
        /// Adiciona ou subistitui um tipo existente
        /// </summary>
        /// <param name="type">Tipo a ser adicionado ou atualizado</param>
        public void AddOrUpdate(Type type)
        {

            _ = Types.AddOrUpdate(type.Name, new TypeInfo(type), updateValueFactory: (o, n) =>
            {
                if (Types.TryGetValue(o, out TypeInfo value))
                {

                    if (Types[o].GetReference() == type)
                    {
                        return n;
                    }

                    value.AtualizarReferencia(type);
                    return Types[o];
                }
                else
                {
                    return new TypeInfo(type);
                }
            });


        }

        /// <summary>
        /// Retorna o tipo pelo nome
        /// </summary>
        /// <param name="name">Nome do tipo</param>
        /// <returns>Retorna o tipo do modulo se houver, caso contrario, <see langword="null"/></returns>
        public Type Get(string name)
        {
            return Types.TryGetValue(name, out TypeInfo value) ? value.GetReference() : null;
        }

        /// <summary>
        /// Obtem a quantidade de vezes que o tipo foi solicitado
        /// </summary>
        /// <param name="name">Nome do tipo</param>
        /// <returns>Quantidade de vezes que o tipo foi solicitado</returns>
        public int GetCount(string name)
        {
            return Types[name].QuantidadeReferencia;

        }

        /// <summary>
        /// Remove um tipo do provedor
        /// </summary>
        /// <param name="name">Nome do tipo</param>
        public void Remove(string name)
        {
            _ = Types.TryRemove(name, out TypeInfo target);
            target.Dispose();
        }

        private bool disposedValue;

        /// <summary>
        /// Remove todos os tipo carregados em memoria
        /// </summary>
        /// <param name="disposing">Indica se deve ser liberado os objetos gerenciaveis da memoria</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach (var item in Types.Values)
                    {
                        item.Dispose();
                    }

                    Types.Clear();
                }

                Types = null;
                disposedValue = true;
            }
        }

        /// <summary>
        /// Remove todos os tipos carregados em memoria
        /// </summary>
        public void Dispose()
        {
            // Não altere este código. Coloque o código de limpeza no método 'Dispose(bool disposing)'
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}