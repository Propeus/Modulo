using System;
using System.Collections.Generic;

using Propeus.Module.Watcher.Contracts;
using Propeus.Module.Abstract;
using Propeus.Module.Abstract.Attributes;

namespace Propeus.Modulo.Dinamico.Modules
{
    /// <summary>
    /// Modulo para definir ouvintes para o <see cref="IModuleWatcherContract"/>
    /// </summary>
    [Module]
    public class ListenerModule : BaseModule
    {
        private readonly IModuleWatcherContract _moduleManager;

        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="moduleManager">Qualquer gerenciador de modulo</param>
        public ListenerModule(IModuleWatcherContract moduleManager)
        {
            _moduleManager = moduleManager;
        }

        /// <summary>
        /// Adiciona evento para escutar eventos de carregamento de modulo
        /// </summary>
        /// <param name="onLoadModule"></param>
        public void SetOnLoadModule(Action<Type> onLoadModule)
        {
            _moduleManager.OnLoadModule += onLoadModule;
        }
        /// <summary>
        /// Adiciona evento para escutar eventos de descarregamento de modulo
        /// </summary>
        /// <param name="onUnloadModule"></param>
        public void SetOnUnloadModule(Action<Type> onUnloadModule)
        {
            _moduleManager.OnUnloadModule += onUnloadModule;
        }
        /// <summary>
        /// Adiciona evento para escutar eventos de regarregamento de modulo
        /// </summary>
        /// <param name="onRebuildModule"></param>
        public void SetOnRebuildModule(Action<Type> onRebuildModule)
        {
            _moduleManager.OnReloadModule += onRebuildModule;
        }

        /// <summary>
        /// Obtem todos tipos de modulos validos
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Type> GetAllModules()
        {
            return _moduleManager.GetAllModules();
        }
    }
}
