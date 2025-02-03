using System.Reflection.Emit;

using Propeus.Module.IL.Core.Proxy;

namespace Propeus.Module.IL.Core.Interfaces
{
    /// <summary>
    /// Interface para executar uma instrução IL em um determinado objeto
    /// </summary>
    internal interface IILPilha : IILExecutor, IDisposable
    {
        /// <summary>
        /// Instrução IL que será executado no objeto
        /// </summary>
        OpCode Code { get; }

        /// <summary>
        /// Builder que será aplicado a instrução IL
        /// </summary>
        ILBuilderProxy? ScopeBuilder { get; }
    }
}