using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

using Propeus.Module.Abstract.Attributes;
using Propeus.Module.Abstract.Interfaces;

namespace Propeus.Module.Package.Contracts
{
    //Contrato para carregar e descarregar qualquer assembly que for necessário
    [ModuleContract("AssemblyLoadContextModule")]
    public interface IAssemblyLoadContextContract : IModule
    {
        bool ExistsAssemblyLoadContext(string FullPathAssembly);
        AssemblyLoadContext GetAssemblyLoadContext(string FullPathAssembly);
        bool RegisterAssemblyLoadContext(string FullPathAssembly);
        bool UnregisterAssemblyLoadContext(string FullPathAssembly);
    }
}
