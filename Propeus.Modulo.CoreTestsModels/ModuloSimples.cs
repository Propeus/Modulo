using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Abstrato.Interfaces;

namespace Propeus.Modulo.CoreTestsModels
{
    [Modulo]
    public class ModuloSimples : ModuloBase
    {
        public ModuloSimples(IGerenciador gerenciador, bool instanciaUnica = false) : base(gerenciador, instanciaUnica)
        {
        }

        public int Teste { get; set; }
    }
}
