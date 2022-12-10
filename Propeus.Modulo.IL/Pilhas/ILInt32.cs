using Propeus.Modulo.IL.Interfaces;
using Propeus.Modulo.IL.Proxy;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace Propeus.Modulo.IL.Pilhas
{
    /// <summary>
    /// <see cref="int"/>
    /// </summary>
    internal struct ILInt32 : IILPilha,IDisposable
    {
        public ILInt32(ILBuilderProxy proxy, int valor = 0)
        {
            Proxy = proxy ?? throw new ArgumentNullException(nameof(proxy));

            Code = OpCodes.Ldc_I4;
            Valor = valor;
        }

        public OpCode Code { get; }
        public int Valor { get; }
        public ILBuilderProxy Proxy { get; private set; }
        public bool Executado { get; private set; }

        public void Executar()
        {
            if (Executado)
                return;

            Proxy.Emit(Code, Valor);

            Executado = true;
        }

        public void Dispose()
        {
            Proxy.Dispose();
            Proxy = null;
        }
    }
}