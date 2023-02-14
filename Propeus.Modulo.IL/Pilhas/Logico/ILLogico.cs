using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

using Propeus.Modulo.IL.Proxy;

namespace Propeus.Modulo.IL.Pilhas.Logico
{
    internal class ILLogico : ILPilha
    {
        public ILLogico(ILBuilderProxy iLBuilderProxy, OpCode opCode) : base(iLBuilderProxy, opCode)
        {
        }

        public ILLogico(ILBuilderProxy iLBuilderProxy, OpCode opCode, Label label) : base(iLBuilderProxy, opCode)
        {
            Label = label;
        }

        public Label? Label { get; internal set; }

        public override void Executar()
        {
            base.Executar();
            if (Label.HasValue)
            {
                Proxy.Emit(Code, Label.Value);
            }
            else
            {
                //Pode ser que nao exista essa opcao
                Proxy.Emit(Code);
            }

        }

        public override string ToString()
        {
            if (Label.HasValue)
            {
                return $"\t\t{_offset} {Code} {Label.Value}";
            }
            else
            {
                return $"\t\t{_offset} {Code}";
            }
        }
    }
}
