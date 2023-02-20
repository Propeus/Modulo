using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Core;

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
