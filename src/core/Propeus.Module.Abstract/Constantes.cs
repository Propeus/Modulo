namespace Propeus.Module.Abstract
{
    /// <summary>
    /// Classe para adicionar valores constantes
    /// </summary>
    public static partial class Constantes
    {
        #region Erros
        #region Atributos
        /// <summary>
        /// O atributo não possui um nome de modulo ou tipo
        /// </summary>
        public const string ERRO_ATRIBUTO_MODULO_CONTRATO_INVALIDO = "O atributo deve possuir um nome ou tipo do modulo";
        /// <summary>
        /// A interface de contrato pertence ao modulo más não foi mapeado
        /// </summary>
        public const string ERRO_MODULO_CONTRATO_NAO_MAPEADO = "O contrato não foi mapeado para o modulo alvo";
        /// <summary>
        /// O atributo não possui um nome de modulo ou tipo
        /// </summary>
        public const string ERRO_ATRIBUTO_MODULO_CONTRATO_OMISSO = "O atributo ModuleContractAttribute não foi encontrado no tipo '{0}'";
        #endregion

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
        /// O nome do modulo foi escrito errado ou não foi carregado
        /// </summary>
        public const string ERRO_MODULO_NAO_ENCONTRADO = "Module {0} não encontrado.";
        /// <summary>
        /// O modulo nao existe 
        /// </summary>
        public const string ERRO_MODULO_ID_NAO_ENCONTRADO = "Module nao encontrado pelo id '{0}'";
        /// <summary>
        /// Modulos criados fora do gerenciador nao podem ser reiniciados
        /// </summary>
        public const string ERRO_MODULO_NEW_REINICIAR = "Não é possivel reiniciar modulos criados fora do gerenciador";
        /// <summary>
        /// Quando é encontrado mais de um tipo com o mesmo nome
        /// </summary>
        public const string ERRO_MODULO_AMBIGUO = "Há mais de um modulo com o nome {0}. Acesse o dicionário de dados para verificar os tipos encontrados";
        /// <summary>
        /// Quando o gerenciador de modulos é descartado pelo <see cref="IDisposable.Dispose"/>
        /// </summary>
        public const string ERRO_GERENCIADOR_DESCARTADO = "O Gerenciador atual foi descartado.";

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

        #endregion



        /// <summary>
        /// Justificativa para exclusão de code-coverage para exceções 
        /// </summary>
        internal const string EXECEPTION_CODE_COVERAGE_JUSTIFICATION = "Implementação do Serializable Pattern";
        /// <summary>
        /// Justificativa para exclusão de code-coverage para exceções 
        /// </summary>
        public const string IMPLEMENTATION_CODE_COVERAGE_JUSTIFICATION = "Implementação burra, não será testado";


        /// <summary>
        /// Fila para carregamento de modulo
        /// </summary>
        public const string MENSAGERIA_LOAD_MODULE = "GLOBAL::LOAD_MODULE";
        /// <summary>
        /// Fila para recarregamento de modulo
        /// </summary>
        public const string MENSAGERIA_RELOAD_MODULE = "GLOBAL::RELOAD_MODULE";
        /// <summary>
        /// Fila para descarregamento de modulo
        /// </summary>
        public const string MENSAGERIA_UNLOAD_MODULE = "GLOBAL::UNLOAD_MODULE";

        /// <summary>
        /// Valor para definir ou verificar modulos temporarios
        /// </summary>
        public const string STR_TYPE_TEMPORARY = "_TEMP";
    }
}