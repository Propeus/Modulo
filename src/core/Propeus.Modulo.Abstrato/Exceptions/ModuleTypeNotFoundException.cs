using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Propeus.Modulo.Abstrato.Exceptions
{
    /// <summary>
    /// Excecao para quando o tipo do modulo informado nao for encontrado no Assembly
    /// </summary>
    [Serializable]
    public class ModuleTypeNotFoundException : ModuleException
    {
        ///<inheritdoc/>
        public ModuleTypeNotFoundException(string message) : base(message) { }

        ///<inheritdoc/>
        [ExcludeFromCodeCoverage(Justification = "Impelementacao do Serializable Pattern")]
        protected ModuleTypeNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}