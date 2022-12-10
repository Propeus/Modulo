using Propeus.Modulo.IL.Interfaces;
using Propeus.Modulo.IL.Proxy;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace Propeus.Modulo.IL.Pilhas
{
    /// <summary>
    /// operador 'new'
    /// </summary>
    internal struct ILNewObj : IILPilha, IDisposable
    {
        public ILNewObj(ILBuilderProxy proxy, ConstructorInfo ctor)
        {
            Proxy = proxy ?? throw new ArgumentNullException(nameof(proxy));
            Valor = ctor ?? throw new ArgumentNullException(nameof(ctor));
            Code = OpCodes.Newobj;
        }

        public OpCode Code { get; }
        public ConstructorInfo Valor { get; private set; }
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