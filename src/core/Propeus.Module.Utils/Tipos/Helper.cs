namespace Propeus.Module.Utils.Tipos
{

    /// <summary>
    /// Classe de ajuda para o tipo <see cref="Type"/>
    /// </summary>
    public static partial class Helper
    {
        /// <summary>
        /// Obtem o valor padrão do tipo passado no parametro <paramref name="type"/>
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object Default(this Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;

        }


    }
}