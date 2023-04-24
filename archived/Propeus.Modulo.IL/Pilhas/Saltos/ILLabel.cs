using System.Reflection.Emit;

using Propeus.Modulo.IL.Proxy;

namespace Propeus.Modulo.IL.Pilhas.Saltos
{
    internal class ILLabel : ILPilha
    {
        public ILLabel(ILBuilderProxy proxy) : base(proxy, OpCodes.Nop)
        {
            Label = proxy.DefineLabel();
        }

        public Label? Label { get; internal set; }
        public string Nome => _offset.ToString();

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
