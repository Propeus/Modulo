using System.Runtime.Loader;

using Propeus.Module.Abstract.Attributes;
using Propeus.Module.Abstract.Interfaces;
using Propeus.Module.AssmblyLoadContext.Modules;

namespace Propeus.Module.AssmblyLoadContext.Contracts
{
    [ModuleContract(typeof(AssemblyLoadContextModule))]
    public interface IAssemblyLoadContextContract : IModule
    {
        bool ExistsAssemblyLoadContext(string FullPathAssembly);
        AssemblyLoadContext GetAssemblyLoadContext(string FullPathAssembly);
        bool RegisterAssemblyLoadContext(string FullPathAssembly);
        bool UnregisterAssemblyLoadContext(string FullPathAssembly);
    }
}