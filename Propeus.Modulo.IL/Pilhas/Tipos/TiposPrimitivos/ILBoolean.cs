using System;
using System.Reflection.Emit;

using Propeus.Modulo.IL.Proxy;

namespace Propeus.Modulo.IL.Pilhas.Tipos.TiposPrimitivos
{
    /// <summary>
    /// <see cref="bool"/> || Boolean || <see cref="OpCodes.Ldc_I4"/>
    /// </summary>
    internal class ILBoolean : ILInt
    {
        /// <summary>
        /// <see cref="bool"/> || Boolean || <see cref="OpCodes.Ldc_I4"/>
        /// </summary>
        public ILBoolean(ILBuilderProxy proxy, bool valor = false) : base(proxy, Convert.ToInt32(valor))
        {

        }
    }
}
