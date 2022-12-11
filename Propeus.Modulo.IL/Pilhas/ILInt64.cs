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
    /// <see cref="long"/>
    /// </summary>
    internal class ILInt64 : ILPilha
    {
        public ILInt64(ILBuilderProxy proxy, long valor = 0) : base(proxy,OpCodes.Ldc_I8)
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

            base.Executar();
        }

        public override string ToString()
        {
            return $"\t\t{_offset} {Code} {Valor}";
        }
    }
}