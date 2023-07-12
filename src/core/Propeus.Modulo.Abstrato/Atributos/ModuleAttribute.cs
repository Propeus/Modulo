using System;
using System.Reflection;

namespace Propeus.Modulo.Abstrato.Atributos
{
    /// <summary>
    /// Identifier de extremidade de um modulo
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class ModuleAttribute : Attribute
    {

        /// <summary>
        /// Indica se o modulo e autoinicializavel
        /// </summary>
        public bool AutoStartable { get; set; } = false;
        /// <summary>
        /// Indica se deve ser recriado todas as instancias do modulo, caso a DLL seja alterada
        /// </summary>
        public bool AutoUpdate { get; set; } = false;
    }
}
