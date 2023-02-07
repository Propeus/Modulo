using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

using Propeus.Modulo.IL.Proxy;

namespace Propeus.Modulo.IL.Pilhas.Aritimetico
{
    internal class ILAdd : ILPilha
    {
        public ILAdd(ILBuilderProxy iLBuilderProxy) : base(iLBuilderProxy, OpCodes.Add)
        {
        }

        public override string ToString()
        {
            return $"\t\t{_offset} {Code}";
        }
    }
}
