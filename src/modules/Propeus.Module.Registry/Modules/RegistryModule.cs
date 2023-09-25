using System.Collections.Concurrent;

using Propeus.Module.Abstract;
using Propeus.Module.Abstract.Attributes;
using Propeus.Module.Abstract.Interfaces;
using Propeus.Module.Registry.Contracts;
using Propeus.Module.Registry.Models;

namespace Propeus.Module.Registry.Modules
{

    [Module(KeepAlive = false, Singleton = false, AutoStartable = false, AutoUpdate = false)]
    public class RegistryModule : BaseModule, IRegistryContract
    {
        //TODO: Criar uma funcao de sincronismo para evitar duplicidade de objetos entre modulos

        public RegistryModule()
        {
            modules = new ConcurrentDictionary<string, IModuleInfo>();
        }

        //K:Id | V:moduleInstance
        private readonly ConcurrentDictionary<string, IModuleInfo> modules;

        public int InitializedModules { get; private set; }

        public IModuleInfo RegisterModule(IModule module)
        {
            if (!ExistsModule(module.Id))
            {
                var moduleInfo = new ModuleInformation(module);
                if (modules.TryAdd(module.Id, new ModuleInformation(module)))
                {
                    InitializedModules++;
                }
                return moduleInfo;
            }
            else
            {
                return GetModuleInformation(module.Id);
            }

        }

        public void UnregisterModule(string idModule)
        {
            if (modules.TryRemove(idModule, out var moduleInfo))
            {
                moduleInfo.Dispose();
                InitializedModules--;
            }
        }

        public IModuleInfo GetModuleInformation(string IdModule)
        {
            if (modules.TryGetValue(IdModule, out IModuleInfo moduleInformation))
            {
                return moduleInformation;
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<IModuleInfo> GetAllModulesInformation()
        {
            foreach (var item in modules)
            {
                yield return item.Value;
            }
        }

        public bool ExistsModule(string IdModule)
        {
            return modules.ContainsKey(IdModule);
        }
    }
}
