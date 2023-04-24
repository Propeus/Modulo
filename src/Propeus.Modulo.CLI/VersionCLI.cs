using Propeus.Modulo.Abstrato.Interfaces;

namespace Propeus.Modulo.CLI
{
    internal static class VersionCli
    {
        public static void Execute(string[] args, IGerenciador gerenciador)
        {
            System.Console.WriteLine(gerenciador.Versao);
        }
    }
}
