using Propeus.Module.Abstract;
using Propeus.Module.Abstract.Attributes;

namespace Propeus.Module.CoreTests.Modulos
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