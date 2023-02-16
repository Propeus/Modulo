using System.Reflection.Emit;

using Propeus.Modulo.IL.Proxy;

namespace Propeus.Modulo.IL.Pilhas.Aritimetico
{
    internal class ILMul : ILPilha
    {
        public ILMul(ILBuilderProxy iLBuilderProxy) : base(iLBuilderProxy, OpCodes.Mul)
        {
        }

        public override void Executar()
        {
            if (_executado)
            {
                return;
            }

            base.Executar();
            Proxy.Emit(Code);

        }

        public override string ToString()
        {
            return $"\t\t{_offset} {Code}";
        }

    }


}
