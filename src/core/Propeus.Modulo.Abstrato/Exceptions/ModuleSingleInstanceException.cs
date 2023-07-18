﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Propeus.Modulo.Abstrato.Exceptions
{
    /// <summary>
    /// Excecao para tentativa de criacao de um novo modulo de instancia unica
    /// </summary>
    [Serializable]
    public class ModuleSingleInstanceException : ModuleException
    {

        ///<inheritdoc/>
        public ModuleSingleInstanceException(string message) : base(message) { }

        ///<inheritdoc/>
        [ExcludeFromCodeCoverage(Justification = "Impelementacao do Serializable Pattern")]
        protected ModuleSingleInstanceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}