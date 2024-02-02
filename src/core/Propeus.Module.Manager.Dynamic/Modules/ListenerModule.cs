﻿using Propeus.Module.Abstract;
using Propeus.Module.Abstract.Attributes;
using Propeus.Module.Watcher.Contracts;

namespace Propeus.Module.Manager.Dynamic.Modules
{
    /// <summary>
    /// Module para definir ouvintes para o <see cref="IModuleWatcherContract"/>
    /// </summary>
    [Module(Description = "Module para definir ouvintes para o IModuleWatcherContract")]
    public class ListenerModule : BaseModule
    {
        private readonly IModuleWatcherContract _moduleManager;

        /// <summary>
        /// ObjectBuilder padrão
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