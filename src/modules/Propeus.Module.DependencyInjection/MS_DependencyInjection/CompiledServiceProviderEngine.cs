using System.Diagnostics.CodeAnalysis;


namespace Propeus.Module.DependencyInjection.MS_DependencyInjection;

internal partial class ServiceProvider
{
    internal abstract class CompiledServiceProviderEngine : ServiceProviderEngine
    {
#if IL_EMIT
        public ILEmitResolverBuilder ResolverBuilder { get; }
#else
        public ExpressionResolverBuilder ResolverBuilder { get; }
#endif

        [RequiresDynamicCode("Creates DynamicMethods")]
        public CompiledServiceProviderEngine(ServiceProvider provider)
        {
            ResolverBuilder = new(provider);
        }

        public override Func<ServiceProviderEngineScope, object?> RealizeService(ServiceCallSite callSite)
        {
            return ResolverBuilder.Build(callSite);
        }
    }
}