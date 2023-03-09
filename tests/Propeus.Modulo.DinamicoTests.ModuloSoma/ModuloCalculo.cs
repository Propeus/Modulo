using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Abstrato.Interfaces;

namespace Propeus.Modulo.DinamicoTests.ModuloSoma
{
    [Modulo]
    public class ModuloCalculo : ModuloBase
    {
        public ModuloCalculo(IGerenciador gerenciador) : base(gerenciador)
        {
        }

        public int Calcular(int a, int b)
        {
            return a + b;
        }
    }
}