using Microsoft.Extensions.Primitives;

using Propeus.Module.Hosting.Contracts;

namespace Propeus.Module.Hosting.ViewEngine
{
    internal class ModuloChangeToken : IChangeToken
    {
        private readonly string _name;

        public ModuloChangeToken(ModuloController moduloController, IMessageQueueManagerContract moduleProvider)
        {
            _name = moduloController.GetType().Name;
            moduleProvider.RegisterOnQueue(Propeus.Module.Abstract.Constantes.MENSAGERIA_LOAD_MODULE,Mensageria_ModuleStateChanged);
            moduleProvider.RegisterOnQueue(Propeus.Module.Abstract.Constantes.MENSAGERIA_RELOAD_MODULE,Mensageria_ModuleStateChanged);
            moduleProvider.RegisterOnQueue(Propeus.Module.Abstract.Constantes.MENSAGERIA_UNLOAD_MODULE,Mensageria_ModuleStateChanged);
        }

        private void Mensageria_ModuleStateChanged(object module)
        {
            Type moduleType = module.GetType();
            HasChanged = moduleType.Name.Contains("Controller") && _name.Contains(moduleType.Name);
        }

        IDisposable IChangeToken.RegisterChangeCallback(Action<object> callback, object state)
        {
            return EmptyDisposable.Instance;
        }

        public bool HasChanged { get; private set; }
        public bool ActiveChangeCallbacks => false;

    }

    internal class EmptyDisposable : IDisposable
    {
        public static EmptyDisposable Instance { get; } = new EmptyDisposable();
        private EmptyDisposable() { }
        public void Dispose() { }
    }
}
