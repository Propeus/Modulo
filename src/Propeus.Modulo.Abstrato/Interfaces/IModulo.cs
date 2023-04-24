namespace Propeus.Modulo.Abstrato.Interfaces
{
    /// <summary>
    /// Modelo base para criação de modulos
    /// </summary>
    public interface IModulo : IBaseModelo
    {
        /// <summary>
        /// Informa se o modulo é instancia unica
        /// </summary>
        /// <value><see langword="false"/></value>
        bool InstanciaUnica { get; }
    }
}