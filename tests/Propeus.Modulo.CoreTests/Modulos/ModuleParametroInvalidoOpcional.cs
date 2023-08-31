using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Attributes;

namespace Propeus.Modulo.CoreTests
{
    public partial class GerenciadorTests
    {
        [Module]
        public class ModuleParametroInvalidoOpcional : BaseModule, IModuleValido
        {
            public ModuleParametroInvalidoOpcional() : base()
            {
            }
        }


    }
}