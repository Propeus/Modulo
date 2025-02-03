using Propeus.Module.Abstract.Attributes;
using Propeus.Module.IL.CoreTests.Modulos;

namespace Propeus.Module.IL.CoreTests.Contratos
{
    /// <summary>
    /// Interface para testar proxy em propriedade indexada
    /// </summary>
    [ModuleContract(typeof(TesteModule))]
    public interface IPropriedadeIndexadaContrato
    {
        int this[int valor] { get; set; }
    }
}