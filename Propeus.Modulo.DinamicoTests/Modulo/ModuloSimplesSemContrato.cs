using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Abstrato.Interfaces;

namespace Propeus.Modulo.DinamicoTests.Modulo
{
    [Modulo]
    public class ModuloSimplesSemContrato : ModuloBase
    {
        public ModuloSimplesSemContrato(IGerenciador gerenciador, bool instanciaUnica = false) : base(gerenciador, instanciaUnica)
        {
        }
    }
}
