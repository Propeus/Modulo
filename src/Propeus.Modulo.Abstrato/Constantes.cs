namespace Propeus.Modulo.Abstrato
{
    /// <summary>
    /// Classe para adicionar valores constantes
    /// </summary>
    public static partial class Constantes
    {
        /// <summary>
        /// Constantes para relato de erro em parâmetros com tipo diferente do especificado
        /// </summary>
        public const string ARGUMENTO_NAO_E_DO_TIPO = "O argumento não é do tipo {0}";
        /// <summary>
        ///Constantes para relato de erro em parâmetros com tipo diferente do especificado 
        /// </summary>
        public const string ARGUMENTO_NAO_PODE_SER_DO_TIPO = "O argumento não pode ser do tipo {0}";
        /// <summary>
        /// O argumento não pode ser nulo
        /// </summary>
        public const string ARGUMENTO_NULO = "O argumento não pode ser nulo";
        /// <summary>
        /// Constantes para relato de erro em parâmetros com valores nulos ou vazios
        /// </summary>
        public const string ARGUMENTO_NULO_OU_VAZIO = "O argumento não pode ser nulo ou vazio";
        /// <summary>
        /// Constantes para relato de erro em atributos não encontrados
        /// </summary>
        public const string ATRIBUTO_NAO_ENCONTRADO = "Não foi possível encontrar o atributo {0} no objeto {1}";
        /// <summary>
        /// Constantes para relato de erro em parâmetros com objetos herdados de Attribute
        /// </summary>
        public const string PARAMETRO_ATRIBUTO_INVALIDO = "Não é possível obter atributos de um tipo herdado de Attribute";
        /// <summary>
        /// Constantes para relato de erro em parâmetros não convertidos para o tipo especificado
        /// </summary>
        public const string PARAMETRO_NAO_CONVERTIDO = "Não foi possível converter o parâmetro '{0}' para '{1}'.";
        /// <summary>
        /// Não foi possível converter o tipo '{0}' para {1}.
        /// </summary>
        public const string TIPO_NAO_CONVERTIDO = "Constantes para relato de erro em tipos de conversão explicita";
        /// <summary>
        /// Constantes para relato de erro em tipos não encontrados no assembly
        /// </summary>
        public const string VALOR_PADRAO_NAO_ENCONTRADO = "Não foi possível encontrar o valor padrão do parâmetro '{0}'.";

        /// <summary>
        /// O gerenciador informado nao emplementa a interface <see cref="Interfaces.IGerenciadorRegistro"/>
        /// </summary>
        public const string GERENCIADOR_INVALIDO = "O gerenciador '{0}' não possui a interface IGerenciadorRegistro implentado.";
        /// <summary>
        /// O parametro deve ser preenchido
        /// </summary>
        public const string PARAMETRO_NULO = "O parametro  '{0}' não pode ser nulo";
        /// <summary>
        /// O parametro deve ser preenchido
        /// </summary>
        public const string PARAMETRO_NULO_OU_VAZIO = "O parametro  '{0}' não pode ser nulo ou vazio";

        
        /// <summary>
        /// Metodo que o gerenciador irá chamar para realizar as configurações
        /// </summary>
        public const string METODO_CONFIGURACAO = "CriarConfiguracao";
        /// <summary>
        /// Metodo que o gerenciado deverá realizar a injeção de dependencias.
        /// </summary>
        public const string METODO_INSTANCIA = "CriarInstancia";

        /// <summary>
        /// O parametro e nulo ou vazio
        /// </summary>
        public const string ERRO_ARGUMENTO_NULO_OU_VAZIO = "O parametro '{0}' é nulo ou vazio";
        /// <summary>
        /// O tipo do argumento nao e o esperado
        /// </summary>
        public const string ERRO_ARGUMENTO_TIPO_ESPERADO = "O argumento '{0}' requer o tipo '{1}', o tipo passado foi '{2}'";
        /// <summary>
        /// O construtor nao possui o parametro IGerenciador
        /// </summary>
        public const string ERRO_CONSTRUTOR_IGERENCIADOR_NAO_ENCONTRADO = "O modulo requer um contrutor com o parametro do tipo IGerenciador";
        /// <summary>
        /// Nenhum construtor publico foi encontrado
        /// </summary>
        public const string ERRO_CONSTRUTOR_NAO_ENCONTRADO = "Não foi possivel encontrar um construtor";
        /// <summary>
        /// Nenhum atribuito do tipo ModuloContratoAttribute foi encontrado na interface atual
        /// </summary>
        public const string ERRO_MODULO_CONTRATO_NAO_ENCONTRADO = "Não foi possivel encontrar o atributo de contrato";
        /// <summary>
        /// O modulo foi descartado
        /// </summary>
        public const string ERRO_MODULO_ID_DESCARTADO = "Modulo '{0}' foi descartado.";
        /// <summary>
        /// O modulo permite somente a instancia unica (singleton)
        /// </summary>
        public const string ERRO_MODULO_INSTANCIA_UNICA = "Não é possivel criar uma nova instancia de um modulo de instancia unica";
        /// <summary>
        /// Nome do modulo incorreto ou inexistente
        /// </summary>
        public const string ERRO_MODULO_NAO_ENCONTRADO = "Modulo '{0}' não encontrado.\nVerifique se o nome esta correto na interface de contrato";
        /// <summary>
        /// _modulos criados fora do gerenciador nao podem ser reiniciados
        /// </summary>
        public const string ERRO_MODULO_NEW_REINICIAR = "Não é possivel reiniciar modulos criados fora do gerenciador";
        /// <summary>
        /// O modulo informado ja foi registrado
        /// </summary>
        public const string ERRO_MODULO_REGISTRADO = "O modulo '{0}::{1}' já foi registrado";
        /// <summary>
        /// O modulo informado ja foi registrado em cache
        /// </summary>
        public const string ERRO_MODULO_REGISTRADO_CACHE = "O modulo '{0}::{1}' já foi registrado no cache.";
        /// <summary>
        /// Modulo nao encontrado ou inexistente
        /// </summary>
        public const string ERRO_NOME_MODULO_NAO_ENCONTRADO = "Modulo '{0}' não encontrado";
        /// <summary>
        /// Existem dois ou mais modulos de mesmo nome
        /// </summary>
        public const string ERRO_TIPO_AMBIGUO = "Foi encontrado mais de um tipo do modulo '{0}'.";
        /// <summary>
        /// O tipo informado nao e uma interface ou classe
        /// </summary>
        public const string ERRO_TIPO_INVALIDO = "O tipo deve ser uma classe ou interface.";
        /// <summary>
        /// O tipo informado nao herda de IModulo
        /// </summary>
        public const string ERRO_TIPO_NAO_HERDADO = "O tipo não é herdado de IModulo";
        /// <summary>
        /// A classe atual nao possui o atributo ModuloAttribute
        /// </summary>
        public const string ERRO_TIPO_NAO_MARCADO = "O tipo não esta marcado como modulo";
        /// <summary>
        /// O tipo informado nao pode ser do tipo <see langword="void"/>
        /// </summary>
        public const string ERRO_TIPO_VOID = "O tipo não pode ser void";
        /// <summary>
        /// O modulo nao existe 
        /// </summary>
        public const string ERRO_MODULO_ID_NAO_ENCONTRADO = "Modulo nao encontrado pelo id '{0}'";

    }
}