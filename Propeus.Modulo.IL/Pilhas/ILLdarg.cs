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
    /// Carrega o agumento no indice desejado
    /// </summary>
    internal class ILLdarg : ILPilha
    {
        public ILLdarg(ILBuilderProxy proxy, int indice = 0) : base(proxy, OpCodes.Ldarg, indice)
        {
            Indice = indice;
        }

        public int Indice { get; }

        ///<inheritdoc/>
        public override void Executar()
        {

            if (_executado)
                return;

            base.Executar();
            Proxy.Emit(Code);

        }

        public override string ToString()
        {
            if (Indice >= 0 && 3 >= Indice)
            {
                return $"\t\t{_offset} {Code}";
            }
            else
            {
                return $"\t\t{_offset} {Code} {Indice}";
            }

        }
    }
}