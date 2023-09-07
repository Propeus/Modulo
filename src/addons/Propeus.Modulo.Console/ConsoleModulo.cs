using System;
using System.Threading;
using System.Threading.Tasks;

using Propeus.Module.Abstract;
using Propeus.Module.Abstract.Attributes;
using Propeus.Module.Abstract.Interfaces;
using Propeus.Modulo.Console.Contracts;

namespace Propeus.Modulo.Console
{

    //TODO: Caso o atributo possua AutoStartable, procurar o metodo Launch e anexar ao taskjob se estiver disponivel,
    //caso contrario inicar o launch de void e o usuario deve lidar com o ciclo de vida do modulo
    /// <summary>
    /// Exemplo de modulo auto inicializavel e funcional
    /// </summary>
    [Module(AutoStartable = true, Singleton = true)]
    public class ConsoleModulo : BaseModule
    {
        private readonly CancellationTokenSource cancellationTokenSource = new();
        private readonly Task tarefa;
        private readonly IModuleManager _moduleManager;
        private readonly IModuloCliContrato moduloCLI;

        /// <summary>
        /// Construtor padrao do <see cref="ConsoleModulo"/>
        /// </summary>
        /// <param name="moduleManager"></param>
        /// <param name="moduloCLI"></param>
        public ConsoleModulo(IModuleManager moduleManager, IModuloCliContrato? moduloCLI = null) : base()
        {
            _moduleManager = moduleManager;
            //Inicia uma task para continuar a execucao do modulo apos a inicializacao dele.
            tarefa = Task.Run(IniciarProcesso, cancellationTokenSource.Token);
            System.Console.CancelKeyPress += Console_CancelKeyPress;
            System.Console.CursorVisible = true;
            System.Console.Title = "Terminal - Gerenciador";
            this.moduloCLI = moduloCLI;
        }

        private void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            cancellationTokenSource.Cancel();
        }

        private void IniciarProcesso()
        {


            if (moduloCLI != null)
            {
                moduloCLI.ExecutarCLI(new string[] { "help" });
            }
            else
            {
                System.Console.Clear();
                System.Console.WriteLine("Console modulo " + "v" + Version);
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
                        System.Console.WriteLine("Console modulo " + "v" + Version);


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
                                        System.Console.WriteLine(_moduleManager);
                                        break;
                                    case "modulo":
                                        foreach (IModule item in _moduleManager.ListAllModules())
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
                                System.Console.WriteLine(_moduleManager.GetModule(cmd[1]));
                                break;
                            case "-e":
                            case "--existe":
                                System.Console.WriteLine(_moduleManager.ExistsModule(cmd[1]));
                                break;
                            case "-r":
                            case "--reciclar":
                                System.Console.WriteLine(_moduleManager.RecycleModule(cmd[1]));
                                break;
                            case "-rm":
                            case "--remover":
                                _moduleManager.RemoveModule(cmd[1]);
                                break;
                            case "--s":
                            case "--sair":
                                cancellationTokenSource.Cancel();
                                this.State = State.Off;
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
                                        _moduleManager.RemoveModule(cmd[2]);
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
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                //Cancela a tarefa atual, pois o gerenciador nao tem os mesmos privilagios de um G.C. sob o objeto.
                cancellationTokenSource.Cancel();

                cancellationTokenSource.Dispose();

                //Continua o disposing padrao
                base.Dispose(disposing);
            }
        }
    }

}
