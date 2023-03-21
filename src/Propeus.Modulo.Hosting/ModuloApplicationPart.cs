using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Primitives;

using Propeus.Modulo.Abstrato.Proveders;

namespace Propeus.Modulo.Hosting
{
    internal class ModuleActionDescriptorChangeProvider : IActionDescriptorChangeProvider
    {
        internal static ModuleActionDescriptorChangeProvider Instance { get; } = new ModuleActionDescriptorChangeProvider();

        internal CancellationTokenSource TokenSource { get; private set; }

        public IChangeToken GetChangeToken()
        {
            TokenSource = new CancellationTokenSource();
            return new CancellationChangeToken(TokenSource.Token);
        }
    }

    
    internal class ModuloApplicationPart
    {
        Microsoft.AspNetCore.Mvc.ApplicationParts.ApplicationPartManager ApplicationPart { get; set; }


        public ModuloApplicationPart(Microsoft.AspNetCore.Mvc.ApplicationParts.ApplicationPartManager applicationPart)
        {
            ApplicationPart = applicationPart;

            TypeProvider.Provider.OnUpdate += Provider_OnUpdate;
            TypeProvider.Provider.OnRegister += Provider_OnRegister;
            TypeProvider.Provider.OnRemove += Provider_OnRemove;

            Init();
        }

        private void Init()
        {
            var assm = Assembly.GetExecutingAssembly().GetTypes().Where(type => typeof(Controller).IsAssignableFrom(type)
            || typeof(Propeus.Modulo.Hosting.ModuloController).IsAssignableFrom(type));

            foreach (var controller in TypeProvider.Provider.ObterModuoController())
            {
                if (!assm.Contains(controller))
                {
                    Provider_OnRegister(controller);
                }
                else
                {

                }
            }

        }

        private void Provider_OnRemove(Type novo)
        {
            if (!novo.Name.Contains("Controller"))
                return;

            ApplicationPart appOld = null;
            foreach (ApplicationPart appApplicationPart in this.ApplicationPart.ApplicationParts)
            {
                if (appApplicationPart.Name == novo.Name)
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

        private void Provider_OnRegister(Type novo)
        {
            if (!novo.Name.Contains("Controller"))
                return;

            this.ApplicationPart.ApplicationParts.Add(new AssemblyPart(novo.Assembly));
            ModuleActionDescriptorChangeProvider.Instance.TokenSource?.Cancel();
        }

        private void Provider_OnUpdate(Type antigo, Type novo)
        {
            if (!novo.Name.Contains("Controller"))
                return;

            ApplicationPart appOld = null;
            foreach (ApplicationPart appApplicationPart in this.ApplicationPart.ApplicationParts)
            {
                if (antigo.Assembly.FullName.Contains(appApplicationPart.Name))
                {
                    appOld = appApplicationPart;
                }
            }

            if (appOld != null)
            {
                this.ApplicationPart.ApplicationParts.Remove(appOld);
            }
            this.ApplicationPart.ApplicationParts.Add(new AssemblyPart(novo.Assembly));
            ModuleActionDescriptorChangeProvider.Instance.TokenSource?.Cancel();
        }

    }
}
