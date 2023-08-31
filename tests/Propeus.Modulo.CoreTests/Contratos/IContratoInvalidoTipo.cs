using System;

using Propeus.Module.Abstract.Attributes;
using Propeus.Module.Abstract.Interfaces;

namespace Propeus.Modulo.CoreTests
{
    public partial class GerenciadorTests
    {


        [ModuleContract(default(Type))]
        public interface IContratoInvalidoTipo : IModule
        {

        }


    }
}