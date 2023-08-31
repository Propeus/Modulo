using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Propeus.Module.Abstract.Interfaces;

namespace Propeus.Module.DependencyInjection
{
    public static partial class Extension
    {
        /// <summary>
        /// Anexa o <see cref="IModuleManager"/> como um provedor de Injecao de dependencia secundario
        /// </summary>
        /// <param name="configureHostBuilder">O builder</param>
        /// <returns></returns>
        public static IHostBuilder UseModuleManager(this IHostBuilder configureHostBuilder, IModuleManager moduleManager, Action<IModuleManager, IServiceCollection>? configureServicesDelegate = null)
        {
            configureHostBuilder.UseServiceProviderFactory(new ModuloServiceProviderFactory());
            configureHostBuilder.ConfigureServices(configureDelegate: (builder, sc) =>
            {
                configureServicesDelegate?.Invoke(moduleManager, sc);
            });
            return configureHostBuilder;

        }
    }
}
