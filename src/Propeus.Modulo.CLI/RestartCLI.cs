using Propeus.Modulo.Abstrato.Interfaces;

namespace Propeus.Modulo.CLI
{
    internal static class RestartCli
    {

        public static void Execute(string[] args, IGerenciador gerenciador)
        {
            switch (args[1])
            {
                case "--id":
                    _ = (gerenciador as IGerenciadorArgumentos).Reciclar(args[2]);
                    break;
                case "help":
                    ObtionsCreateHelp(args, gerenciador);
                    break;
            }
        }

        private static void ObtionsCreateHelp(string[] args, IGerenciador gerenciador)
        {
            System.Console.Clear();
            System.Console.WriteLine("Propeus.Modulo.CLI");
            System.Console.WriteLine("==================");
            System.Console.WriteLine("Uso: dotnet run <> restart [OPTIONS] <nome ou id>");
            System.Console.WriteLine("[--id] <id do modulo>");
            System.Console.WriteLine();
        }
    }
}
