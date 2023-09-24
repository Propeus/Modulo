using System.Reflection.Emit;

using Propeus.Modulo.IL.Proxy;

namespace Propeus.Modulo.IL.Pilhas.Tipos
{
    internal class ILString : ILPilha
    {
        public ILString(ILBuilderProxy iLBuilderProxy, string valor) : base(iLBuilderProxy, OpCodes.Ldstr, valor)
        {
            Valor = valor;
        }

        public string Valor { get; }

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
