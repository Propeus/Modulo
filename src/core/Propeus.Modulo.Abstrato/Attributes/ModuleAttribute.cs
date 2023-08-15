using System;

namespace Propeus.Modulo.Abstrato.Attributes
{
    /// <summary>
    /// Identifier de extremidade de um modulo
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class ModuleAttribute : Attribute
    {

        /// <summary>
        /// Indica se o modulo e auto inicializava
        /// </summary>
        public bool AutoStartable { get; set; } = false;
        /// <summary>
        /// Indica se deve ser recriado todas as instancias do modulo, caso a DLL seja alterada
        /// </summary>
        public bool AutoUpdate { get; set; } = false;
        /// <summary>
        /// Informa que o tipo deve ser de instancia unica ou não
        /// </summary>
        /// <value>Por padrão é <see langword="false"/></value>
        public bool Singleton { get; set; } = false;
        /// <summary>
        /// Indica se o modulo deve ser mantido vivo ou não em caso de ausência de referencia
        /// </summary>
        /// <value>Por padrão é <see langword="false"/></value>
        public bool KeepAlive { get; set; } = false;
    }
}
