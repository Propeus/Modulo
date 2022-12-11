using Propeus.Modulo.IL.Interfaces;
using Propeus.Modulo.IL.Proxy;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Reflection.Emit;
using System.Text;

namespace Propeus.Modulo.IL.Pilhas
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
            if (_executado)
                return;

            Proxy.Emit(Code);

            base.Executar();
        }
        
        public override string ToString()
        {
            return $"\t\t{_offset} {Code}";
        }
    }
}