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

        public static ILPropriedade CriarPropriedade(this ILClasse iLClasse, Type tipoRetorno, string nome)
        {
            var Tprop = iLClasse.CriarPropriedade(tipoRetorno, nome, Array.Empty<Type>());
            return Tprop;
        }

        public static ILPropriedade CriarPropriedade(this ILClasse iLClasse, Type tipoRetorno, string nome, Type[] parametros)
        {
            var Tprop = new ILPropriedade(iLClasse, nome,AcessadorEnum.Publico,  tipoRetorno, parametros);
            iLClasse.CriarCampo(TokenEnum.Privado, tipoRetorno, $"cmp_{nome}");
            iLClasse.Propriedades.Add(Tprop);
            return Tprop;
        }

        public static ILPropriedade DefinirGetter(this ILPropriedade iLPropriedade, ILClasse iLClasse)
        {
            if (!iLClasse.Metodos.Where(mth => mth.Nome == $"get_{iLPropriedade.Nome}").Any())
            {
                string nome = $"get_{iLPropriedade.Nome}";
                iLPropriedade.Getter = iLClasse
                    .CriarMetodo(nome, new TokenEnum[] { iLPropriedade.Acessador , TokenEnum.NomeEspecial, TokenEnum.OcutarAssinatura},  iLPropriedade.Retorno)
                    .CarregarArgumento()
                    .CarregarCampo(iLCampo: iLClasse.Campos.Where(cmp => cmp.Nome == $"cmp_{iLPropriedade.Nome}").First())
                    .CriarRetorno();
            }
            return iLPropriedade;
        }

        public static ILPropriedade DefinirSetter(this ILPropriedade iLPropriedade, ILClasse iLClasse)
        {
            if (!iLClasse.Metodos.Where(mth => mth.Nome == $"set_{iLPropriedade.Nome}").Any())
            {
                string nome = $"set_{iLPropriedade.Nome}";
                iLPropriedade.Setter = iLClasse
                    .CriarMetodo(nome, new TokenEnum[] { iLPropriedade.Acessador, TokenEnum.NomeEspecial, TokenEnum.OcutarAssinatura }, typeof(void), new Type[] { iLPropriedade.Retorno })
                    .CarregarArgumento()
                    .CarregarArgumento(1)
                    .ArmazenarCampo(iLCampo: iLClasse.Campos.Where(cmp => cmp.Nome == $"cmp_{iLPropriedade.Nome}").First())
                    .CriarRetorno();

            }
            return iLPropriedade;

        }

    }
}
