namespace Propeus.Modulo.Abstrato
{
    /// <summary>
    /// Classe para adicionar valores constantes
    /// </summary>
    public static partial class Constantes
    {

        /// <summary>
        /// Metodo que o gerenciador irá chamar para realizar as configurações
        /// </summary>
        public const string METODO_CONFIGURACAO = "CriarConfiguracao";
        /// <summary>
        /// Metodo que o gerenciado deverá realizar a injeção de dependencias.
        /// </summary>
        public const string METODO_INSTANCIA = "CriarInstancia";



        /// <summary>
        /// Nenhum construtor publico foi encontrado
        /// </summary>
        public const string ERRO_CONSTRUTOR_NAO_ENCONTRADO = "Não foi possivel encontrar um construtor";

        /// <summary>
        /// O modulo foi descartado
        /// </summary>
        public const string ERRO_MODULO_ID_DESCARTADO = "Module '{0}' foi descartado.";
        /// <summary>
        /// O modulo permite somente a instancia unica (singleton)
        /// </summary>
        public const string ERRO_MODULO_INSTANCIA_UNICA = "Não é possivel criar uma nova instancia de um modulo de instancia unica";
        /// <summary>
        /// ModuleName do modulo incorreto ou inexistente
        /// </summary>
        public const string ERRO_MODULO_NAO_ENCONTRADO = "Module '{0}' não encontrado.\nVerifique se o nome esta correto na interface de contrato";
        /// <summary>
        /// _modulos criados fora do gerenciador nao podem ser reiniciados
        /// </summary>
        public const string ERRO_MODULO_NEW_REINICIAR = "Não é possivel reiniciar modulos criados fora do gerenciador";

        /// <summary>
        /// Module nao encontrado ou inexistente
        /// </summary>
        public const string ERRO_NOME_MODULO_NAO_ENCONTRADO = "Module '{0}' não encontrado";
        /// <summary>
        /// O tipo informado nao e uma interface ou classe
        /// </summary>
        public const string ERRO_TIPO_INVALIDO = "O tipo deve ser uma classe ou interface.";
        /// <summary>
        /// O tipo informado nao herda de IModule
        /// </summary>
        public const string ERRO_TIPO_NAO_HERDADO = "O tipo não é herdado de IModule";
        /// <summary>
        /// A classe atual nao possui o atributo ModuloAttribute
        /// </summary>
        public const string ERRO_TIPO_NAO_MARCADO = "O tipo não esta marcado como modulo";
        /// <summary>
        /// O modulo nao existe 
        /// </summary>
        public const string ERRO_MODULO_ID_NAO_ENCONTRADO = "Module nao encontrado pelo id '{0}'";

    }
}