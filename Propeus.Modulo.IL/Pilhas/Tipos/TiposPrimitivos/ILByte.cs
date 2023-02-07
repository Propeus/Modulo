using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

using Propeus.Modulo.IL.Proxy;

namespace Propeus.Modulo.IL.Pilhas.Tipos.TiposPrimitivos
{
    /// <summary>
    /// <see cref="byte"/> || uint8 || <see cref="OpCodes.Ldc_I4"/>
    /// </summary>
    internal class ILByte : ILInt
    {
        /// <summary>
        /// <see cref="byte"/> || uint8 || <see cref="OpCodes.Ldc_I4"/>
        /// </summary>
        public ILByte(ILBuilderProxy proxy, byte valor = byte.MinValue) : base(proxy, Convert.ToInt32(valor))
        {

        }


    }
}
