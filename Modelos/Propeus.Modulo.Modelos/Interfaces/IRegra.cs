namespace Propeus.Modulo.Modelos.Interfaces
{
    /// <summary>
    /// Interface basica para execução de regras de negocio
    /// </summary>
    public interface IRegra
    {
        /// <summary>
        /// Função basica para execução de regras de negocio
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        bool Executar(params object[] args);

    }
}
