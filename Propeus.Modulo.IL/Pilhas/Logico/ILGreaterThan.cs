using System.Reflection.Emit;

using Propeus.Modulo.IL.Proxy;

namespace Propeus.Modulo.IL.Pilhas.Logico
{
    /// <summary>
    /// &gt; || <see cref="OpCodes.Ble_Un_S"/> || <see cref="OpCodes.Cgt"/> 
    /// </summary>
    internal class ILGreaterThan : ILLogico
    {
        public ILGreaterThan(ILBuilderProxy iLBuilderProxy) : base(iLBuilderProxy, OpCodes.Cgt)
        {
        }

        public ILGreaterThan(ILBuilderProxy iLBuilderProxy, Label label) : base(iLBuilderProxy, OpCodes.Ble_Un_S, label)
        {
        }
    }
}
