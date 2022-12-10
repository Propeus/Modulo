﻿using Propeus.Modulo.IL.Interfaces;
using Propeus.Modulo.IL.Proxy;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace Propeus.Modulo.IL.Pilhas
{
    /// <summary>
    /// Chama uma função
    /// </summary>
    internal struct ILCall : IILPilha,IDisposable
    {
        public ILCall(ILBuilderProxy proxy, Type metodo)
        {
            Proxy = proxy ?? throw new ArgumentNullException(nameof(proxy));

            Code = OpCodes.Call;
            Valor = metodo ?? throw new ArgumentNullException(nameof(metodo));
        }

        public ILCall(ILBuilderProxy proxy, MethodInfo metodo)
        {
            Proxy = proxy ?? throw new ArgumentNullException(nameof(proxy));

            Code = OpCodes.Call;
            Valor = metodo ?? throw new ArgumentNullException(nameof(metodo));
        }

        public ILCall(ILBuilderProxy proxy, ConstructorInfo metodo)
        {
            Proxy = proxy ?? throw new ArgumentNullException(nameof(proxy));

            Code = OpCodes.Call;
            Valor = metodo ?? throw new ArgumentNullException(nameof(metodo));
        }

        public ILCall(ILBuilderProxy proxy, FieldInfo metodo)
        {
            Proxy = proxy ?? throw new ArgumentNullException(nameof(proxy));

            Code = OpCodes.Call;
            Valor = metodo ?? throw new ArgumentNullException(nameof(metodo));
        }

        public OpCode Code { get; }
        public MemberInfo Valor { get; private set; }
        public ILBuilderProxy Proxy { get; private set;   }
        public bool Executado { get; private set; }

        public void Executar()
        {

            if (Executado)
                return;

            if (Valor is MethodInfo)
                Proxy.Emit(Code, Valor as MethodInfo);
            else if (Valor is ConstructorInfo)
                Proxy.Emit(Code, Valor as ConstructorInfo);
            else
                throw new InvalidCastException("Não foi possivel determinar o tipo de membro o valor pertence");

            Executado = true;
        }

        public void Dispose()
        {
            Proxy.Dispose();
            Proxy = null;
            Valor = null;
        }

    }
}