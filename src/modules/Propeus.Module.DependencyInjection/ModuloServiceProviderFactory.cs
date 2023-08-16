using Microsoft.Extensions.DependencyInjection;

namespace Propeus.Module.DependencyInjection;

public class ModuloServiceProviderFactory : IServiceProviderFactory<IServiceCollection>
{

    public IServiceCollection CreateBuilder(IServiceCollection services)
    {
        return services ?? new ServiceCollection();
    }

    public IServiceProvider CreateServiceProvider(IServiceCollection containerBuilder)
    {
        return new Propeus.Module.DependencyInjection.MS_DependencyInjection.ServiceProvider(containerBuilder, new ServiceProviderOptions());
    }
}