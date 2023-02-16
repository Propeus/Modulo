using System.Reflection.Emit;

using Propeus.Modulo.IL.Pilhas.Saltos;
using Propeus.Modulo.IL.Proxy;

namespace Propeus.Modulo.IL.Pilhas.Logico
{
    //Suspeito que a ordem esteja invertida

    /// <summary>
    /// &lt; || <see cref="OpCodes.Bge_Un_S"/> || <see cref="OpCodes.Clt"/>
    /// </summary>
    internal class ILLessThan : ILLogico
    {


        public ILLessThan(ILBuilderProxy iLBuilderProxy) : base(iLBuilderProxy, OpCodes.Clt)
        {
        }

        public ILLessThan(ILBuilderProxy iLBuilderProxy, ILLabel label) : base(iLBuilderProxy, OpCodes.Bge_Un_S, label)
        {
        }
    }
}
