using System.Reflection;

namespace Propeus.Modulo.Hosting.MS_DependencyInjection;

internal sealed class ConstructorCallSite : ServiceCallSite
{
    internal ConstructorInfo ConstructorInfo { get; }
    internal ServiceCallSite[] ParameterCallSites { get; }

    public ConstructorCallSite(ResultCache cache, Type serviceType, ConstructorInfo constructorInfo) : this(cache, serviceType, constructorInfo, Array.Empty<ServiceCallSite>())
    {
    }

    public ConstructorCallSite(ResultCache cache, Type serviceType, ConstructorInfo constructorInfo, ServiceCallSite[] parameterCallSites) : base(cache)
    {
        if (!serviceType.IsAssignableFrom(constructorInfo.DeclaringType))
        {
            throw new ArgumentException("O tipo umplementado nao pode ser convertido em servico");
        }

        ServiceType = serviceType;
        ConstructorInfo = constructorInfo;
        ParameterCallSites = parameterCallSites;
    }

    public override Type ServiceType { get; }

    public override Type? ImplementationType => ConstructorInfo.DeclaringType;
    public override CallSiteKind Kind { get; } = CallSiteKind.Constructor;
}
