using System;

namespace Propeus.Module.Abstract.Interfaces
{
    /// <summary>
    /// Modelo base para todos os objetos do projeto
    /// </summary>
    public interface IBaseModel : IDisposable
    {

        /// <summary>
        /// Version do modelo
        /// </summary>
        string Version { get; }
        /// <summary>
        /// Representa o estado do objeto.
        /// </summary>
        State State { get; }
        /// <summary>
        /// Representação amigavel do ojeto. 
        /// <para>
        /// Caso seja nulo o nome da classe herdado será informado na propriedade.
        /// </para>
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Representação alfanumerica e unica do objeto.
        /// </summary>
        string Id { get; }
        /// <summary>
        /// <see cref="Guid"/> do <see cref="System.Reflection.Assembly" /> atual
        /// </summary>
        string ManifestId { get; }
    }
}
