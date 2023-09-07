namespace Propeus.Module.Abstract
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
        /// O atributo não possui um nome de modulo ou tipo
        /// </summary>
        public const string ERRO_ATRIBUTO_MODULO_CONTRATO_INVALIDO = "O atributo deve possuir um nome ou tipo do modulo";
        /// <summary>
        /// O atributo não possui um nome de modulo ou tipo
        /// </summary>
        public const string ERRO_ATRIBUTO_MODULO_CONTRATO_OMISSO = "O atributo ModuleContractAttribute não foi encontrado no tipo '{0}'";

        /// <summary>
        /// Nenhum construtor publico foi encontrado
        /// </summary>
        public const string ERRO_CONSTRUTOR_NAO_ENCONTRADO = "Não foi possivel encontrar um construtor publico para o tipo '{0}'";

        /// <summary>
        /// O modulo foi descartado
        /// </summary>
        public const string ERRO_MODULO_ID_DESCARTADO = "Module '{0}' foi descartado.";
        /// <summary>
        /// O modulo permite somente a instancia unica (singleton)
        /// </summary>
        public const string ERRO_MODULO_INSTANCIA_UNICA = "O modulo '{0}' não pode ser inicializado, pois já existe uma instancia em execução definido como instancia unica";
        /// <summary>
        /// ModuleName do modulo incorreto ou inexistente
        /// </summary>
        public const string ERRO_MODULO_NAO_ENCONTRADO = "Module {0} não encontrado.";
        /// <summary>
        /// Modulos criados fora do gerenciador nao podem ser reiniciados
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
        public const string ERRO_TIPO_NAO_HERDADO = "O tipo '{0}' não é herdado de IModule";
        /// <summary>
        /// A classe atual nao possui o atributo ModuloAttribute
        /// </summary>
        public const string ERRO_TIPO_NAO_MARCADO = "O tipo não esta marcado como modulo";
        /// <summary>
        /// O modulo nao existe 
        /// </summary>
        public const string ERRO_MODULO_ID_NAO_ENCONTRADO = "Module nao encontrado pelo id '{0}'";

        /// <summary>
        /// Justificativa para exclusão de code-coverage para exceções 
        /// </summary>
        internal const string EXECEPTION_CODE_COVERAGE_JUSTIFICATION = "Implementação do Serializable Pattern";
    }
}