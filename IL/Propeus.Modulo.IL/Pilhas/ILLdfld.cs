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
    /// Carrega o campo no indice desejado
    /// </summary>
    internal struct ILLdfld : IILPilha
    {
        public ILLdfld(ILBuilderProxy proxy, FieldBuilder fieldBuilder)
        {
            Proxy = proxy ?? throw new ArgumentNullException(nameof(proxy));

            Code = OpCodes.Ldfld;
            Valor = fieldBuilder;
        }

        public OpCode Code { get; }
        public FieldBuilder Valor { get; }
        public ILBuilderProxy Proxy { get; }

        public void Executar()
        {
            Proxy.Emit(Code, Valor);
        }
    }
}