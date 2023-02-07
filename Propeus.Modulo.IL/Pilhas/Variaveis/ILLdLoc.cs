using Propeus.Modulo.IL.Interfaces;
using Propeus.Modulo.IL.Proxy;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection.Emit;
using System.Text;

namespace Propeus.Modulo.IL.Pilhas.Variaveis
{
    internal class ILLdLoc : ILPilha
    {
        public ILLdLoc(ILBuilderProxy proxy, int indice = 0) : base(proxy, OpCodes.Ldloc)
        {
            Valor = indice;
        }

        public int Valor { get; }

        ///<inheritdoc/>
        public override void Executar()
        {

            if (_executado)
                return;

            Proxy.Emit(Code, Valor);

            base.Executar();
        }


        public override string ToString()
        {
            return $"\t\t{_offset} {Code} {Valor}";
        }
    }
}