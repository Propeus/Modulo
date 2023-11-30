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
        /// Indica a qual moduleType o contrato pertence
        /// </summary>
        /// <param name="moduleName">ModuleName do moduleType</param>
        public ModuleContractAttribute(string moduleName)
        {

            if (string.IsNullOrEmpty(moduleName))
            {
                throw new ModuleContractInvalidException();
            }

            ModuleName = moduleName;

        }

        /// <summary>
        /// Indica a qual moduleType o contrato pertence
        /// </summary>
        /// <param name="moduleType">ModuleType do moduleType</param>
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
        /// ModuleName do moduleType
        /// </summary>
        public string ModuleName { get; }
        /// <summary>
        /// ModuleType do moduleType
        /// </summary>
        /// <remarks>
        /// Esta propriedade e opcional e sera preenchida somente quando o tipo for informado no construtor do atributo
        /// </remarks>
        public Type? ModuleType { get; }
    }
}
