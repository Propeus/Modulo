using Propeus.Modulo.IL.Geradores;
using System;
using System.Collections.Generic;
using System.Text;

namespace Propeus.Modulo.IL.Helpers
{
    public static partial class Helper
    {
        /// <summary>
        /// Gera uma classe basica
        /// </summary>
        /// <param name="iLGerador"></param>
        /// <returns></returns>
        public static ILClasse CriarClasse(this ILGerador iLGerador)
        {
            if (iLGerador is null)
            {
                throw new ArgumentNullException(nameof(iLGerador));
            }

            var cls = new ILClasse(iLGerador);
            iLGerador.Classes.Add(cls);
            return cls;
        }

        
    }
}