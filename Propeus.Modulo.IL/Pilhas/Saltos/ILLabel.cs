using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

using Propeus.Modulo.IL.Geradores;
using Propeus.Modulo.IL.Proxy;

namespace Propeus.Modulo.IL.Pilhas.Saltos
{
    internal class ILLabel : ILPilha
    {
        public ILLabel(ILBuilderProxy proxy) : base(proxy, OpCodes.Nop)
        {
            Label = proxy.DefineLabel();
            Nome = Guid.NewGuid().ToString();
        }

        public Label? Label { get; private set; }
        public string Nome { get; }

        public override void Executar()
        {
            base.Executar();
            if (Label.HasValue)
            {
                Proxy.MarkLabel(Label.Value);
            }

        }

        public override string ToString()
        {
            return string.Empty;
        }

        protected override void Dispose(bool disposing)
        {
            Label = null;
            base.Dispose(disposing);

        }
    }
}
