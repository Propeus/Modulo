using Propeus.Modulo.Abstrato.Attributes;
using Propeus.Modulo.Abstrato.Interfaces;

namespace Propeus.Modulo.CoreTests
{
    public partial class GerenciadorTests
    {
    

        [ModuleContract(typeof(OutroModuleDependenciaInterfaceValida))]
        public interface IModuleInstanciaUnica : IModule
        {

        }
        

    }
}