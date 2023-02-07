using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Propeus.Modulo.IL.Enums;
using Propeus.Modulo.IL.Geradores;

namespace Propeus.Modulo.IL.Helpers
{
    public static partial class Helper
    {
        /// <summary>
        /// Cria e inicializa um campo
        /// </summary>
        /// <param name="iLClasse"></param>
        /// <param name="acessadores"></param>
        /// <param name="tipo"></param>
        /// <param name="nome"></param>
        /// <param name="comprimento"></param>
        /// <returns></returns>
        /// <example>
        /// public int a = new int[99];
        /// 
        /// </example>
        public static ILCampo CriarCampoArray(this ILClasseProvider iLClasse, Token[] acessadores,Type tipo, string nome,int comprimento)
        {
            API.ClasseAPI.CriarCampo(iLClasse.Atual, acessadores, tipo, nome);
            var campo = iLClasse.Atual.Campos.Last();
            var construtures = iLClasse.Atual.Metodos.Where(nme => nme.Nome == ".ctor");

            foreach(var constr in construtures)
            {
                API.MetodoAPI.CarregarArgumento(constr);
                API.MetodoAPI.CarregarValorInt(constr,comprimento);
                API.MetodoAPI.CriarArray(constr,tipo);
                API.MetodoAPI.ArmazenarValorCampo(constr,campo);
            }
            return campo;
        }
    }
}
