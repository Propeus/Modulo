using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

using Propeus.Modulo.Abstrato.Interfaces;

namespace Propeus.Modulo.Hosting.ViewEngine
{
    internal class ModuloFileProvider : IFileProvider
    {
        private readonly IGerenciador gerenciador;

        public ModuloFileProvider(IGerenciador gerenciador)
        {
            this.gerenciador = gerenciador;

        }

        public IFileInfo GetFileInfo(string subpath)
        {
            string? controllerName;
            ModuloFileInfo? result;

            controllerName = subpath.Split('.')[0];
            foreach (var item in gerenciador.Listar().Where(x => x.Nome.Contains("Controller")).Cast<ModuloController>())
            {
                if (item.Nome.Contains(controllerName))
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
            foreach (ModuloController item in gerenciador.Listar().Where(x => x.Nome.Contains("Controller")).Cast<ModuloController>())
            {
                if (item.Nome.Contains(controllerName))
                {
                    result = item;
                    return new ModuloChangeToken(result);
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
