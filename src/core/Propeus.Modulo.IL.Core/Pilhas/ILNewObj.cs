using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using Propeus.Module.IL.Core.Helpers;
using Propeus.Module.IL.Core.Proxy;

namespace Propeus.Module.IL.Core.Pilhas
{
    /// <summary>
    /// operador 'new'
    /// </summary>
    internal class ILNewObj : ILPilha
    {
        public ILNewObj(ILBuilderProxy proxy, ConstructorInfo ctor) : base(proxy, OpCodes.Newobj)
        {
            Valor = ctor ?? throw new ArgumentNullException(nameof(ctor));
        }

        public ConstructorInfo Valor { get; private set; }

        ///<inheritdoc/>
        public override void Executar()
        {
            base.Executar();
            Proxy.Emit(Code, Valor);

        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            Valor = null;
        }

        public override string ToString()
        {
            return $"\t\t{_offset} {Code} {Valor.DeclaringType.FullName}::{Valor.Name}({string.Join(",", Valor.ObterTipoParametros().Select(x => x.Name))})";
        }
    }
}