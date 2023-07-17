using System.Diagnostics;

using Microsoft.Extensions.DependencyInjection;

namespace Propeus.Modulo.Hosting.MS_DependencyInjection;

internal struct ResultCache
{
    public static ResultCache None { get; } = new ResultCache(CallSiteResultCacheLocation.None, ServiceCacheKey.Empty);

    internal ResultCache(CallSiteResultCacheLocation lifetime, ServiceCacheKey cacheKey)
    {
        Location = lifetime;
        Key = cacheKey;
    }

    public ResultCache(ServiceLifetime lifetime, Type? type, int slot)
    {
        Debug.Assert(lifetime == ServiceLifetime.Transient || type != null);

        Location = lifetime switch
        {
            ServiceLifetime.Singleton => CallSiteResultCacheLocation.Root,
            ServiceLifetime.Scoped => CallSiteResultCacheLocation.Scope,
            ServiceLifetime.Transient => CallSiteResultCacheLocation.Dispose,
            _ => CallSiteResultCacheLocation.None,
        };
        Key = new ServiceCacheKey(type, slot);
    }

    public CallSiteResultCacheLocation Location { get; set; }

    public ServiceCacheKey Key { get; set; }
}
