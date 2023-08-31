using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Propeus.Module.DependencyInjection;
using Propeus.Module.Abstract.Interfaces;

namespace Propeus.Modulo.Hosting
{
    public static class Extensions
    {
        /// <summary>
        /// Anexa o <see cref="IModuleManager"/> como um provedor de Injecao de dependencia secundario
        /// </summary>
        /// <param name="configureHostBuilder">O builder</param>
        /// <param name="gerenciador">O gerenciador a ser anexado na aplicacao</param>
        /// <returns></returns>
        public static IHostBuilder UseModuleManagerForMvc(this IHostBuilder configureHostBuilder, IModuleManager gerenciador)
        {
            configureHostBuilder.UseModuleManager(gerenciador,(module, serviceDescriptors) => {
                serviceDescriptors.AddSingleton<IActionDescriptorChangeProvider>(ModuleActionDescriptorChangeProvider.Instance);
                serviceDescriptors.AddSingleton(gerenciador);

                ServiceDescriptor descriptor = serviceDescriptors.FirstOrDefault(s => s.ServiceType == typeof(IViewCompilerProvider));
                serviceDescriptors.Remove(descriptor);
                serviceDescriptors.AddSingleton<IViewCompilerProvider, ModuleViewCompilerProvider>();
            });

            return configureHostBuilder;

        }
    }
}
