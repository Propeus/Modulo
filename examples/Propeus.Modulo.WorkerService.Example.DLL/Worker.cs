using Propeus.Modulo.Abstrato.Atributos;

namespace Propeus.Modulo.WorkerService.Exmaple.DLL;

[Modulo(AutoInicializavel = true, AutoAtualizavel = true)]
public class Worker : BackgroundServiceModulo
{

    /**
    * Use este worker no projeto de exemplo 'Propeus.Modulo.WorkerService.Example'
    * **/

    public Worker() : base(false)
    {

    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            System.Console.WriteLine("Worker running at time: {0}", DateTimeOffset.Now);
            await Task.Delay(1000, stoppingToken);
        }
    }
}
