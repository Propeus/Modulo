using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Core;

namespace Propeus.Modulo.DinamicoTestsModels
{
    [Modulo]
    public class ModuloInstanciaUnica : ModuloBase
    {
        public ModuloInstanciaUnica(IGerenciador gerenciador, bool instanciaUnica = true) : base(gerenciador, instanciaUnica)
        {
        }
    }
}
