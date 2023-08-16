namespace Propeus.Module.DependencyInjection.MS_DependencyInjection;

/// <summary>
/// Summary description for ServiceCallSite
/// </summary>
internal abstract class ServiceCallSite
{
    protected ServiceCallSite(ResultCache cache)
    {
        Cache = cache;
    }

    public abstract Type ServiceType { get; }
    public abstract Type? ImplementationType { get; }
    public abstract CallSiteKind Kind { get; }
    public ResultCache Cache { get; }
    public object? Value { get; set; }

    public bool CaptureDisposable =>
        ImplementationType == null ||
        typeof(IDisposable).IsAssignableFrom(ImplementationType) ||
        typeof(IAsyncDisposable).IsAssignableFrom(ImplementationType);
}
