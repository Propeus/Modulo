using Propeus.Modulo.IL.Interfaces;
using Propeus.Modulo.IL.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

namespace Propeus.Modulo.IL.Pilhas.Tipos.TiposPrimitivos
{
    /// <summary>
    /// <see cref="decimal"/> || <see cref="ValueType"/> || <see cref="OpCodes.Newobj"/>
    /// </summary>
    internal class ILDecimal : ILPilha
    {
        /// <summary>
        /// <see cref="decimal"/> || <see cref="ValueType"/> || <see cref="OpCodes.Newobj"/>
        /// </summary>
        public ILDecimal(ILBuilderProxy proxy, decimal valor = 0) : base(proxy, OpCodes.Newobj,valor)
        {
            Valor = valor;

            int[] slot = decimal.GetBits(valor);
            byte[] flag = BitConverter.GetBytes(slot[3]);

            bool flgNumeroNegativo = flag[3] > 0;

            NumeroNegativo = new ILBoolean(Proxy, flgNumeroNegativo);
            QuantidadePontoFlutuante = new ILSbyte(Proxy, (sbyte)flag[2]);

            Slot1 = new ILInt(proxy, slot[0]);
            Slot2 = new ILInt(proxy, slot[1]);
            Slot3 = new ILInt(proxy, slot[2]);

            Constutor = new ILNewObj(proxy, typeof(decimal).GetConstructors().First(c => c.GetParameters().Length == 5));
        }

        public decimal Valor { get; }

        public ILInt Slot1 { get; }
        public ILInt Slot2 { get; }
        public ILInt Slot3 { get; }
        public ILBoolean NumeroNegativo { get; }
        public ILSbyte QuantidadePontoFlutuante { get; }
        public ILNewObj Constutor { get; }

        public override void Executar()
        {
            if (_executado)
                return;

            base.Executar();

            Slot1.Executar();
            Slot2.Executar();
            Slot3.Executar();
            NumeroNegativo.Executar();
            QuantidadePontoFlutuante.Executar();
            Constutor.Executar();

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