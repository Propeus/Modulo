using Propeus.Modulo.IL.Proxy;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace Propeus.Modulo.IL.Interfaces
{
    internal interface IILPilha : IILExecutor
    {
        OpCode Code { get; }

        ILBuilderProxy Proxy { get; }
    }
}