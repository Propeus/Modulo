using System.Reflection.Emit;

using Propeus.Modulo.IL.Proxy;

namespace Propeus.Modulo.IL.Pilhas.Logico
{
    /// <summary>
    /// &gt;= || <see cref="OpCodes.Blt_Un_S"/> || <see cref="OpCodes.Clt_Un"/>
    /// </summary>
    internal class ILGreaterThanOrEquals : ILLogico
    {
      

        public ILGreaterThanOrEquals(ILBuilderProxy iLBuilderProxy) : base(iLBuilderProxy, OpCodes.Clt_Un)
        {
        }

        public ILGreaterThanOrEquals(ILBuilderProxy iLBuilderProxy, Label label) : base(iLBuilderProxy, OpCodes.Bgt_S, label)
        {
        }
    }
}
