namespace Propeus.Module.DependencyInjection.MS_DependencyInjection;

internal abstract class ServiceProviderEngine
{
    public abstract Func<ServiceProviderEngineScope, object?> RealizeService(ServiceCallSite callSite);
}
