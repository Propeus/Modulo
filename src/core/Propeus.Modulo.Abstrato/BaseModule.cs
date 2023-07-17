using System.Text;

using Propeus.Modulo.Abstrato.Interfaces;

namespace Propeus.Modulo.Abstrato
{
    /// <summary>
    /// Classe base para o modulo
    /// </summary>
    public abstract class BaseModule : BaseModel, IModule
    {
        /// <summary>
        /// Inicializa um modulo
        /// </summary>
        /// <param name="isSingleInstance">Informa se a instancia é unica ou multipla</param>
        protected BaseModule(bool isSingleInstance = false) : base()
        {
            IsSingleInstance = isSingleInstance;
            Name = GetType().Name;


        }

        /// <summary>
        /// Informa se o modulo é instancia unica
        /// </summary>
        public bool IsSingleInstance { get; }

        /// <summary>
        /// Exibe informacoes basicas sobre o modulo
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new(base.ToString());

            _ = sb.AppendLine($"Instancia Unica: {IsSingleInstance}");

            return sb.ToString();
        }
    }
}
