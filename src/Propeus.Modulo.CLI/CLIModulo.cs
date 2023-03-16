using System;

using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Abstrato.Interfaces;

namespace Propeus.Modulo.CLI
{
    /// <summary>
    /// Exemplo de modulo auto inicializavel e funcional
    /// </summary>
    [Modulo]
    //[ModuloAutoInicializavel]
    public class CLIModulo : ModuloBase
    {
        /// <summary>
        /// Construtor padrao do <see cref="ModeloBase"/>
        /// </summary>
        /// <param name="gerenciador"></param>
        public CLIModulo(IGerenciador gerenciador) : base(gerenciador, true)
        {

        }

        public void CriarInstancia(string[] args)
        {
            Subcommands(args);
        }

        public void ExecutarCLI(string[] args)
        {
            Subcommands(args);
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposing)
            {
                //Continua o disposing padrao
                base.Dispose(disposing);
            }
        }

        private void Subcommands(string[] args)
        {
            switch (args[0])
            {
                case "help":
                    ObtionsHelp(args);
                    break;
                case "create":
                    CreateCLI.Execute(args, Gerenciador);
                    break;
                case "image":
                    ImageCLI.ExecuteImage(args, Gerenciador);
                    break;
                case "images":
                    ImageCLI.ExecuteImages(args, Gerenciador);
                    break;
                case "info":
                    System.Console.WriteLine(Gerenciador);
                    break;
                case "kill":
                    KillCLI.Execute(args, Gerenciador);
                    break;
                case "restart":
                    RestartCLI.Execute(args, Gerenciador);
                    break;
                case "rm":
                    RemoveCLI.Execute(args, Gerenciador);
                    break;
                case "version":
                    VersionCLI.Execute(args, Gerenciador);
                    break;
                case "cp":
                    CpCLI.Execute(args, Gerenciador);
                    break;
                default:
                    throw new NotImplementedException("Argumento");

            }
        }

        private void ObtionsHelp(string[] args)
        {
            System.Console.Clear();
            System.Console.WriteLine("Propeus.Modulo.CLI");
            System.Console.WriteLine("==================");
            System.Console.WriteLine("Uso: dotnet run <> subcommand [OPTIONS] ");
            System.Console.WriteLine("create [--full-name|--name] command");
            System.Console.WriteLine("image [--id] command");
            System.Console.WriteLine("images");
            System.Console.WriteLine("kill [--id] command");
            System.Console.WriteLine("restart [--id] command");
            System.Console.WriteLine("rm [--id] command");
            System.Console.WriteLine("version");
            System.Console.WriteLine();
        }


    }
}