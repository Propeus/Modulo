using Propeus.Modulo.Modelos;
using Propeus.Modulo.Modelos.Atributos;
using Propeus.Modulo.Modelos.Interfaces;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Propeus.Modulo.Console
{
    [Modulo]
    [ModuloAutoInicializavel]
    public class ConsoleModulo : ModuloBase
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        Task tarefa;
        IModulo modulo;
        public ConsoleModulo(IGerenciador gerenciador, bool instanciaUnica = true) : base(gerenciador, instanciaUnica)
        {
            Util.Thread.LimitedConcurrencyLevelTaskScheduler limitedConcurrencyLevelTaskScheduler = new Util.Thread.LimitedConcurrencyLevelTaskScheduler(2);
            modulo = gerenciador.Listar().First(m => m.Nome == "Gerenciador");
            tarefa = Task.Run(IniciarProcesso);
        }

        private Task IniciarProcesso()
        {
            IGerenciador gerenciador = modulo as IGerenciador;
            System.Console.WriteLine("Console modulo");
            while (!cancellationTokenSource.IsCancellationRequested)
            {
                try
                {
                    var cmd = System.Console.ReadLine().Split(' ');
                    switch (cmd[0])
                    {
                        case "ajuda":
                            System.Console.Clear();
                            System.Console.WriteLine("listar");
                            System.Console.WriteLine("obter <id>");
                            System.Console.WriteLine("existe <id>");
                            System.Console.WriteLine("reiniciar <id>");
                            System.Console.WriteLine("remover [id][todos]");
                            System.Console.WriteLine("sair");
                            System.Console.WriteLine("versao");
                            System.Console.WriteLine("limpar|cls");

                            break;
                        case "listar":
                            foreach (var item in gerenciador.Listar())
                            {
                                System.Console.WriteLine("Nome: " + item.Nome);
                                System.Console.WriteLine("Id: " + item.Id);
                                System.Console.WriteLine("Versao: " + item.Versao);
                                System.Console.WriteLine("Estado: " + item.Estado);
                            }
                            break;
                        case "obter":
                            System.Console.WriteLine(gerenciador.ObterInstancia(cmd[1]));
                            break;
                        case "existe":
                            System.Console.WriteLine(gerenciador.ExisteInstancia(cmd[1]));
                            break;
                        case "reiniciar":
                            System.Console.WriteLine(gerenciador.Reiniciar(cmd[1]));
                            break;
                        case "remover":
                            if (cmd[1].ToLower() == "todos")
                            {
                                gerenciador.RemoverTodos();
                            }
                            else
                            {
                                gerenciador.Remover(cmd[1]);
                            }
                            break;
                        case "sair":
                            cancellationTokenSource.Cancel();
                            break;
                        case "versao":
                            System.Console.WriteLine("ver.: " + Versao);
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
            System.Console.WriteLine("Console finaizado");
            return Task.CompletedTask;
        }

        protected override void Dispose(bool disposing)
        {
            if (!Disposed)
            {

                tarefa.Wait((int)TimeSpan.FromSeconds(2).TotalMilliseconds, cancellationTokenSource.Token);
                cancellationTokenSource.Dispose();
                System.Console.Clear();


                base.Dispose(disposing);
            }
        }

    }
}
