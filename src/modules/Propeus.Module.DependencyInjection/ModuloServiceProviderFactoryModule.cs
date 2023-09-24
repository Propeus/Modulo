using Microsoft.Extensions.DependencyInjection;

using Propeus.Module.Abstract;
using Propeus.Module.Abstract.Attributes;
using Propeus.Module.Abstract.Interfaces;

namespace Propeus.Module.DependencyInjection;

[Module(Description = "Modulo para fabricar o provedor de servico", AutoStartable = false, AutoUpdate = false, Singleton = true, KeepAlive = false)]
public class ModuloServiceProviderFactoryModule : BaseModule, IServiceProviderFactory<IServiceCollection>
{
    private readonly IModuleManager moduleManager;

    public ModuloServiceProviderFactoryModule(IModuleManager moduleManager)
    {
        this.moduleManager = moduleManager;
    }

    public IServiceCollection CreateBuilder(IServiceCollection services)
    {
        return services ?? new ServiceCollection();
    }

    public IServiceProvider CreateServiceProvider(IServiceCollection containerBuilder)
    {
        return moduleManager.CreateModule<Propeus.Module.DependencyInjection.MS_DependencyInjection.ServiceProviderModule>(new object[] { containerBuilder, new ServiceProviderOptions() });
    }
}