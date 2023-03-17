using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Abstrato.Interfaces;

namespace Propeus.Modulo.DinamicoTests.ModuloSoma
{
    [Modulo]
    public class ModuloCalculo : ModuloBase
    {
        public ModuloCalculo() : base()
        {
        }

        public int Calcular(int a, int b)
        {
            return a + b;
        }
    }
}