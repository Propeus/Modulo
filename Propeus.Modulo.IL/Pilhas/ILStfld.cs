using Propeus.Modulo.IL.Interfaces;
using Propeus.Modulo.IL.Proxy;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection.Emit;
using System.Text;

namespace Propeus.Modulo.IL.Pilhas
{
    /// <summary>
    /// Armazena o valor no campo indicado
    /// </summary>
    internal class ILStfld : ILPilha
    {
        public ILStfld(ILBuilderProxy proxy, FieldBuilder valor) : base(proxy, OpCodes.Stfld)
        {
            Valor = valor;
        }

        public FieldBuilder Valor { get; private set; }

        ///<inheritdoc/>
        public override void Executar()
        {
            if (_executado)
                return;

            Proxy.Emit(Code, Valor);

            base.Executar();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            Valor = null;
        }

        public override string ToString()
        {
            return $"\t\t{_offset} {Code} {(Valor as FieldBuilder).FieldType.Name.ToLower(CultureInfo.CurrentCulture)} {Valor.DeclaringType.FullName}::{Valor.Name}";
        }
    }
}