﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

using Propeus.Modulo.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Propeus.Modulo.CoreTests
{
    [TestClass()]
    public class ModuloBinarioTests
    {
        string ns = "Propeus.Modulo.CoreTestsModels";

        [TestMethod]
        public void CarregarBinarios()
        {

#if DEBUG
            Copiar(ns, modo: "Debug");
#else
            Copiar(ns,modo:"Release");
#endif
            ModuloBinario modelo = new ModuloBinario(ns + ".dll");
            modelo.Dispose();
            Assert.IsNotNull(modelo.Memoria);
        }

        [TestMethod]
        public void CarregarInformacao()
        {
#if DEBUG
            Copiar(ns, modo: "Debug");
#else
            Copiar(ns,modo:"Release");
#endif
            ModuloBinario modelo = new ModuloBinario(ns + ".dll");
            ModuloInformacao info = new ModuloInformacao(modelo);
            Assert.IsNotNull(info.Versao);
            Assert.IsNotNull(info.Caminho);
            info.Dispose();
            modelo.Dispose();

        }


        private void Copiar(string modulo, string vCore = "net7.0", string modo = "Debug")
        {
            string pathDll = modulo;
            Console.WriteLine(pathDll);
            DirectoryInfo d = new DirectoryInfo(Directory.GetCurrentDirectory());
            Console.WriteLine(d.FullName);
            d = d.Parent.Parent.Parent.Parent;
            Console.WriteLine(d.FullName);
            d = d.GetDirectories().First(p => p.Name == pathDll);
            Console.WriteLine(d.FullName);
            d = d.GetDirectories().First(p => p.Name == "bin");
            Console.WriteLine(d.FullName);
            d = d.GetDirectories().First(p => p.Name == modo);
            Console.WriteLine(d.FullName);
            d = d.GetDirectories().First(p => p.Name == vCore);
            Console.WriteLine(d.FullName);
            string path = d.GetFiles($"{pathDll}.dll").First().FullName;
            Console.WriteLine(path);

            string ldir = Path.Combine(Directory.GetCurrentDirectory(), $"{pathDll}.dll");
            File.Delete(ldir);
            File.Copy(path, ldir, true);

        }
    }
}