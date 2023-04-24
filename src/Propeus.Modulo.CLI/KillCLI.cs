using Propeus.Modulo.Abstrato.Interfaces;

namespace Propeus.Modulo.CLI
{
    internal static class KillCli
    {

        public static void Execute(string[] args, IGerenciador gerenciador)
        {
            switch (args[1])
            {
                case "--id":
                    gerenciador.Remover(args[2]);
                    break;
                case "--all":
                    gerenciador.RemoverTodos();
                    break;
                case "help":
                    OptionsCreateHelp();
                    break;
            }
        }



        private static void OptionsCreateHelp()
        {
            System.Console.Clear();
            System.Console.WriteLine("Propeus.Modulo.CLI");
            System.Console.WriteLine("==================");
            System.Console.WriteLine("Uso: dotnet run <> kill [OPTIONS] ");
            System.Console.WriteLine("[--id] <nome do modulo>");
            System.Console.WriteLine("[--all]");
            System.Console.WriteLine();
        }
    }
}
