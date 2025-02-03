namespace Propeus.Module.IL.Core
{
    internal static partial class Constantes
    {
        public const string CONST_NME_PROPRIEDADE = "IL_Gerador_{0}_Propriedade_";

        public const string CONST_NME_PROPRIEDADE_METODO_GET = "get_";
        public const string CONST_NME_PROPRIEDADE_METODO_SET = "set_";

        public const string CONST_NME_CAMPO = "IL_Gerador_{0}_Campo_";
        public const string CONST_NME_CAMPO_PROXY = CONST_NME_CAMPO + "Proxy_";

        public const string CONST_NME_METODO = "IL_Gerador_{0}_Metodo_";

        public const string CONST_NME_CLASSE = "IL_Gerador_Classe_";
        public const string CONST_NME_CLASSE_PROXY = CONST_NME_CLASSE + "Proxy_";

        public const string CONST_NME_NAMESPACE_CLASSE = "Propeus.IL.Classes";
        public const string CONST_NME_NAMESPACE_CLASSE_PROXY = CONST_NME_NAMESPACE_CLASSE + ".ScopeBuilder"; //?? -> Para o meu eu do passado, nao seria 'proxy'? Para o meu eu do futuro, mude se nao der merda

        public const string CONST_NME_DELEGATE = "IL_Gerador_Delegate_";
        public const string CONST_NME_DELEGATE_PROXY = CONST_NME_DELEGATE + "Proxy_";

        public const string CONST_NME_NAMESPACE_DELEGATE = "Propeus.IL.Delegates";
        public const string CONST_NME_NAMESPACE_DELEGATE_PROXY = CONST_NME_NAMESPACE_DELEGATE + ".ScopeBuilder";//Idem

        /// <summary>
        /// Valor para definir ou verificar modulos temporarios
        /// </summary>
        public const string CONST_SUFIXO_CLASSE_TEMPORARIA = "_TEMP";

        public const string CONSTNMEASSEMBLY = "IL_Gerador_Assembly_";
        public const string CONSTNMEMODULO = "IL_Gerador_Modulo_";

        public static string GerarNomeModulo()
        {
            return GerarNome(CONSTNMEMODULO);
        }

        public static string GerarNome(string @const)
        {
            return @const + Guid.NewGuid().ToString().Replace('-', '_');
        }

        public const string CONST_NME_PARAMETRO = "IL_Gerador_{0}_Parametro_";
        public static string GerarNomeParametro(string nomeMetodo)
        {
            return Constantes.GerarNome(string.Format(CONST_NME_PARAMETRO, nomeMetodo));
        }
    }
}
