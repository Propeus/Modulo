using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Runtime.Loader;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Abstrato.Helpers;
using Propeus.Modulo.Abstrato.Proveders;
using Propeus.Modulo.Util.Atributos;
using Propeus.Modulo.Util.Thread;
using Propeus.Modulo.Util.Vetores;

namespace Propeus.Modulo.Abstrato
{
    public class ModuleProvider : IDisposable
    {

        /// <summary>
        /// Identificador de instancia
        /// </summary>
        public readonly string Identificador;

        private IEnumerable<string> _assmLibsPath;
        private string _currentDirectory;
        private ConcurrentBag<string> _modulesPath;

        ConcurrentHashSet<string> _modules;

        private ConcurrentDictionary<string, bool> _pathToReload;
        private ConcurrentDictionary<string, DateTime> _pathToReloadDate;
        private ConcurrentDictionary<string, long> _pathToReloadLength;
        private ConcurrentDictionary<string, AssemblyLoadContext> _pathToAssemblyContext;
        private ConcurrentDictionary<string, WeakReference<Assembly>> _pathToAssembly;
        private ConcurrentDictionary<string, string> _typeToPath;
        private ConcurrentDictionary<string, WeakReference<Type>> _typeToType;
        private ConcurrentDictionaryWeakReference<string, Type> _proxyToType;

        private ConcurrentDictionary<string, string> _contractToType;
        private ConcurrentDictionary<string, WeakReference<Type>> _contractToContract;

        private TaskJob _job;

        public Type this[string nameType]
        {
            get
            {
                //if(_proxyToType.TryGetValue(nameType, out Type type))
                //{
                //    return type;
                //}
                if (_proxyToType.TryGetValue(nameType, out var proxyType))
                {
                    return proxyType;
                }

                if (_typeToType.TryGetValue(nameType, out var assmName))
                {
                    assmName.TryGetTarget(out var type);
                    return type;
                }
              
                return null;
            }
            set
            {


                if (!(_proxyToType.ContainsKey(nameType)))
                {
                    _proxyToType.TryAdd(nameType, value);
                }
                else
                {
                    _proxyToType[nameType] = new WeakReference(value);
                }
            }
        }
        public static ModuleProvider Provider
        {
            get
            {
                if (_provider == null || _provider.disposedValue)
                {
                    _provider = new ModuleProvider();
                }

                return _provider;
            }
        }
        private ModuleProvider()
        {
            Identificador = Guid.NewGuid().ToString();

            _assmLibsPath = AppDomain.CurrentDomain.GetAssemblies().Select(x => x.Location);
            _currentDirectory = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
            _modulesPath = new ConcurrentBag<string>();

            _modules = new ConcurrentHashSet<string>();

            _pathToReload = new ConcurrentDictionary<string, bool>();
            _pathToReloadDate = new ConcurrentDictionary<string, DateTime>();
            _pathToReloadLength = new ConcurrentDictionary<string, long>();
            _pathToAssemblyContext = new ConcurrentDictionary<string, AssemblyLoadContext>();
            _pathToAssembly = new ConcurrentDictionary<string, WeakReference<Assembly>>();

            _typeToPath = new ConcurrentDictionary<string, string>();
            _typeToType = new ConcurrentDictionary<string, WeakReference<Type>>();
            _proxyToType = new ConcurrentDictionaryWeakReference<string, Type>();

            _contractToType = new ConcurrentDictionary<string, string>();
            _contractToContract = new ConcurrentDictionary<string, WeakReference<Type>>();

            _job = TaskJob.GetTasker();

            _job.RegisterJob(JOB_LOAD_MODULES_PATH, $"{nameof(ModuleProvider)}::{nameof(JOB_LOAD_MODULES_PATH)}", TimeSpan.FromSeconds(1));
            _job.RegisterJob(JOB_MAP_MODULES, $"{nameof(ModuleProvider)}::{nameof(JOB_MAP_MODULES)}", TimeSpan.FromSeconds(2));
            _job.RegisterJob(JOB_CHANGE_MODULES, $"{nameof(ModuleProvider)}::{nameof(JOB_CHANGE_MODULES)}", TimeSpan.FromSeconds(1));
            _job.RegisterJob(JOB_RELOAD_MODULES_STREAM, $"{nameof(ModuleProvider)}::{nameof(JOB_RELOAD_MODULES_STREAM)}", TimeSpan.FromSeconds(2));
            _job.RegisterJob(JOB_LOAD_ASSEMBLYES, $"{nameof(ModuleProvider)}::{nameof(JOB_LOAD_ASSEMBLYES)}", TimeSpan.FromSeconds(3));
            _job.RegisterJob(JOB_MAP_TYPES_MODULE, $"{nameof(ModuleProvider)}::{nameof(JOB_MAP_TYPES_MODULE)}", TimeSpan.FromSeconds(3));
            _job.RegisterJob(JOB_MAP_TYPES_CONTRACTS, $"{nameof(ModuleProvider)}::{nameof(JOB_MAP_TYPES_CONTRACTS)}", TimeSpan.FromSeconds(3));
            _job.RegisterJob(JOB_LINK_CONTRACTS_TO_TYPE, $"{nameof(ModuleProvider)}::{nameof(JOB_LINK_CONTRACTS_TO_TYPE)}", TimeSpan.FromSeconds(3));
        }

        public IEnumerable<Type> GetContracts(string nmeType)
        {
            foreach (var item in _contractToType)
            {
                if (item.Value == nmeType
                    && _contractToContract.TryGetValue(nmeType, out var contract)
                    && contract.TryGetTarget(out Type target))
                {
                    yield return target;
                }
            }
        }
        public IEnumerable<Type> GetModulesAutoInitialized()
        {
            foreach (var item in _typeToType.Values)
            {
                if (item.TryGetTarget(out Type target)
                     && target.ObterModuloAtributo().AutoInicializavel)
                {
                    yield return target;
                }
            }
        }

        public void LoadModules()
        {
            LoadModulesPath();
            MapModules();
            ChangeModules();
            LoadAssemblyes();
            ReloadModulesStream();
            MapTypeModules();
            MapContracts();
            LinkContractToType();
        }

        //1
        void LoadModulesPath()
        {
            _modulesPath.Clear();
            _modulesPath = new ConcurrentBag<string>(Directory.GetFiles(_currentDirectory, "*.dll").Except(_assmLibsPath));
        }
        //2
        void MapModules()
        {
            _modules.Clear();

            foreach (var modulePath in _modulesPath)
            {
                using (Stream streamReader = LoadModuleStream(modulePath))
                {
                    streamReader.Seek(0, SeekOrigin.Begin);

                    using (PEReader pEReader = new PEReader(streamReader))
                    {
                        MetadataReader metadataReader = pEReader.GetMetadataReader();

                        foreach (var typeHandle in metadataReader.TypeDefinitions)
                        {
                            TypeDefinition typeDef = metadataReader.GetTypeDefinition(typeHandle);

                            foreach (var AttributeHandler in typeDef.GetCustomAttributes())
                            {
                                var attributeDef = metadataReader.GetCustomAttribute(AttributeHandler);
                                var attributeCtorHandle = attributeDef.Constructor;
                                EntityHandle attributeTypeHandle = attributeCtorHandle.Kind switch
                                {
                                    HandleKind.MethodDefinition => metadataReader.GetMethodDefinition((MethodDefinitionHandle)attributeCtorHandle).GetDeclaringType(),
                                    HandleKind.MemberReference => metadataReader.GetMemberReference((MemberReferenceHandle)attributeCtorHandle).Parent,
                                    _ => throw new InvalidOperationException(),
                                };
                                StringHandle attributeTypeNameHandle = attributeTypeHandle.Kind switch
                                {
                                    HandleKind.TypeDefinition => metadataReader.GetTypeDefinition((TypeDefinitionHandle)attributeTypeHandle).Name,
                                    HandleKind.TypeReference => metadataReader.GetTypeReference((TypeReferenceHandle)attributeTypeHandle).Name,
                                    _ => throw new InvalidOperationException(),
                                };
                                if (metadataReader.StringComparer.Equals(attributeTypeNameHandle, nameof(ModuloAttribute)))
                                {
                                    _modules.Add(modulePath);
                                }

                            }

                        }
                    }
                }
            }
        }
        //3
        void ChangeModules()
        {
            foreach (var modulePath in _modules)
            {
                var fileInfo = new FileInfo(modulePath);
                if (_pathToReload.ContainsKey(modulePath))
                {
                    if (_pathToReloadDate[modulePath] != fileInfo.LastWriteTime
                        || _pathToReloadLength[modulePath] != fileInfo.Length)
                    {
                        _pathToReload[modulePath] = true;
                        _pathToReloadDate[modulePath] = fileInfo.LastWriteTime;
                        _pathToReloadLength[modulePath] = fileInfo.Length;
                    }
                }
                else
                {
                    _pathToReload.TryAdd(modulePath, false);
                    _pathToReloadDate.TryAdd(modulePath, fileInfo.LastWriteTime);
                    _pathToReloadLength.TryAdd(modulePath, fileInfo.Length);
                }
            }
        }
        //4
        void LoadAssemblyes()
        {
            foreach (var modulePath in _modules)
            {
                if (!_pathToAssemblyContext.ContainsKey(modulePath))
                {
                    _pathToAssemblyContext.TryAdd(modulePath, new AssemblyLoadContext(modulePath, true));
                    _pathToAssemblyContext[modulePath].Unloading += ModuleProviderV2_Unloading;
                    using (var ms = LoadModuleStream(modulePath))
                    {
                        _pathToAssemblyContext[modulePath].LoadFromStream(ms);
                    }
                }
            }
        }
        //5
        void ReloadModulesStream()
        {
            foreach (var moduleChange in _pathToReload.Where(x => x.Value))
            {
                _pathToAssemblyContext[moduleChange.Key].Unload();
            }
        }
        //6
        void MapTypeModules()
        {

            Dictionary<string, WeakReference<Type>> auxTypes = new Dictionary<string, WeakReference<Type>>();
            foreach (var assmLoadContext in _pathToAssemblyContext)
            {
                var types = _pathToAssemblyContext[assmLoadContext.Key].Assemblies.SelectMany(x => x.ExportedTypes);
                foreach (var type in types)
                {
                    if (type.PossuiAtributo<ModuloAttribute>())
                    {
                        auxTypes.Add(type.Name, new WeakReference<Type>(type));
                        _typeToPath.TryAdd(type.Name, assmLoadContext.Key);
                    }
                }
            }

            try
            {
                var assmTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assm => assm.GetExportedTypes()).ToArray();
                foreach (Type assmType in assmTypes)
                {
                    if (assmType.PossuiAtributo<ModuloAttribute>() && !auxTypes.ContainsKey(assmType.Name))
                    {
                        auxTypes.Add(assmType.Name, new WeakReference<Type>(assmType));
                    }
                }
            }
            catch (TypeLoadException)
            {
                //Ignora, pois acontece de haver um modulo em processo de remocao durante este estagio
            }

            foreach (string type in _typeToType.Keys)
            {
                if (!auxTypes.ContainsKey(type)
                    && _typeToType.Remove(type, out WeakReference<Type> weakReference)
                    && weakReference.TryGetTarget(out Type target))
                {
                    target.NotificarInformacao("unload");
                }
            }

            foreach (var types in auxTypes)
            {
                if (_typeToType.TryAdd(types.Key, types.Value) && types.Value.TryGetTarget(out Type target))
                {
                    target.NotificarInformacao("load");
                }
            }

            auxTypes.Clear();
        }
        //7
        void MapContracts()
        {
            Dictionary<string, WeakReference<Type>> auxTypes = new Dictionary<string, WeakReference<Type>>();
            foreach (var assmLoadContext in _pathToAssemblyContext)
            {
                var types = _pathToAssemblyContext[assmLoadContext.Key].Assemblies.SelectMany(x => x.ExportedTypes);
                foreach (var type in types)
                {
                    if (type.PossuiAtributo<ModuloContratoAttribute>())
                    {
                        auxTypes.Add(type.Name, new WeakReference<Type>(type));
                    }
                }
            }

            foreach (string type in _contractToContract.Keys)
            {
                if (!auxTypes.ContainsKey(type)
                    && _contractToContract.Remove(type, out WeakReference<Type> weakReference)
                    && weakReference.TryGetTarget(out Type target))
                {
                    target.NotificarInformacao("unload");
                }
            }

            foreach (var types in auxTypes)
            {
                if (_contractToContract.TryAdd(types.Key, types.Value) && types.Value.TryGetTarget(out Type target))
                {
                    target.NotificarInformacao("load");
                }
            }

            auxTypes.Clear();
        }
        //8
        void LinkContractToType()
        {
            Dictionary<string, string> auxContracts = new Dictionary<string, string>();
            foreach (var item in _contractToContract)
            {
                if (item.Value.TryGetTarget(out Type target))
                {
                    var nmeType = target.ObterModuloContratoAtributo().Nome;
                    if (_typeToType.ContainsKey(nmeType))
                    {
                        auxContracts.Add(item.Key, nmeType);
                    }
                }
            }
            HashSet<Type> rebuilds = new HashSet<Type>();
            foreach (string contract in _contractToType.Keys)
            {
                if (!auxContracts.ContainsKey(contract)
                    && _contractToType.Remove(contract, out string nmeType)
                    && _typeToType.TryGetValue(nmeType, out WeakReference<Type> type)
                    && type.TryGetTarget(out Type target))
                {
                    rebuilds.Add(target);
                }
            }

            foreach (var contract in auxContracts)
            {
                if (_contractToType.TryAdd(contract.Key, contract.Value)
                    && _typeToType.TryGetValue(contract.Value, out WeakReference<Type> type)
                    && type.TryGetTarget(out Type target))
                {
                    rebuilds.Add(target);
                }
            }

            foreach (var item in rebuilds)
            {
                item.NotificarInformacao("rebuild");
            }

        }

        void ModuleProviderV2_Unloading(AssemblyLoadContext obj)
        {
            _pathToAssemblyContext.TryRemove(obj.Name, out _);
        }

        MemoryStream LoadModuleStream(string modulePath)
        {
            MemoryStream ms = new MemoryStream();
            using (FileStream arquivo = new FileStream(modulePath, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader binario = new BinaryReader(arquivo))
                {
                    binario.BaseStream.CopyTo(ms);
                    binario.Close();
                }
                arquivo.Close();
            }
            _ = ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }

        void JOB_LOAD_MODULES_PATH(object cancellationToken)
        {
            CancellationToken token = (CancellationToken)cancellationToken;
            LoadModulesPath();
        }
        void JOB_MAP_MODULES(object cancellationToken)
        {
            CancellationToken token = (CancellationToken)cancellationToken;
            MapModules();
        }
        void JOB_CHANGE_MODULES(object cancellationToken)
        {
            CancellationToken token = (CancellationToken)cancellationToken;
            ChangeModules();
        }
        void JOB_RELOAD_MODULES_STREAM(object cancellationToken)
        {
            CancellationToken token = (CancellationToken)cancellationToken;
            ReloadModulesStream();
        }
        void JOB_LOAD_ASSEMBLYES(object cancellationToken)
        {
            CancellationToken token = (CancellationToken)cancellationToken;
            LoadAssemblyes();
        }
        void JOB_MAP_TYPES_MODULE(object cancellationToken)
        {
            CancellationToken token = (CancellationToken)cancellationToken;
            MapTypeModules();
        }
        void JOB_MAP_TYPES_CONTRACTS(object cancellationToken)
        {
            CancellationToken token = (CancellationToken)cancellationToken;
            MapContracts();
        }
        void JOB_LINK_CONTRACTS_TO_TYPE(object cancellationToken)
        {
            CancellationToken token = (CancellationToken)cancellationToken;
            LinkContractToType();
        }

        private bool disposedValue;
        private static ModuleProvider _provider;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _job.UnregisterJob($"{nameof(ModuleProvider)}::{nameof(JOB_LOAD_MODULES_PATH)}");
                    _job.UnregisterJob($"{nameof(ModuleProvider)}::{nameof(JOB_MAP_MODULES)}");
                    _job.UnregisterJob($"{nameof(ModuleProvider)}::{nameof(JOB_CHANGE_MODULES)}");
                    _job.UnregisterJob($"{nameof(ModuleProvider)}::{nameof(JOB_RELOAD_MODULES_STREAM)}");
                    _job.UnregisterJob($"{nameof(ModuleProvider)}::{nameof(JOB_LOAD_ASSEMBLYES)}");
                    _job.UnregisterJob($"{nameof(ModuleProvider)}::{nameof(JOB_MAP_TYPES_MODULE)}");
                    _job.UnregisterJob($"{nameof(ModuleProvider)}::{nameof(JOB_MAP_TYPES_CONTRACTS)}");
                    _job.UnregisterJob($"{nameof(ModuleProvider)}::{nameof(JOB_LINK_CONTRACTS_TO_TYPE)}");

                    _modulesPath.Clear();
                    _modules.Clear();
                    _pathToReload.Clear();
                    _pathToReloadDate.Clear();
                    _pathToReloadLength.Clear();
                    foreach (var module in _pathToAssemblyContext)
                    {
                        module.Value.Unload();
                    }
                    _pathToAssemblyContext.Clear();
                    _typeToPath.Clear();
                    _typeToType.Clear();
                    _contractToContract.Clear();
                    _contractToType.Clear();
                }

                _modulesPath = null;
                _modules = null;
                _pathToReload = null;
                _pathToReloadDate = null;
                _pathToReloadLength = null;
                _pathToAssemblyContext = null;
                _typeToPath = null;
                _typeToType = null;
                _contractToContract = null;
                _contractToType = null;

                disposedValue = true;
            }
        }



        public void Dispose()
        {
            // Não altere este código. Coloque o código de limpeza no método 'Dispose(bool disposing)'
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

}
