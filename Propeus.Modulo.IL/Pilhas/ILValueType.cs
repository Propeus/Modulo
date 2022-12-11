using Propeus.Modulo.IL.Interfaces;
using Propeus.Modulo.IL.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

namespace Propeus.Modulo.IL.Pilhas
{
    /// <summary>
    /// <see cref="decimal"/>
    /// </summary>
    internal class ILValueType : ILPilha
    {
        public ILValueType(ILBuilderProxy proxy, decimal valor = 0) : base(proxy, OpCodes.Newobj)
        {
            Valor = valor;

            int[] slot = Decimal.GetBits(valor);
            Byte[] flag = BitConverter.GetBytes(slot[3]);

            bool flgNumeroNegativo = flag[3] > 0;

            NumeroNegativo = new ILInt32(Proxy, Convert.ToInt32(flgNumeroNegativo));
            QuantidadePontoFlutuante = new ILInt8(Proxy, (sbyte)flag[2]);

            Slot1 = new ILInt32(proxy, slot[0]);
            Slot2 = new ILInt32(proxy, slot[1]);
            Slot3 = new ILInt32(proxy, slot[2]);

            Constutor = new ILNewObj(proxy, typeof(decimal).GetConstructors().First(c => c.GetParameters().Length == 5));
        }

        public decimal Valor { get; }

        public ILInt32 Slot1 { get; }
        public ILInt32 Slot2 { get;  }
        public ILInt32 Slot3 { get; }
        public ILInt32 NumeroNegativo { get; }
        public ILInt8 QuantidadePontoFlutuante { get; }
        public ILNewObj Constutor { get; }

        public override void Executar()
        {
            if (_executado)
                return;

            Slot1.Executar();
            Slot2.Executar();
            Slot3.Executar();
            NumeroNegativo.Executar();
            QuantidadePontoFlutuante.Executar();
            Constutor.Executar();

            base.Executar();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
      
            Slot1.Dispose();
            Slot2.Dispose();
            Slot3.Dispose();
            NumeroNegativo.Dispose();
            QuantidadePontoFlutuante.Dispose();
            Constutor.Dispose();

        }

       
    }
}