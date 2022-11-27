using Propeus.Modulo.Modelos.Resources;
using System;

namespace Propeus.Modulo.Modelos.Atributos
{
    /// <summary>
    /// Atributo de identificação de modulo.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public sealed class ModuloContratoAttribute : Attribute
    {
        /// <summary>
        /// Indica a qual modulo o contrato pertence
        /// </summary>
        /// <param name="nome">Nome do modulo</param>
        public ModuloContratoAttribute(string nome)
        {
            if (string.IsNullOrEmpty(nome))
            {
                throw new ArgumentException(string.Format(MensagensErro.Culture, MensagensErro.ParametroNuloOuVazio, nameof(nome)), nameof(nome));
            }

            Nome = nome;

        }

        /// <summary>
        /// Indica a qual modulo o contrato pertence
        /// </summary>
        /// <param name="modulo">Tipo do modulo</param>
        public ModuloContratoAttribute(Type modulo)
        {

            if (modulo is null)
            {
                throw new ArgumentNullException(nameof(modulo), string.Format(MensagensErro.Culture, MensagensErro.ParametroNulo, nameof(modulo)));
            }

            Nome = modulo.Name;
        }

        /// <summary>
        /// Nome do modulo
        /// </summary>
        public string Nome { get; }

    }
}
