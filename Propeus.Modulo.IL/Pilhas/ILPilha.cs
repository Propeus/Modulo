using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

using Propeus.Modulo.IL.Interfaces;
using Propeus.Modulo.IL.Proxy;

namespace Propeus.Modulo.IL.Pilhas
{
    internal class ILPilha : IILPilha, IDisposable
    {
        public ILPilha(ILBuilderProxy iLBuilderProxy, OpCode opCode)
        {
            Proxy = iLBuilderProxy ?? throw new ArgumentNullException(nameof(iLBuilderProxy));
            Code= opCode;
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

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~ILPilha()
        // {
        //     // Não altere este código. Coloque o código de limpeza no método 'Dispose(bool disposing)'
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Não altere este código. Coloque o código de limpeza no método 'Dispose(bool disposing)'
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        //public void Dispose()
        //{
        //    Proxy.Dispose();
        //    Proxy = null;
        //}
    }
}
