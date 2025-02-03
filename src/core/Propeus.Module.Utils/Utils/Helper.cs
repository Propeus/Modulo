using System.Reflection;
using System.Runtime.Versioning;

namespace Propeus.Module.Utils.Utils
{
    /// <summary>
    /// Classe de apoio em utilidades
    /// </summary>
    public static partial class Helper
    {
        /// <summary>
        /// Obtem o diretorio do %userprofile% atual
        /// </summary>
        public static readonly string USER_PROFILE_DIRECTORY = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
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

        /// <summary>
        /// Obtem o nome da framework atual
        /// </summary>
        /// <returns></returns>
        public static string? GetCurrentTargetFramework()
        {
            return Assembly
            .GetEntryAssembly()?
            .GetCustomAttribute<TargetFrameworkAttribute>()?
            .FrameworkDisplayName?.Replace(" ", string.Empty).Remove(0, 1).ToLower();
        }
    }

}
