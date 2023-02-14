using System.Reflection.Emit;

using Propeus.Modulo.IL.Proxy;

namespace Propeus.Modulo.IL.Pilhas.Logico
{
    /// <summary>
    /// &lt;= || <see cref="OpCodes.Bgt_Un_S"/> || <see cref="OpCodes.Cgt_Un"/>
    /// </summary>
    internal class ILLessThanOrEquals : ILLogico
    {
      
        public ILLessThanOrEquals(ILBuilderProxy iLBuilderProxy) : base(iLBuilderProxy, OpCodes.Cgt_Un)
        {
        }

        public ILLessThanOrEquals(ILBuilderProxy iLBuilderProxy, Label label) : base(iLBuilderProxy, OpCodes.Bgt_Un_S, label)
        {
        }
    }
}
