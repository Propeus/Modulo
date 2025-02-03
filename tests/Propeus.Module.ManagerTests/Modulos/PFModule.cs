using Propeus.Module.Abstract;
using Propeus.Module.Abstract.Attributes;
using Propeus.Module.Abstract.Interfaces;

namespace Propeus.Module.ManagerTests.Modulos
{
    [Module(Description = "Modulo para pessoa Fisica")]
    internal class PFModule : BaseModule
    {
        public PFModule(IModuleManager moduleManager)
        {
            ModuleManager = moduleManager;
        }

        public IModuleManager ModuleManager { get; }
    }

    [Module(Description = "Modulo para pessoa Juridica")]
    internal class PJModule : BaseModule
    {
        public PJModule(PFModule pFModule)
        {
            PFModule = pFModule;
        }

        public PFModule PFModule { get; }
    }


}
