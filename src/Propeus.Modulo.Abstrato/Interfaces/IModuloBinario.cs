namespace Propeus.Modulo.Abstrato.Interfaces
{
    /// <summary>
    /// Interface de modelo para obter informações do binario do modulo
    /// </summary>
    public interface IModuloBinario : IBaseModelo
    {
        /// <summary>
        /// Caminho onde se encontra o binario do modulo
        /// </summary>
        string Caminho { get; }
        /// <summary>
        /// Hash do binario do modulo
        /// </summary>
        string Hash { get; }
        /// <summary>
        /// Conteudo do modulo armazenado em memoria
        /// </summary>
        MemoryStream Memoria { get; }
        /// <summary>
        /// ModuloInformacao mapeados do binario
        /// </summary>
        IModuloInformacao ModuloInformacao { get; }
        /// <summary>
        /// "Ponteiro" onde se encontra o binario em memoria
        /// </summary>
        Span<byte> Referencia { get; }
        bool BinarioValido { get; }

    }
}
