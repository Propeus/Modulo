using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

using Propeus.Modulo.IL.Proxy;

namespace Propeus.Modulo.IL.Pilhas.Aritimetico
{
    internal class ILDiv : ILPilha
    {
        public ILDiv(ILBuilderProxy iLBuilderProxy) : base(iLBuilderProxy, OpCodes.Div)
        {
        }

        public override void Executar()
        {
            if (_executado)
                return;

            base.Executar();
            Proxy.Emit(Code);

        }

        public override string ToString()
        {
            return $"\t\t{_offset} {Code}";
        }
    }
}
