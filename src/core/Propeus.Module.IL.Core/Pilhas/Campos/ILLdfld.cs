using System.Globalization;
using System.Reflection.Emit;

using Propeus.Module.IL.Core.Proxy;

namespace Propeus.Module.IL.Core.Pilhas.Campos
{
    /// <summary>
    /// Carrega o campo no índice desejado
    /// </summary>
    /// <remarks>
    /// Pilha para carregar um valor de um campo especifico
    /// </remarks>
    internal class ILLdfld : ILStack
    {
        
        ///<inheritdoc/>
        public ILLdfld(ILBuilderProxy ScopeBuilder, FieldBuilder fieldBuilder) : base(ScopeBuilder, OpCodes.Ldfld)
        {
            ObjectBuilder = fieldBuilder;
        }
        /// <summary>
        /// ObjectBuilder de campos
        /// </summary>
        public FieldBuilder? ObjectBuilder { get; private set; }

        ///<inheritdoc/>
        public override void Apply()
        {
            base.Apply();
            if(ObjectBuilder is null)
            {
                throw new ObjectDisposedException(nameof(ObjectBuilder));
            }

            ScopeBuilder.Emit(Code, ObjectBuilder);

        }

        ///<inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            ObjectBuilder = null;
        }

        ///<inheritdoc/>
        public override string ToString()
        {
            return $"\t\t{_offset} {Code} {ObjectBuilder.FieldType.Name.ToLower(CultureInfo.CurrentCulture)} {ObjectBuilder.DeclaringType.FullName}::{ObjectBuilder.Name}";
        }
    }
}