using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Abstrato;
using System;
using System.Text;

using static Propeus.Modulo.Abstrato.Constante;

namespace Propeus.Modulo.Core
{
    /// <summary>
    /// Classe base para o modulo
    /// </summary>
    public class ModuloBase : BaseModelo, IModulo
    {
        /// <summary>
        /// Inicia um modulo com um gerenciador
        /// </summary>
        /// <param name="gerenciador">Gerenciador que irá controlar o modulo</param>
        /// <param name="instanciaUnica">Informa se a instancia é unica ou multipla</param>
        public ModuloBase(IGerenciador gerenciador, bool instanciaUnica = false)
        {
            Gerenciador = gerenciador ?? throw new System.ArgumentNullException(nameof(gerenciador));
            InstanciaUnica = instanciaUnica;
            if (gerenciador is IGerenciadorRegistro)
            {
                (gerenciador as IGerenciadorRegistro).Registrar(this);
            }
            else
            {
                throw new ArgumentException(string.Format(GerenciadorInvalido, nameof(gerenciador)), nameof(gerenciador));
            }

            Nome = GetType().Name;
        }

        /// <summary>
        /// Informa se o modulo é instancia unica
        /// </summary>
        public bool InstanciaUnica { get; }

        /// <summary>
        /// Gerenciador que está manipulando o modulo
        /// </summary>
        public IGerenciador Gerenciador { get; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(base.ToString());

            sb.AppendLine($"---Modulo---");
            sb.AppendLine($"Instancia Unica: {InstanciaUnica}");
            sb.AppendLine($"---Modulo---");
            sb.AppendLine();

            return sb.ToString();
        }
    }
}
