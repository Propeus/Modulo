using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Attributes;
using Propeus.Modulo.Abstrato.Exceptions;
using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Dinamico.Contracts;
using Propeus.Modulo.Dinamico.Models;
using Propeus.Modulo.Util.Atributos;
using Propeus.Modulo.Util.Thread;

namespace Propeus.Modulo.Dinamico.Modules
{
    /// <summary>
    /// Modulo para mapear e atualizar outros modulos em tempo de execucao
    /// </summary>
    [Module]
    public class ModuleWatcherModule : BaseModule, IModuleWatcherContract
    {
        ///<inheritdoc/>
        public event Action<Type>? OnLoadModule;
        ///<inheritdoc/>
        public event Action<Type>? OnUnloadModule;
        ///<inheritdoc/>
        public event Action<Type>? OnReloadModule;

        private ConcurrentDictionary<string, ModuleProviderInfo> _modulesInfo;
        private FileSystemWatcher _fileSystemWatcher;
        ///<inheritdoc/>
        public Type? this[string nameType]
        {
            get
            {
                IEnumerable<KeyValuePair<string, ModuleProviderInfo>> info = _modulesInfo.Where(x => x.Value.Modules.ContainsKey(nameType));
                if (info.Any() && info.Count() == 1 && info.First().Value.Modules[nameType].IsValid)
                {
                    if (info.First().Value.Modules[nameType].ModuleProxy is not null && info.First().Value.Modules[nameType].ModuleProxy.TryGetTarget(out Type? proxy))
                    {
                        return proxy;
                    }
                    if (info.First().Value.Modules[nameType].Module.TryGetTarget(out Type? target))
                    {
                        return target;
                    }
                }

                return default;


            }
            set
            {

                IEnumerable<KeyValuePair<string, ModuleProviderInfo>> info = _modulesInfo.Where(x => x.Value.Modules.ContainsKey(nameType));
                if (info.Any() && info.Count() == 1 && info.First().Value.Modules[nameType].IsValid)
                {
                    info.First().Value.Modules[nameType].ModuleProxy.SetTarget(value);
                }
            }
        }

        #region init
        /// <summary>
        /// Construtor padrao
        /// </summary>
        /// <param name="moduleManager">Gerenciador de modulos</param>
        public ModuleWatcherModule(IModuleManager moduleManager) : base(true)
        {
            _moduleManager = moduleManager;
        }


        /// <summary>
        /// Metodo para criar instancias de objetos essenciais para o funcionamento do modulo
        /// </summary>
        public void CriarInstancia() => CriarInstancia(null, null, null);
        /// <summary>
        /// Metodo para criar instancias de objetos essenciais para o funcionamento do modulo
        /// </summary>
        public void CriarInstancia(Action<Type>? onLoadModule) => CriarInstancia(onLoadModule, null, null);
        /// <summary>
        /// Metodo para criar instancias de objetos essenciais para o funcionamento do modulo
        /// </summary>
        public void CriarInstancia(Action<Type>? onLoadModule, Action<Type>? onReloadModule) => CriarInstancia(onLoadModule, onReloadModule, null);

        /// <summary>
        /// Metodo para criar instancias de objetos essenciais para o funcionamento do modulo
        /// </summary>
        public void CriarInstancia(Action<Type>? onLoadModule, Action<Type>? onReloadModule, Action<Type>? onUnloadModule)
        {
            _currentDirectory = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
            _assmLibsPath = AppDomain.CurrentDomain.GetAssemblies().Select(x => x.Location).Distinct();
            _modulesInfo = new ConcurrentDictionary<string, ModuleProviderInfo>();


            if (onLoadModule != null)
            {
                OnLoadModule += onLoadModule;
            }
            if (onReloadModule != null)
            {
                OnReloadModule += onReloadModule;
            }
            if (onUnloadModule != null)
            {
                OnUnloadModule += onUnloadModule;
            }

            _fileSystemWatcher = new FileSystemWatcher
            {
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.Size

            };
            _fileSystemWatcher.Changed += _fileSystemWatcher_OnEvent;
            _fileSystemWatcher.Created += _fileSystemWatcher_OnEvent;
            _fileSystemWatcher.Deleted += _fileSystemWatcher_OnEvent;
            _fileSystemWatcher.Renamed += _fileSystemWatcher_Renamed;
            _fileSystemWatcher.Filter = "*.dll";
            _fileSystemWatcher.IncludeSubdirectories = true;
            _fileSystemWatcher.Path = _currentDirectory;
            _fileSystemWatcher.EnableRaisingEvents = true;

        }
        /// <summary>
        /// Metodo para configuracao do modulo
        /// </summary>
        public void CriarConfiguracao()
        {
            foreach (string modulePath in _assmLibsPath)
            {
                if (string.IsNullOrEmpty(modulePath))
                {
                    continue;  //Por algum motivo a lista esta vindo com caminhos vazios
                }

                FileInfo fi = new FileInfo(modulePath);
                ModuleProviderInfo mp = new ModuleProviderInfo(modulePath, true, _moduleManager);
                _fileSystemWatcher_OnEvent(mp, new FileSystemEventArgs(WatcherChangeTypes.Created, fi.Directory.FullName, fi.Name));
            }
            foreach (string? modulePath in Directory.GetFiles(_currentDirectory, "*.dll", SearchOption.AllDirectories).Except(_assmLibsPath))
            {
                FileInfo fi = new FileInfo(modulePath);
                _fileSystemWatcher_OnEvent(null, new FileSystemEventArgs(WatcherChangeTypes.Created, fi.Directory.FullName, fi.Name));
            }

            State = Abstrato.State.Ready;
        }
        #endregion

        #region Eventos & Funcoes
        private void _fileSystemWatcher_Renamed(object sender, RenamedEventArgs e)
        {
            if (_modulesInfo.ContainsKey(e.OldFullPath))
            {
                if (_modulesInfo.TryGetValue(e.FullPath, out ModuleProviderInfo target))
                {
                    target.Reload(e.FullPath);
                }
            }
            else
            {
                //Tem algo de errado, como mudou se nao existe?
                throw new NotImplementedException();
            }
        }

        private void _fileSystemWatcher_OnEvent(object sender, FileSystemEventArgs e)
        {
            switch (e.ChangeType)
            {
                case WatcherChangeTypes.Created:
                    ModuleProviderInfo mpf = sender as ModuleProviderInfo ?? new ModuleProviderInfo(e.FullPath, false, _moduleManager);
                    if (_modulesInfo.TryAdd(e.FullPath, mpf))
                    {
                        mpf.Load();
                    }
                    else if (_modulesInfo.TryGetValue(e.FullPath, out ModuleProviderInfo target))
                    {
                        target.Reload();
                    }
                    foreach (var item in mpf.Modules)
                    {
                        if (item.Value.ModuleProxy.TryGetTarget(out var target))
                        {
                            OnLoadModule?.Invoke(target);
                        }
                        else if (item.Value.Module.TryGetTarget(out target))
                        {
                            OnLoadModule?.Invoke(target);
                        }
                    }

                    break;
                case WatcherChangeTypes.Deleted:
                    if (_modulesInfo.TryRemove(e.FullPath, out mpf))
                    {
                        foreach (var item in mpf.Modules)
                        {
                            if (item.Value.ModuleProxy.TryGetTarget(out var target))
                            {
                                OnUnloadModule?.Invoke(target);
                            }
                            else if (item.Value.Module.TryGetTarget(out target))
                            {
                                OnUnloadModule?.Invoke(target);
                            }
                        }

                        //Adicionar semaforo
                    }
                    break;
                case WatcherChangeTypes.Changed:
                    if (_flgWatcherChange)
                    {
                        return;
                    }
                    else
                    {
                        _flgWatcherChange = true;
                    }

                    if (_modulesInfo.ContainsKey(e.FullPath))
                    {
                        if (_modulesInfo.TryGetValue(e.FullPath, out mpf))
                        {
                            if (mpf.Reload())
                            {
                                foreach (var item in mpf.Modules)
                                {
                                    if (item.Value.ModuleProxy.TryGetTarget(out var target))
                                    {
                                        OnReloadModule?.Invoke(target);
                                    }
                                    else if (item.Value.Module.TryGetTarget(out target))
                                    {
                                        OnReloadModule?.Invoke(target);
                                    }
                                }
                            }

                        }
                    }
                    else if (_assmLibsPath.Contains(e.FullPath))
                    {
                        mpf = new ModuleProviderInfo(e.FullPath, true, _moduleManager);
                        if (_modulesInfo.TryAdd(e.FullPath, mpf))
                        {
                            mpf.Load();
                            foreach (var item in mpf.Modules)
                            {
                                if (item.Value.ModuleProxy.TryGetTarget(out var target))
                                {
                                    OnLoadModule?.Invoke(target);
                                }
                                else if (item.Value.Module.TryGetTarget(out target))
                                {
                                    OnLoadModule?.Invoke(target);
                                }
                            }
                        }
                    }
                    else
                    {
                        mpf = sender as ModuleProviderInfo ?? new ModuleProviderInfo(e.FullPath, false, _moduleManager);
                        if (_modulesInfo.TryAdd(e.FullPath, mpf))
                        {
                            mpf.Load();
                        }
                        else
                        {
                            mpf.Reload();
                        }
                    }

                    _flgWatcherChange = false;
                    break;
                case WatcherChangeTypes.All:
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        #endregion

        #region Variaveis
        private IEnumerable<string> _assmLibsPath;
        private string _currentDirectory;

        private IModuleManager _moduleManager;
        private bool _flgWatcherChange;
        #endregion

        #region Funcoes
        ///<inheritdoc/>
        public Type GetModuleFromContract(Type contractType)
        {
            if (contractType.PossuiAtributo<ModuleContractAttribute>())
            {
                ModuleContractAttribute moduleContractAttribute = contractType.GetCustomAttribute<ModuleContractAttribute>();


                foreach (KeyValuePair<string, ModuleProviderInfo> moduleInfo in _modulesInfo)
                {

                    foreach (KeyValuePair<string, ModuleInfo> module in moduleInfo.Value.Modules)
                    {
                        if (module.Value.ModuleName == moduleContractAttribute.ModuleName && module.Value.IsValid)
                        {
                            module.Value.AddContract(contractType);
                            module.Value.BuildModuleProxy();

                            if (module.Value.ModuleProxy.TryGetTarget(out Type? target))
                            {
                                return target;
                            }


                        }


                    }
                }


                throw new ModuleContractInvalidException();

            }
            else
            {
                throw new ModuleContractNotFoundException(contractType);
            }



        }


        /// <summary>
        /// Obtem todos tipos de modulos validos
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Type> GetAllModules()
        {
            foreach (KeyValuePair<string, ModuleProviderInfo> moduleInfo in _modulesInfo)
            {

                foreach (KeyValuePair<string, ModuleInfo> module in moduleInfo.Value.Modules)
                {

                    if (module.Value.ModuleProxy.TryGetTarget(out Type? moduleTypeProxy))
                    {
                        yield return moduleTypeProxy;
                    }

                    if (module.Value.Module.TryGetTarget(out Type? moduleType))
                    {
                        yield return moduleType;
                    }

                }
            }
        }


        #endregion


    }

}
