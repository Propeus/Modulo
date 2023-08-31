using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Attributes;

namespace Propeus.Modulo.CoreTests
{
    public partial class GerenciadorTests
    {
        [Module]
        public class ModuleParametroInvalido : BaseModule, IModuleValido
        {
            public ModuleParametroInvalido(int a) : base()
            {
            }
        }


    }
}