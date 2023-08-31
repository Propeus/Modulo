using System.Runtime.CompilerServices;
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
        protected BaseModule() : base()
        {
            Name = GetType().Name;
        }



        /// <summary>
        /// Exibe informacoes basicas sobre o modulo
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new(base.ToString());

            _ = sb.AppendLine($"Nome: {Name}");

            return sb.ToString();
        }
    }
}
