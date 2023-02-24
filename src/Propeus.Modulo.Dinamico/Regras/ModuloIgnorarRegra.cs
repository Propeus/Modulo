using System.Linq;

using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Util;

namespace Propeus.Modulo.Dinamico.Regras
{
    public class ModuloIgnorarRegra : IRegra
    {
        public bool Executar(params object[] args)
        {
            IModuloBinario path = (IModuloBinario)args[0];
            ModuloAssemblyLoadContext loader = new();
            System.Reflection.Assembly assembly = loader.LoadFromStream(path.Memoria);
            bool result = assembly.DefinedTypes.Any(t => t.UnderlyingSystemType.Is<Gerenciador>());
            assembly = null;
            loader.Unload();
            loader = null;
            _ = path.Memoria.Seek(0, System.IO.SeekOrigin.Begin);
            path = null;
            return !result;
        }
    }
}
