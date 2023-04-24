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
            EventoProvider.RegistrarOuvinteInformacao(OnTypeLoadUnload);
        }

        private void OnTypeLoadUnload(Type fonte, string mensagem, Exception exception)
        {
            switch (mensagem) {
                case "load":
                    if (!fonte.Name.Contains("Controller"))
                        return;

                    this.ApplicationPart.ApplicationParts.Add(new AssemblyPart(fonte.Assembly));
                    ModuleActionDescriptorChangeProvider.Instance.TokenSource?.Cancel();
                    break;
                case "unload":
                    if (!fonte.Name.Contains("Controller"))
                        return;

                    ApplicationPart appOld = null;
                    foreach (ApplicationPart appApplicationPart in this.ApplicationPart.ApplicationParts)
                    {
                        if (appApplicationPart.Name == fonte.Name)
                        {
                            appOld = appApplicationPart;
                        }
                    }

                    if (appOld != null)
                    {
                        this.ApplicationPart.ApplicationParts.Remove(appOld);
                    }
                    ModuleActionDescriptorChangeProvider.Instance.TokenSource?.Cancel();
                    break;
            }
        }
    }
}
