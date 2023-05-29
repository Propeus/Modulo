using System.Diagnostics.CodeAnalysis;

namespace Propeus.Modulo.Hosting.MS_DependencyInjection;

partial class ServiceProvider
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

        public override Func<ServiceProviderEngineScope, object?> RealizeService(ServiceCallSite callSite) => ResolverBuilder.Build(callSite);
    }
}