using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using Propeus.Modulo.IL.Core.Helpers;
using Propeus.Modulo.IL.Core.Proxy;

namespace Propeus.Modulo.IL.Core.Pilhas
{
    internal class ILCallVirt : ILPilha
    {
        public ILCallVirt(ILBuilderProxy proxy, MethodInfo metodo) : base(proxy, OpCodes.Callvirt)
        {
            Valor = metodo ?? throw new ArgumentNullException(nameof(metodo));
        }

        public MemberInfo Valor { get; private set; }

        ///<inheritdoc/>
        public override void Executar()
        {
            base.Executar();
            Proxy.Emit(Code, Valor as MethodInfo);

        }


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            Valor = null;
        }

        public override string ToString()
        {

            return Valor is MethodInfo
                ? $"\t\t{_offset} {Code} {(Valor as MethodInfo).ReturnType.Name.ToLower(CultureInfo.CurrentCulture)} {Valor.DeclaringType.FullName}::{Valor.Name}"
                : $"\t\t{_offset} {Code} {Valor.DeclaringType.FullName}::{Valor.Name}({string.Join(",", (Valor as ConstructorInfo).ObterTipoParametros().Select(x => x.Name))})";

        }
    }
}