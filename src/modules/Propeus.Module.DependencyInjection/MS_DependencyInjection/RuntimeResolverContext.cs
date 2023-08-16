namespace Propeus.Module.DependencyInjection.MS_DependencyInjection;

internal struct RuntimeResolverContext
{
    public ServiceProviderEngineScope Scope { get; set; }

    public RuntimeResolverLock AcquiredLocks { get; set; }
}
