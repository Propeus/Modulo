using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;

using Propeus.Modulo.Abstrato.Interfaces;
using Microsoft.Extensions.Hosting;

namespace Propeus.Modulo.Hosting
{
    public static class Extensions
    {
        public static IHostBuilder ConfigureGerenciador(this IHostBuilder configureHostBuilder, IGerenciador gerenciador)
        {
            configureHostBuilder.UseServiceProviderFactory(new ModuloServiceProviderFactory());
            configureHostBuilder.ConfigureServices((ctx, serviceDescriptors) =>
            {
                serviceDescriptors.AddSingleton<IActionDescriptorChangeProvider>(ModuleActionDescriptorChangeProvider.Instance);
                serviceDescriptors.AddSingleton<IGerenciador>(gerenciador);
                var map = new ModuloApplicationPart(serviceDescriptors.AddMvcCore().PartManager);
                serviceDescriptors.AddSingleton(map);
            });
            return configureHostBuilder;
        }
    }
}
