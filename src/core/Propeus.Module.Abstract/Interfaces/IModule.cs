namespace Propeus.Module.Abstract.Interfaces
{
    /// <summary>
    /// Modelo base para criação de modulos
    /// </summary>
    public interface IModule : IBaseModel
    {
        /// <summary>
        /// Metodo para configurar modulo antes de inicializar de fato.
        /// </summary>
        void ConfigureModule();

        /// <summary>
        /// Metodo para inicializar o modulo
        /// </summary>
        void Launch();
    }
}