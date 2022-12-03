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

       

        public static ILCampo CriarCampo(this ILClasse iLClasse)
        {
            if (iLClasse is null)
            {
                throw new ArgumentNullException(nameof(iLClasse));
            }

            var fld = new ILCampo(iLClasse);
            if (iLClasse.Campos.Any(m => m.Assinatura == fld.Assinatura))
            {
                throw new InvalidOperationException("Já existe um metodo com a mesma assinatura");
            }
            iLClasse.Campos.Add(fld);
            return fld;
        }

        public static ILCampo CriarCampo(this ILClasse iLClasse, TokenEnum acessador)
        {
            if (iLClasse is null)
            {
                throw new ArgumentNullException(nameof(iLClasse));
            }

            var fld = new ILCampo(iLClasse, acessadores: new TokenEnum[] { acessador });
            if (iLClasse.Campos.Any(m => m.Assinatura == fld.Assinatura))
            {
                throw new InvalidOperationException("Já existe um metodo com a mesma assinatura");
            }
            iLClasse.Campos.Add(fld);
            return fld;
        }

        public static ILCampo CriarCampo(this ILClasse iLClasse, TokenEnum acessador, Type tipo)
        {
            if (iLClasse is null)
            {
                throw new ArgumentNullException(nameof(iLClasse));
            }

            if (tipo is null)
            {
                throw new ArgumentNullException(nameof(tipo));
            }
            var fld = new ILCampo(iLClasse, new TokenEnum[] { acessador }, tipo);
            if (iLClasse.Campos.Any(m => m.Assinatura == fld.Assinatura))
            {
                throw new InvalidOperationException("Já existe um metodo com a mesma assinatura");
            }
            iLClasse.Campos.Add(fld);
            return fld;
        }

        public static ILCampo CriarCampo(this ILClasse iLClasse, TokenEnum acessador, Type tipo, string nome)
        {
            if (iLClasse is null)
            {
                throw new ArgumentNullException(nameof(iLClasse));
            }

            if (tipo is null)
            {
                throw new ArgumentNullException(nameof(tipo));
            }

            if (string.IsNullOrEmpty(nome))
            {
                throw new ArgumentException("Nome obrigatório", nameof(nome));
            }

            var fld = new ILCampo(iLClasse, new TokenEnum[] { acessador }, tipo, nome);
            if (iLClasse.Campos.Any(m => m.Assinatura == fld.Assinatura))
            {
                throw new InvalidOperationException("Já existe um metodo com a mesma assinatura");
            }
            iLClasse.Campos.Add(fld);
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