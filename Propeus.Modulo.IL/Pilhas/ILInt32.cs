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
    /// <see cref="int"/>
    /// </summary>
    internal class ILInt32 : ILPilha
    {
        public ILInt32(ILBuilderProxy proxy, int valor = 0) : base(proxy,OpCodes.Ldc_I4)
        {
            Valor = valor;
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