﻿using System;

using Propeus.Modulo.Abstrato.Proveders;

using static Propeus.Modulo.Abstrato.Constantes;

namespace Propeus.Modulo.Abstrato.Atributos
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
                throw new ArgumentException(string.Format(PARAMETRO_NULO_OU_VAZIO, nameof(nome)), nameof(nome));
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
                throw new ArgumentNullException(nameof(modulo), string.Format(PARAMETRO_NULO, nameof(modulo)));
            }

            Nome = modulo.Name;
        }

        /// <summary>
        /// Nome do modulo
        /// </summary>
        public string Nome { get; }
        /// <summary>
        /// Tipo do modulo
        /// </summary>
        /// <remarks>
        /// Esta propriedade e opcional e sera preenchida somente quando o tipo for informado no construtor do atributo
        /// </remarks>
        public Type Tipo => TypeProvider.Get(Nome);
    }
}
