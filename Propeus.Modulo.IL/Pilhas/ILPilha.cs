﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

using Propeus.Modulo.Abstrato.Util;
using Propeus.Modulo.IL.Geradores;
using Propeus.Modulo.IL.Interfaces;
using Propeus.Modulo.IL.Proxy;

namespace Propeus.Modulo.IL.Pilhas
{
    internal class ILPilha : IILPilha, IDisposable
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
                switch (valor)
                {
                    case 0:
                        Code = OpCodes.Ldarg_0;
                        break;
                    case 1:
                        Code = OpCodes.Ldarg_1;
                        break;
                    case 2:
                        Code = OpCodes.Ldarg_2;
                        break;
                    case 3:
                        Code = OpCodes.Ldarg_3;
                        break;
                }
            }
            else if (valor.GetType().IsPrimitive)
            {

                if (valor is bool || valor is byte || valor is sbyte || valor is short || valor is int || valor is char)
                {
                    int valorC = Convert.ToInt32(valor);

                    switch (valorC)
                    {
                        case 0:
                            Code = OpCodes.Ldc_I4_0;
                            break;
                        case 1:
                            Code = OpCodes.Ldc_I4_1;
                            break;
                        case 2:
                            Code = OpCodes.Ldc_I4_2;
                            break;
                        case 3:
                            Code = OpCodes.Ldc_I4_3;
                            break;
                        case 4:
                            Code = OpCodes.Ldc_I4_4;
                            break;
                        case 5:
                            Code = OpCodes.Ldc_I4_5;
                            break;
                        case 6:
                            Code = OpCodes.Ldc_I4_6;
                            break;
                        case 7:
                            Code = OpCodes.Ldc_I4_7;
                            break;
                        case 8:
                            Code = OpCodes.Ldc_I4_8;
                            break;
                        case (>= sbyte.MinValue and <= sbyte.MaxValue):
                            Code = OpCodes.Ldc_I4_S;
                            break;
                        case (>= int.MinValue and <= int.MaxValue):
                            Code = OpCodes.Ldc_I4;
                            break;
                    }



                }
                else if (valor is ushort || valor is uint)
                {
                    uint valorC = Convert.ToUInt32(valor);

                    switch (valorC)
                    {
                        case 0:
                            Code = OpCodes.Ldc_I4_0;
                            break;
                        case 1:
                            Code = OpCodes.Ldc_I4_1;
                            break;
                        case 2:
                            Code = OpCodes.Ldc_I4_2;
                            break;
                        case 3:
                            Code = OpCodes.Ldc_I4_3;
                            break;
                        case 4:
                            Code = OpCodes.Ldc_I4_4;
                            break;
                        case 5:
                            Code = OpCodes.Ldc_I4_5;
                            break;
                        case 6:
                            Code = OpCodes.Ldc_I4_6;
                            break;
                        case 7:
                            Code = OpCodes.Ldc_I4_7;
                            break;
                        case 8:
                            Code = OpCodes.Ldc_I4_8;
                            break;
                        case <= ushort.MaxValue:
                            Code = OpCodes.Ldc_I4;
                            break;
                        case <= uint.MaxValue:
                            Code = OpCodes.Ldc_I4_M1;
                            break;
                    }
                }
                else if (valor is long)
                {
                    long valorC = Convert.ToInt64(valor);

                    switch (valorC)
                    {
                        case 0:
                            Code = OpCodes.Ldc_I4_0;
                            break;
                        case 1:
                            Code = OpCodes.Ldc_I4_1;
                            break;
                        case 2:
                            Code = OpCodes.Ldc_I4_2;
                            break;
                        case 3:
                            Code = OpCodes.Ldc_I4_3;
                            break;
                        case 4:
                            Code = OpCodes.Ldc_I4_4;
                            break;
                        case 5:
                            Code = OpCodes.Ldc_I4_5;
                            break;
                        case 6:
                            Code = OpCodes.Ldc_I4_6;
                            break;
                        case 7:
                            Code = OpCodes.Ldc_I4_7;
                            break;
                        case 8:
                            Code = OpCodes.Ldc_I4_8;
                            break;
                        case (>= sbyte.MinValue and <= sbyte.MaxValue):
                            Code = OpCodes.Ldc_I4_S;
                            break;
                        case (>= int.MinValue and <= int.MaxValue):
                            Code = OpCodes.Ldc_I4;
                            break;
                        case (>= long.MinValue and <= long.MaxValue):
                            Code = OpCodes.Ldc_I8;
                            break;
                    }
                }

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