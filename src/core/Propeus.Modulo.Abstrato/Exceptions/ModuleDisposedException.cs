using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Propeus.Modulo.Abstrato.Exceptions
{
    /// <summary>
    /// Excecao para quando o modulo é descartado pelo <see cref="IDisposable.Dispose"/> ou quando o <see cref="GC"/> coleta o objeto
    /// </summary>
    [Serializable]
    public class ModuleDisposedException : ModuleException
    {
        ///<inheritdoc/>
        public ModuleDisposedException(string message) : base(message) { }

        ///<inheritdoc/>
        [ExcludeFromCodeCoverage(Justification = "Impelementacao do Serializable Pattern")]
        protected ModuleDisposedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}