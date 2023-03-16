using Propeus.Modulo.Abstrato.Interfaces;

namespace Propeus.Modulo.CLI
{
    internal static class VersionCLI
    {
        public static void Execute(string[] args, IGerenciador gerenciador)
        {
            System.Console.WriteLine(gerenciador.Versao);
        }
    }
}
