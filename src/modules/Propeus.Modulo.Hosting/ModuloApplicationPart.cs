using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Primitives;
using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Hosting.Contracts;

namespace Propeus.Modulo.Hosting
{
    public class ModuleActionDescriptorChangeProvider : IActionDescriptorChangeProvider
    {
        public static ModuleActionDescriptorChangeProvider Instance { get; } = new ModuleActionDescriptorChangeProvider();

        public CancellationTokenSource TokenSource { get; private set; }

        public bool HasChanged { get; set; }

        public IChangeToken GetChangeToken()
        {
            TokenSource = new CancellationTokenSource();
            return new CancellationChangeToken(TokenSource.Token);
        }
    }


    internal class ModuloApplicationPart : BaseModule
    {
        Microsoft.AspNetCore.Mvc.ApplicationParts.ApplicationPartManager ApplicationPart { get; set; }

        bool _sincronizado = false;

        public ModuloApplicationPart(ApplicationPartManager applicationPart, IModuleManager moduleManager) : base(true)
        {
            ApplicationPart = applicationPart;
            IModuleProviderModuleContract moduleProviderModuleContract;
            if (!moduleManager.ExistsModule(typeof(IModuleProviderModuleContract)))
            {
                moduleProviderModuleContract = moduleManager.CreateModule<IModuleProviderModuleContract>();
            }
            else
            {
                moduleProviderModuleContract = moduleManager.GetModule<IModuleProviderModuleContract>();
            }

            var modules = moduleProviderModuleContract.GetAllModules();

            foreach (var moduleType in modules)
            {
                OnLoadModuleController(moduleType);
            }

            moduleProviderModuleContract.SetOnLoadModule(OnLoadModuleController);
            moduleProviderModuleContract.SetOnUnloadModule(OnUnloadModuleController);

        }

        private void OnUnloadModuleController(Type moduleType)
        {
            if (!moduleType.Name.Contains("Controller"))
                return;

            List<ApplicationPart> parts = new List<ApplicationPart>();
            foreach (ApplicationPart appApplicationPart in this.ApplicationPart.ApplicationParts)
            {
                if (appApplicationPart.Name == moduleType.Name)
                {
                    parts.Add(appApplicationPart);
                }
            }

            foreach (var part in parts)
            {
                this.ApplicationPart.ApplicationParts.Remove(part);
            }

            ModuleActionDescriptorChangeProvider.Instance.TokenSource?.Cancel();
        }

        private void OnLoadModuleController(Type moduleType)
        {
            if (moduleType.Name.Contains("Controller"))
            {
                var app = new AssemblyPart(moduleType.Assembly);
                var razorPart = new CompiledRazorAssemblyPart(moduleType.Assembly);
                if (!this.ApplicationPart.ApplicationParts.Any(x => x.Name == app.Name))
                {
                    //Tem que carregar os dois
                    this.ApplicationPart.ApplicationParts.Add(app);
                    this.ApplicationPart.ApplicationParts.Add(razorPart);

                    ModuleActionDescriptorChangeProvider.Instance.HasChanged = true;
                    ModuleActionDescriptorChangeProvider.Instance.TokenSource?.Cancel();
                }
            }


        }



    }
}
