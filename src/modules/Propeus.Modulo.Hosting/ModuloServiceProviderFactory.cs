using System.Collections;

using Microsoft.Extensions.DependencyInjection;

namespace Propeus.Modulo.Hosting;

public class ModuloServiceProviderFactory : IServiceProviderFactory<IServiceCollection>
{

    public IServiceCollection CreateBuilder(IServiceCollection services)
    {
        return services ?? new ServiceCollection();
    }

    public IServiceProvider CreateServiceProvider(IServiceCollection containerBuilder)
    {
        return new Propeus.Modulo.Hosting.MS_DependencyInjection.ServiceProvider(containerBuilder, new ServiceProviderOptions());
    }
}