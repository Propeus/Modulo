using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

using Propeus.Modulo.IL.Interfaces;
using Propeus.Modulo.IL.Proxy;

namespace Propeus.Modulo.IL.Pilhas.Tipos.TiposPrimitivos
{
    /// <summary>
    /// <see cref="uint"/> || uint32 || <see cref="OpCodes.Ldc_I4_M1"/>
    /// </summary>
    internal class ILUint : ILPilha
    {

        /// <summary>
        /// <see cref="uint"/> || uint32 || <see cref="OpCodes.Ldc_I4_M1"/>
        /// </summary>
        public ILUint(ILBuilderProxy iLBuilderProxy, uint valor) : base(iLBuilderProxy, OpCodes.Ldc_I4_M1, valor)
        {
            Valor = valor;
        }

        public uint Valor { get; }

        ///<inheritdoc/>
        public override void Executar()
        {
            if (_executado)
                return;

            base.Executar();
            Proxy.Emit(Code, Valor);

        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"\t\t{_offset} {Code} {Valor}";
        }
    }
}
