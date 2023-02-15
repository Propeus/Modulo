using System.Reflection.Emit;

using Propeus.Modulo.IL.Pilhas.Saltos;
using Propeus.Modulo.IL.Proxy;

namespace Propeus.Modulo.IL.Pilhas.Logico
{
    /// <summary>
    /// != || <see cref="OpCodes.Beq_S"/>
    /// </summary>
    internal class ILNotEquals : ILLogico
    {
        /*Mapeie esta kcta
         *int => Beq_S
         *decimal =>
         *
         */

        public ILNotEquals(ILBuilderProxy iLBuilderProxy) : base(iLBuilderProxy, OpCodes.Beq_S)
        {
        }

        public ILNotEquals(ILBuilderProxy iLBuilderProxy, ILLabel label) : base(iLBuilderProxy, OpCodes.Beq_S, label)
        {
        }
    }
}
