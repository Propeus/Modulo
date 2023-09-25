using Microsoft.Extensions.Primitives;

using Propeus.Module.Hosting.Contracts;

namespace Propeus.Module.Hosting.ViewEngine
{
    internal class ModuloChangeToken : IChangeToken
    {
        private readonly string _name;

        public ModuloChangeToken(ModuloController moduloController, IModuleProviderModuleContract moduleProvider)
        {
            _name = moduloController.GetType().Name;
            moduleProvider.SetOnLoadModule((moduleType) =>
            {
                HasChanged = moduleType.Name.Contains("Controller") && _name.Contains(moduleType.Name);
            });
            moduleProvider.SetOnUnloadModule((moduleType) =>
            {
                HasChanged = moduleType.Name.Contains("Controller") && _name.Contains(moduleType.Name);
            });
            moduleProvider.SetOnRebuildModule((moduleType) =>
            {
                HasChanged = moduleType.Name.Contains("Controller") && _name.Contains(moduleType.Name);
            });

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
