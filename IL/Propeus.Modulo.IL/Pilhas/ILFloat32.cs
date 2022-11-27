using Propeus.Modulo.IL.Interfaces;
using Propeus.Modulo.IL.Proxy;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace Propeus.Modulo.IL.Pilhas
{
    /// <summary>
    /// <see cref="float"/>
    /// </summary>
    internal struct ILFloat32 : IILPilha
    {
        public ILFloat32(ILBuilderProxy proxy, float valor = 0)
        {
            Proxy = proxy ?? throw new ArgumentNullException(nameof(proxy));

            Code = OpCodes.Ldc_R4;
            Valor = valor;
        }

        public OpCode Code { get; }
        public float Valor { get; }
        public ILBuilderProxy Proxy { get; }

        public void Executar()
        {
            Proxy.Emit(Code, Valor);
        }
    }
}