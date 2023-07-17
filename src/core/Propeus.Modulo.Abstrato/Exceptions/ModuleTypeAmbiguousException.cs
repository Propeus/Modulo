﻿using System;
using System.Runtime.Serialization;

namespace Propeus.Modulo.Abstrato.Exceptions
{
    /// <summary>
    /// Excecao para quando for encontrado mais de um modulo de mesmo nome
    /// </summary>
    [Serializable]
    public class ModuleTypeAmbiguousException : ModuleException
    {

        ///<inheritdoc/>
        public ModuleTypeAmbiguousException(string message) : base(message) { }

        ///<inheritdoc/>
        protected ModuleTypeAmbiguousException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}