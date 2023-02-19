using System.Reflection;

namespace Propeus.Modulo.Abstrato.Interfaces
{
    /// <summary>
    /// Interface de modelo para detalhar informações sobre o modulo
    /// </summary>
    public interface IModuloInformacao : IBaseModelo
    {
        /// <summary>
        /// Concatenação da versão 
        /// </summary>
        int NumeroVersaoAssembly { get; }
        /// <summary>
        /// Assembly a qual o modulo pertence
        /// </summary>
        Assembly Assembly { get; }
        /// <summary>
        /// Informações sobre o assembly do modulo
        /// </summary>
        AssemblyName AssemblyName { get; }
        /// <summary>
        /// Modulos mapeados do assembly
        /// </summary>
        Dictionary<string, IModuloTipo> Modulos { get; }
        /// <summary>
        /// Caminho do modulo em disco
        /// </summary>
        string Caminho { get; }
        /// <summary>
        /// Hash obtido de <see cref="ModuloBinario"/>
        /// </summary>
        string Hash { get; }

        /// <summary>
        /// Obtem o tipo do modulo
        /// </summary>
        /// <param name="nomeModulo"></param>
        /// <returns></returns>
        Type ObterTipoModulo(string nomeModulo);
    }
}
