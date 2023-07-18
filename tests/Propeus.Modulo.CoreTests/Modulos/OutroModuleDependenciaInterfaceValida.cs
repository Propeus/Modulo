using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Attributes;

namespace Propeus.Modulo.CoreTests
{
    public partial class GerenciadorTests
    {
        [Module]
        public class OutroModuleDependenciaInterfaceValida : BaseModule, IModuleValido
        {
            public OutroModuleDependenciaInterfaceValida() : base(false)
            {
            }
        }


    }
}