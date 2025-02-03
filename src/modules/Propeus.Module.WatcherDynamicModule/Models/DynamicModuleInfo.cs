using System.Text;

using Propeus.Module.Abstract;
using Propeus.Module.IL.Core.Geradores;

namespace Propeus.Module.WatcherDynamicModule.Models
{
    internal class DynamicModuleInfo : BaseModel
    {
        public DynamicModuleInfo()
        {
            ModuleProxy = new WeakReference<Type>(null);
            Module = new WeakReference<Type>(null);
            Contracts = new List<WeakReference<Type>>();
        }

        public DynamicModuleInfo(Type moduleType) : this()
        {
            this.ModuleName = moduleType.Name;
            Module = new WeakReference<Type>(moduleType);
        }

        public DynamicModuleInfo(string moduleName) : this()
        {
            this.ModuleName = moduleName;
        }

        public bool HasProxyTypeModule => ModuleProxy is not null && ModuleProxy.TryGetTarget(out _);

        public bool IsValid => Module.TryGetTarget(out _) || ModuleProxy.TryGetTarget(out _);
        public string ModuleName { get; set; }
        public WeakReference<Type> ModuleProxy { get; set; }
        public WeakReference<Type> Module { get; set; }
        public List<WeakReference<Type>> Contracts { get; set; }

        internal ILClasseProvider _proxyBuilder;
        internal string? _currentHash;


        public IEnumerable<string> GetContractNames()
        {
            foreach (WeakReference<Type> item in Contracts)
            {
                if (item.TryGetTarget(out Type? contract))
                {
                    yield return contract.Name;
                }
            }
        }

        public void AddContract(Type contract)
        {
            if (IndexOfContract(contract) == -1)
            {
                Contracts.Add(new WeakReference<Type>(contract));
            }
        }
        public void RemoveContract(Type contract)
        {
            int idx = IndexOfContract(contract);

            if (idx != -1)
            {
                Contracts.RemoveAt(idx);
            }

        }
        public int IndexOfContract(Type contract)
        {
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
        public bool ContainsContract(Type contract)
        {
            return IndexOfContract(contract) != -1;
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
