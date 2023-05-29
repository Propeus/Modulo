using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Propeus.Modulo.Hosting.MS_DependencyInjection;

internal sealed class IEnumerableCallSite : ServiceCallSite
{
    internal Type ItemType { get; }
    internal ServiceCallSite[] ServiceCallSites { get; }

    public IEnumerableCallSite(ResultCache cache, Type itemType, ServiceCallSite[] serviceCallSites) : base(cache)
    {
        Debug.Assert(!ServiceProvider.VerifyAotCompatibility || !itemType.IsValueType, "If VerifyAotCompatibility=true, an IEnumerableCallSite should not be created with a ValueType.");

        ItemType = itemType;
        ServiceCallSites = serviceCallSites;
    }

    [UnconditionalSuppressMessage("AotAnalysis", "IL3050:RequiresDynamicCode",
        Justification = "When ServiceProvider.VerifyAotCompatibility is true, which it is by default when PublishAot=true, " +
        "CallSiteFactory ensures ItemType is not a ValueType.")]
    public override Type ServiceType => typeof(IEnumerable<>).MakeGenericType(ItemType);

    [UnconditionalSuppressMessage("AotAnalysis", "IL3050:RequiresDynamicCode",
        Justification = "When ServiceProvider.VerifyAotCompatibility is true, which it is by default when PublishAot=true, " +
        "CallSiteFactory ensures ItemType is not a ValueType.")]
    public override Type ImplementationType => ItemType.MakeArrayType();

    public override CallSiteKind Kind { get; } = CallSiteKind.IEnumerable;
}
