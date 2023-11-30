using System.Reflection;
using System.Reflection.Emit;

using Propeus.Module.IL.Core.Proxy;

namespace Propeus.Module.IL.Core.Pilhas
{
    /// <summary>
    /// Carrega o argumento no índice desejado
    /// </summary>
    /// <remarks>
    /// Pilha para 'obter' um valor de um parâmetro qualquer pela sua posição ao invés do nome
    /// </remarks>
    internal class ILLdarg : ILStack
    {
        /// <summary>
        /// Construtor para criar instrução IL  <see cref="OpCodes.Ldarg"/>
        /// </summary>
        /// <param name="scopeBuilder">Escopo onde será aplicado a instrução IL</param>
        /// <param name="index">TODO: preencher este campo</param>
        public ILLdarg(ILBuilderProxy scopeBuilder, int index = 0) : base(scopeBuilder, OpCodes.Ldarg, index)
        {
            ParamIndex = index;
        }

        /// <summary>
        /// Posição do parâmetro no objeto
        /// </summary>
        public int ParamIndex { get; }

        ///<inheritdoc/>
        public override void Apply()
        {

            base.Apply();
            if (ParamIndex > 3)
            {
                ScopeBuilder.Emit(Code, ParamIndex);
            }
            else
            {
                ScopeBuilder.Emit(Code);
            }

        }

        ///<inheritdoc/>
        public override string ToString()
        {
            return ParamIndex is >= 0 and <= 3 ? $"\t\t{_offset} {Code}" : $"\t\t{_offset} {Code} {ParamIndex}";

        }
    }
}