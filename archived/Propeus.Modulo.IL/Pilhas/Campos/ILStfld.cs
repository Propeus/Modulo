using System.Globalization;
using System.Reflection.Emit;

using Propeus.Modulo.IL.Proxy;

namespace Propeus.Modulo.IL.Pilhas.Campos
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
            return $"\t\t{_offset} {Code} {Valor.FieldType.Name.ToLower(CultureInfo.CurrentCulture)} {Valor.DeclaringType.FullName}::{Valor.Name}";
        }
    }
}