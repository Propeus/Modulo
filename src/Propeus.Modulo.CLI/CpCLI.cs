using Propeus.Modulo.Abstrato.Interfaces;

namespace Propeus.Modulo.CLI
{
    internal static class CpCLI
    {

        public static void Execute(string[] args, IGerenciador gerenciador)
        {
            switch (args[1])
            {
                case "--source":
                case "-s":
                    switch (args[3])
                    {
                        case "--destination":
                        case "-d":
                            System.IO.File.Copy(args[2], args[4], true);
                            break;
                        default:
                            break;
                    }
                    gerenciador.Remover(args[2]);
                    break;
                default:
                    switch (args[2])
                    {
                        case "--destination":
                        case "-d":
                            System.IO.File.Copy(args[1], args[3], true);
                            break;
                        default:
                            System.IO.File.Copy(args[1], args[2], true);
                            break;
                    }
                    break;
            }
        }



        private static void ObtionsCreateHelp(string[] args, IGerenciador gerenciador)
        {
            System.Console.Clear();
            System.Console.WriteLine("Propeus.Modulo.CLI");
            System.Console.WriteLine("==================");
            System.Console.WriteLine("Uso: dotnet run <> cp [-s|--source] command [-d|--destination] command");
            System.Console.WriteLine("[-s|--source] <caminho>");
            System.Console.WriteLine("[-d|--destination] <caminho>");
            System.Console.WriteLine();
        }
    }
}
