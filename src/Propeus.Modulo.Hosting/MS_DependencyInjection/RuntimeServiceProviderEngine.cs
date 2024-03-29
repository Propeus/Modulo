﻿using Propeus.Modulo.Hosting.MS_DependencyInjection;

namespace Propeus.Modulo.Hosting.MS_DependencyInjection;

partial class ServiceProvider
{
    internal sealed class RuntimeServiceProviderEngine : ServiceProviderEngine
    {
        public static RuntimeServiceProviderEngine Instance { get; } = new RuntimeServiceProviderEngine();

        private RuntimeServiceProviderEngine() { }

        public override Func<ServiceProviderEngineScope, object?> RealizeService(ServiceCallSite callSite)
        {
            return scope =>
            {
                return CallSiteRuntimeResolver.Instance.Resolve(callSite, scope);
            };
        }
    }
}