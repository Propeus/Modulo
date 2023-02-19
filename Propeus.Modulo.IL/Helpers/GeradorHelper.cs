using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Propeus.Modulo.IL.Geradores;

namespace Propeus.Modulo.IL.Helpers
{
    public static class GeradorHelper
    {
        static ILGerador _gerador;
        private static ILModulo _modulo;

        public static ILGerador ObterGerador()
        {
            if (_gerador == null)
            {
                _gerador = new ILGerador();
            }

            return _gerador;
        }

        public static ILModulo ObterModuloGenerico()
        {
            if (_modulo ==null)
            {
                _modulo = ObterGerador().CriarModulo(); 
            }

            return _modulo;
        }

    }
}
