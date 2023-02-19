using System.Linq;

using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Abstrato.Util;
using Propeus.Modulo.Core;

namespace Propeus.Modulo.Dinamico.Regras
{
    public class ModuloComAtributoRegra : IRegra
    {
        public bool Executar(params object[] args)
        {
            IModuloBinario path = (IModuloBinario)args[0];
            ModuloAssemblyLoadContext loader = new();
            System.Reflection.Assembly assembly = loader.LoadFromStream(path.Memoria);
            int src = assembly.DefinedTypes.Where(t => t.UnderlyingSystemType.Herdado<IModulo>() && t.UnderlyingSystemType.PossuiAtributo<ModuloAttribute>()).Count();
            assembly = null;
            loader.Unload();
            loader = null;
            _ = path.Memoria.Seek(0, System.IO.SeekOrigin.Begin);
            path = null;
            return src != 0;
        }
    }
}
