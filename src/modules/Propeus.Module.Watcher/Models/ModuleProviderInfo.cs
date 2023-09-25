using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Runtime.Loader;

using Propeus.Module.Abstract.Attributes;
using Propeus.Module.Abstract.Interfaces;

namespace Propeus.Module.Watcher.Models
{
    internal class ModuleProviderInfo
    {



        public ModuleProviderInfo(string modulePath, bool isCurrentDomain, IModuleManager moduleManager, List<string> listNameIgnoreModules)
        {
            Modules = new Dictionary<string, ModuleInfo>();

            FullPathModule = modulePath;
            FileNameModule = new FileInfo(modulePath).Name;
            IsCurrentDomain = isCurrentDomain;
            this.moduleManager = moduleManager;

            _listNameIgnoreModules = listNameIgnoreModules ?? new List<string>();
        }

        /// <summary>
        /// Indica se o modulo atual possui classes com o atributo <see cref="ModuleAttribute"/>
        /// </summary>
        public bool IsValidModule { get; private set; }
        /// <summary>
        /// Indica se o modulo e valido
        /// </summary>
        public bool IsValid => !IsCurrentDomain && !HasError && Modules.Count > 0;
        /// <summary>
        /// Informa se o modulo atual foi carregado junto com o programa principal
        /// </summary>
        public bool IsCurrentDomain { get; set; }
        /// <summary>
        /// Informa se houve mudanca no arquivo do modulo
        /// </summary>
        public bool IsChanged { get; set; }
        /// <summary>
        /// Informa se o modulo atual foi carregado em memoria
        /// </summary>
        public bool IsLoaded { get; private set; }

        /// <summary>
        /// Indica se houve erro durante o carregamento do modulo
        /// </summary>
        public bool HasError { get; set; }
        /// <summary>
        /// Excecao do modulo em caso de erro
        /// </summary>
        public Exception? Error { get; set; }

        /// <summary>
        /// Informa se o arquivo do modulo existe
        /// </summary>
        public bool Exists => File.Exists(FullPathModule);
        /// <summary>
        /// Caminho completo do arquivo DLL do modulo
        /// </summary>
        public string FullPathModule { get; set; }
        /// <summary>
        /// Nome do arquivo
        /// </summary>
        public string FileNameModule { get; private set; }

        /// <summary>
        /// Identificação unica do modulo
        /// </summary>
        public Guid? ModuleGuid { get; set; }

        /// <summary>
        /// Dicionário de módulos da DLL
        /// </summary>
        public Dictionary<string, ModuleInfo> Modules { get; private set; }


        private Task? _loadModuletask;
        private AssemblyLoadContext? _assemblyLoadContext;
        private readonly IModuleManager moduleManager;
        private readonly List<string> _listNameIgnoreModules;

        public bool Reload(string? newFullPath = null)
        {

            GetModuleInfo();

            UpdateAssembly();
            MapModules();

            var result = IsChanged;
            IsChanged = false;

            return result;
        }

        /// <summary>
        /// Carrega ou atualiza os módulos
        /// </summary>
        /// <param name="newFullPath">Novo caminho do modulo caso seja necessário mudar ou renomear</param>
        /// <returns>Retorna um valor booleano informando de o modulo foi efetivamente carregado ou não</returns>
        public bool Load(string? newFullPath = null)
        {
            if (!IsLoaded)
            {
                GetModuleInfo();
                UpdateAssembly();
                MapModules();
            }
            var result = !IsLoaded || IsChanged;

            IsLoaded = true;
            IsChanged = false;

            return result;
        }

        private void GetModuleInfo()
        {
            if (Exists)
            {
                foreach (string ignoreModule in _listNameIgnoreModules)
                {
                    if (FileNameModule.StartsWith(ignoreModule, StringComparison.CurrentCultureIgnoreCase))
                    {
                        IsValidModule = false;
                        return;
                    }
                }

                using (Stream streamReader = LoadModuleStream(FullPathModule))
                {
                    streamReader.Seek(0, SeekOrigin.Begin);

                    using (PEReader pEReader = new PEReader(streamReader))
                    {
                        MetadataReader metadataReader = pEReader.GetMetadataReader();

                        GuidHandle mvidHandle = metadataReader.GetModuleDefinition().Mvid;
                        Guid guid = metadataReader.GetGuid(mvidHandle);



                        foreach (TypeDefinitionHandle typeHandle in metadataReader.TypeDefinitions)
                        {
                            TypeDefinition typeDef = metadataReader.GetTypeDefinition(typeHandle);

                            foreach (CustomAttributeHandle AttributeHandler in typeDef.GetCustomAttributes())
                            {
                                CustomAttribute attributeDef = metadataReader.GetCustomAttribute(AttributeHandler);
                                EntityHandle attributeCtorHandle = attributeDef.Constructor;
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
                                    if (ModuleGuid is not null && ModuleGuid != guid)
                                    {
                                        IsChanged = true;
                                    }
                                    IsValidModule = true;
                                    ModuleGuid = guid;
                                    return;
                                }

                            }

                        }
                    }
                }
            }
        }
        object _lockAssemblyLoadContext = new object();
        private void UpdateAssembly()
        {
            if (IsValidModule && !IsCurrentDomain && _assemblyLoadContext is null)
            {
                lock (_lockAssemblyLoadContext)
                {
                    _assemblyLoadContext = new AssemblyLoadContext(FullPathModule, true);
                    using (MemoryStream ms = LoadModuleStream(FullPathModule))
                    {
                        _assemblyLoadContext.LoadFromStream(ms);
                    }
                }
            }
            else if (_assemblyLoadContext is not null && IsChanged)
            {
                lock (_lockAssemblyLoadContext)
                {
                    _assemblyLoadContext.Unload();
                    _assemblyLoadContext = null;
                }
                UpdateAssembly();
            }
        }
        private void MapModules()
        {
            if (IsValidModule && (!IsLoaded || IsChanged))
            {


                Dictionary<string, ModuleInfo> auxTypes = new Dictionary<string, ModuleInfo>();
                List<Type> types = new List<Type>();
                if (Exists)
                {
                    if (!IsCurrentDomain)
                    {
                        try
                        {
                            foreach (Assembly item in _assemblyLoadContext.Assemblies)
                            {
                                foreach (Type item1 in item.ExportedTypes)
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
                            foreach (Assembly? item in AppDomain.CurrentDomain.GetAssemblies().Where(x => x.Location == FullPathModule))
                            {
                                foreach (Type item1 in item.ExportedTypes)
                                {
                                    types.Add(item1);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Error = ex;
                            return;
                        }
                    }

                }


                foreach (Type type in types)
                {
                    ModuleAttribute? moduleAttr = type.GetCustomAttribute<ModuleAttribute>();
                    ModuleContractAttribute? moduleContractAttr = type.GetCustomAttribute<ModuleContractAttribute>();

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
                        if (auxTypes.ContainsKey(moduleContractAttr.ModuleName))
                        {
                            auxTypes[moduleContractAttr.ModuleName].AddContract(type);
                        }
                        else
                        {
                            var moduleInfo = new ModuleInfo { ModuleName = moduleContractAttr.ModuleName };
                            moduleInfo.AddContract(type);

                            auxTypes.Add(type.Name, moduleInfo);
                        }
                    }
                }

                foreach (KeyValuePair<string, ModuleInfo> lf in auxTypes)
                {
                    if (!Modules.ContainsKey(lf.Key) && lf.Value.Module.TryGetTarget(out Type? target))
                    {
                        //Gera o proxy para os novos modulos
                        lf.Value.BuildModuleProxy();
                    }
                }

                foreach (KeyValuePair<string, ModuleInfo> rh in Modules)
                {
                    if (!auxTypes.ContainsKey(rh.Key) && rh.Value.Module.TryGetTarget(out Type? target))
                    {
                        rh.Value.Dispose();
                    }
                }

                foreach (KeyValuePair<string, ModuleInfo> lf in auxTypes)
                {
                    if (Modules.ContainsKey(lf.Key) && Modules[lf.Key].Contracts?.Count != lf.Value.Contracts?.Count && lf.Value.Module.TryGetTarget(out Type? target))
                    {
                        //Passa o proxybuilder existente para nao ter que ficar criando toda hora
                        lf.Value._proxyBuilder = Modules[lf.Key]._proxyBuilder;
                        //Atualiza o proxy 
                        lf.Value.BuildModuleProxy();
                    }
                }

                Modules = auxTypes;
            }
        }

        private MemoryStream LoadModuleStream(string modulePath)
        {
            MemoryStream ms = new MemoryStream();
            for (int i = 0; ; i++)
            {
                try
                {
                    using (FileStream arquivo = new FileStream(modulePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        using (BinaryReader binario = new BinaryReader(arquivo))
                        {
                            binario.BaseStream.CopyTo(ms);
                            binario.Close();
                        }
                        arquivo.Close();
                    }
                    break;
                }
                catch (IOException)
                {

                    if (i > 3)
                    {
                        throw;
                    }
                    else
                    {
                        Task.Delay(TimeSpan.FromSeconds(1)).Wait();
                    }

                }
            }
            _ = ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }

    }

}
