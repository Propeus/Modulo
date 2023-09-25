using System.Text;

using Propeus.Module.Abstract;
using Propeus.Module.Abstract.Attributes;
using Propeus.Module.IL.Core.Geradores;
using Propeus.Module.IL.Core.Helpers;

namespace Propeus.Module.Watcher.Models
{
    internal class ModuleInfo : BaseModel
    {
        public ModuleInfo()
        {
            ModuleProxy = new WeakReference<Type>(null);
            Module = new WeakReference<Type>(null);

        }

        public bool HasProxyTypeModule => ModuleProxy is not null && ModuleProxy.TryGetTarget(out _);

        public bool IsValid => Module.TryGetTarget(out _) || ModuleProxy.TryGetTarget(out _);
        public string ModuleName { get; set; }
        public WeakReference<Type> ModuleProxy { get; set; }
        public WeakReference<Type> Module { get; set; }
        public List<WeakReference<Type>>? Contracts { get; set; }

        internal ILClasseProvider _proxyBuilder;
        private string? _currentHash;

        public void BuildModuleProxy()
        {
            if (Contracts is null)
            {
                return;
            }

            string aux = HashContracts();
            if (_currentHash != aux)
            {
                _currentHash = aux;
            }
            else
            {
                return;
            }

            if (Module.TryGetTarget(out Type target))
            {
                if (_proxyBuilder is null)
                {
                    _proxyBuilder = GeradorHelper.Modulo.CriarOuObterProxyClasse(target, GetContractsType().ToArray(), new Type[] { typeof(ModuleAttribute) });
                }
                else
                {
                    _proxyBuilder.NovaVersao(interfaces: GetContractsType().ToArray()).CriarProxyClasse(target, GetContractsType().ToArray());
                }

                _proxyBuilder.Executar();
                ModuleProxy.SetTarget(_proxyBuilder.ObterTipoGerado());
            }
        }
        public IEnumerable<string> GetContractNames()
        {
            if (Contracts is null)
            {
                yield break;
            }

            foreach (WeakReference<Type> item in Contracts)
            {
                if (item.TryGetTarget(out Type? contract))
                {
                    yield return contract.Name;
                }
            }
        }
        public IEnumerable<Type> GetContractsType()
        {
            if (Contracts is null)
            {
                yield break;
            }

            foreach (WeakReference<Type> item in Contracts)
            {
                if (item.TryGetTarget(out Type? contract))
                {
                    yield return contract;
                }
            }
        }
        public void AddContract(Type contract)
        {
            if (ContainsContract(contract) == -1)
            {
                Contracts.Add(new WeakReference<Type>(contract));
            }
        }
        public void RemoveContract(Type contract)
        {
            int idx = ContainsContract(contract);

            if (idx != -1)
            {
                Contracts.RemoveAt(idx);
            }

        }
        public int ContainsContract(Type contract)
        {
            if (Contracts is null)
            {
                Contracts = new List<WeakReference<Type>>();
            }

            int idx = -1;
            foreach (WeakReference<Type> item in Contracts)
            {
                if (item.TryGetTarget(out Type? c) && c == contract)
                {
                    idx = Contracts.IndexOf(item);
                }
            }
            return idx;
        }
        private string HashContracts()
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (WeakReference<Type> item in Contracts)
            {
                if (item.TryGetTarget(out Type? c))
                {
                    stringBuilder.Append(c.GUID.ToString());
                }
            }
            return stringBuilder.ToString();
        }
        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                _proxyBuilder.Dispose();
            }
            base.Dispose(disposing);
        }
    }

}
