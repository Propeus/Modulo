using System;

namespace Propeus.Modulo.Abstrato.Atributos
{
    /// <summary>
    /// Informa se o modulo atual é opcional a sua instancia
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public sealed class ModuloOpcionalAttribute : Attribute
    {
    }
}
