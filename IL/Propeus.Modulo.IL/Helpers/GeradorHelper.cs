using Propeus.Modulo.IL.Geradores;
using System;
using System.Collections.Generic;
using System.Text;

namespace Propeus.Modulo.IL.Helpers
{
    public static partial class Helper
    {
        /// <summary>
        /// Gera uma classe identica ao original
        /// </summary>
        /// <param name="iLGerador"></param>
        /// <returns></returns>
        public static ILGerador ClonarClasse<TClasse>(this ILGerador iLGerador)
        where TClasse : class
        {
            if (iLGerador is null)
            {
                throw new ArgumentNullException(nameof(iLGerador));
            }

            var tClasse = typeof(TClasse);

            var cls = new ILClasse(iLGerador, tClasse.Name, interfaces: tClasse.GetInterfaces());



            iLGerador.Classes.Add(cls);

            var campos = tClasse.GetFields();
            foreach (var campo in campos)
            {
                cls.CriarCampo(campo.Attributes.to,)
            }

            return cls;
        }


    }
}