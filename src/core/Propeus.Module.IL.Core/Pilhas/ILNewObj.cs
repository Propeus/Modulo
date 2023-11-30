using System.Reflection;
using System.Reflection.Emit;

using Propeus.Module.IL.Core.Helpers;
using Propeus.Module.IL.Core.Proxy;

namespace Propeus.Module.IL.Core.Pilhas
{
    /// <summary>
    /// operador 'new'
    /// </summary>
    /// <remarks>
    /// Pilha para iniciar uma nova instancia de um objeto
    /// </remarks>
    internal class ILNewObj : ILStack
    {
        public ILNewObj(ILBuilderProxy scopeBuilder, ConstructorInfo constructorInfo) : base(scopeBuilder, OpCodes.Newobj)
        {
            MemberInfo = constructorInfo ?? throw new ArgumentNullException(nameof(constructorInfo));
        }

        /// <summary>
        /// Método a ser chamado na instrução IL
        /// </summary>
        public MemberInfo? MemberInfo { get; private set; }

        ///<inheritdoc/>
        public override void Apply()
        {
            base.Apply();
            if (MemberInfo is null)
            {
                throw new ObjectDisposedException(nameof(MemberInfo));
            }
            switch (MemberInfo)
            {
                case ConstructorInfo:
                    ScopeBuilder.Emit(Code, (ConstructorInfo)MemberInfo);
                    break;
                default:
                    throw new InvalidOperationException($"O tipo do {nameof(MemberInfo)} não pode ser '{typeof(ConstructorInfo).Name}'");
            }

        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            MemberInfo = null;
        }

        public override string ToString()
        {
            if (MemberInfo is not null && MemberInfo.DeclaringType is not null)
            {
                return MemberInfo switch
                {
                    ConstructorInfo => $"\t\t{_offset} {Code} {MemberInfo.DeclaringType.FullName}::{MemberInfo.Name}({string.Join(",", ((ConstructorInfo)MemberInfo).GetTypeParams().Select(x => x.Name))})",
                    _ => string.Empty,
                };
            }

            return string.Empty;

        }
    }
}