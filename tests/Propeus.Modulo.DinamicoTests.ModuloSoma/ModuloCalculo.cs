using Propeus.Module.Abstract;
using Propeus.Module.Abstract.Attributes;
using Propeus.Module.Abstract.Interfaces;

namespace Propeus.Module.DinamicoTests.ModuloSoma
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