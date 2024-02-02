using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

using Propeus.Module.Abstract;
using Propeus.Module.Abstract.Attributes;
using Propeus.Module.AssmblyLoadContext.Contracts;

namespace Propeus.Module.AssmblyLoadContext.Modules
{
    /// <summary>
    /// Controla os AssemblyLoadContext gerados pelo ModuleProviderInfo
    /// </summary>
    [Module(Description = "Controla os AssemblyLoadContext", AutoUpdate = false, AutoStartable = false, KeepAlive = true, Singleton = true)]
    public class AssemblyLoadContextModule : BaseModule, IAssemblyLoadContextContract
    {

        Dictionary<string, AssemblyLoadContext> _registry;

        public AssemblyLoadContextModule()
        {
            _registry = new Dictionary<string, AssemblyLoadContext>();

        }

        /// <summary>
        /// Registra um novo AssemblyLoadContext para a dll informada
        /// </summary>
        /// <param name="FullPathAssembly">Caminho absoluto da DLL</param>
        /// <returns>Verdadeiro se for registrado com sucesso. Caso contrario retorna falso</returns>
        public bool RegisterAssemblyLoadContext(string FullPathAssembly)
        {
            if (_registry.ContainsKey(FullPathAssembly))
            {
                return false;
            }

            AssemblyLoadContext? _assemblyLoadContext = new AssemblyLoadContext(FullPathAssembly, true);
            using (MemoryStream ms = LoadModuleStream(FullPathAssembly))
            {
                _assemblyLoadContext.LoadFromStream(ms);
            }
            _registry.Add(FullPathAssembly, _assemblyLoadContext);
            return true;
        }
        /// <summary>
        /// Remove o AssemblyLoadContext do registro.
        /// </summary>
        /// <param name="FullPathAssembly">Caminho absoluto da DLL</param>
        /// <returns>Verdadeiro se for removido com sucesso. Caso contrario retorna falso</returns>
        public bool UnregisterAssemblyLoadContext(string FullPathAssembly)
        {

            if (_registry.ContainsKey(FullPathAssembly))
            {
                AssemblyLoadContext? _assemblyLoadContext = _registry[FullPathAssembly];
                _assemblyLoadContext.Unload();
                _registry.Remove(FullPathAssembly);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Obtém o AssemblyLoadContext do registro.
        /// </summary>
        /// <param name="FullPathAssembly">Caminho absoluto da DLL</param>
        /// <returns>Retorna o AssemblyLoadContext caso encontrado</returns>
        /// <exception cref="KeyNotFoundException">Caso nao seja encontrado no registro</exception>
        public AssemblyLoadContext GetAssemblyLoadContext(string FullPathAssembly)
        {
            if (_registry.ContainsKey(FullPathAssembly)) { return _registry[FullPathAssembly]; }

            throw new KeyNotFoundException();
        }
        /// <summary>
        /// Verifica se existe o AssemblyLoadContext no registro.
        /// </summary>
        /// <param name="FullPathAssembly">Caminho absoluto da DLL</param>
        /// <returns>Retorna verdadeiro caso exista</returns>
        public bool ExistsAssemblyLoadContext(string FullPathAssembly)
        {
            return _registry.ContainsKey(FullPathAssembly);
        }

        private MemoryStream LoadModuleStream(string modulePath)
        {
            MemoryStream ms = new MemoryStream();
            for (int i = 0; ; i++)
            {
                try
                {
                    using (FileStream arquivo = new FileStream(modulePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        using (BinaryReader binario = new BinaryReader(arquivo))
                        {
                            binario.BaseStream.CopyTo(ms);
                            binario.Close();
                        }
                        arquivo.Close();
                    }
                    break;
                }
                catch (IOException)
                {

                    if (i > 3)
                    {
                        throw;
                    }
                    else
                    {
                        Task.Delay(TimeSpan.FromSeconds(1)).Wait();
                    }

                }
            }
            _ = ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }
    }
}
