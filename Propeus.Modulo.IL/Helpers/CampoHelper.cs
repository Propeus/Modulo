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

namespace Propeus.Modulo.IL.Helpers
{
    public static partial class Helper
    {

        public static ILCampo CriarCampo(this ILClasseProvider iLClasse, Token[] acessador = null, Type tipo = null, string nome = Constantes.CONST_NME_CAMPO)
        {
            if (iLClasse is null)
            {
                throw new ArgumentNullException(nameof(iLClasse));
            }

            if (tipo is null)
            {
                throw new ArgumentNullException(nameof(tipo));
            }

            var fld = new ILCampo(iLClasse.Atual.Proxy,iLClasse.Nome, acessador, tipo, nome);
            iLClasse.Atual.Campos.Add(fld);
            return fld;
        }

        /// <summary>
        /// Armazena o valor no campo especificado
        /// </summary>
        /// <param name="iLMetodo"></param>
        /// <param name="iLCampo"></param>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static ILMetodo AtribuirValor(this ILMetodo iLMetodo, ILCampo iLCampo, int valor)
        {
            if (iLMetodo.IsDefault())
            {
                throw new ArgumentNullException(nameof(iLMetodo));
            }

            if (iLCampo.IsDefault())
            {
                throw new ArgumentNullException(nameof(iLCampo));
            }

            iLMetodo.PilhaExecucao.Add(new ILInt32(iLMetodo.Proxy, valor));

            return iLMetodo;
        }



       
    }
}