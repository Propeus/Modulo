﻿using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Attributes;

namespace Propeus.Modulo.CoreTests
{
    public partial class GerenciadorTests
    {
        [Module]
        public class ModuleDependenciaInterfaceInvalidaOpcional : BaseModule
        {
            public ModuleDependenciaInterfaceInvalidaOpcional(InterfaceModuleInvalido interfaceModuleInvalido = null) : base(false)
            {
            }
        }
       

    }
}