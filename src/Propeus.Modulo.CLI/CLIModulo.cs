using System;
using System.Security.Cryptography;

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
                    ObtionsCreate(args);
                    break;
                case "image":
                    OptionsImage(args);
                    break;
                case "images":
                    OptionsImages(args);
                    break;
                case "attach":
                    throw new NotImplementedException();
                    break;
                case "build":
                    throw new NotImplementedException();
                    break;
                case "builder":
                    throw new NotImplementedException();
                    break;
                case "checkpoint":
                    throw new NotImplementedException();
                    break;
                case "commit":
                    throw new NotImplementedException();
                    break;
                case "config":
                    throw new NotImplementedException();
                    break;
                case "container":
                    throw new NotImplementedException();
                    break;
                case "context":
                    throw new NotImplementedException();
                    break;
                case "cp":
                    throw new NotImplementedException();
                    break;
                case "diff":
                    throw new NotImplementedException();
                    break;
                case "events":
                    throw new NotImplementedException();
                    break;
                case "exec":
                    throw new NotImplementedException();
                    break;
                case "export":
                    throw new NotImplementedException();
                    break;
                case "history":
                    throw new NotImplementedException();
                    break;
                case "import":
                    throw new NotImplementedException();
                    break;
                case "info":
                    throw new NotImplementedException();
                    break;
                case "inspect":
                    throw new NotImplementedException();
                    break;
                case "kill":
                    OptionsKill(args);
                    break;
                case "load":
                    break;
                case "login":
                    break;
                case "logout":
                    break;
                case "logs":
                    break;
                case "manifest":
                    break;
                case "network":
                    break;
                case "node":
                    break;
                case "pause":
                    break;
                case "plugin":
                    break;
                case "port":
                    break;
                case "ps":
                    break;
                case "pull":
                    break;
                case "push":
                    break;
                case "rename":
                    break;
                case "restart":
                    break;
                case "rm":
                    break;
                case "rmi":
                    break;
                case "run":
                    break;
                case "save":
                    break;
                case "search":
                    break;
                case "secret":
                    break;
                case "service":
                    break;
                case "stack":
                    break;
                case "stats":
                    break;
                case "stop":
                    break;
                case "swarm":
                    break;
                case "system":
                    break;
                case "tag":
                    break;
                case "top":
                    break;
                case "trust":
                    break;
                case "unpause":
                    break;
                case "update":
                    break;
                case "version":
                    OptionsVersion(args);
                    break;
                case "volume":
                    break;
                case "wait":
                    break;

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
            System.Console.WriteLine("version");
            System.Console.WriteLine();
        }

        private void OptionsKill(string[] args)
        {
            switch (args[1])
            {
                case "--id":
                    Gerenciador.Remover(args[2]);
                    break;
                case "--all":
                    Gerenciador.RemoverTodos();
                    break;
            }
        }

        private void OptionsImage(string[] args)
        {
            switch (args[1])
            {
                case "--id":
                    Gerenciador.Obter(args[2]);
                    break;
            }
        }

        private void OptionsVersion(string[] args)
        {
            System.Console.WriteLine(Gerenciador.Versao);
        }

        private void OptionsImages(string[] args)
        {
            foreach (var item in Gerenciador.Listar())
            {
                System.Console.WriteLine(item);
            }
        }

        private void ObtionsCreate(string[] args)
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
                                (Gerenciador as IGerenciadorArgumentos).Criar(args[2], args[3..-1]);
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        Gerenciador.Criar(args[2]);
                    }
                    break;
            }
        }
    }
}