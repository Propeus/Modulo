using System;

namespace Propeus.Module.Abstract.Interfaces
{
    /// <summary>
    /// Interface para criar instancias de moduleType passando argumentos
    /// </summary>
    public interface IModuleManagerArguments : IModuleManager
    {
        /// <summary>
        /// Cria uma nova instancia do moduleType <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Qualquer tipo herdado de <see cref="IModule"/></typeparam>
        /// <param name="args">Qualquer argumento necessário para o moduleType </param>
        /// <returns></returns>
        T CreateModule<T>(object[] args) where T : IModule;
        /// <summary>
        /// Cria uma nova instancia do moduleType usando o tipo do parametro <paramref name="moduleType"/>
        /// </summary>
        /// <param name="moduleType">ModuleType do moduleType</param>
        /// <param name="args">Qualquer argumento necessário para o moduleType </param>
        /// <returns><see cref="IModule"/></returns>
        IModule CreateModule(Type moduleType, object[] args);
        /// <summary>
        /// Cria uma nova instancia do moduleType buscando o tipo pelo nome
        /// </summary>
        /// <param name="moduleName">ModuleName do moduleType</param>
        /// <param name="args">Qualquer argumento necessário para o moduleType </param>
        /// <returns><see cref="IModule"/></returns>
        IModule CreateModule(string moduleName, object[] args);
    }
}
