using System.Collections.Concurrent;
using System.Reflection;

using Propeus.Module.Abstract;
using Propeus.Module.Abstract.Attributes;
using Propeus.Module.Abstract.Exceptions;
using Propeus.Module.Abstract.Interfaces;
using Propeus.Module.Utils.Atributos;
using Propeus.Module.Watcher.Contracts;
using Propeus.Module.Watcher.Models;

namespace Propeus.Module.Watcher.Modules
{
    /// <summary>
    /// Module para mapear e atualizar outros modulos em tempo de execucao
    /// </summary>
    [Module(Singleton = true, AutoUpdate = false, AutoStartable = false, KeepAlive = true)]
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
        SemaphoreSlim _semaphoreSlimChangeFile;

        #region init
        /// <summary>
        /// ObjectBuilder padrao
        /// </summary>
        /// <param name="moduleManager">Gerenciador de modulos</param>
        public ModuleWatcherModule(IModuleManager moduleManager, Action<Type>? onLoadModule, Action<Type>? onReloadModule, Action<Type>? onUnloadModule) : base()
        {
            _semaphoreSlimChangeFile = new SemaphoreSlim(1);
            _moduleManager = moduleManager;
            _listNameIgnoreModules = new List<string>() {
            "Microsoft",
            "System"
            };

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
            _fileSystemWatcher.IncludeSubdirectories = false; //Desabilitado pois o fine coverage copia as dll, duplicando e causando erros inesperados
            _fileSystemWatcher.Path = _currentDirectory;
        }

        ///<inheritdoc/>
        public override void ConfigureModule()
        {
            foreach (string? modulePath in Directory.GetFiles(_currentDirectory, searchPattern: "*.dll", SearchOption.TopDirectoryOnly))
            {
                FileInfo fi = new FileInfo(modulePath);
                ModuleProviderInfo mp = new ModuleProviderInfo(modulePath, _assmLibsPath.Contains(modulePath), _moduleManager, _listNameIgnoreModules);
                _fileSystemWatcher_OnEvent(mp, new FileSystemEventArgs(WatcherChangeTypes.Created, fi.Directory.FullName, fi.Name));
            }

            base.ConfigureModule();
        }

        ///<inheritdoc/>
        public override void Launch()
        {
            _fileSystemWatcher.EnableRaisingEvents = true;
            base.Launch();
        }


        #endregion

        ///<inheritdoc/>
        public Type? this[string nameType]
        {
            get
            {
                IEnumerable<KeyValuePair<string, ModuleProviderInfo>> info = _modulesInfo.Where(x => x.Value.Modules.ContainsKey(nameType) && x.Value.Modules[nameType].IsValid);
                if (info.Count() == 1)
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
                else if (info.Count() > 1)
                {
                    List<Type> modulosduplicados = new List<Type>();
                    foreach (var item in info)
                    {
                        if (item.Value.Modules[nameType].ModuleProxy is not null && item.Value.Modules[nameType].ModuleProxy.TryGetTarget(out Type? proxy))
                        {
                            modulosduplicados.Add(proxy);
                        }
                        if (item.Value.Modules[nameType].Module.TryGetTarget(out Type? target))
                        {
                            modulosduplicados.Add(target);
                        }
                    }
                    throw new ModuleAmbiguousException(modulosduplicados);
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
            ModuleProviderInfo mpf = sender as ModuleProviderInfo ?? new ModuleProviderInfo(e.FullPath, _assmLibsPath.Contains(e.FullPath), _moduleManager, _listNameIgnoreModules);
            switch (e.ChangeType)
            {
                case WatcherChangeTypes.Created:
                    if ((_modulesInfo.TryAdd(e.FullPath, mpf) && mpf.Load()) || (_modulesInfo.TryGetValue(e.FullPath, out mpf) && mpf.Reload()))
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
                    _semaphoreSlimChangeFile.Wait();

                    if ((_modulesInfo.TryAdd(e.FullPath, mpf) && mpf.Load()) || (_modulesInfo.TryGetValue(e.FullPath, out mpf) && mpf.Reload()))
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
                    _semaphoreSlimChangeFile.Release();
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
        private List<string> _listNameIgnoreModules;
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
