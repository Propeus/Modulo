using System.Reflection.Emit;

using Propeus.Modulo.IL.Core.Proxy;

namespace Propeus.Modulo.IL.Core.Pilhas
{
    /// <summary>
    /// Cria uma instrução IL de retorno
    /// </summary>
    internal class ILRet : ILPilha
    {
        public ILRet(ILBuilderProxy proxy) : base(proxy, OpCodes.Ret)
        {
        }

        ///<inheritdoc/>
        public override void Executar()
        {
            base.Executar();
            Proxy.Emit(Code);

        }

        public override string ToString()
        {
            return $"\t\t{_offset} {Code}";
        }
    }
}