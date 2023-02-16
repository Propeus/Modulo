using System.Reflection.Emit;

using Propeus.Modulo.IL.Proxy;

namespace Propeus.Modulo.IL.Pilhas.Tipos.TiposPrimitivos
{
    /// <summary>
    /// <see cref="double"/> || float64 || <see cref="OpCodes.Ldc_R8"/>
    /// </summary>
    internal class ILDouble : ILPilha
    {
        /// <summary>
        /// <see cref="double"/> || float64 || <see cref="OpCodes.Ldc_R8"/>
        /// </summary>
        public ILDouble(ILBuilderProxy proxy, double valor = 0) : base(proxy, OpCodes.Ldc_R8, valor)
        {
            Valor = valor;
        }

        public double Valor { get; }

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