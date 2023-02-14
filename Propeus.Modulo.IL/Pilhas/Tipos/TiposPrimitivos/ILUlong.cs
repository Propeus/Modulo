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
    /// <see cref="ulong"/> || uint64 || <see cref="OpCodes.Ldc_I4_M1"/>
    /// </summary>
    internal class ILUlong : ILPilha
    {

        /// <summary>
        /// <see cref="ulong"/> || uint64 || <see cref="OpCodes.Ldc_I4_M1"/>
        /// </summary>
        public ILUlong(ILBuilderProxy iLBuilderProxy, ulong valor) : base(iLBuilderProxy, OpCodes.Ldc_I4_M1, valor)
        {
            Valor = valor;
        }

        public ulong Valor { get; }

        /// <inheritdoc/>
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
