using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

using Propeus.Modulo.IL.Proxy;

namespace Propeus.Modulo.IL.Pilhas.Tipos
{
    internal class ILString : ILPilha
    {
        public ILString(ILBuilderProxy iLBuilderProxy, string valor) : base(iLBuilderProxy, OpCodes.Ldstr,valor)
        {
            Valor = valor;
        }

        public string Valor { get; }

        ///<inheritdoc/>
        public override void Executar()
        {
            if (_executado)
                return;

            Proxy.Emit(Code, Valor);

            base.Executar();
        }

        public override string ToString()
        {
            return $"\t\t{_offset} {Code} {Valor}";
        }
    }
}
