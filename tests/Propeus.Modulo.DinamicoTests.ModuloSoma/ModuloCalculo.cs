using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Abstrato.Modulos;

namespace Propeus.Modulo.DinamicoTests.ModuloSoma
{
    [ModuleContract("ModuloCalculo")]
    public interface ModuloCalculoContrato: IModule
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