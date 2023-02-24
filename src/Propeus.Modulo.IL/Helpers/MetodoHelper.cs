using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Propeus.Modulo.Util;
using Propeus.Modulo.IL.Enums;
using Propeus.Modulo.IL.Geradores;

namespace Propeus.Modulo.IL.Helpers
{
    /// <summary>
    /// Classe de ajuda para montagem de classes
    /// </summary>
    public static partial class Helper
    {




        /// <summary>
        /// Retorna os metodos nao implementados (por outras interfaces ou por proxy)
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <param name="iLClasse"></param>
        /// <returns></returns>
        public static IEnumerable<ILMetodo> ImplementarMetodoInterface<TInterface>(this ILClasseProvider iLClasse)
        {
            if (iLClasse is null)
            {
                throw new ArgumentNullException(nameof(iLClasse));
            }

            Type tpInterface = typeof(TInterface);

            if (!iLClasse.Atual.Interfaces.Contains(tpInterface))
            {
                _ = iLClasse.NovaVersao(interfaces: new Type[] { tpInterface });
            }

            MethodInfo[] metodos = tpInterface.GetMethods();


            foreach (MethodInfo mth in metodos)
            {
                if (!iLClasse.Atual.Metodos.Any(m => m.Nome == mth.Name && m.Parametros.Cast<Type>().SequenceEqual(mth.ObterTipoParametros())))
                {
                    ILParametro[] parametros = mth.ObterTipoParametros().Select(p => new ILParametro(mth.Name, p)).ToArray();
                    API.ClasseAPI.CriarMetodo(iLClasse.Atual, mth.Attributes.DividirEnum().ParseEnum<MethodAttributes, Token>(), mth.ReturnType, mth.Name, parametros);
                    yield return iLClasse.Atual.Metodos.Last();
                }


            }

        }

        public static ILMetodo CriarMetodo(this ILClasseProvider iLClasseProvider, Token[] acessadores, Type retorno, string nome, ILParametro[] parametros)
        {
            API.ClasseAPI.CriarMetodo(iLClasseProvider.Atual, acessadores, retorno, nome, parametros);
            return iLClasseProvider.Atual.Metodos.Last();
        }
        public static ILMetodo CriarMetodo(this ILDelegate iLClasseProvider, Token[] acessadores, Type retorno, string nome, params ILParametro[] parametros)
        {
            API.ClasseAPI.CriarMetodo(iLClasseProvider, acessadores, retorno, nome, parametros);
            ILMetodo mth = iLClasseProvider.Metodos.Last();
            mth._metodoBuilder.SetImplementationFlags(MethodImplAttributes.Runtime | MethodImplAttributes.Managed);
            return mth;
        }
        public static ILMetodo CriarConstrutor(this ILDelegate iLClasseProvider, Token[] acessadores, params ILParametro[] parametros)
        {
            API.ClasseAPI.CriarMetodo(iLClasseProvider, acessadores, typeof(void), ".ctor", parametros);
            ILMetodo mth = iLClasseProvider.Metodos.Last();
            _ = iLClasseProvider.Metodos.Remove(mth);
            mth._metodoBuilder.SetImplementationFlags(MethodImplAttributes.Runtime | MethodImplAttributes.Managed);
            iLClasseProvider.Construtores.Add(mth);
            return mth;
        }


        public static ILMetodo Soma(this ILMetodo iLMetodo, ILVariavel iLVariavel1, ILVariavel iLVariavel2)
        {
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLVariavel1.Indice);
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLVariavel2.Indice);
            API.MetodoAPI.Soma(iLMetodo);
            return iLMetodo;
        }
        public static ILMetodo Soma(this ILMetodo iLMetodo, ILParametro iLParametro1, ILParametro iLParametro2)
        {
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLParametro1.Indice);
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLParametro2.Indice);
            API.MetodoAPI.Soma(iLMetodo);
            return iLMetodo;
        }
        public static ILMetodo Subitrair(this ILMetodo iLMetodo, ILVariavel iLVariavel1, ILVariavel iLVariavel2)
        {
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLVariavel1.Indice);
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLVariavel2.Indice);
            API.MetodoAPI.Subitrair(iLMetodo);
            return iLMetodo;
        }
        public static ILMetodo Subitrair(this ILMetodo iLMetodo, ILParametro iLParametro1, ILParametro iLParametro2)
        {
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLParametro1.Indice);
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLParametro2.Indice);
            API.MetodoAPI.Subitrair(iLMetodo);
            return iLMetodo;
        }
        public static ILMetodo Dividir(this ILMetodo iLMetodo, ILVariavel iLVariavel1, ILVariavel iLVariavel2)
        {
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLVariavel1.Indice);
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLVariavel2.Indice);
            API.MetodoAPI.Dividir(iLMetodo);
            return iLMetodo;
        }
        public static ILMetodo Dividir(this ILMetodo iLMetodo, ILParametro iLParametro1, ILParametro iLParametro2)
        {
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLParametro1.Indice);
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLParametro2.Indice);
            API.MetodoAPI.Dividir(iLMetodo);
            return iLMetodo;
        }
        public static ILMetodo Multiplicar(this ILMetodo iLMetodo, ILVariavel iLVariavel1, ILVariavel iLVariavel2)
        {
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLVariavel1.Indice);
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLVariavel2.Indice);
            API.MetodoAPI.Multiplicar(iLMetodo);
            return iLMetodo;
        }
        public static ILMetodo Multiplicar(this ILMetodo iLMetodo, ILParametro iLParametro1, ILParametro iLParametro2)
        {
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLParametro1.Indice);
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLParametro2.Indice);
            API.MetodoAPI.Multiplicar(iLMetodo);
            return iLMetodo;
        }


        public static ILMetodo Igual(this ILMetodo iLMetodo, ILParametro iLParametro1, ILParametro iLParametro2)
        {

            API.MetodoAPI.CarregarArgumento(iLMetodo, iLParametro1.Indice);
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLParametro2.Indice);
            API.MetodoAPI.Igual(iLMetodo);
            return iLMetodo;
        }
        public static ILMetodo Diferente(this ILMetodo iLMetodo, ILParametro iLParametro1, ILParametro iLParametro2)
        {
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLParametro1.Indice);
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLParametro2.Indice);
            API.MetodoAPI.Diferente(iLMetodo);
            return iLMetodo;
        }
        public static ILMetodo MaiorQue(this ILMetodo iLMetodo, ILParametro iLParametro1, ILParametro iLParametro2)
        {
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLParametro1.Indice);
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLParametro2.Indice);
            API.MetodoAPI.MaiorQue(iLMetodo);
            return iLMetodo;
        }
        public static ILMetodo MenorQue(this ILMetodo iLMetodo, ILParametro iLParametro1, ILParametro iLParametro2)
        {
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLParametro1.Indice);
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLParametro2.Indice);
            API.MetodoAPI.MenorQue(iLMetodo);
            return iLMetodo;
        }
        public static ILMetodo MaiorOuIgualQue(this ILMetodo iLMetodo, ILParametro iLParametro1, ILParametro iLParametro2)
        {
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLParametro1.Indice);
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLParametro2.Indice);
            API.MetodoAPI.MaiorOuIgualQue(iLMetodo);
            return iLMetodo;
        }
        public static ILMetodo MenorOuIgualQue(this ILMetodo iLMetodo, ILParametro iLParametro1, ILParametro iLParametro2)
        {
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLParametro1.Indice);
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLParametro2.Indice);
            API.MetodoAPI.MenorOuIgualQue(iLMetodo);
            return iLMetodo;
        }

        public static ILMetodo SeFim(this ILMetodo iLMetodo)
        {
            API.MetodoAPI.SeFim(iLMetodo);
            return iLMetodo;
        }
        public static ILMetodo Se(this ILMetodo iLMetodo, ILParametro iLParametro1, Func<ILParametro, ILParametro, ILMetodo> operador, ILParametro iLParametro2)
        {
            API.MetodoAPI.Se(iLMetodo);
            _ = operador.Invoke(iLParametro1, iLParametro2);
            return iLMetodo;
        }

        public static ILMetodo E(this ILMetodo iLMetodo, ILParametro iLParametro1, Func<ILParametro, ILParametro, ILMetodo> operador, ILParametro iLParametro2)
        {
            _ = operador.Invoke(iLParametro1, iLParametro2);
            return iLMetodo;
        }
        public static ILMetodo Ou(this ILMetodo iLMetodo, ILParametro iLParametro1, Func<ILParametro, ILParametro, ILMetodo> operador, ILParametro iLParametro2)
        {
            _ = iLMetodo.Se(iLParametro1, operador, iLParametro2);
            if (iLMetodo.PilhasAuxiliares.Count == 2)
            {
                Interfaces.IILPilha aux = iLMetodo.PilhasAuxiliares.Pop();
                Interfaces.IILPilha aux2 = iLMetodo.PilhasAuxiliares.Pop();

                iLMetodo.PilhasAuxiliares.Push(aux);
                iLMetodo.PilhasAuxiliares.Push(aux2);
            }
            _ = iLMetodo.SeFim();

            return iLMetodo;
        }

        public static ILMetodo CarregarParametro(this ILMetodo iLMetodo, ILParametro iLParametro)
        {
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLParametro.Indice);
            return iLMetodo;
        }
        public static ILMetodo CarregarTrue(this ILMetodo iLMetodo)
        {
            API.MetodoAPI.CarregarValorBoolean(iLMetodo, true);
            return iLMetodo;
        }
        public static ILMetodo CarregarFalse(this ILMetodo iLMetodo)
        {
            API.MetodoAPI.CarregarValorBoolean(iLMetodo, false);
            return iLMetodo;
        }
        public static ILMetodo CriarRetorno(this ILMetodo iLMetodo)
        {
            API.MetodoAPI.CriarRetorno(iLMetodo);
            return iLMetodo;
        }
        public static ILMetodo ChamarFuncao(this ILMetodo iLMetodo, MethodInfo methodInfo)
        {

            API.MetodoAPI.ChamarFuncao(iLMetodo, methodInfo);

            return iLMetodo;
        }

        public static ILMetodo AtribuirMetodoEmDelegate(this ILMetodo iLMetodo, ILDelegate iLDelegate, ILMetodo metodoDelegate)
        {
            API.MetodoAPI.CarregarArgumento(iLMetodo);
            API.MetodoAPI.CriarPonteiro(iLMetodo, metodoDelegate.MethodInfo);
            API.MetodoAPI.CriarInstancia(iLMetodo, iLDelegate.ConstructorInfo);
            return iLMetodo;
        }

        public static ILMetodo InvocarDelegate(this ILMetodo iLMetodo, ILDelegate iLDelegate, params ILParametro[] iLParametros)
        {
            for (int i = 0; i < iLParametros.Length; i++)
            {
                API.MetodoAPI.CarregarArgumento(iLMetodo, iLParametros[i].Indice);
            }

            API.MetodoAPI.ChamarFuncaoVirtual(iLMetodo, iLDelegate.InvokeInfo);

            return iLMetodo;
        }

    }
}