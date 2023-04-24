using System.Reflection.Emit;

using Propeus.Modulo.IL.Proxy;

namespace Propeus.Modulo.IL.Pilhas.Variaveis
{
    internal class ILStLoc : ILPilha
    {
        public ILStLoc(ILBuilderProxy proxy, int valor = 0) : base(proxy, OpCodes.Stloc)
        {
            Valor = valor;
        }

        public int Valor { get; }

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