using Propeus.Modulo.Abstrato.Util;
using Propeus.Modulo.IL.Interfaces;
using Propeus.Modulo.IL.Proxy;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace Propeus.Modulo.IL.Pilhas
{
    internal class ILCallVirt : ILPilha
    {
        public ILCallVirt(ILBuilderProxy proxy, MethodInfo metodo) : base(proxy,OpCodes.Callvirt)
        {
            Valor = metodo ?? throw new ArgumentNullException(nameof(metodo));
        }

        public MemberInfo Valor { get; private set; }

        ///<inheritdoc/>
        public override void Executar()
        {
            if (_executado)
                return;

            if (Valor is MethodInfo)
                Proxy.Emit(Code, Valor as MethodInfo);
            else if (Valor is ConstructorInfo)
                Proxy.Emit(Code, Valor as ConstructorInfo);
            else
                throw new InvalidCastException("Não foi possivel determinar o tipo de membro o valor pertence");

            base.Executar();
        }


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            Valor = null;
        }

        public override string ToString()
        {

            if (Valor is MethodInfo)
                return $"\t\t{_offset} {Code} {(Valor as MethodInfo).ReturnType.Name.ToLower(CultureInfo.CurrentCulture)} {Valor.DeclaringType.FullName}::{Valor.Name}";
            else
                return $"\t\t{_offset} {Code} {Valor.DeclaringType.FullName}::{Valor.Name}({string.Join(",", (Valor as ConstructorInfo).ObterTipoParametros().Select(x => x.Name))})";

        }
    }
}