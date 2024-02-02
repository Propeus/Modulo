using Propeus.Module.Abstract;
using Propeus.Module.Abstract.Attributes;
using Propeus.Module.Package.Contracts;

namespace Propeus.Module.Package.Modules
{
    [Module(Description = "Modulo para controlar dependência de módulos", AutoStartable = false, AutoUpdate = false, KeepAlive = false, Singleton = true)]
    public class ModuleDependeceModule : BaseModule, IModuleDependeceContract
    {

        private readonly IAssemblyLoadContextContract _assemblyLoadContext;

        /// <summary>
        /// Chave e Valores. A chave é a dependência e os valores são os módulos que a usam.
        /// </summary>
        Dictionary<string, List<string>> _dependenceModules;
        Dictionary<string, string> _dependecePath;
        public ModuleDependeceModule(IAssemblyLoadContextContract assemblyLoadContext)
        {
            _assemblyLoadContext = assemblyLoadContext;
            _dependenceModules = new Dictionary<string, List<string>>();
            _dependecePath = new Dictionary<string, string>();
        }


        public void RegisterDependencePackage(string dependenceName, string fullPathDependence)
        {
            _dependenceModules.Add(dependenceName, new List<string>());

            _dependecePath.Add(dependenceName, fullPathDependence);
            _assemblyLoadContext.RegisterAssemblyLoadContext(fullPathDependence);
        }

        public void AddDependencePackage(string dependenceName, string fullPathModule)
        {
            if (_dependenceModules.ContainsKey(dependenceName))
            {
                _dependenceModules[dependenceName].Add(fullPathModule);
            }
        }

        public void RemoveDependencePackAge(string dependenceName, string fullPathModule)
        {
            if (_dependenceModules.ContainsKey(dependenceName))
            {
                _dependenceModules[dependenceName].Remove(fullPathModule);
            }
            UnloadAssemblyIfDependenceNotExists(dependenceName);
        }

        private void UnloadAssemblyIfDependenceNotExists(string dependenceName)
        {
            if (_dependenceModules[dependenceName].Count == 0)
            {
                _assemblyLoadContext.UnregisterAssemblyLoadContext(_dependecePath[dependenceName]);
                _dependecePath.Remove(dependenceName);
            }
        }

        protected override void Dispose(bool disposing)
        {
            foreach (var item in _dependenceModules)
            {
                UnloadAssemblyIfDependenceNotExists(item.Key);
            }

            base.Dispose(disposing);
        }
    }
}
