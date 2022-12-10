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
    internal struct ILLdstr : IILPilha, IDisposable
    {
        public ILLdstr(ILBuilderProxy proxy, string valor = "")
        {
            Proxy = proxy ?? throw new ArgumentNullException(nameof(proxy));

            Code = OpCodes.Ldstr;
            Valor = valor;
        }

        public OpCode Code { get; }
        public string Valor { get; private set; }
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