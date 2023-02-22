using Microsoft.VisualStudio.TestTools.UnitTesting;

using Propeus.Modulo.Abstrato;

namespace Propeus.Modulo.CoreTests
{
    [TestClass()]
    public class ModuloBinarioTests
    {
        private readonly string ns = "Propeus.Modulo.CoreTestsModels";

      

     

        private void Copiar(string modulo, string vCore = "net7.0", string modo = "Debug")
        {
            string pathDll = modulo;
            Console.WriteLine(pathDll);
            DirectoryInfo d = new(Directory.GetCurrentDirectory());
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