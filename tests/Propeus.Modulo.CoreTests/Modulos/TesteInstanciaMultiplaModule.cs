using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Attributes;

namespace Propeus.Modulo.CoreTests.Modulos
{
    [Module]
    public class TesteInstanciaMultiplaModule : BaseModule
    {
        public TesteInstanciaMultiplaModule() : base()
        {
        }

        public override string ToString()
        {
            return Id;
        }
    }
}