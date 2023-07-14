using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Abstrato.Modulos;

namespace Propeus.Modulo.Dinamico.Modules
{
    /// <summary>
    /// Modulo para definir ouvintes para o <see cref="ModuleProviderModule"/>
    /// </summary>
    [Module]
    public class ListenerModule : BaseModule
    {
        private readonly IModuleManager _moduleManager;

        public ListenerModule(IModuleManager moduleManager)
        {
            _moduleManager = moduleManager;
        }

        /// <summary>
        /// Adiciona evento para escutar eventos de carregamento de modulo
        /// </summary>
        /// <param name="onLoadModule"></param>
        public void SetOnLoadModule(Action<Type> onLoadModule)
        {
            _moduleManager.GetModule<ModuleProviderModule>().OnLoadModule += onLoadModule;
        }
        /// <summary>
        /// Adiciona evento para escutar eventos de descarregamento de modulo
        /// </summary>
        /// <param name="onUnloadModule"></param>
        public void SetOnUnloadModule(Action<Type> onUnloadModule)
        {
            _moduleManager.GetModule<ModuleProviderModule>().OnUnloadModule += onUnloadModule;
        }
        /// <summary>
        /// Adiciona evento para escutar eventos de regarregamento de modulo
        /// </summary>
        /// <param name="onRebuildModule"></param>
        public void SetOnRebuildModule(Action<Type> onRebuildModule)
        {
            _moduleManager.GetModule<ModuleProviderModule>().OnRebuildModule += onRebuildModule;
        }

        /// <summary>
        /// Obtem todos tipos de modulos validos
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Type> GetAllModules()
        {
            return _moduleManager.GetModule<ModuleProviderModule>().GetAllModules().ToList();
        }
    }
}
