using Propeus.Module.Abstract.Attributes;
using Propeus.Module.Abstract.Interfaces;

namespace Propeus.Module.CoreTests
{
    public partial class GerenciadorTests
    {


        [ModuleContract(default(string))]
        public interface IContratoInvalido : IModule
        {

        }


    }
}