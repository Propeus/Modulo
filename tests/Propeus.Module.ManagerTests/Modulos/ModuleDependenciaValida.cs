using Propeus.Module.Abstract;
using Propeus.Module.Abstract.Attributes;

namespace Propeus.Module.CoreTests
{
    public partial class GerenciadorTests
    {
        [Module(Singleton = true)]
        public class ModuleDependenciaValida : BaseModule
        {
            public ModuleDependenciaValida(ModuleDependenciaInterfaceInvalidaOpcional Module) : base()
            {
            }
        }


    }
}