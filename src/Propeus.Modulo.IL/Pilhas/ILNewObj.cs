using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using Propeus.Modulo.Util;
using Propeus.Modulo.IL.Proxy;

namespace Propeus.Modulo.IL.Pilhas
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
            if (_executado)
            {
                return;
            }

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