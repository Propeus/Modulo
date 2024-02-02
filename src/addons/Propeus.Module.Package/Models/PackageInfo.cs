using System.IO.Compression;

using Propeus.Module.Abstract;
using Propeus.Module.Package.Contracts;
using Propeus.Module.Package.Models;

namespace Propeus.Module.Package.Modules
{
    public partial class PackageModule
    {
      
        private class PackageInfo : BaseModel
        {
            private readonly IModuleDependeceContract _moduleDependeceContract;

            public PackageInfo(string fullPath, IModuleDependeceContract moduleDependeceContract)
            {
                this._moduleDependeceContract = moduleDependeceContract;



                this.FileInfo = new FileInfo(fullPath);
                this.PathFolderDestinationPackage = Path.Combine(Utils.Utils.Helper.CURRENT_FOLDER_PACKAGES, FileInfo.Name);
                UnzipPackage();

                this.PathManifestFile = Path.Combine(PathFolderDestinationPackage, MANIFEST_PACKAGE_NAME);
                Load();
            }

            public void Reload()
            {
                UnzipPackage();
                Unload();
                Load();
            }

            public void Unload()
            {
                //Unload Module
                File.Delete(PathDestinationModuleFile);
                //Unload Module

                //Unload Dependences module
                foreach (var dependence in PackageManifest.Dependences)
                {
                    var destModuleFileDependence = Path.Combine(Utils.Utils.Helper.CURRENT_FOLDER_DEPENDENCES, dependence.Key + ".dll");
                    File.Delete(destModuleFileDependence);
                    _moduleDependeceContract.RemoveDependencePackAge(dependence.Key, destModuleFileDependence);
                }
                //Unload Dependences module

            }

            public FileInfo FileInfo { get; private set; }
            public string PathFolderDestinationPackage { get; private set; }
            public string PathManifestFile { get; private set; }
            public PackageManifest PackageManifest { get; private set; }
            public string PathDestinationModuleFile { get; }

            private void Load()
            {
                //Manifest package
                this.PackageManifest = System.Text.Json.JsonSerializer.Deserialize<PackageManifest>(PathManifestFile);
                //Manifest package

                //Load Dependences module
                foreach (var dependence in PackageManifest.Dependences)
                {
                    var sourceModuleFileDependence = Path.Combine(PathFolderDestinationPackage, dependence.Key + ".dll");
                    var destModuleFileDependence = Path.Combine(Utils.Utils.Helper.CURRENT_FOLDER_DEPENDENCES, dependence.Key + ".dll");
                    if (!File.Exists(destModuleFileDependence))
                    {
                        File.Move(sourceModuleFileDependence, destModuleFileDependence);
                        _moduleDependeceContract.RegisterDependencePackage(dependence.Key, destModuleFileDependence);
                    }
                }
                //Load Dependences module

                //Reload Module
                var PathSourceModuleFile = Path.Combine(Utils.Utils.Helper.CURRENT_FOLDER_PACKAGES, FileInfo.Name + ".dll");
                var PathDestinationModuleFile = Path.Combine(Utils.Utils.Helper.CURRENT_FOLDER_MODULES, FileInfo.Name + ".dll");
                File.Move(PathSourceModuleFile, PathDestinationModuleFile, true);
                //Reload Module
            }
            private void UnzipPackage()
            {
                //Package file
                if (Directory.Exists(PathFolderDestinationPackage))
                {
                    Directory.Delete(PathFolderDestinationPackage, true);
                }
                Directory.CreateDirectory(PathFolderDestinationPackage);
                ZipFile.ExtractToDirectory(this.FileInfo.FullName, PathFolderDestinationPackage);
                //Package file
            }

        }
    }
}
