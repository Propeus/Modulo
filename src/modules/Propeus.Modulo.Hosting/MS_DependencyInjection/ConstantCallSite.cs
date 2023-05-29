namespace Propeus.Modulo.Hosting.MS_DependencyInjection;

internal sealed class ConstantCallSite : ServiceCallSite
{
    private readonly Type _serviceType;
    internal object? DefaultValue => Value;

    public ConstantCallSite(Type serviceType, object? defaultValue) : base(ResultCache.None)
    {
        _serviceType = serviceType ?? throw new ArgumentNullException(nameof(serviceType));
        if (defaultValue != null && !serviceType.IsInstanceOfType(defaultValue))
        {
            throw new ArgumentException("ConstantCantBeConvertedToServiceType");
        }

        Value = defaultValue;
    }

    public override Type ServiceType => _serviceType;
    public override Type ImplementationType => DefaultValue?.GetType() ?? _serviceType;
    public override CallSiteKind Kind { get; } = CallSiteKind.Constant;
}
