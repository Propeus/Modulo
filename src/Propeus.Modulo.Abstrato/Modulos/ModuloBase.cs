using System;
using System.Text;

using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Abstrato.Proveders;

using static Propeus.Modulo.Abstrato.Constantes;

namespace Propeus.Modulo.Abstrato.Modulos
{
    /// <summary>
    /// Classe base para o modulo
    /// </summary>
    public abstract class ModuloBase : ModeloBase, IModulo
    {
        /// <summary>
        /// Inicializa um modulo
        /// </summary>
        /// <param name="instanciaUnica">Informa se a instancia é unica ou multipla</param>
        protected ModuloBase(bool instanciaUnica = false) : base()
        {
            InstanciaUnica = instanciaUnica;
            Nome = GetType().Name;

            InstanciaProvider.Provider.Register(this);
        }

        /// <summary>
        /// Informa se o modulo é instancia unica
        /// </summary>
        public bool InstanciaUnica { get; }

        /// <summary>
        /// Exibe informacoes basicas sobre o modulo
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new(base.ToString());

            _ = sb.AppendLine($"Instancia Unica: {InstanciaUnica}");

            return sb.ToString();
        }
    }
}
