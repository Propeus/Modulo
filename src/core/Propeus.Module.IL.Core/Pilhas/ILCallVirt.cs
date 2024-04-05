using Propeus.Module.IL.Core.Helpers;
using Propeus.Module.IL.Core.Proxy;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;

namespace Propeus.Module.IL.Core.Pilhas
{
    /// <summary>
    /// Chama uma função virtual
    /// </summary>
    /// <remarks>
    /// Pilha para executar uma chamada de um método virtual
    /// </remarks>
    internal class ILCallVirt : ILStack
    {
        ///<inheritdoc/>
        public ILCallVirt(ILBuilderProxy scopeBuilder, MethodInfo methodInfo) : base(scopeBuilder, OpCodes.Callvirt)
        {
            MemberInfo = methodInfo ?? throw new ArgumentNullException(nameof(methodInfo));
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
                case MethodInfo:
                    ScopeBuilder.Emit(Code, (MethodInfo)MemberInfo);
                    break;
                default:
                    throw new InvalidOperationException($"O tipo do {nameof(MemberInfo)} não pode ser '{typeof(ConstructorInfo).Name}'");
            }
        }


        ///<inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            MemberInfo = null;
        }

        ///<inheritdoc/>
        public override string ToString()
        {

            return MemberInfo is null
                ? string.Empty
                : MemberInfo switch
                {
                    MethodInfo => $"\t\t{_offset} {Code} {((MethodInfo)MemberInfo).ReturnType.Name.ToLower(CultureInfo.CurrentCulture)} {MemberInfo.DeclaringType?.FullName}::{MemberInfo.Name}",
                    ConstructorInfo => $"\t\t{_offset} {Code} {MemberInfo.DeclaringType?.FullName}::{MemberInfo.Name}({string.Join(",", ((ConstructorInfo)MemberInfo).GetTypeParams().Select(x => x.Name))})",
                    _ => string.Empty,
                };
        }
    }
}