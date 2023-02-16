using System;
using System.Reflection.Emit;

using Propeus.Modulo.IL.Proxy;

namespace Propeus.Modulo.IL.Interfaces
{
    internal interface IILPilha : IILExecutor, IDisposable
    {
        OpCode Code { get; }

        ILBuilderProxy Proxy { get; }
    }
}