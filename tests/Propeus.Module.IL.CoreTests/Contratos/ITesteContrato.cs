using Propeus.Module.Abstract.Attributes;
using Propeus.Module.Abstract.Interfaces;
using Propeus.Module.IL.CoreTests.Modulos;

namespace Propeus.Module.IL.CoreTests.Contratos
{
    /// <summary>
    /// Interface para testar proxy em propriedade e metodos
    /// </summary>
    [ModuleContract(typeof(TesteModule))]
    public interface ITesteContrato : IModule
    {

        int testeProp { get; }
        int testeProp2 { get; set; }

        int TesteMetodo();
        int TesteMetodo2(int a);
    }
}