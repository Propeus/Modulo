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
    /// <see cref="ushort"/> || uint16 || <see cref="OpCodes.Ldc_I4"/>
    /// </summary>
    internal class ILUshort : ILPilha
    {

        /// <summary>
        /// <see cref="ushort"/> || uint16 || <see cref="OpCodes.Ldc_I4"/>
        /// </summary>
        public ILUshort(ILBuilderProxy iLBuilderProxy, ulong valor) : base(iLBuilderProxy, OpCodes.Ldc_I4, valor)
        {
            Valor = valor;
        }

        public ulong Valor { get; }

        ///<inheritdoc/>
        public override void Executar()
        {
            if (_executado)
                return;

            Proxy.Emit(Code, Valor);

            base.Executar();
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"\t\t{_offset} {Code} {Valor}";
        }
    }
}
