using Propeus.Module.Abstract;
using Propeus.Module.Abstract.Attributes;

namespace Propeus.Module.CoreTests
{
    public partial class GerenciadorTests
    {
        [Module(Singleton = true)]
        public class ModuleIntanciaUnica : BaseModule
        {
            public ModuleIntanciaUnica() : base()
            {
            }
        }

    }
}