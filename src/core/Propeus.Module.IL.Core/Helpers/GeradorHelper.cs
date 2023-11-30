using Propeus.Module.IL.Geradores;

namespace Propeus.Module.IL.Core.Helpers
{
    public static class GeradorHelper
    {
        private static ILGerador? _gerador;
       
        /// <summary>
        /// Retorna uma instancia existente do gerador ou uma nova instancia caso nao exista
        /// </summary>
        /// <param name="iLGerador"></param>
        /// <returns></returns>
        public static bool GetCurrentInstanceOrNew(out ILGerador iLGerador)
        {
            if(_gerador == null)
            {
                _gerador = new ILGerador();
                iLGerador = _gerador;
                return false;
            }
            else
            {
                iLGerador = _gerador;
                return true;
            }
        }



        /// <summary>
        /// Libera o gerador da memoria
        /// </summary>
        public static void DisposeGerador()
        {
            if(_gerador != null)
            {
                _gerador.Dispose();
                _gerador = null;
            }
        }



    }
}
