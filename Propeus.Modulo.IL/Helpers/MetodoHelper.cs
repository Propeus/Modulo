using Propeus.Modulo.IL.Enums;
using Propeus.Modulo.IL.Geradores;
using Propeus.Modulo.IL.Pilhas;

using Propeus.Modulo.Abstrato.Util;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using Propeus.Modulo.IL.Pilhas.Tipos.TiposPrimitivos;

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

            var tpInterface = typeof(TInterface);

            if (!iLClasse.Atual.Interfaces.Contains(tpInterface))
            {
                _ = iLClasse.NovaVersao(interfaces: new Type[] { tpInterface });
            }

            var metodos = tpInterface.GetMethods();


            foreach (var mth in metodos)
            {
                if (!iLClasse.Atual.Metodos.Any(m => m.Nome == mth.Name && m.Parametros.Cast<Type>().SequenceEqual(mth.ObterTipoParametros())))
                {
                    var parametros = mth.ObterTipoParametros().Select(p => new ILParametro(mth.Name, p)).ToArray();
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
    
        public static ILMetodo Soma(this ILMetodo iLMetodo, ILVariavel iLVariavel_1, ILVariavel iLVariavel_2)
        {
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLVariavel_1.Indice);
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLVariavel_2.Indice);
            API.MetodoAPI.Soma(iLMetodo);
            return iLMetodo;
        }
        public static ILMetodo Soma(this ILMetodo iLMetodo, ILParametro iLParametro_1, ILParametro iLParametro_2)
            {
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLParametro_1.Indice);
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLParametro_2.Indice);
            API.MetodoAPI.Soma(iLMetodo);
            return iLMetodo;
        }
        public static ILMetodo Subitrair(this ILMetodo iLMetodo, ILVariavel iLVariavel_1, ILVariavel iLVariavel_2)
        {
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLVariavel_1.Indice);
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLVariavel_2.Indice);
            API.MetodoAPI.Subitrair(iLMetodo);
            return iLMetodo;
        }
        public static ILMetodo Subitrair(this ILMetodo iLMetodo, ILParametro iLParametro_1, ILParametro iLParametro_2)
        {
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLParametro_1.Indice);
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLParametro_2.Indice);
            API.MetodoAPI.Subitrair(iLMetodo);
            return iLMetodo;
        }
        public static ILMetodo Dividir(this ILMetodo iLMetodo, ILVariavel iLVariavel_1, ILVariavel iLVariavel_2)
        {
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLVariavel_1.Indice);
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLVariavel_2.Indice);
            API.MetodoAPI.Dividir(iLMetodo);
            return iLMetodo;
        }
        public static ILMetodo Dividir(this ILMetodo iLMetodo, ILParametro iLParametro_1, ILParametro iLParametro_2)
        {
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLParametro_1.Indice);
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLParametro_2.Indice);
            API.MetodoAPI.Dividir(iLMetodo);
            return iLMetodo;
        }
        public static ILMetodo Multiplicar(this ILMetodo iLMetodo, ILVariavel iLVariavel_1, ILVariavel iLVariavel_2)
        {
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLVariavel_1.Indice);
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLVariavel_2.Indice);
            API.MetodoAPI.Multiplicar(iLMetodo);
            return iLMetodo;
        }
        public static ILMetodo Multiplicar(this ILMetodo iLMetodo, ILParametro iLParametro_1, ILParametro iLParametro_2)
        {
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLParametro_1.Indice);
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLParametro_2.Indice);
            API.MetodoAPI.Multiplicar(iLMetodo);
            return iLMetodo;
        }


        public static ILMetodo Igual(this ILMetodo iLMetodo, ILParametro iLParametro_1,ILParametro iLParametro_2)
        {

            API.MetodoAPI.CarregarArgumento(iLMetodo, iLParametro_1.Indice);
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLParametro_2.Indice); 
            API.MetodoAPI.Igual(iLMetodo);
            return iLMetodo;
        }
        public static ILMetodo Diferente(this ILMetodo iLMetodo, ILParametro iLParametro_1, ILParametro iLParametro_2)
        {
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLParametro_1.Indice);
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLParametro_2.Indice);
            API.MetodoAPI.Diferente(iLMetodo);
            return iLMetodo;
        }
        public static ILMetodo MaiorQue(this ILMetodo iLMetodo, ILParametro iLParametro_1, ILParametro iLParametro_2)
        {
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLParametro_1.Indice);
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLParametro_2.Indice);
            API.MetodoAPI.MaiorQue(iLMetodo);
            return iLMetodo;
        }
        public static ILMetodo MenorQue(this ILMetodo iLMetodo, ILParametro iLParametro_1, ILParametro iLParametro_2)
        {
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLParametro_1.Indice);
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLParametro_2.Indice);
            API.MetodoAPI.MenorQue(iLMetodo);
            return iLMetodo;
        }
        public static ILMetodo MaiorOuIgualQue(this ILMetodo iLMetodo, ILParametro iLParametro_1, ILParametro iLParametro_2)
        {
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLParametro_1.Indice);
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLParametro_2.Indice);
            API.MetodoAPI.MaiorOuIgualQue(iLMetodo);
            return iLMetodo;
        }
        public static ILMetodo MenorOuIgualQue(this ILMetodo iLMetodo, ILParametro iLParametro_1, ILParametro iLParametro_2)
        {
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLParametro_1.Indice);
            API.MetodoAPI.CarregarArgumento(iLMetodo, iLParametro_2.Indice);
            API.MetodoAPI.MenorOuIgualQue(iLMetodo);
            return iLMetodo;
        }
        public static ILMetodo SeFim(this ILMetodo iLMetodo)
        {
            API.MetodoAPI.SeFim(iLMetodo);
            return iLMetodo;
        }
        public static ILMetodo Se(this ILMetodo iLMetodo)
        {
            API.MetodoAPI.Se(iLMetodo);
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

    }
}