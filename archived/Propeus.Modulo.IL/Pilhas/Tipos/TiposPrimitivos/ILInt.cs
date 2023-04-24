using System.Reflection.Emit;

using Propeus.Modulo.IL.Proxy;

namespace Propeus.Modulo.IL.Pilhas.Tipos.TiposPrimitivos
{
    /// <summary>
    /// <see cref="int"/> || int32 || <see cref="OpCodes.Ldc_I4"/>
    /// </summary>
    internal class ILInt : ILPilha
    {
        /// <summary>
        /// <see cref="int"/> || int32 || <see cref="OpCodes.Ldc_I4"/>
        /// </summary>
        public ILInt(ILBuilderProxy proxy, int valor = 0) : base(proxy, OpCodes.Ldc_I4, valor)
        {
            Valor = valor;
        }

        public int Valor { get; }

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