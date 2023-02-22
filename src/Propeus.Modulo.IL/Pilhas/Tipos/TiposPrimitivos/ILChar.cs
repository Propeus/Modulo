using System.Reflection.Emit;

using Propeus.Modulo.IL.Proxy;

namespace Propeus.Modulo.IL.Pilhas.Tipos.TiposPrimitivos
{
    /// <summary>
    /// <see cref="char"/> || <see cref="char"/> || <see cref="OpCodes.Ldc_I4"/>
    /// </summary>
    internal class ILChar : ILInt
    {
        /// <summary>
        /// <see cref="char"/> || <see cref="char"/> || <see cref="OpCodes.Ldc_I4"/>
        /// </summary>
        public ILChar(ILBuilderProxy proxy, char valor = char.MinValue) : base(proxy, valor)
        {
        }
    }
}
