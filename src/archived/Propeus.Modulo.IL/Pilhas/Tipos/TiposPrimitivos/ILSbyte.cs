using System.Reflection.Emit;

using Propeus.Modulo.IL.Proxy;

namespace Propeus.Modulo.IL.Pilhas.Tipos.TiposPrimitivos
{
    /// <summary>
    /// <see cref="sbyte"/> || int8 || <see cref="OpCodes.Ldc_I4_S"/>
    /// </summary>
    internal class ILSbyte : ILPilha
    {
        /// <summary>
        /// <see cref="sbyte"/> || int8 || <see cref="OpCodes.Ldc_I4_S"/>
        /// </summary>
        public ILSbyte(ILBuilderProxy proxy, sbyte valor = 0) : base(proxy, OpCodes.Ldc_I4_S, valor)
        {
            Valor = valor;
        }

        public sbyte Valor { get; }

        ///<inheritdoc/>
        public override void Executar()
        {
            if (_executado)
            {
                return;
            }

            base.Executar();
            Proxy.Emit(Code, Valor);

        }

        public override string ToString()
        {
            return $"\t\t{_offset} {Code} {Valor}";
        }
    }
}