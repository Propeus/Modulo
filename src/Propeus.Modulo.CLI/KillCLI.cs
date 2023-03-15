using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Propeus.Modulo.Abstrato.Interfaces;

namespace Propeus.Modulo.CLI
{
    static class KillCLI
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
            }
        }

      

        private static void ObtionsCreateHelp(string[] args, IGerenciador gerenciador)
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
