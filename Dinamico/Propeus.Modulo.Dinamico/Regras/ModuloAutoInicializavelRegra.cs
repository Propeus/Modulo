using Propeus.Modulo.Modelos;
using Propeus.Modulo.Modelos.Atributos;
using Propeus.Modulo.Modelos.Interfaces;
using Propeus.Modulo.Util.Atributos;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Propeus.Modulo.Dinamico.Regras
{
    public class ModuloAutoInicializavelRegra : IRegra
    {
        public IEnumerable<TypeInfo> modulosValidos { get; private set; } = new List<TypeInfo>();
        public bool Executar(params object[] args)
        {
            IModuloBinario path = (IModuloBinario)args[0];
            ModuloAssemblyLoadContext loader = new ModuloAssemblyLoadContext();
            Assembly assembly = loader.LoadFromStream(path.Memoria);
            modulosValidos = assembly.DefinedTypes.Where(t => t.UnderlyingSystemType.PossuiAtributo<ModuloAutoInicializavelAttribute>());
            assembly = null;
            loader.Unload();
            loader = null;
            path.Memoria.Seek(0, System.IO.SeekOrigin.Begin);
            path = null;
            return modulosValidos.Any();
        }
    }
}
