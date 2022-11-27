using Propeus.Modulo.IL.Interfaces;
using Propeus.Modulo.IL.Proxy;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace Propeus.Modulo.IL.Pilhas
{
    /// <summary>
    /// Carrega uma <see cref="string"/> para a pilha de execução
    /// </summary>
    internal struct ILLdstr : IILPilha
    {
        public ILLdstr(ILBuilderProxy proxy, string valor = "")
        {
            Proxy = proxy ?? throw new ArgumentNullException(nameof(proxy));

            Code = OpCodes.Ldstr;
            Valor = valor;
        }

        public OpCode Code { get; }
        public string Valor { get; }
        public ILBuilderProxy Proxy { get; }

        public void Executar()
        {
            Proxy.Emit(Code, Valor);
        }
    }
}