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
        /// <summary>
        /// Retorna o <see cref="IModuloTipo"/> com base no <paramref name="nome"/> do modulo
        /// </summary>
        /// <param name="nome">Nome do modulo</param>
        /// <returns><see cref="IModuloTipo"/></returns>
        IModuloTipo this[string nome] { get; set; }

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
        /// Informa a quantidade de modulos disponiveis dentro de uma DLL
        /// </summary>
        int ModulosDescobertos { get; }
        /// <summary>
        /// Informa a quantidade de modulos criados.
        /// </summary>
        int ModulosCarregados { get; }

        /// <summary>
        /// Atrela uma interface de contrato ao modulo
        /// </summary>
        /// <param name="nomeModulo">Nome do modulo que ira receber o contrato</param>
        /// <param name="contrato">O tipo da interface de contrato</param>
        void AdicionarContrato(string nomeModulo, Type contrato);
        /// <summary>
        /// Obtem o <see cref="Type"/> do modulo informado
        /// </summary>
        /// <param name="nomeModulo">Nome do modulo a ser obtido o <see cref="Type"/></param>
        /// <returns><see cref="Type"/></returns>
        Type CarregarTipoModulo(string nomeModulo);
        /// <summary>
        /// Obtem as interfaces de contratos atrelados ao modulo
        /// </summary>
        /// <param name="nomeModulo">Nome do modulo a ser buscado as interfaces de contrato</param>
        /// <returns><see cref="List{Type}"/></returns>
        List<Type> ObterContratos(string nomeModulo);
        /// <summary>
        /// Indica se o modulo informado esta prensente
        /// </summary>
        /// <param name="nomeModulo">Nome do modulo</param>
        /// <returns>Retorna <see langword="true"/> caso ache o modulo, caso contrario retorna <see langword="false"/></returns>
        bool PossuiModulo(string nomeModulo);


    }
}
