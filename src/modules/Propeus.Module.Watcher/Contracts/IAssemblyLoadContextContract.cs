using Propeus.Module.Abstract.Attributes;
using Propeus.Module.Abstract.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

namespace Propeus.Module.Watcher.Contracts
{
    [ModuleContract("AssemblyLoadContextModule")]
    public interface IAssemblyLoadContextContract : IModule
    {
        bool ExistsAssemblyLoadContext(string FullPathAssembly);
        AssemblyLoadContext GetAssemblyLoadContext(string FullPathAssembly);
        bool RegisterAssemblyLoadContext(string FullPathAssembly);
        bool UnregisterAssemblyLoadContext(string FullPathAssembly);
    }
}
