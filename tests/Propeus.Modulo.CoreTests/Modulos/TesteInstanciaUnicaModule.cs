using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Attributes;

namespace Propeus.Modulo.CoreTests.Modulos
{
    [Module]
    public class TesteInstanciaUnicaModule : BaseModule
    {
        public TesteInstanciaUnicaModule() : base(true)
        {
        }

        public override string ToString()
        {
            return Id;
        }
    }
}