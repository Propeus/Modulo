using Propeus.Modulo.IL.Interfaces;
using Propeus.Modulo.IL.Proxy;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection.Emit;
using System.Text;

namespace Propeus.Modulo.IL.Pilhas
{
    internal class ILNewArr : ILPilha
    {
        public ILNewArr(ILBuilderProxy proxy, Type type) : base(proxy,OpCodes.Newarr)
        {
            Valor = type;
        }

        public Type Valor { get; private set; }

        ///<inheritdoc/>
        public override void Executar()
        {
            if (_executado)
                return;

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