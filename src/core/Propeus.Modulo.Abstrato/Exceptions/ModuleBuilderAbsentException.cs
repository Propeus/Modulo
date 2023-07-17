using System;
using System.Runtime.Serialization;

namespace Propeus.Modulo.Abstrato.Exceptions
{
    /// <summary>
    /// Excecao para quando o modulo nao possuir nenhum construtor publico
    /// </summary>
    [Serializable]
    public class ModuleBuilderAbsentException : ModuleException
    {

        ///<inheritdoc/>
        public ModuleBuilderAbsentException(string message) : base(message)
        {
        }
        ///<inheritdoc/>
        protected ModuleBuilderAbsentException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}