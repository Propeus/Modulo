using System;
using System.Reflection.Emit;

using Propeus.Module.IL.Core.Interfaces;
using Propeus.Module.IL.Core.Proxy;

namespace Propeus.Module.IL.Core.Pilhas
{
    internal class ILPilha : IILPilha
    {
        public ILPilha(ILBuilderProxy iLBuilderProxy, OpCode opCode)
        {
            Proxy = iLBuilderProxy ?? throw new ArgumentNullException(nameof(iLBuilderProxy));
            Code = opCode;

        }

        public ILPilha(ILBuilderProxy iLBuilderProxy, OpCode opCode, object valor) : this(iLBuilderProxy, opCode)
        {

            if (opCode == OpCodes.Ldarg)
            {
                Code = valor switch
                {
                    0 => OpCodes.Ldarg_0,
                    1 => OpCodes.Ldarg_1,
                    2 => OpCodes.Ldarg_2,
                    3 => OpCodes.Ldarg_3,
                    _ => OpCodes.Ldarg_S,
                };
            }
        }

        public ILBuilderProxy Proxy { get; private set; }

        public OpCode Code { get; }


        protected bool _executado;
        protected int _offset;

        public virtual void Executar()
        {
            _executado = true;
            _offset = Proxy?.ILGenerator?.ILOffset ?? 0;
        }

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Proxy.Dispose();
                    Proxy = null;
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Não altere este código. Coloque o código de limpeza no método 'DisposeMethod(bool disposing)'
            Dispose(disposing: true);
        }
    }
}
