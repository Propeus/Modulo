using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Propeus.Modulo.Abstrato.Exceptions
{
    /// <summary>
    /// Excecao para quando o modulo informado nao foi encontrado
    /// </summary>
    [Serializable]
    public class ModuleNotFoundException : ModuleException
    {
        ///<inheritdoc/>
        public ModuleNotFoundException(string message) : base(message) { }

        ///<inheritdoc/>
        [ExcludeFromCodeCoverage(Justification = "Impelementacao do Serializable Pattern")]
        protected ModuleNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}