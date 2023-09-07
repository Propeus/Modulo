using Propeus.Module.Abstract.Interfaces;

namespace Propeus.Modulo.CLI
{
    internal static class RemoveCli
    {

        public static void Execute(string[] args, IModuleManager gerenciador)
        {
            switch (args[1])
            {
                case "--id":
                    gerenciador.RemoveModule(args[2]);
                    break;
                case "help":
                    ObtionsCreateHelp(args, gerenciador);
                    break;
            }
        }

        private static void ObtionsCreateHelp(string[] args, IModuleManager gerenciador)
        {
            System.Console.Clear();
            System.Console.WriteLine("Propeus.Modulo.CLI");
            System.Console.WriteLine("==================");
            System.Console.WriteLine("Uso: dotnet run <> restart [OPTIONS] <id>");
            System.Console.WriteLine("[--id] <id do modulo>");
            System.Console.WriteLine("[--all]");
            System.Console.WriteLine();
        }
    }
}
