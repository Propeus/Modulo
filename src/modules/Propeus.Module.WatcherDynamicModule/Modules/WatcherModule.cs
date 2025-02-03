using System.Collections.Concurrent;
using System.Reflection;

using Propeus.Module.Abstract;
using Propeus.Module.Abstract.Attributes;
using Propeus.Module.Abstract.Exceptions;
using Propeus.Module.Abstract.Interfaces;
using Propeus.Module.MessageQueue.Contracts;
using Propeus.Module.Utils.Atributos;
using Propeus.Module.WatcherDynamicModule.Contracts;
using Propeus.Module.WatcherDynamicModule.Models;

namespace Propeus.Module.WatcherDynamicModule.Modules
{
    /// <summary>
    /// Module para mapear e atualizar outros modulos em tempo de execucao
    /// </summary>
    [Module(Description = "Monitora a alteração de estado dos modulos em arquivo", Singleton = true, AutoUpdate = false, AutoStartable = false, KeepAlive = true)]
    public class WatcherModule : BaseModule, IModuleWatcherContract
    {
        private const string DISABLE_MODULE_EXTENSION = ".disable";


        private ConcurrentDictionary<string, ModuleProviderInfo> _modulesInfo;
        private FileSystemWatcher _fileSystemWatcher;
        private SemaphoreSlim _semaphoreSlimChangeFile;

        private IMessageQueueManagerContract _messageQueueManager;
        private readonly IBuilderProxyContract _builderProxyContract;

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
        /// <param name="assemblyLoadContextContract"></param>
        /// <param name="messageQueueManagerContract">Mensageria </param>
        /// <param name="builderProxyContract"></param>
        /// <param name="folderModules">Nome da pasta a ser observado</param>
        public WatcherModule(IModuleManager moduleManager, IAssemblyLoadContextContract assemblyLoadContextContract, IMessageQueueManagerContract messageQueueManagerContract, IBuilderProxyContract builderProxyContract, string folderModules = "modules") : base()
        {
            _semaphoreSlimChangeFile = new SemaphoreSlim(1);
            _moduleManager = moduleManager;
            _listNameIgnoreModules = new List<string>() {
            "Microsoft",
            "System"
            };

            _messageQueueManager = messageQueueManagerContract;
            _builderProxyContract = builderProxyContract;
            _currentDirectory = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
            _currentFolderModules = Path.Combine(_currentDirectory, folderModules);
            _assmLibsPath = AppDomain.CurrentDomain.GetAssemblies().Select(x => x.Location).Distinct().ToList();
            AppDomain.CurrentDomain.AssemblyLoad += CurrentDomain_AssemblyLoad;
            _modulesInfo = new ConcurrentDictionary<string, ModuleProviderInfo>();

            if (!Directory.Exists(_currentFolderModules))
            {
                Directory.CreateDirectory(_currentFolderModules);
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

            _assemblyLoadContextContract = assemblyLoadContextContract;
        }

        private void CurrentDomain_AssemblyLoad(object? sender, AssemblyLoadEventArgs args)
        {
            _assmLibsPath = AppDomain.CurrentDomain.GetAssemblies().Select(x => x.Location).Distinct().ToList();
        }

        ///<inheritdoc/>
        public override void ConfigureModule()
        {
            var files = Directory.GetFiles(_currentDirectory, searchPattern: "*.dll", SearchOption.TopDirectoryOnly);
            foreach (string? modulePath in files)
            {
                FileInfo fi = new FileInfo(modulePath);
                ModuleProviderInfo mp = new ModuleProviderInfo(modulePath, _assmLibsPath.Contains(modulePath), _assemblyLoadContextContract, _builderProxyContract, _listNameIgnoreModules);
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
                    List<Type> modulosduplicados = [];
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

            _fileSystemWatcher_OnEvent(null, e);
        }

        private void _fileSystemWatcher_OnEvent(object sender, FileSystemEventArgs e)
        {
            ModuleProviderInfo moduleProviderInfo = sender as ModuleProviderInfo ?? new ModuleProviderInfo(e.FullPath, _assmLibsPath.Contains(e.FullPath), _assemblyLoadContextContract, _builderProxyContract, _listNameIgnoreModules);
            switch (e.ChangeType)
            {
                case WatcherChangeTypes.Created:
                    if ((_modulesInfo.TryAdd(e.FullPath, moduleProviderInfo) && moduleProviderInfo.Load()) || (_modulesInfo.TryGetValue(e.FullPath, out moduleProviderInfo) && moduleProviderInfo.Reload()))
                    {
                        LoadModule(moduleProviderInfo);
                    }

                    break;
                case WatcherChangeTypes.Deleted:
                    if (_modulesInfo.TryRemove(e.FullPath, out moduleProviderInfo))
                    {
                        UnloadModule(moduleProviderInfo);
                    }
                    break;
                case WatcherChangeTypes.Changed:
                    _semaphoreSlimChangeFile.Wait();

                    if ((_modulesInfo.TryAdd(e.FullPath, moduleProviderInfo) && moduleProviderInfo.Load()) || (_modulesInfo.TryGetValue(e.FullPath, out moduleProviderInfo) && moduleProviderInfo.Reload()))
                    {
                        ReloadModule(moduleProviderInfo);
                    }
                    _semaphoreSlimChangeFile.Release();
                    break;
                case WatcherChangeTypes.Renamed:
                    if ((e as RenamedEventArgs).OldFullPath.EndsWith(DISABLE_MODULE_EXTENSION))
                    {
                        if ((_modulesInfo.TryAdd(e.FullPath, moduleProviderInfo) && moduleProviderInfo.Load()) || (_modulesInfo.TryGetValue(e.FullPath, out moduleProviderInfo) && moduleProviderInfo.Reload()))
                        {
                            LoadModule(moduleProviderInfo);
                        }
                    }
                    else
                    {
                        if (_modulesInfo.TryRemove((e as RenamedEventArgs).OldFullPath, out moduleProviderInfo))
                        {
                            UnloadModule(moduleProviderInfo);
                        }
                    }
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
                if (item.Value.ModuleProxy.TryGetTarget(out var target) || item.Value.Module.TryGetTarget(out target))
                {
                    _messageQueueManager.SendData(Constantes.MENSAGERIA_RELOAD_MODULE, target);
                }
            }
        }

        private void LoadModule(ModuleProviderInfo mpf)
        {
            foreach (var item in mpf.Modules)
            {
                if (item.Value.ModuleProxy.TryGetTarget(out var target) || item.Value.Module.TryGetTarget(out target))
                {
                    _messageQueueManager.SendData(Constantes.MENSAGERIA_LOAD_MODULE, target);
                }
            }
        }

        private void UnloadModule(ModuleProviderInfo mpf)
        {
            foreach (var item in mpf.Modules)
            {
                if (item.Value.ModuleProxy.TryGetTarget(out var target) || item.Value.Module.TryGetTarget(out target))
                {
                    _messageQueueManager.SendData(Constantes.MENSAGERIA_UNLOAD_MODULE, target);
                }
            }
        }

        #endregion



        #region Funcoes
        ///<inheritdoc/>
        ///<exception cref="ModuleContractNotMapedException">Quando o contrato não foi mapeado para o modulo</exception>
        public Type GetModuleTypeFromContract(Type contractType)
        {
            if (contractType.PossuiAtributo<ModuleContractAttribute>())
            {
                ModuleContractAttribute moduleContractAttribute = contractType.GetCustomAttribute<ModuleContractAttribute>();

                foreach (KeyValuePair<string, ModuleProviderInfo> moduleInfo in _modulesInfo)
                {

                    foreach (KeyValuePair<string, DynamicModuleInfo> module in moduleInfo.Value.Modules)
                    {
                        if (module.Value.ModuleName == moduleContractAttribute.ModuleName && module.Value.IsValid)
                        {

                            if (module.Value.ContainsContract(contractType))
                            {
                                if (module.Value.ModuleProxy.TryGetTarget(out Type? target))
                                {
                                    return target;
                                }
                            }
                            throw new ModuleContractNotMapedException();
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

                foreach (KeyValuePair<string, DynamicModuleInfo> module in moduleInfo.Value.Modules)
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

        /// <summary>
        /// Constroi um novo proxy com o contrato informado
        /// </summary>
        /// <param name="contractType">A interfece de contrato de deve ser implementado</param>
        /// <exception cref="ModuleContractInvalidException">A interface de contrato não esta nos padrões</exception>
        /// <exception cref="ModuleContractNotFoundException">O modulo da interfece de contrato não foi carregado ou não foi encontrado</exception>
        /// <returns>Retorna um novo proxy com o a iterface de contrato contido</returns>
        public Type BuildModuleTypeFromContract(Type contractType)
        {
            //1 - Pesquisar, Achar, Guardar
            DynamicModuleInfo? pesquisa = null;
            ModuleContractAttribute? moduleContractAttribute = contractType.GetCustomAttribute<ModuleContractAttribute>();
            if (moduleContractAttribute is not null)
            {
                foreach (KeyValuePair<string, ModuleProviderInfo> moduleInfo in _modulesInfo)
                {
                    if (pesquisa is not null)
                    {
                        break;
                    }

                    foreach (KeyValuePair<string, DynamicModuleInfo> module in moduleInfo.Value.Modules)
                    {
                        if (module.Value.ModuleName == moduleContractAttribute.ModuleName && module.Value.IsValid)
                        {
                            pesquisa = module.Value;
                            break;
                        }
                    }
                }
            }
            else
            {
                throw new ModuleContractInvalidException();
            }
            if (pesquisa is null)
            {
                throw new ModuleNotFoundException(contractType);
            }
            //    4 - processar
            if (!pesquisa.ContainsContract(contractType))
            {
                pesquisa.AddContract(contractType);
                _builderProxyContract.BuildModuleProxy(pesquisa.Module, pesquisa.Contracts, ref pesquisa._currentHash, pesquisa.ModuleProxy);
            }
            //    5 - retorniar
            if (pesquisa.ModuleProxy.TryGetTarget(out Type? target))
            {
                return target;
            }

            throw new ModuleContractNotFoundException(contractType);

        }
        /// <summary>
        /// Verifica se o contrato está no proxy atual
        /// </summary>
        /// <param name="contractType">Tipo da interface de contrato</param>
        /// <returns>Retorna verdadeiro se o tipo estiver contido no proxy, caso contrario, retorna false</returns>
        public bool HasModuleTypeFromContract(Type contractType)
        {
            //1 - Pesquisar, Achar, Guardar
            DynamicModuleInfo? pesquisa = null;
            ModuleContractAttribute? moduleContractAttribute = contractType.GetCustomAttribute<ModuleContractAttribute>();
            if (moduleContractAttribute is not null)
            {
                foreach (KeyValuePair<string, ModuleProviderInfo> moduleInfo in _modulesInfo)
                {
                    foreach (KeyValuePair<string, DynamicModuleInfo> module in moduleInfo.Value.Modules)
                    {
                        if (module.Value.ModuleName == moduleContractAttribute.ModuleName && module.Value.IsValid)
                        {
                            pesquisa = module.Value;
                            break;
                        }
                    }
                }
            }
            else
            {
                return false;
            }
            if (pesquisa is null)
            {
                return false;
            }

            return pesquisa.ContainsContract(contractType);
        }
        #endregion


    }

}
