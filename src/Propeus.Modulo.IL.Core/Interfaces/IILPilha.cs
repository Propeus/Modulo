using System;
using System.Reflection.Emit;

using Propeus.Modulo.IL.Core.Proxy;

namespace Propeus.Modulo.IL.Core.Interfaces
{
    internal interface IILPilha : IILExecutor, IDisposable
    {
        OpCode Code { get; }

        ILBuilderProxy Proxy { get; }
    }
}