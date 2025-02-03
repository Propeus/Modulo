using Propeus.Module.Abstract;
using Propeus.Module.Abstract.Attributes;
using Propeus.Module.Package.Contracts;
using Propeus.Module.Taskjob;
using Propeus.Module.Taskjob.Modules;

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Propeus.Module.Package.Modules
{
    [Module(Description = "Modulo para carregar pacotes nuget", AutoStartable = false, AutoUpdate = false, KeepAlive = false, Singleton = false)]
    public class NugetPackageLoader : BaseModule
    {


        private const string PATH_PACKAGE = "packages";
        private static readonly string PATH_PACKAGE_REPO = $"{Utils.Utils.Helper.CURRENT_FOLDER_PACKAGES}/repos/nuget";
        private static readonly string PATH_PACKAGE_LIBS = $"{PATH_PACKAGE_REPO}/lib";
        private static readonly string PATH_PACKAGE_MAPS = $"{PATH_PACKAGE_REPO}/map";//Pasta para mapear os arquivos de pacotes
        private readonly IAssemblyLoadContextContract assemblyLoadContextContract;
        Dictionary<string, int> Dependences;

        public string PathNugetPackage { get; }

        public NugetPackageLoader(IAssemblyLoadContextContract assemblyLoadContextContract)
        {
            if (!Directory.Exists(PATH_PACKAGE))
            {
                Directory.CreateDirectory(PATH_PACKAGE);
            }
            if (!Directory.Exists(PATH_PACKAGE_REPO))
            {
                Directory.CreateDirectory(PATH_PACKAGE_REPO);
            }
            if (!Directory.Exists(PATH_PACKAGE_LIBS))
            {
                Directory.CreateDirectory(PATH_PACKAGE_LIBS);
            }

            Dependences = new Dictionary<string, int>();
            this.assemblyLoadContextContract = assemblyLoadContextContract;
        }


        public void Load(string id, string version)
        {

            MapDependenceNuget nugetMap = new MapDependenceNuget(id, version);


            ConcurrentDictionary<string, MapDependenceNuget> dependences = MapNugetDependences(nugetMap);
            SaveDependenceMap(nugetMap);

            foreach (var item in dependences)
            {
                if (Dependences.ContainsKey(item.Key))
                {
                    Dependences[item.Key]++;
                }
                else
                {
                    Dependences.Add(item.Key, 1);
                    assemblyLoadContextContract.RegisterAssemblyLoadContext(item.Value.PackagePath);
                }
            }
        }


        public void Unload() { }
        public void Reload() { }

        private ConcurrentDictionary<string, MapDependenceNuget> MapNugetDependences(MapDependenceNuget nuget)
        {
            nuget.MapPackage();

            var current_targetFramework = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;
            var current_idPackage = nuget.Nuspec["package"]["metadata"]["id"].InnerText;
            var current_versionPackage = nuget.Nuspec["package"]["metadata"]["version"].InnerText;

            ConcurrentDictionary<string, MapDependenceNuget> dependeces = new ConcurrentDictionary<string, MapDependenceNuget>();

            foreach (var dependece in nuget.Dependences)
            {
                if (!dependeces.ContainsKey(dependece.Key))
                {
                    dependeces.TryAdd(dependece.Key, dependece.Value);
                }
            }
            List<Task> tasks = new List<Task>();

            for (int i = 0; i < dependeces.Count; i++)
            {
                if (!dependeces.ElementAt(i).Value.HasMaped)
                {
                    MapDependenceNuget element = dependeces.ElementAt(i).Value;
                    var bag = new TaskJobModule.BagAction();
                    bag.Add("element", element);
                    bag.Add("dependeces", dependeces);
                    tasks.Add(Task.Factory.StartNew(task_action, bag));
                }

                if (dependeces.Count - 2 == i)
                {
                    Task.WaitAll(tasks.ToArray());
                    tasks.Clear();

                }

            }
            Task.WaitAll(tasks.ToArray());

            return dependeces;
        }
        private void task_action(object? state)
        {
            var element = (state as TaskJobModule.BagAction).GetBag("element") as MapDependenceNuget;
            var dependeces = (state as TaskJobModule.BagAction).GetBag("dependeces") as ConcurrentDictionary<string, MapDependenceNuget>;

            element.MapPackage();
            foreach (var dependece in element.Dependences)
            {
                if (!dependeces.ContainsKey(dependece.Key))
                {
                    dependeces.TryAdd(dependece.Key, dependece.Value);
                }
            }
        }
        private void SaveDependenceMap(MapDependenceNuget mapDependenceNuget)
        {
            var path_mapFile = Path.Combine(PATH_PACKAGE_MAPS, $"{mapDependenceNuget.Id}.{mapDependenceNuget.Version}.map");
            //Gerar um arquivo com todo o mapreamento do componente para assim não ter que baixar e mapear tudo novamente
            if (!File.Exists(path_mapFile))
            {
                var data_map = System.Text.Json.JsonSerializer.Serialize<MapDependenceNuget>(mapDependenceNuget, new System.Text.Json.JsonSerializerOptions()
                {
                    IncludeFields = true
                });
                File.WriteAllText(path_mapFile, data_map);
            }

        }

    }

    class MapDependenceNuget
    {
        private static readonly string PATH_NUGET_CACHE = Path.Combine(Propeus.Module.Utils.Utils.Helper.USER_PROFILE_DIRECTORY, ".nuget\\packages");

        private const string PATH_PACKAGE = "packages";
        private const string PATH_PACKAGE_REPO = $"{PATH_PACKAGE}/repos/nuget";
        private const string PATH_PACKAGE_LIBS = $"{PATH_PACKAGE_REPO}/lib";
        private XmlDocument nuspec;

        public string Id { get; private set; }
        public string Version { get; private set; }
        public Dictionary<string, MapDependenceNuget> Dependences { get; private set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string PackageId => $"{Id}::{Version}";

        [System.Text.Json.Serialization.JsonIgnore]
        public string PackagePath => GetPackagePath();
        [System.Text.Json.Serialization.JsonIgnore]
        public string PackageLib => GetPackageLibPath();



        [System.Text.Json.Serialization.JsonIgnore]
        public string UrlPackage => $"https://www.nuget.org/api/v2/package/{Id}/{Version}";

        [System.Text.Json.Serialization.JsonIgnore]
        public bool HasCache => HasNuspecFromCache(Id, Version);
        [System.Text.Json.Serialization.JsonIgnore]
        public bool HasDownloaded => File.Exists(PackagePath) || HasCache;
        public bool HasMaped { get; private set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public XmlDocument Nuspec { get => nuspec; }

        public MapDependenceNuget(string Id, string Version)
        {
            this.Id = Id;
            this.Version = Version;
            Dependences = new Dictionary<string, MapDependenceNuget>();

            HasMaped = false;
        }


        public void DownloadPackage()
        {
            if (!HasDownloaded)
            {
                using (var client = new HttpClient())
                {
                    System.Console.WriteLine($"Baixando o pacote pela URL: {UrlPackage}");
                    using (var s = client.GetStreamAsync(UrlPackage))
                    {
                        using (var fs = new FileStream(PackagePath, FileMode.OpenOrCreate))
                        {
                            s.Result.CopyTo(fs);
                        }
                    }
                }
            }
        }
        private XmlDocument LoadNuspec(string packagePath)
        {
            XmlDocument document = new XmlDocument();
            using (ZipArchive zip = ZipFile.Open(packagePath, ZipArchiveMode.Read))
            {
                foreach (ZipArchiveEntry entry in zip.Entries)
                {
                    if (entry.Name.Contains(".nuspec"))
                    {
                        Stream readerStream = entry.Open();
                        using (var ms = new MemoryStream())
                        {
                            readerStream.CopyTo(ms);
                            ms.Position = 0;
                            document.Load(ms);

                        }
                        break;
                    }
                }
            }
            return document;
        }
        private XmlNodeList FindDependences(XmlDocument document)
        {
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(document.DocumentElement.OwnerDocument.NameTable);
            nsmgr.AddNamespace("d", document.DocumentElement.NamespaceURI);
            XmlNodeList? xmlNodes = document.SelectNodes("//d:dependency", nsmgr); //Mudar depois para pegar a framework
            return xmlNodes;
        }
        private XmlDocument LoadNuspecFromCache(string idPackage, string versionPackage)
        {
            var pathPackageCache = Path.Combine(PATH_NUGET_CACHE, idPackage, versionPackage, idPackage + ".nuspec");
            XmlDocument document = new XmlDocument();
            using (StreamReader streamReader = new StreamReader(pathPackageCache))
            {
                document.Load(streamReader);
            }
            return document;
        }
        private bool HasNuspecFromCache(string idPackage, string versionPackage)
        {
            var pathPackageCache = Path.Combine(PATH_NUGET_CACHE, idPackage, versionPackage, idPackage + ".nuspec");
            return File.Exists(pathPackageCache);



        }
        private string GetPackagePath()
        {
            if (HasCache)
            {
                return Path.Combine(PATH_NUGET_CACHE, Id, Version, Id + "." + Version + ".nupkg");
            }
            else
            {
                return $"{PATH_PACKAGE_REPO}/{Id}.{Version}.nupkg";
            }
        }
        private string GetPackageLibPath()
        {
            var currentFramework = Utils.Utils.Helper.GetCurrentTargetFramework();

            if (HasCache)
            {
                var path = Path.Combine(PATH_NUGET_CACHE, Id, Version, Id, "lib", currentFramework, Id + ".dll");
                if (!File.Exists(path))
                {
                    for (int major = 2; major > 0; major--)
                    {
                        for (int minor = 2; minor > 0; minor--)
                        {
                            path = Path.Combine(PATH_NUGET_CACHE, Id, Version, Id, "lib", "netstandard"+major+"."+minor, Id + ".dll");
                            if (File.Exists(path))
                                goto GT_NetStandard_Encontrado;
                         
                        }
                    }
                }
                GT_NetStandard_Encontrado:
                return path;
            }
            else
            {
                //Aqui tenho que supunhetar que o arquivo ja esteja na pasta lib, caso nao esteja, descompactar

                var pathLib = $"{PATH_PACKAGE_LIBS}/{Id}.{Version}.nupkg";
                throw new NotImplementedException();
            }
        }
        public void MapPackage()
        {
            if (!HasCache)
            {
                DownloadPackage();

                if (!HasMaped)
                {

                    nuspec ??= LoadNuspec(PackagePath);

                }
            }
            else
            {
                nuspec ??= LoadNuspecFromCache(Id, Version);
            }

            XmlNodeList? xmlNodes = FindDependences(nuspec);

            foreach (XmlNode xmlNode in xmlNodes)
            {

                var idPackage_dependence = xmlNode.Attributes["id"].Value;
                var versionPackage_dependence = xmlNode.Attributes["version"].Value;
                var id_dependencePackage = $"{idPackage_dependence}::{versionPackage_dependence}";
                if (!Dependences.ContainsKey(id_dependencePackage))
                {
                    var map = new MapDependenceNuget(idPackage_dependence, versionPackage_dependence);

                    Dependences.Add(map.ToString(), map);
                }
            }

            HasMaped = true;

        }

        public override string ToString()
        {
            return PackageId;
        }

        public override bool Equals(object? obj)
        {
            return this.ToString() == obj?.ToString();
        }

    }

    public class LinkedLibLoader : IDisposable
    {
        internal Queue<LinkedLibLoader> _nextLibs;
        private bool disposedValue;
        private readonly string pathLib;

        /// <summary>
        /// Carrega informações basica da lib e suas dependencias
        /// </summary>
        /// <param name="pathLib"></param>
        public LinkedLibLoader(string pathLib, string fileLib)
        {
            if (!Directory.Exists(pathLib))
            {
                Directory.CreateDirectory(pathLib);
            }

            this.pathLib = Path.Combine(pathLib, fileLib);
            _nextLibs = new Queue<LinkedLibLoader>();

            //Extrair nome, versao e framework do path "../package/@id/@version/@targetFramework"
        }

        //Carrega a lib atual
        public void Load(IAssemblyLoadContextContract assemblyLoadContextContract)
        {

            if (!assemblyLoadContextContract.ExistsAssemblyLoadContext(pathLib))
            {
                assemblyLoadContextContract.RegisterAssemblyLoadContext(pathLib);
            }
        }
        public void Unload(IAssemblyLoadContextContract assemblyLoadContextContract)
        {
            if (assemblyLoadContextContract.ExistsAssemblyLoadContext(pathLib))
            {
                assemblyLoadContextContract.UnregisterAssemblyLoadContext(pathLib);
            }
        }

        //Indica se ha outra lib a ser carregado
        public bool HasNextLib() { return _nextLibs == null; }
        public LinkedLibLoader GetNextLib()
        {
            return _nextLibs.Dequeue();
        }
        public void AddNextLib(LinkedLibLoader linkedLibLoader)
        {
            _nextLibs.Enqueue(linkedLibLoader);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~LinkedLibLoader()
        // {
        //     // Não altere este código. Coloque o código de limpeza no método 'Dispose(bool disposing)'
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Não altere este código. Coloque o código de limpeza no método 'Dispose(bool disposing)'
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
