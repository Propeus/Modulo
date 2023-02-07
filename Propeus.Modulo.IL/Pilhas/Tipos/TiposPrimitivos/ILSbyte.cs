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
    /// <see cref="sbyte"/> || int8 || <see cref="OpCodes.Ldc_I4_S"/>
    /// </summary>
    internal class ILSbyte : ILPilha
    {
        /// <summary>
        /// <see cref="sbyte"/> || int8 || <see cref="OpCodes.Ldc_I4_S"/>
        /// </summary>
        public ILSbyte(ILBuilderProxy proxy, sbyte valor = 0) : base(proxy, OpCodes.Ldc_I4_S, valor)
        {
            Valor = valor;
        }

        public sbyte Valor { get; }

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