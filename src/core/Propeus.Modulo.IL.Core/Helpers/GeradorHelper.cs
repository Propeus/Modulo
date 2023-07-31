using Propeus.Modulo.IL.Geradores;

namespace Propeus.Modulo.IL.Core.Helpers
{
    public static class GeradorHelper
    {
        private static ILGerador _gerador;
        private static ILModulo _modulo;


        public static ILModulo Modulo
        {
            get
            {
                if (_modulo == null || _modulo.disposedValue)
                {
                    _modulo = Gerador.CriarModulo();
                }
                return _modulo;
            }
        }
        public static ILGerador Gerador
        {
            get
            {
                if (_gerador == null || _gerador.disposedValue)
                {
                    _gerador = new ILGerador();
                }
                return _gerador;
            }
        }
        public static void Dispose(this ILGerador iLGerador)
        {
            iLGerador.Dispose();
        }
        public static void Dispose(this ILModulo iLModulo)
        {
            iLModulo.Dispose();
        }



    }
}
