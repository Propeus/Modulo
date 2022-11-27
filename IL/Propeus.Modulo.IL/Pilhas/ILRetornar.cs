using Propeus.Modulo.IL.Interfaces;
using Propeus.Modulo.IL.Proxy;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace Propeus.Modulo.IL.Pilhas
{
    /// <summary>
    /// Cria uma instrução IL de retorno
    /// </summary>
    internal struct ILRetornar : IILPilha
    {
        public ILRetornar(ILBuilderProxy proxy)
        {
            Proxy = proxy ?? throw new ArgumentNullException(nameof(proxy));
            Code = OpCodes.Ret;
        }

        public OpCode Code { get; }
        public ILBuilderProxy Proxy { get; }

        public void Executar()
        {
            Proxy.Emit(Code);
        }
    }
}