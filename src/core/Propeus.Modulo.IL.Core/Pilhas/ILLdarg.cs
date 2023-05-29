using System.Reflection.Emit;

using Propeus.Modulo.IL.Core.Proxy;

namespace Propeus.Modulo.IL.Core.Pilhas
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

            base.Executar();
            if (Indice > 3)
            {
                Proxy.Emit(Code, Indice);
            }
            else
            {
                Proxy.Emit(Code);
            }

        }

        public override string ToString()
        {
            return Indice is >= 0 and <= 3 ? $"\t\t{_offset} {Code}" : $"\t\t{_offset} {Code} {Indice}";

        }
    }
}