using System.Reflection.Emit;

using Propeus.Modulo.IL.Proxy;

namespace Propeus.Modulo.IL.Pilhas.Tipos.TiposPrimitivos
{
    /// <summary>
    /// <see cref="short"/> || int16 || <see cref="OpCodes.Ldc_I4"/>
    /// </summary>
    internal class ILShort : ILPilha
    {
        /// <summary>
        /// <see cref="short"/> || int16 || <see cref="OpCodes.Ldc_I4"/>
        /// </summary>
        public ILShort(ILBuilderProxy proxy, short valor = 0) : base(proxy, OpCodes.Ldc_I4, valor)
        {
            Valor = valor;
        }

        public short Valor { get; }

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