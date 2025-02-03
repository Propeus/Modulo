using System.Reflection.Emit;

using Propeus.Module.IL.Core.Proxy;

namespace Propeus.Module.IL.Core.Pilhas
{
    /// <summary>
    /// Cria uma instrução IL de retorno
    /// </summary>
    internal class ILRet : ILStack
    {
        public ILRet(ILBuilderProxy proxy) : base(proxy, OpCodes.Ret)
        {
        }

        ///<inheritdoc/>
        public override void Apply()
        {
            base.Apply();
            ScopeBuilder.Emit(Code);

        }

        public override string ToString()
        {
            return $"\t\t{_offset} {Code}";
        }
    }
}