using System;
using System.Collections.Generic;
using System.Text;

namespace Propeus.Modulo.Abstrato.Atributos
{
    /// <summary>
    /// Indica se o modulo marcado deve ser inicializado após o mapeamento 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ModuloAutoInicializavelAttribute : Attribute
    {
    }
}
