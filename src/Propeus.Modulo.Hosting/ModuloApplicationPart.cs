using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Proveders;

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


    internal class ModuloApplicationPart
    {
        Microsoft.AspNetCore.Mvc.ApplicationParts.ApplicationPartManager ApplicationPart { get; set; }

        public ModuloApplicationPart(Microsoft.AspNetCore.Mvc.ApplicationParts.ApplicationPartManager applicationPart)
        {
            ApplicationPart = applicationPart;
            LoadTypes();
            EventoProvider.RegistrarOuvinteInformacao(OnTypeLoadUnload);
        }

        private void LoadTypes()
        {
            foreach (var item in ModuleProvider.Provider.GetTypes())
            {
                OnTypeLoadUnload(item, "load", null);
            }
        }

        private void OnTypeLoadUnload(Type fonte, string mensagem, Exception exception)
        {
            switch (mensagem)
            {
                case "load":
                    if (!fonte.Name.Contains("Controller"))
                        return;
                    var app = new AssemblyPart(fonte.Assembly);

                    if (!this.ApplicationPart.ApplicationParts.Any(x => x.Name == app.Name))
                    {
                        this.ApplicationPart.ApplicationParts.Add(app);
                        ModuleActionDescriptorChangeProvider.Instance.HasChanged = true;
                        ModuleActionDescriptorChangeProvider.Instance.TokenSource?.Cancel();
                    }


                    //this.ApplicationPart.ApplicationParts.Add(new EmbeddedFileAssemblyPart())
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
