using System.Reflection.Emit;

using Propeus.Modulo.IL.Pilhas.Saltos;
using Propeus.Modulo.IL.Proxy;

namespace Propeus.Modulo.IL.Pilhas.Logico
{
    internal class ILLogico : ILPilha
    {
        public ILLogico(ILBuilderProxy iLBuilderProxy, OpCode opCode) : base(iLBuilderProxy, opCode)
        {
        }

        public ILLogico(ILBuilderProxy iLBuilderProxy, OpCode opCode, ILLabel label) : base(iLBuilderProxy, opCode)
        {
            Label = label;
        }

        public ILLabel Label { get; internal set; }

        public override void Executar()
        {
            base.Executar();
            if (Label != null)
            {
                Proxy.Emit(Code, Label.Label.Value);
            }
            else
            {
                //Pode ser que nao exista essa opcao
                Proxy.Emit(Code);
            }

        }

        public override string ToString()
        {
            return Label != null ? $"\t\t{_offset} {Code} {Label.Nome}" : $"\t\t{_offset} {Code}";
        }
    }
}
