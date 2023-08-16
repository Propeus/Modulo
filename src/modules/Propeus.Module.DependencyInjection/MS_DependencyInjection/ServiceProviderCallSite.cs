
namespace Propeus.Module.DependencyInjection.MS_DependencyInjection;

internal sealed class ServiceProviderCallSite : ServiceCallSite
{
    public ServiceProviderCallSite() : base(ResultCache.None)
    {
    }

    public override Type ServiceType { get; } = typeof(IServiceProvider);
    public override Type ImplementationType { get; } = typeof(ServiceProvider);
    public override CallSiteKind Kind { get; } = CallSiteKind.ServiceProvider;
}
