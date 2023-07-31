using System;
using System.Collections.Generic;
using System.Linq;

using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Attributes;
using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Dinamico.Contracts;

namespace Propeus.Modulo.Dinamico.Modules
{
    /// <summary>
    /// Modulo para definir ouvintes para o <see cref="IModuleWatcherModule"/>
    /// </summary>
    [Module]
    public class ListenerModule : BaseModule
    {
        private readonly IModuleWatcherModule _moduleManager;

        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="moduleManager">Qualquer gerenciador de modulo</param>
        public ListenerModule(IModuleWatcherModule moduleManager)
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
