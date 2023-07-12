﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Runtime.Loader;
using System.Text;

using Microsoft.VisualBasic;

using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Abstrato.Exceptions;
using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Abstrato.Modulos;
using Propeus.Modulo.IL.Core.Geradores;
using Propeus.Modulo.IL.Core.Helpers;
using Propeus.Modulo.Util.Atributos;
using Propeus.Modulo.Util.Thread;

namespace Propeus.Modulo.Dinamico.Modules
{
    internal class ModuleProviderInfo
    {

        internal class ModuleInfo : BaseModel
        {
            public ModuleInfo()
            {
                ModuleProxy = new WeakReference<Type>(null);
                Module = new WeakReference<Type>(null);
           
            }

            public bool HasProxyTypeModule => ModuleProxy is not null && ModuleProxy.TryGetTarget(out _);

            public bool IsValid => Module.TryGetTarget(out _) || ModuleProxy.TryGetTarget(out _);
            public string ModuleName { get; set; }
            public WeakReference<Type> ModuleProxy { get; set; }
            public WeakReference<Type> Module { get; set; }
            public List<WeakReference<Type>> Contracts { get; set; }

            internal ILClasseProvider _proxyBuilder;
            private string? _currentHash;

            public void BuildModuleProxy()
            {
                if (Contracts is null)
                    return;
                var aux = HashContracts();
                if (_currentHash != aux)
                {
                    _currentHash = aux;
                }
                else
                {
                    return;
                }

                if (Module.TryGetTarget(out Type target))
                {
                    if (_proxyBuilder is null)
                    {
                        _proxyBuilder = GeradorHelper.Modulo.CriarProxyClasse(target, GetContractsType().ToArray(),new Type[] {typeof(ModuleAttribute)});
                    }
                    else
                    {
                        //TODO: Corrigir esta funcao 'NovaVersao'
                        _proxyBuilder.NovaVersao(interfaces: GetContractsType().ToArray()).CriarProxyClasse(target, GetContractsType().ToArray());
                    }

                    _proxyBuilder.Executar();
                    ModuleProxy.SetTarget(_proxyBuilder.ObterTipoGerado());
                }
            }
            public IEnumerable<string> GetContractNames()
            {
                if (Contracts is null)
                    yield break;

                foreach (var item in Contracts)
                {
                    if (item.TryGetTarget(out var contract))
                    {
                        yield return contract.Name;
                    }
                }
            }
            public IEnumerable<Type> GetContractsType()
            {
                if (Contracts is null)
                    yield break;

                foreach (var item in Contracts)
                {
                    if (item.TryGetTarget(out var contract))
                    {
                        yield return contract;
                    }
                }
            }
            public void AddContract(Type contract)
            {
                if (ContainsContract(contract) == -1)
                    Contracts.Add(new WeakReference<Type>(contract));
            }
            public void RemoveContract(Type contract)
            {
                int idx = ContainsContract(contract);

                if (idx != -1)
                {
                    Contracts.RemoveAt(idx);
                }

            }
            public int ContainsContract(Type contract)
            {
                if (Contracts is null)
                    Contracts = new List<WeakReference<Type>>();

                int idx = -1;
                foreach (var item in Contracts)
                {
                    if (item.TryGetTarget(out var c))
                    {
                        if (c == contract)
                        {
                            idx = Contracts.IndexOf(item);
                        }
                    }
                }
                return idx;
            }
            private string HashContracts()
            {
                StringBuilder stringBuilder = new StringBuilder();

                foreach (var item in Contracts)
                {
                    if (item.TryGetTarget(out var c))
                    {
                        stringBuilder.Append(c.GUID.ToString());
                    }
                }
                return stringBuilder.ToString();
            }
            protected override void Dispose(bool disposing)
            {
                if (!disposedValue)
                {
                    _proxyBuilder.Dispose();
                }
                base.Dispose(disposing);
            }
        }

        public ModuleProviderInfo(string modulePath, bool isCurrentDomain, IModuleManager moduleManager)
        {
            this.Modules = new Dictionary<string, ModuleInfo>();

            this.ModulePath = modulePath;
            this.IsCurrentDomain = isCurrentDomain;
            this.moduleManager = moduleManager;
            GetModuleInfo();
            UpdateAssembly();
            MapModules();
        }


        /// <summary>
        /// Informa se o modulo atual foi carregado junto com o programa principal
        /// </summary>
        public bool IsCurrentDomain { get; set; }
        /// <summary>
        /// Informa se houve mudanca no arquivo do modulo
        /// </summary>
        public bool IsChanged { get; set; }
        /// <summary>
        /// Indica se houve erro durante o carregamento do modulo
        /// </summary>
        public bool HasError { get; set; }

        /// <summary>
        /// Indica se o modulo e valido
        /// </summary>
        public bool IsValid => !IsCurrentDomain && !HasError;

        public Exception? Error { get; set; }

        /// <summary>
        /// Informa se o arquivo do modulo existe
        /// </summary>
        public bool Exists => File.Exists(ModulePath);

        /// <summary>
        /// Caminho completo do arquivo DLL do modulo
        /// </summary>
        public string ModulePath { get; set; }
        /// <summary>
        /// Identificacao unica do modulo
        /// </summary>
        public Guid? ModuleGuid { get; set; }
        public Dictionary<string, ModuleInfo> Modules { get; set; }



        private AssemblyLoadContext _assemblyLoadContext;
        private readonly IModuleManager moduleManager;

        private void GetModuleInfo()
        {
            if (Exists)
            {
                List<string> modules = new List<string>();

                using (Stream streamReader = LoadModuleStream(ModulePath))
                {
                    streamReader.Seek(0, SeekOrigin.Begin);

                    using (PEReader pEReader = new PEReader(streamReader))
                    {
                        MetadataReader metadataReader = pEReader.GetMetadataReader();

                        var mvidHandle = metadataReader.GetModuleDefinition().Mvid;
                        var guid = metadataReader.GetGuid(mvidHandle);



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
                                if (metadataReader.StringComparer.Equals(attributeTypeNameHandle, nameof(ModuleAttribute)))
                                {
                                    if (this.ModuleGuid is not null && this.ModuleGuid != guid)
                                    {
                                        IsChanged = true;
                                    }
                                    this.ModuleGuid = guid;
                                    return;
                                }

                            }

                        }
                    }
                }
            }
        }
        private void UpdateAssembly()
        {
            if (Exists && !IsCurrentDomain)
            {
                if (!IsChanged && _assemblyLoadContext is null)
                {

                    _assemblyLoadContext = new AssemblyLoadContext(ModulePath, true);
                    using (var ms = LoadModuleStream(ModulePath))
                    {
                        _assemblyLoadContext.LoadFromStream(ms);
                    }

                }
                else
                {
                    _assemblyLoadContext.Unload();
                    _assemblyLoadContext = null;
                    IsChanged = false;
                    UpdateAssembly();
                }
            }
        }
        private void MapModules()
        {

            Dictionary<string, ModuleInfo> auxTypes = new Dictionary<string, ModuleInfo>();
            List<Type> types = new List<Type>();
            if (Exists)
            {
                if (!IsCurrentDomain)
                {
                    try
                    {
                        foreach (var item in _assemblyLoadContext.Assemblies)
                        {
                            foreach (var item1 in item.ExportedTypes)
                            {
                                types.Add(item1);
                            }
                        }

                        //types = _assemblyLoadContext.Assemblies.SelectMany(x => x.ExportedTypes);
                    }
                    catch (Exception ex)
                    {
                        Error = ex;
                        return;
                    }
                }
                else
                {

                    try
                    {
                        foreach (var item in AppDomain.CurrentDomain.GetAssemblies().Where(x => x.Location == ModulePath))
                        {
                            foreach (var item1 in item.ExportedTypes)
                            {
                                types.Add(item1);
                            }
                        }

                        //types = _assemblyLoadContext.Assemblies.SelectMany(x => x.ExportedTypes);
                    }
                    catch (Exception ex)
                    {
                        Error = ex;
                        return;
                    }
                }

            }


            foreach (var type in types)
            {
                var moduleAttr = type.GetCustomAttribute<ModuleAttribute>();
                var moduleContractAttr = type.GetCustomAttribute<ModuleContractAttribute>();

                if (moduleAttr is not null)
                {
                    if (auxTypes.ContainsKey(type.Name))
                    {
                        auxTypes[moduleContractAttr.ModuleName].ModuleName = type.Name;
                        auxTypes[moduleContractAttr.ModuleName].Module = new WeakReference<Type>(type);
                    }
                    else
                    {
                        auxTypes.Add(type.Name, new ModuleInfo { ModuleName = type.Name, Module = new WeakReference<Type>(type) });
                    }
                }

                if (moduleContractAttr is not null)
                {
                    var wrAttr = new WeakReference<Type>(type);
                    if (auxTypes.ContainsKey(moduleContractAttr.ModuleName))
                    {
                        auxTypes[moduleContractAttr.ModuleName].Contracts.Add(wrAttr);
                    }
                    else
                    {
                        auxTypes.Add(type.Name, new ModuleInfo { ModuleName = moduleContractAttr.ModuleName, Contracts = new List<WeakReference<Type>>() { wrAttr } });
                    }
                }
            }

            foreach (var lf in auxTypes)
            {
                if (!Modules.ContainsKey(lf.Key) && lf.Value.Module.TryGetTarget(out var target))
                {
                    //Gera o proxy para os novos modulos
                    lf.Value.BuildModuleProxy();
                    moduleManager.GetModule<QueueMessageModule>().SendMessage(target, "load");
                }
            }

            foreach (var rh in Modules)
            {
                if (!auxTypes.ContainsKey(rh.Key) && rh.Value.Module.TryGetTarget(out var target))
                {
                    rh.Value.Dispose();
                    moduleManager.GetModule<QueueMessageModule>().SendMessage(target, "unload");
                }
            }

            foreach (var lf in auxTypes)
            {
                if (Modules.ContainsKey(lf.Key) && Modules[lf.Key].Contracts.Count != lf.Value.Contracts.Count && lf.Value.Module.TryGetTarget(out var target))
                {
                    //Passa o proxybuilder existente para nao ter que ficar criando toda hora
                    lf.Value._proxyBuilder = Modules[lf.Key]._proxyBuilder;
                    //Atualiza o proxy 
                    lf.Value.BuildModuleProxy();
                    moduleManager.GetModule<QueueMessageModule>().SendMessage(target, "rebuild");
                }
            }

            Modules = auxTypes;

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

    }

    [Module]
    public class ModuleProviderModule : BaseModule
    {
        public event Action<Type> OnLoadModule;
        public event Action<Type> OnUnloadModule;
        public event Action<Type> OnRebuildModule;

        ConcurrentBag<ModuleProviderInfo> _modulesInfo;

        #region init
        public ModuleProviderModule(IModuleManager moduleManager) : base(true)
        {
            CriarInstancia(moduleManager);
            CriarConfiguracao();
        }

        public void CriarInstancia(IModuleManager moduleManager)
        {
            _currentDirectory = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
            _assmLibsPath = AppDomain.CurrentDomain.GetAssemblies().Select(x => x.Location).Distinct();
            _modulesInfo = new ConcurrentBag<ModuleProviderInfo>();
            this._moduleManager = moduleManager;
        }
        public void CriarConfiguracao()
        {

            foreach (var modulePath in _assmLibsPath)
            {
                _modulesInfo.Add(new ModuleProviderInfo(modulePath, true, _moduleManager));
            }
            foreach (var modulePath in Directory.GetFiles(_currentDirectory, "*.dll").Except(_assmLibsPath))
            {
                _modulesInfo.Add(new ModuleProviderInfo(modulePath, false, _moduleManager));
            }

            State = Abstrato.State.Ready;
        }

        public Type this[string nameType]
        {
            get
            {
                var info = _modulesInfo.Where(x => x.Modules.ContainsKey(nameType));
                if (info.Any() && info.Count() == 1 && info.First().Modules[nameType].IsValid)
                {
                    if (info.First().Modules[nameType].ModuleProxy is not null && info.First().Modules[nameType].ModuleProxy.TryGetTarget(out var proxy))
                    {
                        return proxy;
                    }
                    if (info.First().Modules[nameType].Module.TryGetTarget(out var target))
                    {
                        return target;
                    }
                }

                return null;


            }
            set
            {

                var info = _modulesInfo.Where(x => x.Modules.ContainsKey(nameType));
                if (info.Any() && info.Count() == 1 && info.First().Modules[nameType].IsValid)
                {
                    info.First().Modules[nameType].ModuleProxy.SetTarget(value);
                }
            }
        }
        #endregion

        #region Variaveis
        private IEnumerable<string> _assmLibsPath;
        private string _currentDirectory;

        private TaskJob _job;
        private IModuleManager _moduleManager;
        #endregion

        #region Funcoes
        public IEnumerable<Type> GetContracts(string nmeType)
        {
            var query = _modulesInfo.FirstOrDefault(x => x.Modules.ContainsKey(nmeType));
            if (query is not null)
            {
                foreach (var item in query.Modules[nmeType].Contracts)
                {
                    if (item.TryGetTarget(out var target))
                    {
                        yield return target;
                    }
                }
            }
            yield break;

        }
        public bool HasProxyType(string moduleName)
        {
            if (_modulesInfo.Any(x => x.Modules.ContainsKey(moduleName)))
            {
                var info = _modulesInfo.First(x => x.Modules.ContainsKey(moduleName));
                return info.Modules[moduleName].HasProxyTypeModule;
            }
            return false;
        }
        public Type GetModuleFromContract(string contractName)
        {
            foreach (var moduleInfo in _modulesInfo)
            {
                foreach (var module in moduleInfo.Modules)
                {
                    if (module.Value.Contracts is null)
                        continue;

                    foreach (var contract in module.Value.Contracts)
                    {
                        if (contract.TryGetTarget(out var target) && target.Name == contractName)
                        {
                            if (module.Value.HasProxyTypeModule)
                            {
                                if (module.Value.ModuleProxy.TryGetTarget(out var moduleType))
                                {
                                    return moduleType;
                                }
                            }
                            else
                            {
                                if (module.Value.Module.TryGetTarget(out var moduleType))
                                {
                                    return moduleType;
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }
        public Type GetModuleFromContract(Type contractType)
        {
            if (contractType.PossuiAtributo<ModuleContractAttribute>())
            {
                ModuleContractAttribute moduleContractAttribute = contractType.GetCustomAttribute<ModuleContractAttribute>();


                foreach (var moduleInfo in _modulesInfo)
                {
                    foreach (var module in moduleInfo.Modules)
                    {
                        if (module.Value.ModuleName == moduleContractAttribute.ModuleName && module.Value.IsValid)
                        {
                            module.Value.AddContract(contractType);
                            module.Value.BuildModuleProxy();

                            if (module.Value.ModuleProxy.TryGetTarget(out Type target))
                            {
                                return target;
                            }


                        }


                    }
                }


                throw new ModuleNotFoundException("ModuleProxy nao encontrado");

            }
            else
            {
                throw new ModuleContractNotFoundException("Atributo nao encontrado no tipo informado");
            }


         
        }

        public IEnumerable<Type> GetAllModules()
        {
            foreach (var moduleInfo in _modulesInfo)
            {
                foreach (var module in moduleInfo.Modules)
                {
                    if (module.Value.HasProxyTypeModule)
                    {
                        if (module.Value.ModuleProxy.TryGetTarget(out Type moduleTypeProxy))
                        {
                            yield return moduleTypeProxy;
                        }
                        if (module.Value.Module.TryGetTarget(out Type moduleType))
                        {
                            yield return moduleType;
                        }
                    }
                }
            }
        }
        #endregion


    }

}
