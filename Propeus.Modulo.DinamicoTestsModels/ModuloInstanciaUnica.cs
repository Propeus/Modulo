using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Abstrato.Interfaces;

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
