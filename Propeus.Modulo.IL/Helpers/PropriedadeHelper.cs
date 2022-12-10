using Propeus.Modulo.Abstrato.Util;
using Propeus.Modulo.IL.Enums;
using Propeus.Modulo.IL.Geradores;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;


namespace Propeus.Modulo.IL.Helpers
{
    public static class PropriedadeHelper
    {

        public static ILPropriedade CriarPropriedade(this ILClasseProvider iLClasseProvider, Token acessador, Type tipoRetorno, string nome)
        {
            var prop = iLClasseProvider.CriarPropriedade(tipoRetorno, nome, null, new Token[] { acessador }, new Token[] { acessador });
            prop.Campo = iLClasseProvider.CriarCampo(new Token[] { Token.Privado }, tipoRetorno, nome);
            prop.Getter
                  .CarregarArgumento()
                  .CarregarCampo(iLCampo: prop.Campo)
                  .CriarRetorno();

            prop.Setter
                .CarregarArgumento()
                .CarregarArgumento(1)
                .ArmazenarCampo(iLCampo: prop.Campo)
                .CriarRetorno();

            return prop;
        }

        public static ILPropriedade CriarPropriedade(this ILClasseProvider iLClasse, Type tipoRetorno, string nome, Type[] parametrosIndice = null, Token[] acessadoresGet = null, Token[] acessadoresSet = null)
        {
            if (iLClasse is null)
            {
                throw new ArgumentNullException(nameof(iLClasse));
            }

            var Tprop = new ILPropriedade(iLClasse.Atual.Proxy, iLClasse.Nome, nome, tipoRetorno, parametrosIndice);
            iLClasse.Atual.Propriedades.Add(Tprop);

            if (acessadoresGet != null)
            {
                Tprop.Getter = iLClasse.CriarMetodo(Constantes.CONST_NME_PROPRIEDADE_METODO_GET + Tprop.Nome, new Token[] { Token.OcutarAssinatura, Token.NomeEspecial }.ConcatDistinct(acessadoresGet).ToArray(), tipoRetorno);
            }

            if (acessadoresSet != null)
            {
                Tprop.Setter = iLClasse.CriarMetodo(Constantes.CONST_NME_PROPRIEDADE_METODO_SET + Tprop.Nome, new Token[] { Token.OcutarAssinatura, Token.NomeEspecial }.ConcatDistinct(acessadoresSet).ToArray(), parametros: new Type[] { tipoRetorno });
            }
            return Tprop;
        }





    }
}
