using System.Reflection.Emit;

using Propeus.Modulo.IL.Pilhas.Saltos;
using Propeus.Modulo.IL.Proxy;

namespace Propeus.Modulo.IL.Pilhas.Logico
{
    /// <summary>
    /// == || <see cref="OpCodes.Bne_Un_S"/> || <see cref="OpCodes.Ceq"/>
    /// </summary>
    internal class ILEquals : ILLogico
    {
        public ILEquals(ILBuilderProxy iLBuilderProxy) : base(iLBuilderProxy, OpCodes.Ceq)
        {
        }

        public ILEquals(ILBuilderProxy iLBuilderProxy, ILLabel label) : base(iLBuilderProxy, OpCodes.Bne_Un_S, label)
        {
        }
    }
}
