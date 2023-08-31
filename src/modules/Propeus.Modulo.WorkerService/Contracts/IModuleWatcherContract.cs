using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Propeus.Module.Abstract.Attributes;
using Propeus.Module.Abstract.Exceptions;

namespace Propeus.Modulo.WorkerService.Contracts
{
    /// <summary>
    /// Contrato para o modulo de nome 'ModuleWatcherModule'
    /// </summary>
    [ModuleContract("ModuleWatcherModule")]
    public interface IModuleWatcherContract
    {
        /// <summary>
        /// Obtém o tipo real ou proxy com base no nome do tipo do contrato ou nome do tipo real
        /// </summary>
        /// <param name="nameType">Nome do tipo ou contrato</param>
        /// <returns>Retorna o tipo implementado ou o proxy dele</returns>
        Type? this[string nameType] { get; set; }

        /// <summary>
        /// Obtém todos tipos de módulos validos
        /// </summary>
        /// <returns>Lista de tipos de módulos registrados</returns>
        IEnumerable<Type> GetAllModules();
        /// <summary>
        /// Obtém o tipo implementado passando o tipo do contrato
        /// </summary>
        /// <param name="contractType">Tipo do contrato</param>
        /// <returns>Retorna o tipo implementado ou o proxy dele</returns>
        /// <exception cref="ModuleContractInvalidException">Exceção para quando a interface de contrato possui o atributo <see cref="ModuleContractInvalidException"/> invalido</exception>
        /// <exception cref="ModuleContractNotFoundException">Exceção para quando a interface de contrato nao possui o atributo <see cref="ModuleContractAttribute"/></exception>
        Type GetModuleFromContract(Type contractType);

        /// <summary>
        /// Evento para carregamento de um novo modulo
        /// </summary>
        event Action<Type>? OnLoadModule;
        /// <summary>
        /// Evento para recarregamento de um novo modulo
        /// </summary>
        event Action<Type>? OnReloadModule;
        /// <summary>
        /// Evento para descarregamento de um modulo existente
        /// </summary>
        event Action<Type>? OnUnloadModule;
    }
}
