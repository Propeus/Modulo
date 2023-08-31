using Propeus.Module.Abstract;
using Propeus.Module.Abstract.Attributes;

namespace Propeus.Modulo.CoreTests.Modulos
{
    [Module(Singleton = true)]
    public class TesteInstanciaUnicaModule : BaseModule
    {
        public TesteInstanciaUnicaModule() : base()
        {
        }

        public override string ToString()
        {
            return Id;
        }
    }
}