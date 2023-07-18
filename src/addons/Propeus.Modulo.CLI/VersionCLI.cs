using Propeus.Modulo.Abstrato.Interfaces;

namespace Propeus.Modulo.CLI
{
    internal static class VersionCli
    {
        public static void Execute(string[] args, IModuleManager gerenciador)
        {
            System.Console.WriteLine(gerenciador.Version);
        }
    }
}
