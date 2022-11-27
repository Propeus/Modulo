namespace Propeus.Modulo.Abstrato.Util
{
    /// <summary>
    /// Classe de ajuda para outros tipos de menor importancia
    /// </summary>
    public static partial class Helper
    {
        /// <summary>
        /// Nega o resultado booleano
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool Not(this bool result)
        {
            return !result;
        }
    }
}