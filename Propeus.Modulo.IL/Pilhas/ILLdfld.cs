using Propeus.Modulo.IL.Interfaces;
using Propeus.Modulo.IL.Proxy;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace Propeus.Modulo.IL.Pilhas
{
    /// <summary>
    /// Carrega o campo no indice desejado
    /// </summary>
    internal class ILLdfld : ILPilha
    {
        public ILLdfld(ILBuilderProxy proxy, FieldBuilder fieldBuilder) :base(proxy, OpCodes.Ldfld)
        {
            Valor = fieldBuilder;
        }

        public FieldBuilder Valor { get; private set; }

        public override void Executar()
        {
            if (_executado)
                return;

            Proxy.Emit(Code, Valor);

            base.Executar();
        }

        ///<inheritdoc/>
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