using Propeus.Module.Abstract;
using Propeus.Module.Abstract.Attributes;

namespace Propeus.Modulo.CoreTests
{
    public partial class GerenciadorTests
    {
        [Module(Singleton = true)]
        public class ModuleDependenciaInvalida : BaseModule
        {
            public ModuleDependenciaInvalida(ModuleInvallido ModuleInvallido) : base()
            {
            }
        }


    }
}