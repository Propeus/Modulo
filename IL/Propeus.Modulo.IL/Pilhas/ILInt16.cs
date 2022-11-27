using Propeus.Modulo.IL.Interfaces;
using Propeus.Modulo.IL.Proxy;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace Propeus.Modulo.IL.Pilhas
{
    /// <summary>
    /// <see cref="short"/>
    /// </summary>
    internal struct ILInt16 : IILPilha
    {
        public ILInt16(ILBuilderProxy proxy, short valor = 0)
        {
            Proxy = proxy ?? throw new ArgumentNullException(nameof(proxy));

            Code = OpCodes.Ldc_I4;
            Valor = valor;
        }

        public OpCode Code { get; }
        public short Valor { get; }
        public ILBuilderProxy Proxy { get; }

        public void Executar()
        {
            Proxy.Emit(Code, Valor);
        }
    }
}