using System.Collections.Concurrent;
using System.Reflection;

using Propeus.Module.Abstract;
using Propeus.Module.Abstract.Attributes;
using Propeus.Module.Abstract.Exceptions;
using Propeus.Module.Abstract.Interfaces;
using Propeus.Module.AssmblyLoadContext.Contracts;
using Propeus.Module.AssmblyLoadContext.Modules;
using Propeus.Module.Utils.Atributos;
using Propeus.Module.Watcher.Contracts;
using Propeus.Module.Watcher.Models;

namespace Propeus.Module.Watcher.Modules
{
    /// <summary>
    /// Module para mapear e atualizar outros modulos em tempo de execucao
    /// </summary>
    [Module(Description ="Monitora a alteração de estado dos modulos em arquivo", Singleton = true, AutoUpdate = false, AutoStartable = false, KeepAlive = true)]
    public class ModuleWatcherModule : BaseModule, IModuleWatcherContract
    {
        const string DISABLE_MODULE_EXTENSION = ".disable";

        ///<inheritdoc/>
        public event Action<Type>? OnLoadModule;
        ///<inheritdoc/>
        public event Action<Type>? OnUnloadModule;
        ///<inheritdoc/>
        public event Action<Type>? OnReloadModule;

        private ConcurrentDictionary<string, ModuleProviderInfo> _modulesInfo;
        private FileSystemWatcher _fileSystemWatcher;
        SemaphoreSlim _semaphoreSlimChangeFile;

        #region Variaveis
        private IEnumerable<string> _assmLibsPath;
        private string _currentDirectory;
        private string _currentFolderModules;
        private IModuleManager _moduleManager;
        private IAssemblyLoadContextContract _assemblyLoadContextContract;
        private List<string> _listNameIgnoreModules;
        private bool _flgWatcherChange;
        #endregion

        #region init
        /// <summary>
        /// Inicia a instancia do modulo para observar mudanças nos arquivos de modulo
        /// </summary>
        /// <param name="moduleManager">Gerenciador de módulos atual</param>
        /// <param name="onLoadModule">Callback para carregamento de modulo</param>
        /// <param name="onReloadModule">Callback para recarregamento de modulo</param>
        /// <param name="onUnloadModule">Callback para descarregamento de modulo</param>
        /// <param name="folderModules">Nome da pasta a ser observado</param>
        public ModuleWatcherModule(IModuleManager moduleManager, Action<Type>? onLoadModule, Action<Type>? onReloadModule, Action<Type>? onUnloadModule, string folderModules = "modules") : base()
        {
            _semaphoreSlimChangeFile = new SemaphoreSlim(1);
            _moduleManager = moduleManager;
            _listNameIgnoreModules = new List<string>() {
            "Microsoft",
            "System"
            };

            _currentDirectory = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
            _currentFolderModules = Path.Combine(_currentDirectory, folderModules);
            _assmLibsPath = AppDomain.CurrentDomain.GetAssemblies().Select(x => x.Location).Distinct();
            _modulesInfo = new ConcurrentDictionary<string, ModuleProviderInfo>();

            if (!Directory.Exists(_currentFolderModules))
            {
                Directory.CreateDirectory(_currentFolderModules);
            }

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
            _fileSystemWatcher.Path = Path.Combine(_currentDirectory, folderModules);
        }

        ///<inheritdoc/>
        public override void ConfigureModule()
        {
            if (_moduleManager.ExistsModule(typeof(IAssemblyLoadContextContract)))
            {
                _assemblyLoadContextContract = _moduleManager.GetModule<AssemblyLoadContextModule>();
            }
            else
            {
                _assemblyLoadContextContract = _moduleManager.CreateModule<AssemblyLoadContextModule>();
            }
            var files = Directory.GetFiles(_currentDirectory, searchPattern: "*.dll", SearchOption.TopDirectoryOnly);
            foreach (string? modulePath in files)
            {
                FileInfo fi = new FileInfo(modulePath);
                ModuleProviderInfo mp = new ModuleProviderInfo(modulePath, _assmLibsPath.Contains(modulePath), _assemblyLoadContextContract, _listNameIgnoreModules);
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
                if (_modulesInfo.TryRemove(e.OldFullPath, out ModuleProviderInfo target))
                {
                    if (e.FullPath.EndsWith(DISABLE_MODULE_EXTENSION))
                    {
                        UnloadModule(target);
                    }
                    else
                    {
                        target.Reload(e.FullPath);
                        _modulesInfo.TryAdd(e.FullPath, target);
                    }

                }
            }
            else
            {
                //Tem algo de errado, como mudou se nao existe?
                //Existe os seguites casos
                // 1 - Ja existe o arquivo em formado de DLL e renomeia para outro nome .old por exemplo
                throw new NotImplementedException();
            }
        }

        private void _fileSystemWatcher_OnEvent(object sender, FileSystemEventArgs e)
        {
            ModuleProviderInfo mpf = sender as ModuleProviderInfo ?? new ModuleProviderInfo(e.FullPath, _assmLibsPath.Contains(e.FullPath), _assemblyLoadContextContract, _listNameIgnoreModules);
            switch (e.ChangeType)
            {
                case WatcherChangeTypes.Created:
                    if ((_modulesInfo.TryAdd(e.FullPath, mpf) && mpf.Load()) || (_modulesInfo.TryGetValue(e.FullPath, out mpf) && mpf.Reload()))
                    {
                        LoadModule(mpf);
                    }

                    break;
                case WatcherChangeTypes.Deleted:
                    if (_modulesInfo.TryRemove(e.FullPath, out mpf))
                    {
                        UnloadModule(mpf);
                    }
                    break;
                case WatcherChangeTypes.Changed:
                    _semaphoreSlimChangeFile.Wait();

                    if ((_modulesInfo.TryAdd(e.FullPath, mpf) && mpf.Load()) || (_modulesInfo.TryGetValue(e.FullPath, out mpf) && mpf.Reload()))
                    {
                        ReloadModule(mpf);
                    }
                    _semaphoreSlimChangeFile.Release();
                    break;
                case WatcherChangeTypes.All:
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        private void ReloadModule(ModuleProviderInfo mpf)
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

        private void LoadModule(ModuleProviderInfo mpf)
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

        private void UnloadModule(ModuleProviderInfo mpf)
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
        }

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
