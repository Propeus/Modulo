using Propeus.Module.Abstract.Attributes;
using Propeus.Module.Abstract.Interfaces;
using Propeus.Module.WatcherDynamicModule.Modules;

namespace Propeus.Module.WatcherDynamicModule.Contracts
{
    [ModuleContract(typeof(BuilderProxyModule))]
    public interface IBuilderProxyContract : IModule
    {
        void BuildModuleProxy(WeakReference<Type> module, List<WeakReference<Type>>? contracts, ref string? currentHash, WeakReference<Type> moduleProxy);
    }
}