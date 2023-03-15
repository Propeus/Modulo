using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Propeus.Modulo.Abstrato.Interfaces;

namespace Propeus.Modulo.CLI
{
    static class VersionCLI
    {
        public static void Execute(string[] args, IGerenciador gerenciador)
        {
            System.Console.WriteLine(gerenciador.Versao);
        }
    }
}
