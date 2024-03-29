﻿using System;

using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Abstrato.Modulos;

namespace Propeus.Modulo.CLI
{
    /// <summary>
    /// Exemplo de modulo auto inicializavel e funcional
    /// </summary>
    [Modulo]
    //[ModuloAutoInicializavel]
    public class CliModulo : ModuloBase
    {
        private IGerenciador Gerenciador { get; }

        /// <summary>
        /// Construtor padrao do <see cref="ModeloBase"/>
        /// </summary>
        /// <param name="gerenciador"></param>
        public CliModulo(IGerenciador gerenciador) : base(true)
        {
            Gerenciador = gerenciador;
        }

        /// <summary>
        /// Exibe CLI para criar uma nova instancia
        /// </summary>
        /// <param name="args">Lista de argumentos</param>
        public void CriarInstancia(string[] args)
        {
            Subcommands(args);
        }
        /// <summary>
        /// Exibe CLI para criar uma nova instancia
        /// </summary>
        /// <param name="args">Lista de argumentos</param>
        public void ExecutarCLI(string[] args)
        {
            Subcommands(args);
        }

        ///<inheritdoc/>
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
                    ObtionsHelp();
                    break;
                case "create":
                    CreateCli.Execute(args, Gerenciador);
                    break;
                case "image":
                    ImageCli.ExecuteImage(args, Gerenciador);
                    break;
                case "images":
                    ImageCli.ExecuteImages(args, Gerenciador);
                    break;
                case "info":
                    System.Console.WriteLine(Gerenciador);
                    break;
                case "kill":
                    KillCli.Execute(args, Gerenciador);
                    break;
                case "restart":
                    RestartCli.Execute(args, Gerenciador);
                    break;
                case "rm":
                    RemoveCli.Execute(args, Gerenciador);
                    break;
                case "version":
                    VersionCli.Execute(args, Gerenciador);
                    break;
                case "cp":
                    CpCli.Execute(args, Gerenciador);
                    break;
                default:
                    throw new NotImplementedException("Argumento");

            }
        }

        private static void ObtionsHelp()
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