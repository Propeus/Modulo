﻿using System.Runtime.Loader;

namespace Propeus.Modulo.Modelos
{
    /// <summary>
    /// AssemblyLoadContext customizado
    /// </summary>
    public class ModuloAssemblyLoadContext : AssemblyLoadContext
    {
        /// <summary>
        /// Construtor padrão
        /// </summary>
        public ModuloAssemblyLoadContext() : base(true)
        {
        }
    }
}
