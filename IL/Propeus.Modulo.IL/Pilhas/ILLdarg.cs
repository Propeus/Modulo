using Propeus.Modulo.IL.Interfaces;
using Propeus.Modulo.IL.Proxy;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace Propeus.Modulo.IL.Pilhas
{
    /// <summary>
    /// Carrega o agumento no indice desejado
    /// </summary>
    internal struct ILLdarg : IILPilha
    {
        public ILLdarg(ILBuilderProxy proxy, int indice = 0)
        {
            Proxy = proxy ?? throw new ArgumentNullException(nameof(proxy));

            Code = OpCodes.Ldarg;
            Valor = indice;
        }

        public OpCode Code { get; }
        public int Valor { get; }
        public ILBuilderProxy Proxy { get; }

        public void Executar()
        {
            Proxy.Emit(Code, Valor);
        }
    }
}