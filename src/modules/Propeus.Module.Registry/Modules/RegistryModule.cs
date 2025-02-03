using System.Collections.Concurrent;

using Propeus.Module.Abstract;
using Propeus.Module.Abstract.Attributes;
using Propeus.Module.Abstract.Interfaces;
using Propeus.Module.Registry.Contracts;
using Propeus.Module.Registry.Models;

namespace Propeus.Module.Registry.Modules
{
    /// <summary>
    /// Modulo paa armazenar e gerenciar informações de outros módulos
    /// </summary>
    /// <remarks>
    /// Inclusive o dele mesmo
    /// </remarks>
    [Module(Description = "Modulo para armazenar instancia e gerenciar informações de outros módulos", KeepAlive = false, Singleton = false, AutoStartable = false, AutoUpdate = false)]
    public class RegistryModule : BaseModule, IRegistryContract
    {
        //TODO: Criar uma funcao de sincronismo para evitar duplicidade de objetos entre modulos

        public RegistryModule()
        {
            modules = new ConcurrentDictionary<string, IModuleInformation>();
        }

        //K:Id | V:moduleInstance
        private readonly ConcurrentDictionary<string, IModuleInformation> modules;

        /// <inheritdoc/>
        public int InitializedModules { get; private set; }

        /// <inheritdoc/>
        public IModuleInformation RegisterModule(IModule module)
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
                //TODO: Analisar melhor uma forma de sobrepor um modulo singleton onde é adicionado mais um contrato
                var moduleRoot = GetModuleInformation(module.Id);
                if (module.GetType().Name.Contains("TEMP"))
                {
                    var moduleInfo = new ModuleInformation(module);
                    moduleRoot.ModulesTemporary.Add(moduleInfo);
                    return moduleInfo;
                }
                return moduleRoot;
            }

        }

        /// <inheritdoc/>
        public void UnregisterModule(string idModule)
        {
            if (modules.TryRemove(idModule, out IModuleInformation? moduleInfo))
            {
                moduleInfo.Dispose();
                InitializedModules--;
            }
        }

        /// <inheritdoc/>
        public IModuleInformation? GetModuleInformation(string IdModule)
        {
            if (modules.TryGetValue(IdModule, out IModuleInformation? moduleInformation))
            {
                return moduleInformation;
            }
            else
            {
                return null;
            }
        }

        /// <inheritdoc/>
        public IEnumerable<IModuleInformation> GetAllModulesInformation()
        {
            foreach (KeyValuePair<string, IModuleInformation> item in modules)
            {
                yield return item.Value;
            }
        }

        /// <inheritdoc/>
        public bool ExistsModule(string IdModule)
        {
            return modules.ContainsKey(IdModule);
        }
    }
}
