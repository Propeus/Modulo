using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Attributes;
using Propeus.Modulo.Abstrato.Interfaces;

namespace Propeus.Modulo.DinamicoTests.ModuloSoma
{
    [ModuleContract("ModuloCalculo")]
    public interface ModuloCalculoContrato : IModule
    {
        int Calcular(int a, int b);
    }

    [Module]
    public class ModuloCalculo : BaseModule
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