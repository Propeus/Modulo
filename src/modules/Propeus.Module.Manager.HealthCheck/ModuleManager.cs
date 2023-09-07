using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Propeus.Module.Abstract;
using Propeus.Module.Abstract.Attributes;
using Propeus.Module.Abstract.Exceptions;
using Propeus.Module.Abstract.Helpers;
using Propeus.Module.Abstract.Interfaces;
using Propeus.Module.Utils.Atributos;
using Propeus.Module.Utils.Objetos;
using Propeus.Module.Utils.Tipos;

namespace Propeus.Module.Manager.HealthCheck
{

    /// <summary>
    /// Controlador de módulos
    /// </summary>
    /// <example>
    /// Para iniciar este gerenciador deve utilizar o método de extensão chamado <see cref="ModuleManagerExtensions.CreateModuleManager"/>
    /// <code>
    ///  using (gerenciador = ModuleManagerExtensions.CreateModuleManager())
    ///  {
    ///     //Seu código aqui...
    ///  }
    /// </code>
    /// </example>
    [Module(Singleton = true, AutoStartable = false, AutoUpdate = false, KeepAlive = true, Description = "Proxy de gerenciador para monitorar recursos utilizados")]
    public class ModuleManagerHealthCheck : BaseModule, IModuleManager, IModuleManagerHealthCheck
    {
        private readonly IModuleManager _moduleManager;

        /// <inheritdoc/>
        public ModuleManagerHealthCheck(IModuleManager moduleManager)
        {
            _moduleManager = moduleManager;
        }
        /// <inheritdoc/>
        public event Action<ModuleManagerHealthCheckEvent>? ModuleManagerEvent;
        /// <inheritdoc/>
        public event Action<IModule>? ModuleCreated;
        /// <inheritdoc/>
        public event Action? ModuleRemoved;
        /// <inheritdoc/>
        public event Action<IModule>? ModuleRecycled;

        /// <inheritdoc/>
        public DateTime StartDate => _moduleManager.StartDate;
        /// <inheritdoc/>
        public DateTime LastUpdate => _moduleManager.LastUpdate;
        /// <inheritdoc/>
        public int InitializedModules => _moduleManager.InitializedModules;

        /// <inheritdoc/>
        public T CreateModule<T>() where T : IModule
        {
            var module = _moduleManager.CreateModule<T>();
            ModuleCreated?.Invoke(module);
            ModuleManagerEvent?.Invoke(ModuleManagerHealthCheckEvent.Created);
            return module;
        }
        /// <inheritdoc/>
        public IModule CreateModule(Type moduleType)
        {
            var module = _moduleManager.CreateModule(moduleType);
            ModuleCreated?.Invoke(module);
            ModuleManagerEvent?.Invoke(ModuleManagerHealthCheckEvent.Created);
            return module;
        }
        /// <inheritdoc/>
        public IModule CreateModule(string moduleName)
        {
            var module = _moduleManager.CreateModule(moduleName);
            ModuleCreated?.Invoke(module);
            ModuleManagerEvent?.Invoke(ModuleManagerHealthCheckEvent.Created);
            return module;
        }
        /// <inheritdoc/>
        public void RemoveModule(string idModule)
        {
            _moduleManager.RemoveModule(idModule);
            ModuleManagerEvent?.Invoke(ModuleManagerHealthCheckEvent.Removed);
            ModuleRemoved?.Invoke();
        }
        /// <inheritdoc/>
        public void RemoveModule<T>(T moduleInstance) where T : IModule
        {
            _moduleManager.RemoveModule(moduleInstance);
            ModuleManagerEvent?.Invoke(ModuleManagerHealthCheckEvent.Removed);
            ModuleRemoved?.Invoke();
        }
        /// <inheritdoc/>
        public T GetModule<T>() where T : IModule
        {
            var module = _moduleManager.GetModule<T>();
            ModuleManagerEvent?.Invoke(ModuleManagerHealthCheckEvent.Obtained);
            return module;
        }
        /// <inheritdoc/>
        public IModule GetModule(Type moduleType)
        {
            var module = _moduleManager.GetModule(moduleType);
            ModuleManagerEvent?.Invoke(ModuleManagerHealthCheckEvent.Obtained);
            return module;
        }
        /// <inheritdoc/>
        public IModule GetModule(string idModule)
        {
            var module = _moduleManager.GetModule(idModule);
            ModuleManagerEvent?.Invoke(ModuleManagerHealthCheckEvent.Obtained);
            return module;
        }
        /// <inheritdoc/>
        public bool ExistsModule(IModule moduleInstance)
        {
            return _moduleManager.ExistsModule(moduleInstance);
        }
        /// <inheritdoc/>
        public bool ExistsModule(Type moduleType)
        {
            return _moduleManager.ExistsModule(moduleType);
        }
        /// <inheritdoc/>
        public bool ExistsModule(string idModule)
        {
            return _moduleManager.ExistsModule(idModule);
        }
        /// <inheritdoc/>
        public T RecycleModule<T>(T moduleInstance) where T : IModule
        {
            var module = _moduleManager.RecycleModule(moduleInstance);
            ModuleRecycled?.Invoke(module);
            ModuleManagerEvent?.Invoke(ModuleManagerHealthCheckEvent.Recycled);
            return module;
        }
        /// <inheritdoc/>
        public IModule RecycleModule(string idModule)
        {
            var module = _moduleManager.RecycleModule(idModule);
            ModuleRecycled?.Invoke(module);
            ModuleManagerEvent?.Invoke(ModuleManagerHealthCheckEvent.Recycled);
            return module;
        }
        /// <inheritdoc/>
        public IEnumerable<IModule> ListAllModules()
        {
            var modules = _moduleManager.ListAllModules();
            ModuleManagerEvent?.Invoke(ModuleManagerHealthCheckEvent.Listed);
            return modules;
        }
        /// <inheritdoc/>
        public void KeepAliveModule(IModule moduleInstance)
        {
            _moduleManager.KeepAliveModule(moduleInstance);
        }

        public T CreateModule<T>(object[] args) where T : IModule
        {
            return _moduleManager.CreateModule<T>(args);
        }

        public IModule CreateModule(Type moduleType, object[] args)
        {
            return _moduleManager.CreateModule(moduleType, args);
        }

        public IModule CreateModule(string moduleName, object[] args)
        {
            return _moduleManager.CreateModule(moduleName, args);
        }
    }
}
