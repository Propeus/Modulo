using System;
using System.Globalization;
using System.Reflection.Emit;

using Propeus.Modulo.IL.Proxy;

namespace Propeus.Modulo.IL.Pilhas
{
    internal class ILBox : ILPilha
    {
        public ILBox(ILBuilderProxy proxy, Type valor) : base(proxy, OpCodes.Box)
        {
            Valor = valor;
        }

        public Type Valor { get; private set; }

        ///<inheritdoc/>
        public override void Executar()
        {
            if (_executado)
            {
                return;
            }

            base.Executar();
            Proxy.Emit(Code, Valor);

        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            Valor = null;
        }

        public override string ToString()
        {
            return $"\t\t{_offset} {Code} [{Valor.Namespace}]{Valor.Name.ToLower(CultureInfo.CurrentCulture)}";

        }
    }
}