using System;
using System.Collections.Generic;
using System.Linq;

using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Attributes;
using Propeus.Modulo.Abstrato.Interfaces;

namespace Propeus.Modulo.Dinamico.Modules
{
    /// <summary>
    /// Modulo para definir ouvintes para o <see cref="ModuleWatcherModule"/>
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
            _moduleManager.GetModule<ModuleWatcherModule>().OnLoadModule += onLoadModule;
        }
        /// <summary>
        /// Adiciona evento para escutar eventos de descarregamento de modulo
        /// </summary>
        /// <param name="onUnloadModule"></param>
        public void SetOnUnloadModule(Action<Type> onUnloadModule)
        {
            _moduleManager.GetModule<ModuleWatcherModule>().OnUnloadModule += onUnloadModule;
        }
        /// <summary>
        /// Adiciona evento para escutar eventos de regarregamento de modulo
        /// </summary>
        /// <param name="onRebuildModule"></param>
        public void SetOnRebuildModule(Action<Type> onRebuildModule)
        {
            _moduleManager.GetModule<ModuleWatcherModule>().OnReloadModule += onRebuildModule;
        }

        /// <summary>
        /// Obtem todos tipos de modulos validos
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Type> GetAllModules()
        {
            return _moduleManager.GetModule<ModuleWatcherModule>().GetAllModules().ToList();
        }
    }
}
