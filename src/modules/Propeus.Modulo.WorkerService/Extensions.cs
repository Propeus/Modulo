using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Propeus.Modulo.Abstrato.Interfaces;

namespace Propeus.Modulo.WorkerService
{
    public static class Extensions
    {
        public static IHostBuilder UseGerenciador(this IHostBuilder hostBuilder,IModuleManager gerenciador)
        {
            _ = hostBuilder.ConfigureServices((ctx, services) =>
            {
                _ = services.AddSingleton(gerenciador);
            });
            return hostBuilder;
        }
    }
}
