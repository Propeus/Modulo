using System.Reflection;

namespace Propeus.Module.Utils.Atributos
{
    /// <summary>
    /// Classe de ajuda para <see cref="Attribute"/>
    /// </summary>
    public static partial class Helper
    {


        /// <summary>
        /// Verifica se o <paramref name="obj"/> possui o atributo
        /// </summary>
        /// <typeparam name="T">Tipo do atributo a ser procurado</typeparam>
        /// <param name="obj">Qualquer objeto do tipo <see cref="Type"/></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Argumento <paramref name="obj"/> nulo</exception>
        public static bool PossuiAtributo<T>(this Type obj) where T : Attribute
        {
            return obj.GetCustomAttribute<T>() != null;
        }
    }
}