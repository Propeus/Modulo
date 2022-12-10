using Propeus.Modulo.IL.Geradores;
using Propeus.Modulo.IL.Pilhas;

using Propeus.Modulo.Abstrato.Util;

using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace Propeus.Modulo.IL.Helpers
{
    public static partial class Helper
    {
        public static ILVariavel CriarVariavel(this ILMetodo iLMetodo, Type tipo, string nome = null)
        {
            if (iLMetodo is null)
            {
                throw new ArgumentNullException(nameof(iLMetodo));
            }

            if (tipo is null)
            {
                throw new ArgumentNullException(nameof(tipo));
            }

          

            var v = new ILVariavel(iLMetodo.Proxy,iLMetodo.Nome, tipo, nome);
            iLMetodo.Variaveis.Add(v);
            return v;
        }

        /// <summary>
        /// Cria um array com o tamanho especificado
        /// </summary>
        /// <param name="iLVariavel"></param>
        /// <param name="iLMetodo"></param>
        /// <param name="tamanho"></param>
        /// <returns></returns>
        public static ILMetodo CriarArray(this ILMetodo iLMetodo, ILVariavel iLVariavel, int tamanho = 0)
        {
            if (iLMetodo.IsDefault())
            {
                throw new ArgumentNullException(nameof(iLMetodo));
            }

            if (iLVariavel is null)
            {
                throw new ArgumentNullException(nameof(iLVariavel));
            }

            iLMetodo.PilhaExecucao.Add(new ILInt32(iLMetodo.Proxy, tamanho));
            iLMetodo.PilhaExecucao.Add(new ILNewArr(iLMetodo.Proxy, iLVariavel.Retorno));

            return iLMetodo;
        }
    }
}