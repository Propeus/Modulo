using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;

namespace Propeus.Modulo.Modelos.Teste
{
    [TestClass]
    [TestCategory("Modelos.ModuloBinario")]
    public class Teste
    {
        [TestMethod]
        public void CarregarBinarios()
        {
#if DEBUG
            Copiar("Propeus.Modulo.Modelos.Teste.Modulo", modo: "Debug");
#else
            Copiar("Propeus.Modulo.Modelos.Teste.Modulo",modo:"Release");
#endif
            ModuloBinario modelo = new ModuloBinario("Propeus.Modulo.Modelos.Teste.Modulo.dll");
            modelo.Dispose();
            Assert.IsNotNull(modelo.Memoria);
        }

        [TestMethod]
        public void CarregarInformacao()
        {
#if DEBUG
            Copiar("Propeus.Modulo.Modelos.Teste.Modulo", modo: "Debug");
#else
            Copiar("Propeus.Modulo.Modelos.Teste.Modulo",modo:"Release");
#endif
            ModuloBinario modelo = new ModuloBinario("Propeus.Modulo.Modelos.Teste.Modulo.dll");
            ModuloInformacao info = new ModuloInformacao(modelo);
            Assert.IsNotNull(info.Versao);
            Assert.IsNotNull(info.Caminho);
            info.Dispose();
            modelo.Dispose();

        }


        private void Copiar(string modulo, string vCore = "net5", string modo = "Debug")
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
