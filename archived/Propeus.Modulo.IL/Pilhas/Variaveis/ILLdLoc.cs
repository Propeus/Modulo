using System.Reflection.Emit;

using Propeus.Modulo.IL.Proxy;

namespace Propeus.Modulo.IL.Pilhas.Variaveis
{
    internal class ILLdLoc : ILPilha
    {
        public ILLdLoc(ILBuilderProxy proxy, int indice = 0) : base(proxy, OpCodes.Ldloc)
        {
            Valor = indice;
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