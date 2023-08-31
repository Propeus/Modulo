using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Attributes;

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