using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Propeus.Modulo.Abstrato.Util;
using Propeus.Modulo.IL.Enums;
using Propeus.Modulo.IL.Geradores;

namespace Propeus.Modulo.IL.API
{
    internal class ClasseAPI
    {

        public static void CriarMetodo(ILClasse iLClasse, Token[] acessadores, Type tipoRetorno, string nomeMetodo = Constantes.CONST_NME_METODO, Type[] parametros = null)
        {
            if (iLClasse is null)
            {
                throw new ArgumentNullException(nameof(iLClasse));
            }

            if (acessadores is null)
            {
                throw new ArgumentNullException(nameof(acessadores));
            }

            if (tipoRetorno is null)
            {
                throw new ArgumentNullException(nameof(tipoRetorno));
            }

            if (string.IsNullOrEmpty(nomeMetodo))
            {
                throw new ArgumentException($"'{nameof(nomeMetodo)}' não pode ser nulo nem vazio.", nameof(nomeMetodo));
            }

            iLClasse.Metodos.Add(new ILMetodo(iLClasse.Proxy, iLClasse.Nome, nomeMetodo, acessadores, tipoRetorno, parametros?.Select(p => new ILParametro(nomeMetodo,p)).ToArray()));
        }
        public static void CriarCampo(ILClasse iLClasse, Token[] acessador, Type tipo, string nome = Constantes.CONST_NME_CAMPO)
        {
            if (iLClasse is null)
            {
                throw new ArgumentNullException(nameof(iLClasse));
            }

            if (acessador is null)
            {
                throw new ArgumentNullException(nameof(acessador));
            }

            if (tipo is null)
            {
                throw new ArgumentNullException(nameof(tipo));
            }

            if (string.IsNullOrEmpty(nome))
            {
                throw new ArgumentException($"'{nameof(nome)}' não pode ser nulo nem vazio.", nameof(nome));
            }

            iLClasse.Campos.Add(new ILCampo(iLClasse.Proxy, iLClasse.Nome, acessador, tipo, nome));
        }
        public static void CriarPropriedade(ILClasse iLClasse, Type tipo, string nome = Constantes.CONST_NME_PROPRIEDADE, Type[] parametros= null)
        {
            iLClasse.Propriedades.Add(new ILPropriedade(iLClasse.Proxy, iLClasse.Nome, nome, tipo, parametros));
        }
    }
}
