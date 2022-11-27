using Propeus.Modulo.Modelos;
using Propeus.Modulo.Modelos.Atributos;
using Propeus.Modulo.Modelos.Interfaces;

namespace Propeus.Modulo.Dinamico.Teste.Modulo
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
