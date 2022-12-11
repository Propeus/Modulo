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
    /// <see cref="sbyte"/>
    /// </summary>
    internal class ILInt8 : ILPilha
    {
        public ILInt8(ILBuilderProxy proxy, sbyte valor = 0) : base(proxy, OpCodes.Ldc_I4_S)
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