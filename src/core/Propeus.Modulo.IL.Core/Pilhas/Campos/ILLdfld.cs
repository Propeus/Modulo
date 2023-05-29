using System.Globalization;
using System.Reflection.Emit;

using Propeus.Modulo.IL.Core.Pilhas;
using Propeus.Modulo.IL.Core.Proxy;

namespace Propeus.Modulo.IL.Core.Pilhas.Campos
{
    /// <summary>
    /// Carrega o campo no indice desejado
    /// </summary>
    internal class ILLdfld : ILPilha
    {
        public ILLdfld(ILBuilderProxy proxy, FieldBuilder fieldBuilder) : base(proxy, OpCodes.Ldfld)
        {
            Valor = fieldBuilder;
        }

        public FieldBuilder Valor { get; private set; }

        public override void Executar()
        {
           
            base.Executar();
            Proxy.Emit(Code, Valor);

        }

        ///<inheritdoc/>
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