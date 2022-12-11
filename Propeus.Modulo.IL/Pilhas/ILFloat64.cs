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
    /// <see cref="double"/>
    /// </summary>
    internal class ILFloat64 : ILPilha
    {
        public ILFloat64(ILBuilderProxy proxy, double valor = 0) : base(proxy, OpCodes.Ldc_R8)
        {
            Valor = valor;
        }

        public double Valor { get; }

        ///<inheritdoc/>
        public override  void Executar()
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