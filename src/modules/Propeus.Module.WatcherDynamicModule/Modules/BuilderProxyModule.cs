using System.Text;

using Propeus.Module.Abstract;
using Propeus.Module.Abstract.Attributes;
using Propeus.Module.Abstract.Helpers;
using Propeus.Module.IL.Core.Geradores;
using Propeus.Module.IL.Core.Helpers;
using Propeus.Module.WatcherDynamicModule.Contracts;
using Propeus.Module.WatcherDynamicModule.Models;

namespace Propeus.Module.WatcherDynamicModule.Modules
{
    /// <summary>
    /// Classe responsável por criar proxy dinamico das classes
    /// </summary>
    [Module(Description = "Modulo para gerar proxy dinamico de outros modulos", AutoUpdate = false, AutoStartable = false, KeepAlive = false, Singleton = false)]
    public class BuilderProxyModule : BaseModule, IBuilderProxyContract
    {

        private ILGerador _iLGerador;

        /// <summary>
        /// Construtor padrão
        /// </summary>
        public BuilderProxyModule()
        {
            GeradorHelper.GetCurrentInstanceOrNew(out _iLGerador);
        }

        /// <summary>
        /// Criar um novo proxy para o modulo especificado
        /// </summary>
        /// <param name="module">Modulo alvo</param>
        /// <param name="contracts">Contratos a serem implementados no proxy</param>
        /// <param name="currentHash">Hash do proxy atual, se houver</param>
        /// <param name="moduleProxy">Proxy atual do modulo, se houver</param>
        public void BuildModuleProxy(WeakReference<Type> module, List<WeakReference<Type>>? contracts, ref string? currentHash, WeakReference<Type> moduleProxy)
        {
            if (contracts is null || contracts.Count == 0)
            {
                return;
            }

            string aux = HashContracts(contracts);
            if (currentHash != aux)
            {
                currentHash = aux;
            }
            else
            {
                return;
            }

            if (module.TryGetTarget(out Type? target))
            {
                ILClasseProvider proxyBuilder;
                if (_iLGerador.Modulo.ExisteProxyClasse(target))
                {
                    if (target.GetModuleAttribute().Singleton)
                    {
                        proxyBuilder = _iLGerador.Modulo.CriarOuObterClasseTemporaria(target, GetContractsType(contracts).ToArray());
                    }
                    else
                    {
                    proxyBuilder = _iLGerador.Modulo.ObterProxyClasse(target).NewVersion(interfaces: GetContractsType(contracts).ToArray())
                        .CriarProxyClasse(target);
                    }
                }
                else
                {
                    proxyBuilder = _iLGerador.Modulo.CriarProxyClasse(target, GetContractsType(contracts).ToArray());
                }

                proxyBuilder.Apply();
                moduleProxy.SetTarget(proxyBuilder.ObterTipoGerado());
            }
        }

        /// <summary>
        /// Criar um novo proxy para o modulo especificado
        /// </summary>
        internal void BuildModuleProxy(DynamicModuleInfo dynamicModuleInfo)
        {
            BuildModuleProxy(dynamicModuleInfo.Module, dynamicModuleInfo.Contracts, ref dynamicModuleInfo._currentHash, dynamicModuleInfo.ModuleProxy);
        }

        private IEnumerable<Type> GetContractsType(List<WeakReference<Type>>? contracts)
        {
            if (contracts is null)
            {
                yield break;
            }

            foreach (WeakReference<Type> item in contracts)
            {
                if (item.TryGetTarget(out Type? contract))
                {
                    yield return contract;
                }
            }
        }

        private string HashContracts(List<WeakReference<Type>> contracts)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (WeakReference<Type> item in contracts)
            {
                if (item.TryGetTarget(out Type? c))
                {
                    stringBuilder.Append(c.GUID.ToString());
                }
            }
            return stringBuilder.ToString();
        }
    }
}
