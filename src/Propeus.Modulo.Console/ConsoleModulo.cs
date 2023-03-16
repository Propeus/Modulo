using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Abstrato.Interfaces;

namespace Propeus.Modulo.Console
{

    /// <summary>
    /// Interface de contrato para o Propeus.Modulo.CLI.CLIModulo
    /// </summary>
    [ModuloContrato("CliModulo")]
    public interface IModuloCliContrato : IModulo
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
        private readonly CancellationTokenSource cancellationTokenSource = new();
        private readonly Task tarefa;
        private readonly IModulo modulo;
        private readonly IModuloCliContrato moduloCLI;

        /// <summary>
        /// Construtor padrao do <see cref="ModeloBase"/>
        /// </summary>
        /// <param name="gerenciador"></param>
        /// <param name="moduloCLI"></param>
        public ConsoleModulo(IGerenciador gerenciador, IModuloCliContrato moduloCLI = null) : base(gerenciador, true)
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

            if (moduloCLI != null)
            {
                moduloCLI.ExecutarCLI(new string[] { "help" });
            }
            else
            {
                System.Console.Clear();
                System.Console.WriteLine("Console modulo " + "v" + Versao);
            }


            while (!cancellationTokenSource.IsCancellationRequested)
            {

                if (moduloCLI != null)
                {
                    System.Console.Write("Digite o comando: ");
                    string[] cmd = System.Console.ReadLine().Split(' ');
                    System.Console.Clear();
                    moduloCLI.ExecutarCLI(cmd);
                }
                else
                {
                    try
                    {
                        System.Console.Write("Digite o comando: ");
                        string[] cmd = System.Console.ReadLine().Split(' ');
                        System.Console.Clear();
                        System.Console.WriteLine("Console modulo " + "v" + Versao);


                        switch (cmd[0])
                        {
                            case "-a":
                            case "--ajuda":
                                System.Console.WriteLine("Ajuda");
                                System.Console.WriteLine("-l ou --listar: Lista todos os modulos do gerenciador atual ");
                                System.Console.WriteLine("-o ou --obter [id]: Obtem um modulo em execucao pelo id ou nome");
                                System.Console.WriteLine("-e ou --existe [id]: Verifica se o modulo esta em execucao");
                                System.Console.WriteLine("-r ou --reciclar [id]: Recicla um modulo");
                                System.Console.WriteLine("-cp [-s|--source] path [-d|--destination] path : Copia um modulo para um destino");
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
                                        foreach (IModulo item in gerenciador.Listar())
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
                                Gerenciador.Remover(this);
                                break;
                            case "-v":
                            case "--versao":
                                break;
                            case "--cp":
                                switch (cmd[1])
                                {
                                    case "--source":
                                    case "-s":
                                        switch (cmd[3])
                                        {
                                            case "--destination":
                                            case "-d":
                                                System.IO.File.Copy(cmd[2], cmd[4], true);
                                                break;
                                            default:
                                                break;
                                        }
                                        gerenciador.Remover(cmd[2]);
                                        break;
                                    default:
                                        switch (cmd[2])
                                        {
                                            case "--destination":
                                            case "-d":
                                                System.IO.File.Copy(cmd[1], cmd[3], true);
                                                break;
                                            default:
                                                System.IO.File.Copy(cmd[1], cmd[2], true);
                                                break;
                                        }
                                        break;
                                }
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
                    catch (Exception ex)
                    {
                        System.Console.WriteLine(ex.Message);
                    }
                }



            }
            System.Console.WriteLine("Console finaizado");
            return Task.CompletedTask;
        }
        /// <inheritdoc/>

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
    }

}
