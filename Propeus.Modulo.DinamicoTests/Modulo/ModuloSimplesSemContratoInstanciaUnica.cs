using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Abstrato.Interfaces;

namespace Propeus.Modulo.DinamicoTests.Modulo
{
    [Modulo]
    public class ModuloSimplesSemContratoInstanciaUnica : ModuloBase
    {
        public ModuloSimplesSemContratoInstanciaUnica(IGerenciador gerenciador, bool instanciaUnica = true) : base(gerenciador, instanciaUnica)
        {
        }
    }
}
