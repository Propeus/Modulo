using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

using Propeus.Modulo.Abstrato.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Propeus.Modulo.Hosting.ViewEngine;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;

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
        public static IHostBuilder ConfigureGerenciador(this IHostBuilder configureHostBuilder, IModuleManager gerenciador)
        {
            configureHostBuilder.UseServiceProviderFactory(new ModuloServiceProviderFactory());
            configureHostBuilder.ConfigureServices((ctx, serviceDescriptors) =>
            {
                serviceDescriptors.AddSingleton<IActionDescriptorChangeProvider>(ModuleActionDescriptorChangeProvider.Instance);
                serviceDescriptors.AddSingleton<IModuleManager>(gerenciador);
                serviceDescriptors.AddSingleton(new ModuloApplicationPart(serviceDescriptors.AddMvcCore().PartManager, gerenciador));
                serviceDescriptors.Configure<MvcRazorRuntimeCompilationOptions>(opts =>
                {

                    opts.FileProviders.Add(new ModuloFileProvider(gerenciador));
                });
                serviceDescriptors
                .AddControllersWithViews()
                //.AddRazorRuntimeCompilation()
                ;
            });
            return configureHostBuilder;
        }
    }
}
