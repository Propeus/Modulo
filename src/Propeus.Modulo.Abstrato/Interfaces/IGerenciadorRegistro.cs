namespace Propeus.Modulo.Abstrato.Interfaces
{
    /// <summary>
    /// Modelo base para criação de gerenciadores
    /// </summary>
    public interface IGerenciadorRegistro
    {
        /// <summary>
        /// Registra o modulo no gerenciador
        /// </summary>
        /// <param name="modulo">Instancia do modulo</param>
        void Registrar(IModulo modulo);
    }
}

