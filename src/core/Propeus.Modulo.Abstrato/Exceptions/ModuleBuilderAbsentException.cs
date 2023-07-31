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

        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="moduleType">Tipo do modulo</param>
        public ModuleBuilderAbsentException(Type moduleType) : base(string.Format(Constantes.ERRO_CONSTRUTOR_NAO_ENCONTRADO, moduleType.Name))
        {
        }
        ///<inheritdoc/>
        [ExcludeFromCodeCoverage(Justification = Constantes.EXECEPTION_CODE_COVERAGE_JUSTIFICATION)]
        protected ModuleBuilderAbsentException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}