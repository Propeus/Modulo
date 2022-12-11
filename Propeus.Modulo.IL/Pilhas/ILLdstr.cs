using Propeus.Modulo.IL.Interfaces;
using Propeus.Modulo.IL.Proxy;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection.Emit;
using System.Text;

namespace Propeus.Modulo.IL.Pilhas
{
    /// <summary>
    /// Carrega uma <see cref="string"/> para a pilha de execução
    /// </summary>
    internal class ILLdstr : ILPilha
    {
        public ILLdstr(ILBuilderProxy proxy, string valor = "") : base(proxy, OpCodes.Ldstr)
        {
            Valor = valor;
        }

        public string Valor { get; private set; }

        ///<inheritdoc/>
        public override void Executar()
        {
            if (_executado)
                return;

            Proxy.Emit(Code, Valor);

            base.Executar();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            Valor = null;
        }

        public override string ToString()
        {
            return $"\t\t{_offset} {Code} {Valor}";
        }
    }
}