using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Attributes;

namespace Propeus.Modulo.CoreTests
{
    public partial class GerenciadorTests
    {
        [Module]
        public class ModuleDependenciaValida : BaseModule
        {
            public ModuleDependenciaValida(ModuleDependenciaInterfaceInvalidaOpcional Module) : base(true)
            {
            }
        }


    }
}