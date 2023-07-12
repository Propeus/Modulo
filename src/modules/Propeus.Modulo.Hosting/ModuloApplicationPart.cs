using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Primitives;

using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Abstrato.Modulos;
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

            foreach (var moduleType in moduleManager.GetModule<IModuleProviderModuleContract>().GetAllModules())
            {
                OnLoadModuleController(moduleType);
            }            
            moduleManager.GetModule<IModuleProviderModuleContract>().OnLoadModule += OnLoadModuleController;
            moduleManager.GetModule<IModuleProviderModuleContract>().OnUnloadModule += OnUnloadModuleController;
            
        }

        private void OnUnloadModuleController(Type moduleType)
        {
            if (!moduleType.Name.Contains("Controller"))
                return;

            ApplicationPart appOld = null;
            foreach (ApplicationPart appApplicationPart in this.ApplicationPart.ApplicationParts)
            {
                if (appApplicationPart.Name == moduleType.Name)
                {
                    appOld = appApplicationPart;
                }
            }

            if (appOld != null)
            {
                this.ApplicationPart.ApplicationParts.Remove(appOld);
            }
            ModuleActionDescriptorChangeProvider.Instance.TokenSource?.Cancel();
        }

        private void OnLoadModuleController(Type moduleType)
        {
            if (!moduleType.Name.Contains("Controller"))
                return;
            var app = new AssemblyPart(moduleType.Assembly);
            if (!this.ApplicationPart.ApplicationParts.Any(x => x.Name == app.Name))
            {
                this.ApplicationPart.ApplicationParts.Add(app);

                ModuleActionDescriptorChangeProvider.Instance.HasChanged = true;
                ModuleActionDescriptorChangeProvider.Instance.TokenSource?.Cancel();
            }
        }


    
    }
}
