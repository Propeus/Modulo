using Propeus.Modulo.IL.Interfaces;
using Propeus.Modulo.IL.Proxy;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection.Emit;
using System.Text;

namespace Propeus.Modulo.IL.Pilhas
{
    internal class ILStLoc : ILPilha
    {
        public ILStLoc(ILBuilderProxy proxy, int valor = 0) : base(proxy, OpCodes.Stloc)
        {
            Valor = valor;
        }

        public int Valor { get; }

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