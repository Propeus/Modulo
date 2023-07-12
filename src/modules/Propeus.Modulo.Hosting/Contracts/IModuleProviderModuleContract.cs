using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Abstrato.Interfaces;

namespace Propeus.Modulo.Hosting.Contracts
{
    [ModuleContract("ModuleProviderModule")]
    internal interface IModuleProviderModuleContract : IModule
    {
        public event Action<Type> OnLoadModule;
        public event Action<Type> OnUnloadModule;
        public event Action<Type> OnRebuildModule;

        IEnumerable<Type> GetAllModules();
    }
}
