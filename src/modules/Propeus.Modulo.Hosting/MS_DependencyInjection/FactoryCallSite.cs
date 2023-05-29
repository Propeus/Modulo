namespace Propeus.Modulo.Hosting.MS_DependencyInjection;

internal sealed class FactoryCallSite : ServiceCallSite
{
    public Func<IServiceProvider, object> Factory { get; }

    public FactoryCallSite(ResultCache cache, Type serviceType, Func<IServiceProvider, object> factory) : base(cache)
    {
        Factory = factory;
        ServiceType = serviceType;
    }

    public override Type ServiceType { get; }
    public override Type? ImplementationType => null;

    public override CallSiteKind Kind { get; } = CallSiteKind.Factory;
}
