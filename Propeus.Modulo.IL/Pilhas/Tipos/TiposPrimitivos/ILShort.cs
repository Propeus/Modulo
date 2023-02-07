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
    /// <see cref="short"/> || int16 || <see cref="OpCodes.Ldc_I4"/>
    /// </summary>
    internal class ILShort : ILPilha
    {
        /// <summary>
        /// <see cref="short"/> || int16 || <see cref="OpCodes.Ldc_I4"/>
        /// </summary>
        public ILShort(ILBuilderProxy proxy, short valor = 0) : base(proxy, OpCodes.Ldc_I4, valor)
        {
            Valor = valor;
        }

        public short Valor { get; }

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