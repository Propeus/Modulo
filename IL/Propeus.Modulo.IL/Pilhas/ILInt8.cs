using Propeus.Modulo.IL.Interfaces;
using Propeus.Modulo.IL.Proxy;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace Propeus.Modulo.IL.Pilhas
{
    /// <summary>
    /// <see cref="sbyte"/>
    /// </summary>
    internal struct ILInt8 : IILPilha
    {
        public ILInt8(ILBuilderProxy proxy, sbyte valor = 0)
        {
            Proxy = proxy ?? throw new ArgumentNullException(nameof(proxy));
            Code = OpCodes.Ldc_I4_S;
            Valor = valor;
        }

        public OpCode Code { get; }
        public sbyte Valor { get; }
        public ILBuilderProxy Proxy { get; }

        public void Executar()
        {
            Proxy.Emit(Code, Valor);
        }
    }
}