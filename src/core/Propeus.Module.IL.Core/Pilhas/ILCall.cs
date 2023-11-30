using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;

using Propeus.Module.IL.Core.Helpers;
using Propeus.Module.IL.Core.Proxy;

namespace Propeus.Module.IL.Core.Pilhas
{
    /// <summary>
    /// Chama uma função
    /// </summary>
    /// <remarks>
    /// Pilha para executar uma chamada de um método
    /// </remarks>
    internal class ILCall : ILStack
    {


        ///<inheritdoc/>
        public ILCall(ILBuilderProxy scopeBuilder, MethodInfo methodInfo) : base(scopeBuilder, OpCodes.Call)
        {

            MemberInfo = methodInfo ?? throw new ArgumentNullException(nameof(methodInfo));
        }

        ///<inheritdoc/>
        public ILCall(ILBuilderProxy scopeBuilder, ConstructorInfo constructorInfo) : base(scopeBuilder, OpCodes.Call)
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
                case MethodInfo:
                    ScopeBuilder.Emit(Code, (MethodInfo)MemberInfo);
                    break;
                case ConstructorInfo:
                    ScopeBuilder.Emit(Code, (ConstructorInfo)MemberInfo);
                    break;
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
            if (MemberInfo is null)
            {
                return string.Empty;
            }

            switch (MemberInfo)
            {
                case MethodInfo:
                    return $"\t\t{_offset} {Code} {((MethodInfo)MemberInfo).ReturnType.Name.ToLower(CultureInfo.CurrentCulture)} {MemberInfo.DeclaringType?.FullName}::{MemberInfo.Name}";
                case ConstructorInfo:
                    return $"\t\t{_offset} {Code} {MemberInfo.DeclaringType?.FullName}::{MemberInfo.Name}({string.Join(",", ((ConstructorInfo)MemberInfo).GetTypeParams().Select(x => x.Name))})";
                default:
                    return string.Empty;
            }

            
        }

    }
}