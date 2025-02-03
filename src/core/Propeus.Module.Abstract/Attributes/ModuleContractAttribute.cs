using Propeus.Module.Abstract.Exceptions;

namespace Propeus.Module.Abstract.Attributes
{
    /**
    * MINI MANUAL
    * 1 - A propriedade "ModuleName" informa o nome do tipo do modulo.
    * 1.1 - O nome do modulo sempre será o mesmo nome do tipo
    * 2 - A propriedade "ModuleType" informa o tipo do modulo.
    * **/


    /// <summary>
    /// Atributo de identificação de moduleType.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public sealed class ModuleContractAttribute : Attribute
    {

        /// <summary>
        /// Inicializa o atributo informando o nome do modulo alvo
        /// </summary>
        /// <param name="moduleName">Nome do modulo</param>
        public ModuleContractAttribute(string moduleName)
        {

            if (string.IsNullOrEmpty(moduleName))
            {
                throw new ModuleContractInvalidException();
            }

            ModuleName = moduleName;

        }

        /// <summary>
        /// Inicializa o atributo informando o tipo do modulo alvo
        /// </summary>
        /// <param name="moduleType">Tipo do modulo</param>
        public ModuleContractAttribute(Type moduleType)
        {

            if (moduleType != null)
            {
                ModuleName = moduleType.Name;
                ModuleType = moduleType;
            }
            else
            {
                throw new ModuleContractInvalidException();
            }
        }

        /// <summary>
        /// Nome do modulo
        /// </summary>
        public string ModuleName { get; }
        /// <summary>
        /// Tipo do modulo
        /// </summary>
        /// <remarks>
        /// Esta propriedade e opcional e sera preenchida somente quando o tipo for informado no construtor do atributo
        /// </remarks>
        public Type? ModuleType { get; }
    }
}
