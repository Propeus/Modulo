using Propeus.Modulo.IL.Interfaces;
using Propeus.Modulo.IL.Proxy;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace Propeus.Modulo.IL.Pilhas
{
    /// <summary>
    /// operador 'new'
    /// </summary>
    internal struct ILNewObj : IILPilha
    {
        public ILNewObj(ILBuilderProxy proxy, ConstructorInfo ctor)
        {
            Proxy = proxy ?? throw new ArgumentNullException(nameof(proxy));
            Ctor = ctor ?? throw new ArgumentNullException(nameof(ctor));
            Code = OpCodes.Newobj;
        }

        public OpCode Code { get; }
        public ILBuilderProxy Proxy { get; }
        public ConstructorInfo Ctor { get; }

        public void Executar()
        {
            Proxy.Emit(Code, Ctor);
        }
    }
}