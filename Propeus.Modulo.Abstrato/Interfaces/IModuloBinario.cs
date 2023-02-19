using System;
using System.Collections.Generic;
using System.IO;

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
        /// Modulos mapeados do binario
        /// </summary>
        ICollection<IModuloInformacao> Modulos { get; }
        /// <summary>
        /// "Ponteiro" onde se encontra o binario em memoria
        /// </summary>
        Span<byte> Referencia { get; }

        /// <summary>
        /// Registra novas informações de modulo.
        /// </summary>
        /// <param name="moduloInformarcao"></param>
        void Registrar(IModuloInformacao moduloInformarcao);
        /// <summary>
        /// Remove as informações de modulo.
        /// </summary>
        /// <param name="moduloInformarcao"></param>
        void Remover(IModuloInformacao moduloInformarcao);
    }
}
