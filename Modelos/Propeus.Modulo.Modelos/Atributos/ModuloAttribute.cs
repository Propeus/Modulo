using System;

namespace Propeus.Modulo.Modelos.Atributos
{
    /// <summary>
    /// Identificador de extremidade de um modulo
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class ModuloAttribute : Attribute
    { }
}
