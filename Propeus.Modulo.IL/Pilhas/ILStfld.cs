using Propeus.Modulo.IL.Interfaces;
using Propeus.Modulo.IL.Proxy;

using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace Propeus.Modulo.IL.Pilhas
{
    /// <summary>
    /// Armazena o valor no campo indicado
    /// </summary>
    internal struct ILStfld : IILPilha, IDisposable
    {
        public ILStfld(ILBuilderProxy proxy, FieldBuilder valor)
        {
            Proxy = proxy ?? throw new ArgumentNullException(nameof(proxy));

            Code = OpCodes.Stfld;
            Valor = valor;
        }

        public OpCode Code { get; }
        public FieldBuilder Valor { get; private set; }
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
            Valor = null;
        }
    }
}