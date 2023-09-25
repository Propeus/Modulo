using Propeus.Module.Abstract.Attributes;
using Propeus.Module.Abstract.Interfaces;

namespace Propeus.Module.CoreTests
{
    public partial class GerenciadorTests
    {


        [ModuleContract(typeof(OutroModuleDependenciaInterfaceValida))]
        public interface IModuleInstanciaUnica : IModule
        {

        }


    }
}