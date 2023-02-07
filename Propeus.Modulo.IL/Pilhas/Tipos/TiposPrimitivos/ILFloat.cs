using Propeus.Modulo.IL.Interfaces;
using Propeus.Modulo.IL.Proxy;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace Propeus.Modulo.IL.Pilhas.Tipos.TiposPrimitivos
{
    /// <summary>
    /// <see cref="float"/> || float32 || <see cref="OpCodes.Ldc_R4"/>
    /// </summary>
    internal class ILFloat : ILPilha
    {
        /// <summary>
        /// <see cref="float"/> || float32 || <see cref="OpCodes.Ldc_R4"/>
        /// </summary>
        public ILFloat(ILBuilderProxy proxy, float valor = 0) : base(proxy, OpCodes.Ldc_R4, valor)
        {
            Valor = valor;
        }

        public float Valor { get; }

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