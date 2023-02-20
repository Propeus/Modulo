using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Core;

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
