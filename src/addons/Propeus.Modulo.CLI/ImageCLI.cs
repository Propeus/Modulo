using Propeus.Module.Abstract.Interfaces;

namespace Propeus.Modulo.CLI
{
    internal static class ImageCli
    {

        public static void ExecuteImage(string[] args, IModuleManager gerenciador)
        {
            switch (args[1])
            {
                case "--id":
                    _ = gerenciador.GetModule(args[2]);
                    break;
                case "help":
                    OptionsCreateHelp(args);
                    break;
            }
        }

        public static void ExecuteImages(string[] args, IModuleManager gerenciador)
        {
            foreach (IModule item in gerenciador.ListAllModules())
            {
                System.Console.WriteLine(item);
            }
        }

        private static void OptionsCreateHelp(string[] args)
        {
            System.Console.Clear();
            System.Console.WriteLine("Propeus.Modulo.CLI");
            System.Console.WriteLine("==================");
            switch (args[1])
            {
                case "image":
                    System.Console.WriteLine("Uso: dotnet run <> image [OPTIONS] ");
                    System.Console.WriteLine("[--id] <id do modulo>");
                    break;
                default:
                    System.Console.WriteLine("Uso: dotnet run <> images");
                    break;
            }
            System.Console.WriteLine();
        }
    }
}
