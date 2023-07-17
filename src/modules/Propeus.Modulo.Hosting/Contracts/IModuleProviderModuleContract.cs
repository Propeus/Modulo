using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Propeus.Modulo.Abstrato.Attributes;
using Propeus.Modulo.Abstrato.Interfaces;

namespace Propeus.Modulo.Hosting.Contracts
{
    [ModuleContract("ListenerModule")]
    public interface IModuleProviderModuleContract : IModule
    {
        /// <summary>
        /// Adiciona evento para escutar eventos de carregamento de modulo
        /// </summary>
        /// <param name="onLoadModule"></param>
        public void SetOnLoadModule(Action<Type> onLoadModule);
        /// <summary>
        /// Adiciona evento para escutar eventos de descarregamento de modulo
        /// </summary>
        /// <param name="onUnloadModule"></param>
        public void SetOnUnloadModule(Action<Type> onUnloadModule);
        /// <summary>
        /// Adiciona evento para escutar eventos de regarregamento de modulo
        /// </summary>
        /// <param name="onRebuildModule"></param>
        public void SetOnRebuildModule(Action<Type> onRebuildModule);

        /// <summary>
        /// Obtem todos tipos de modulos validos
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Type> GetAllModules();
    }
}
