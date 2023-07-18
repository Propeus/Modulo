using System;

using Propeus.Modulo.Abstrato.Attributes;
using Propeus.Modulo.Abstrato.Interfaces;

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