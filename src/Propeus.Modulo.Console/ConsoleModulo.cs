using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Abstrato.Interfaces;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Propeus.Modulo.Console
{

    /// <summary>
    /// Interface de contrato para o Propeus.Modulo.CLI.CLIModulo
    /// </summary>
    [ModuloContrato("CLIModulo")]
    public interface IModuloCLIContrato : IModulo
    {
        /// <summary>
        /// Executa a CLI do Propeus.Modulo.CLI
        /// </summary>
        /// <param name="args"></param>
        void ExecutarCLI(string[] args);
    }


    /// <summary>
    /// Exemplo de modulo auto inicializavel e funcional
    /// </summary>
    [Modulo]
    [ModuloAutoInicializavel]
    public class ConsoleModulo : ModuloBase
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        Task tarefa;
        IModulo modulo;
        private readonly IModuloCLIContrato moduloCLI;

        /// <summary>
        /// Construtor padrao do <see cref="ModeloBase"/>
        /// </summary>
        /// <param name="gerenciador"></param>
        /// <param name="moduloCLI"></param>
        /// <param name="instanciaUnica"></param>
        public ConsoleModulo(IGerenciador gerenciador, IModuloCLIContrato moduloCLI) : base(gerenciador, true)
        {
            modulo = gerenciador.Listar().FirstOrDefault(m => m.Nome == "Gerenciador");
            //Inicia uma task para continuar a execucao do modulo apos a inicializacao dele.
            tarefa = Task.Run(IniciarProcesso);
            System.Console.CancelKeyPress += Console_CancelKeyPress;
            System.Console.Title = "Terminal - Gerenciador";
            this.moduloCLI = moduloCLI;
        }

        private void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            cancellationTokenSource.Cancel();
        }

        private Task IniciarProcesso()
        {
            IGerenciador gerenciador = modulo as IGerenciador;
            System.Console.Clear();
            System.Console.WriteLine("Console modulo " + "v" + Versao);
            while (!cancellationTokenSource.IsCancellationRequested)
            {

                try
                {
                    System.Console.Write("Digite o comando: ");
                    var cmd = Util.Console.Console.ReadLine(cancellationTokenSource.Token).Split(' ');
                    System.Console.Clear();
                    System.Console.WriteLine("Console modulo " + "v" + Versao);

                    if (moduloCLI != null)
                    {
                        moduloCLI.ExecutarCLI(cmd);
                    }
                    else
                    {
                        switch (cmd[0])
                        {
                            case "-a":
                            case "--ajuda":
                                System.Console.WriteLine("Ajuda");
                                System.Console.WriteLine("-l ou --listar: Lista todos os modulos do gerenciador atual ");
                                System.Console.WriteLine("-o ou --obter [id]: Obtem um modulo em execucao pelo id ou nome");
                                System.Console.WriteLine("-e ou --existe [id]: Verifica se o modulo esta em execucao");
                                System.Console.WriteLine("-r ou --reciclar [id]: Recicla um modulo");
                                System.Console.WriteLine("-rm ou --remover [ id | all ]: Remove um modulo especifico ou todos");
                                System.Console.WriteLine("--sair: Finaliza este modulo");
                                System.Console.WriteLine("-v ou --versao: Obtem a versao do gerenciador atual");
                                System.Console.WriteLine("limpar ou cls: Limpa a tela do console");

                                break;
                            case "-l":
                            case "--list":
                                switch (cmd[1])
                                {
                                    case "gerenciador":
                                        System.Console.WriteLine(gerenciador);
                                        break;
                                    case "modulo":
                                        foreach (var item in gerenciador.Listar())
                                        {
                                            System.Console.WriteLine(item);
                                        }
                                        break;
                                    default:
                                        System.Console.WriteLine("Comando invalido");
                                        break;
                                }
                                break;
                            case "-o":
                            case "--obter":
                                System.Console.WriteLine(gerenciador.Obter(cmd[1]));
                                break;
                            case "-e":
                            case "--existe":
                                System.Console.WriteLine(gerenciador.Existe(cmd[1]));
                                break;
                            case "-r":
                            case "--reciclar":
                                System.Console.WriteLine(gerenciador.Reciclar(cmd[1]));
                                break;
                            case "-rm":
                            case "--remover":
                                if (cmd[1].ToLower() == "all")
                                {
                                    gerenciador.RemoverTodos();
                                }
                                else
                                {
                                    gerenciador.Remover(cmd[1]);
                                }
                                break;
                            case "--sair":
                                cancellationTokenSource.Cancel();
                                break;
                            case "-v":
                            case "--versao":
                                break;
                            case "cls":
                            case "limpar":
                                System.Console.Clear();
                                break;
                            default:
                                System.Console.WriteLine("Comando invalido");
                                break;
                        }
                    }


                }
                catch (Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                }

            }
            System.Console.WriteLine("Console finaizado");
            return Task.CompletedTask;
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposing)
            {
                //Cancela a tarefa atual, pois o gerenciador nao tem os mesmos privilagios de um G.C. sob o objeto.
                cancellationTokenSource.Cancel();
                //Aguarta a tarefa se finalizada
                tarefa.Wait();

                cancellationTokenSource.Dispose();

                //Continua o disposing padrao
                base.Dispose(disposing);
            }
        }

        private void Subcommands(string[] args)
        {
            switch (args[0])
            {
                case "create":
                    ObtionsCreate(args);
                    break;
                case "image":
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
                    break;
                case "checkpoint":
                    break;
                case "commit":
                    break;
                case "config":
                    break;
                case "container":
                    break;
                case "context":
                    break;
                case "cp":
                    break;
                case "diff":
                    break;
                case "events":
                    break;
                case "exec":
                    break;
                case "export":
                    break;
                case "history":
                    break;
                case "import":
                    break;
                case "info":
                    break;
                case "inspect":
                    break;
                case "kill":
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
                    break;
                case "volume":
                    break;
                case "wait":
                    break;

            }
        }

        private void OptionsImages(string[] args)
        {
            System.Console.WriteLine(Gerenciador.Listar());
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
