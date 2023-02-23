using System;
using System.Collections.Generic;
using System.Reflection;

namespace Propeus.Modulo.Abstrato.Interfaces
{
    /// <summary>
    /// Interface de modelo para detalhar informações sobre o modulo
    /// </summary>
    public interface IModuloInformacao : IBaseModelo
    {
        IModuloTipo this[string nome] { get; set; }

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
        /// ModuloInformacao mapeados do assembly
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
        /// Informa a quantidade de modulos disponiveis dentro de uma DLL
        /// </summary>
        int ModulosDescobertos { get; }
        /// <summary>
        /// Informa a quantidade de modulos criados.
        /// </summary>
        int ModulosCarregados { get; }

        void AdicionarContrato(string nomeModulo, Type contrato);
        Type CarregarTipoModulo(string nomeModulo);
        List<Type> ObterContratos(string nomeModulo);
        bool PossuiModulo(string nomeModulo);

        ///// <summary>
        ///// Obtem o tipo do modulo
        ///// </summary>
        ///// <param name="nomeModulo"></param>
        ///// <returns></returns>
        //Type ObterTipoModulo(string nomeModulo);
    }
}
