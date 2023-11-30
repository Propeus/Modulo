using Propeus.Module.IL.Core.Enums;
using Propeus.Module.IL.Core.Geradores;
using Propeus.Module.IL.Geradores;

namespace Propeus.Module.IL.Core.API
{
    /// <summary>
    /// Api para montagem de classe via IL
    /// </summary>
    internal static class ClassApi
    {

        /// <summary>
        /// Cria um novo método dentro da classe informada
        /// </summary>
        /// <param name="iLClasse">Classe onde será gerado o método</param>
        /// <param name="modificadorAcesso">_accessors do método</param>
        /// <param name="tipoRetorno">Tipo do retorno do método</param>
        /// <param name="nomeMetodo">ClassName do método</param>
        /// <param name="parametros">Parameters do método</param>
        internal static void CreateMethod(ILClasse iLClasse, Token[] modificadorAcesso, Type tipoRetorno, string nomeMetodo = Constantes.CONST_NME_METODO, ILParametro[]? parametros = null)
        {
            iLClasse.Methods.Add(new ILMethodComponent(iLClasse.Proxy.CreateScope(), modificadorAcesso, tipoRetorno, nomeMetodo, parametros));
        }
        /// <summary>
        /// Cria um novo campo dentro da classe informada
        /// </summary>
        /// <param name="iLClasse">Classe onde será gerado o campo</param>
        /// <param name="modificadorAcesso">Modificador de acesso do campo</param>
        /// <param name="tipoRetorno">Tipo do retorno do campo</param>
        /// <param name="nomeCampo">ClassName do campo</param>
        internal static void CreateField(ILClasse iLClasse, Token[] modificadorAcesso, Type tipoRetorno, string nomeCampo = Constantes.CONST_NME_CAMPO)
        {
            iLClasse.Fields.Add(new ILFieldComponent(iLClasse.Proxy.CreateScope(), modificadorAcesso, tipoRetorno, nomeCampo));
        }
        /// <summary>
        /// Cria uma nova propriedade dentro da classe informada
        /// </summary>
        /// <param name="iLClasse">Classe onde será gerado o campo</param>
        /// <param name="tipoRetorno">Tipo do retorno do campo</param>
        /// <param name="nomePropriedade">ClassName da propriedade</param>
        /// <param name="parametros"></param>
        internal static void CreateProperty(ILClasse iLClasse, Type tipoRetorno, string nomePropriedade = Constantes.CONST_NME_PROPRIEDADE, Type[]? parametros = null)
        {
            iLClasse.Properties.Add(new ILPropertyComponent(iLClasse.Proxy.CreateScope(), Array.Empty<Token>(), tipoRetorno, nomePropriedade, parametros.Select(parametro => new ILParametro(parametro)).ToArray()));
        }
    }
}
