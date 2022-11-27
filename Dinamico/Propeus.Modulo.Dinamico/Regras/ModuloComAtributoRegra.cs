using Propeus.Modulo.Modelos;
using Propeus.Modulo.Modelos.Atributos;
using Propeus.Modulo.Modelos.Interfaces;
using Propeus.Modulo.Util.Objetos;
using Propeus.Modulo.Util.Atributos;
using System.Linq;

namespace Propeus.Modulo.Dinamico.Regras
{
    public class ModuloComAtributoRegra : IRegra
    {
        public bool Executar(params object[] args)
        {
            IModuloBinario path = (IModuloBinario)args[0];
            ModuloAssemblyLoadContext loader = new ModuloAssemblyLoadContext();
            System.Reflection.Assembly assembly = loader.LoadFromStream(path.Memoria);
            int src = assembly.DefinedTypes.Where(t => t.UnderlyingSystemType.Herdado<IModulo>() && t.UnderlyingSystemType.PossuiAtributo<ModuloAttribute>()).Count();
            assembly = null;
            loader.Unload();
            loader = null;
            path.Memoria.Seek(0, System.IO.SeekOrigin.Begin);
            path = null;
            return src != 0;
        }
    }
}
