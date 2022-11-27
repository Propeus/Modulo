using Propeus.Modulo.Modelos.Delegates;

namespace Propeus.Modulo.Modelos.Interfaces
{
    /// <summary>
    /// Interface base para utilização de eventos
    /// </summary>
    public interface IBaseEvento
    {
        /// <summary>
        /// Evento base para qualquer ação não especificado
        /// </summary>
        public event Evento OnEvento;
    }
}
