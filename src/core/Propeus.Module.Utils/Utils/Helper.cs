using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Propeus.Module.Utils.Utils
{
    /// <summary>
    /// Classe de apoio em utilidades
    /// </summary>
    public static partial class Helper
    {
        /// <summary>
        /// Obtém o caminho do diretório do programa em execução
        /// </summary>
        public static readonly string CURRENT_DIRECTORY = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
        /// <summary>
        /// Obtém o caminho da pasta "modules"
        /// </summary>
        public static readonly string CURRENT_FOLDER_MODULES = Path.Combine(CURRENT_DIRECTORY, "modules");
        /// <summary>
        /// Obtém o caminho da pasta "packages"
        /// </summary>
        public static readonly string CURRENT_FOLDER_PACKAGES = Path.Combine(CURRENT_DIRECTORY, "packages");
        /// <summary>
        /// Obtém o caminho da pasta "dependences"
        /// </summary>
        public static readonly string CURRENT_FOLDER_DEPENDENCES = Path.Combine(CURRENT_DIRECTORY, "dependences");
    }
}
