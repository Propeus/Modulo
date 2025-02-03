using Propeus.Module.Abstract.Attributes;
using Propeus.Module.Abstract.Interfaces;
using Propeus.Module.ManagerTests.Modulos;

namespace Propeus.Module.ManagerTests.Contratos
{
    [ModuleContract(typeof(PessoaModule))]
    public interface IPessoaContrato : IModule
    {
        public string Nome { get; }
        public int Idade { get; }
        public string? Cpf { get; }
        public string Genero { get; }
    }
}
