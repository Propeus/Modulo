using Propeus.Modulo.IL.Geradores;

namespace Propeus.Modulo.IL.Helpers
{
    public static class GeradorHelper
    {
        private static ILGerador _gerador;
        private static ILModulo _modulo;

        public static ILGerador ObterGerador()
        {
            _gerador ??= new ILGerador();

            return _gerador;
        }

        public static ILModulo ObterModuloGenerico()
        {
            _modulo ??= ObterGerador().CriarModulo();

            return _modulo;
        }

    }
}
