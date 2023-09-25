using Propeus.Module.Abstract.Exceptions;

namespace Propeus.Module.Abstract.Attributes
{
    /// <summary>
    /// Atributo de identificação de moduleType.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public sealed class ModuleContractAttribute : Attribute
    {
        /// <summary>
        /// Indica se o atributo possui os parâmetros necessários
        /// </summary>
        public bool IsValid => ModuleName != null || ModuleType != null;

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
        public Type ModuleType { get; private set; }
    }
}
