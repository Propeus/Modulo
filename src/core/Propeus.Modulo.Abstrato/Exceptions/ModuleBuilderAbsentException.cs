using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Propeus.Modulo.Abstrato.Exceptions
{
    /// <summary>
    /// Exceção para quando o modulo nao possuir nenhum construtor publico
    /// </summary>
    [Serializable]
    public class ModuleBuilderAbsentException : ModuleException
    {

        ///<inheritdoc/>
        public ModuleBuilderAbsentException(string message) : base(message)
        {
        }
        ///<inheritdoc/>
        [ExcludeFromCodeCoverage(Justification ="Impelementacao do Serializable Pattern")]
        protected ModuleBuilderAbsentException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}