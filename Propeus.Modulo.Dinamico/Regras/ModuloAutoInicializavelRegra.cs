using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Abstrato.Util;

namespace Propeus.Modulo.Dinamico.Regras
{
    public class ModuloAutoInicializavelRegra : IRegra
    {
        public IEnumerable<TypeInfo> modulosValidos { get; private set; } = new List<TypeInfo>();
        public bool Executar(params object[] args)
        {
            IModuloBinario path = (IModuloBinario)args[0];
            ModuloAssemblyLoadContext loader = new();
            Assembly assembly = loader.LoadFromStream(path.Memoria);
            modulosValidos = assembly.DefinedTypes.Where(t => t.UnderlyingSystemType.PossuiAtributo<ModuloAutoInicializavelAttribute>());
            assembly = null;
            loader.Unload();
            loader = null;
            _ = path.Memoria.Seek(0, System.IO.SeekOrigin.Begin);
            path = null;
            return modulosValidos.Any();
        }
    }
}
