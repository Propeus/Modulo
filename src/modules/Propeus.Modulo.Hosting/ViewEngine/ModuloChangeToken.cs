using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Primitives;

using Propeus.Modulo.Abstrato.Proveders;

namespace Propeus.Modulo.Hosting.ViewEngine
{
    internal class ModuloChangeToken : IChangeToken
    {
        private string _name;

        public ModuloChangeToken(ModuloController moduloController)
        {
            _name = moduloController.GetType().Name;
            EventoProvider.RegistrarOuvinteInformacao(OnTypeLoadUnload);
        }

        IDisposable IChangeToken.RegisterChangeCallback(Action<object> callback, object state) => EmptyDisposable.Instance;

        public bool HasChanged { get; private set; }
        public bool ActiveChangeCallbacks => false;

        private void OnTypeLoadUnload(Type fonte, string mensagem, Exception exception)
        {
            switch (mensagem)
            {
                case "load":
                    HasChanged = fonte.Name.Contains("Controller") && _name.Contains(fonte.Name);
                    break;
                case "unload":
                    HasChanged = fonte.Name.Contains("Controller") && _name.Contains(fonte.Name);
                    break;
            }
        }
    }

    internal class EmptyDisposable : IDisposable
    {
        public static EmptyDisposable Instance { get; } = new EmptyDisposable();
        private EmptyDisposable() { }
        public void Dispose() { }
    }
}
