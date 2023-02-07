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
    /// <see cref="long"/> || int64 || <see cref="OpCodes.Ldc_I8"/>
    /// </summary>
    internal class ILLong : ILPilha
    {
        /// <summary>
        /// <see cref="long"/> || int64 || <see cref="OpCodes.Ldc_I8"/>
        /// </summary>
        public ILLong(ILBuilderProxy proxy, long valor = 0) : base(proxy, OpCodes.Ldc_I8, valor)
        {
            Valor = valor;
        }

        public long Valor { get; }

        ///<inheritdoc/>
        public override void Executar()
        {
            if (_executado)
                return;

            Proxy.Emit(Code, Valor);

            if (Valor <= int.MaxValue)
            {
                Proxy.Emit(OpCodes.Conv_I8);
            }

            base.Executar();
        }

        public override string ToString()
        {
            return $"\t\t{_offset} {Code} {Valor}";
        }
    }
}