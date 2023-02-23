namespace Propeus.Modulo.Abstrato.Interfaces
{
    /// <summary>
    /// Interface basica para execução de regras de negocio
    /// </summary>
    public interface IRegra
    {
        /// <summary>
        /// Função basica para execução de regras de negocio
        /// </summary>
        /// <param name="args">Argumentos para a regra</param>
        /// <returns>Retorna <see langword="true"/> caso a regra tenha sido executada com sucesso, caso contrario retorna <see langword="false"/></returns>
        bool Executar(params object[] args);

    }
}
