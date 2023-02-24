using System;
using System.Text;

using Propeus.Modulo.Abstrato.Interfaces;

using static Propeus.Modulo.Compartilhado.Constantes;

namespace Propeus.Modulo.Abstrato
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
            Gerenciador = gerenciador ?? throw new ArgumentNullException(nameof(gerenciador));
            InstanciaUnica = instanciaUnica;
            if (gerenciador is IGerenciadorRegistro)
            {
                (gerenciador as IGerenciadorRegistro).Registrar(this);
            }
            else
            {
                throw new ArgumentException(string.Format(GERENCIADOR_INVALIDO, nameof(gerenciador)), nameof(gerenciador));
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
