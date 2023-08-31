using System;
using System.Reflection.Emit;

using Propeus.Module.IL.Core.Proxy;

namespace Propeus.Module.IL.Core.Interfaces
{
    internal interface IILPilha : IILExecutor, IDisposable
    {
        OpCode Code { get; }

        ILBuilderProxy Proxy { get; }
    }
}