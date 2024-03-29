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
                throw new ArgumentException($"'{nameof(nome)}' não pode ser nulo nem vazio.", nameof(nome));
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
                throw new ArgumentNullException(nameof(modulo));
            }

            Nome = modulo.Name;
            if (ModuleProvider.Provider[modulo.Name] is null)
                ModuleProvider.Provider[modulo.Name] = modulo;
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
        public Type Tipo => ModuleProvider.Provider[Nome];
    }
}
