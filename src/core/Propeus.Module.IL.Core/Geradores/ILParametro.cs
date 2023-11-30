namespace Propeus.Module.IL.Geradores
{
    internal static partial class Constantes
    {
        public const string CONST_NME_PARAMETRO = "IL_Gerador_{0}_Parametro_";
        public static string GerarNomeParametro(string nomeMetodo)
        {
            return Constantes.GerarNome(string.Format(CONST_NME_PARAMETRO, nomeMetodo));
        }
    }

    internal class ILParametro
    {
        /// <summary>
        /// Constrói uma instancia de parâmetro IL
        /// </summary>
        /// <param name="nomeMetodo">Nome do método que irá possuir o parâmetro atual</param>
        /// <param name="tipo">Tipo do parâmetro</param>
        /// <param name="opcional">Informa se o parâmetro é opcional</param>
        /// <param name="defaultValue">Valor padrão do parâmetro</param>
        /// <param name="nome">Nome do parâmetro</param>
        public ILParametro(string nomeMetodo
        , Type tipo
        , bool opcional = false
        , object? defaultValue = null
        , string nome = Constantes.CONST_NME_PARAMETRO)
        {
            if (nome == Constantes.CONST_NME_PARAMETRO)
            {
                nome = Constantes.GerarNomeParametro(nomeMetodo);
            }
            tipo ??= typeof(object);

            Nome = nome;
            Tipo = tipo;
            Opcional = opcional;
            DefaultValue = defaultValue;
        }

        /// <summary>
        /// Constrói uma instancia de parâmetro IL somente com o tipo
        /// </summary>
        /// <param name="tipo">Tipo do parametro</param>
        public ILParametro(Type tipo) : this(string.Empty, tipo)
        {

        }


        public Type Tipo { get; private set; }
        public bool Opcional { get; private set; }
        public object? DefaultValue { get; private set; }
        public string Nome { get; private set; }
        public int Indice { get; internal set; }

        public static explicit operator Type(ILParametro obj)
        {
            return obj.ToType();
        }

        public Type ToType()
        {
            return Tipo;
        }
    }
}
