using Propeus.Modulo.IL.Interfaces;
using Propeus.Modulo.IL.Proxy;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection.Emit;
using System.Text;

namespace Propeus.Modulo.IL.Pilhas.Tipos.TiposPrimitivos
{
    /// <summary>
    /// <see cref="double"/> || float64 || <see cref="OpCodes.Ldc_R8"/>
    /// </summary>
    internal class ILDouble : ILPilha
    {
        /// <summary>
        /// <see cref="double"/> || float64 || <see cref="OpCodes.Ldc_R8"/>
        /// </summary>
        public ILDouble(ILBuilderProxy proxy, double valor = 0) : base(proxy, OpCodes.Ldc_R8,valor)
        {
            Valor = valor;
        }

        public double Valor { get; }

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