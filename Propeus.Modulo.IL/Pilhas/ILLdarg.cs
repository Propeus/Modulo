using System.Reflection.Emit;

using Propeus.Modulo.IL.Proxy;

namespace Propeus.Modulo.IL.Pilhas
{
    /// <summary>
    /// Carrega o agumento no indice desejado
    /// </summary>
    internal class ILLdarg : ILPilha
    {
        public ILLdarg(ILBuilderProxy proxy, int indice = 0) : base(proxy, OpCodes.Ldarg, indice)
        {
            Indice = indice;
        }

        public int Indice { get; }

        ///<inheritdoc/>
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
            return Indice is >= 0 and <= 3 ? $"\t\t{_offset} {Code}" : $"\t\t{_offset} {Code} {Indice}";

        }
    }
}