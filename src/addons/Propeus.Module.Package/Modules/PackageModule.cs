using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using Propeus.Module.Abstract;
using Propeus.Module.Abstract.Attributes;
using Propeus.Module.Abstract.Interfaces;
using Propeus.Module.Package.Contracts;

namespace Propeus.Module.Package.Modules
{


    [Module(Description = "Modulo para controlar pacotes", AutoStartable = true, AutoUpdate = false, KeepAlive = true, Singleton = true)]
    public partial class PackageModule : BaseModule
    {
        const string PACKAGE_EXTENSION = ".mpkg";
        static readonly string MANIFEST_PACKAGE_NAME = "manifest.json";
        private readonly IModuleDependeceContract _dependece;
        private string _currentDirectory;
        private string _currentFolderModules;
        private FileSystemWatcher _fileSystemWatcher;
        private Dictionary<string, PackageInfo> _packeages;

        public PackageModule(IModuleDependeceContract dependece, string folderModules = "packages")
        {
            _currentDirectory = Utils.Utils.Helper.CURRENT_DIRECTORY;
            _currentFolderModules = Path.Combine(_currentDirectory, folderModules);
            _dependece = dependece;
            _packeages = new Dictionary<string, PackageInfo>();
        }

        public override void ConfigureModule()
        {
            _fileSystemWatcher = new FileSystemWatcher
            {
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.Size

            };
            _fileSystemWatcher.Changed += _fileSystemWatcher_OnEvent;
            _fileSystemWatcher.Created += _fileSystemWatcher_OnEvent;
            _fileSystemWatcher.Deleted += _fileSystemWatcher_OnEvent;
            _fileSystemWatcher.Renamed += _fileSystemWatcher_Renamed;
            _fileSystemWatcher.Filter = "*" + PACKAGE_EXTENSION;
            _fileSystemWatcher.IncludeSubdirectories = true; //Desabilitado pois o fine coverage copia as dll, duplicando e causando erros inesperados
            _fileSystemWatcher.Path = _currentFolderModules;

            base.ConfigureModule();
        }

        public override void Launch()
        {
            _fileSystemWatcher.EnableRaisingEvents = true;
            base.Launch();
        }

        #region Eventos & Funcoes

        private void _fileSystemWatcher_Renamed(object sender, RenamedEventArgs e)
        {

        }

        private void _fileSystemWatcher_OnEvent(object sender, FileSystemEventArgs e)
        {
            var packageInfo = _packeages.ContainsKey(e.FullPath) ? _packeages[e.FullPath] : new PackageInfo(e.FullPath, _dependece);
            switch (e.ChangeType)
            {
                case WatcherChangeTypes.Created:
                    //Se nao existir no _packeages logo será criado e carregado
                    break;
                case WatcherChangeTypes.Deleted:
                    packageInfo.Unload();
                    break;
                case WatcherChangeTypes.Changed:
                    packageInfo.Reload();
                    break;
                case WatcherChangeTypes.All:
                    break;
                default:
                    throw new InvalidOperationException();

            }
        }

        #endregion
    }
}
