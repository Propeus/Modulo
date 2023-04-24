using System;

using Propeus.Modulo.IL.Core.Enums;
using Propeus.Modulo.IL.Core.Geradores;
using Propeus.Modulo.IL.Geradores;

namespace Propeus.Modulo.IL.Core.API
{
    internal class ClasseAPI
    {

        internal static void CriarMetodo(ILClasse iLClasse, Token[] acessadores, Type tipoRetorno, string nomeMetodo = Constantes.CONST_NME_METODO, ILParametro[] parametros = null)
        {
            iLClasse.Metodos.Add(new ILMetodo(iLClasse.Proxy, iLClasse.Nome, nomeMetodo, acessadores, tipoRetorno, parametros));
        }
        internal static void CriarCampo(ILClasse iLClasse, Token[] acessador, Type tipo, string nome = Constantes.CONST_NME_CAMPO)
        {
            iLClasse.Campos.Add(new ILCampo(iLClasse.Proxy, iLClasse.Nome, acessador, tipo, nome));
        }
        internal static void CriarPropriedade(ILClasse iLClasse, Type tipo, string nome = Constantes.CONST_NME_PROPRIEDADE, Type[] parametros = null)
        {
            iLClasse.Propriedades.Add(new ILPropriedade(iLClasse.Proxy, iLClasse.Nome, nome, tipo, parametros));
        }
    }
}
