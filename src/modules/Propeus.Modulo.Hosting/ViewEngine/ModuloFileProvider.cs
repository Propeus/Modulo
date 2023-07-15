using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Hosting.Contracts;

namespace Propeus.Modulo.Hosting.ViewEngine
{
    internal class ModuloFileProvider : IFileProvider
    {
        private readonly IModuleManager gerenciador;

        public ModuloFileProvider(IModuleManager gerenciador)
        {
            this.gerenciador = gerenciador;

        }

        public IFileInfo GetFileInfo(string subpath)
        {
            string? controllerName;
            ModuloFileInfo? result;

            controllerName = subpath.Split('.')[0];
            var modules = gerenciador.ListAllModules().ToList();
            foreach (var item in modules.Where(x => x.Name.Contains("Controller")).Cast<ModuloController>())
            {
                if (item.Name.Contains(controllerName))
                    return new ModuloFileInfo(item, controllerName);
            }
            return new NotFoundFileInfo(subpath);
        }

        IDirectoryContents IFileProvider.GetDirectoryContents(string subpath)
        {
            return null;
        }

        public IChangeToken Watch(string filter)
        {
            string? controllerName;
            ModuloController? result = null;

            controllerName = filter.Split('.')[0];
            var modules = gerenciador.ListAllModules().ToList();
            foreach (ModuloController item in modules.Where(x => x.Name.Contains("Controller")).Cast<ModuloController>())
            {
                if (item.Name.Contains(controllerName))
                {
                    result = item;
                    return new ModuloChangeToken(result,gerenciador.GetModule<IModuleProviderModuleContract>());
                }
            }
            return null;

        }
    }

    public class ModuloFileInfo : IFileInfo
    {
        private string _view;
        private byte[] _viewContent;

        public ModuloFileInfo(ModuloController moduloController, string viewName)
        {
            var _resource = new ResourceManager("Resource", moduloController.GetType().Assembly);
            var ns = moduloController.GetType().FullName.Replace("Controller", "");
            ns += viewName + "cshtml";

            _view = _resource.GetString(ns);
            _viewContent = Encoding.UTF8.GetBytes(_view);

            Name = viewName;
            PhysicalPath = null;
            Exists = true;
            using (var ms = CreateReadStream())
            {
                Length = ms.Length;
            }
            LastModified = DateTime.Now;
            IsDirectory = false;
        }

        public Stream CreateReadStream()
        {
            return new MemoryStream(_viewContent);
        }

        public bool Exists { get; }
        public long Length { get; }
        public string PhysicalPath { get; }
        public string Name { get; }
        public DateTimeOffset LastModified { get; }
        public bool IsDirectory { get; }
    }
}
