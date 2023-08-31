﻿using Propeus.Module.Abstract;
using Propeus.Module.Abstract.Attributes;

namespace Propeus.Modulo.CoreTests
{
    public partial class GerenciadorTests
    {
        [Module]
        public class ModuleInstanciaUnica : BaseModule, IModuleInstanciaUnica
        {
            public ModuleInstanciaUnica() : base()
            {
            }
        }


    }
}