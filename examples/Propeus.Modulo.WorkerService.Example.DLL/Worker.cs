using Propeus.Module.Abstract.Attributes;

namespace Propeus.Module.WorkerService.Example.DLL;

[Module(AutoStartable = true, AutoUpdate = true, KeepAlive = true, Singleton = false)]
public class Worker : BackgroundServiceModulo
{

    /**
    * Use este worker no projeto de exemplo 'Propeus.Module.WorkerService.Example'
    * **/

    public Worker() : base()
    {

    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            Console.WriteLine("[B] Worker executando no tempo: {0}", DateTimeOffset.Now);
            await Task.Delay(1000, stoppingToken);
        }
    }
}
