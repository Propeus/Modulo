using Propeus.Module.Abstract.Interfaces;

namespace Propeus.Modulo.CLI
{
    internal static class CreateCli
    {

        public static void Execute(string[] args, IModuleManager gerenciador)
        {
            switch (args[1])
            {
                case "--full-name":
                case "--name":
                    if (args.Length > 2)
                    {
                        switch (args[3])
                        {
                            case "--args":
                                _ = gerenciador.CreateModule(args[2], args[3..-1]);
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        _ = gerenciador.CreateModule(args[2]);
                    }
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
            System.Console.WriteLine("Uso: dotnet run <> create [OPTIONS] ");
            System.Console.WriteLine("[--full-name|--name] <nome do modulo>");
            System.Console.WriteLine();
        }
    }
}
