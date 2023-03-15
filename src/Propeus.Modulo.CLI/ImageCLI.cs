using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Propeus.Modulo.Abstrato.Interfaces;

namespace Propeus.Modulo.CLI
{
    static class ImageCLI
    {

        public static void ExecuteImage(string[] args, IGerenciador gerenciador)
        {
            switch (args[1])
            {
                case "--id":
                    gerenciador.Obter(args[2]);
                    break;
            }
        }

        public static void ExecuteImages(string[] args, IGerenciador gerenciador)
        {
            foreach (var item in gerenciador.Listar())
            {
                System.Console.WriteLine(item);
            }
        }

        private static void ObtionsCreateHelp(string[] args, IGerenciador gerenciador)
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
