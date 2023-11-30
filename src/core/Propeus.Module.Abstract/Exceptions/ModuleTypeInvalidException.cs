using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Propeus.Module.Abstract.Exceptions
{
    /// <summary>
    /// Excecao para quando o modulo nao seguir os padroes de implementacao
    /// </summary>
    [Serializable]
    public class ModuleTypeInvalidException : ModuleException
    {
        ///<inheritdoc/>
        public ModuleTypeInvalidException(string message) : base(message) { }

        ///<inheritdoc/>
        [ExcludeFromCodeCoverage(Justification = "Impelementacao do Serializable Pattern")]
        protected ModuleTypeInvalidException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// Excecao para quando o modulo nao seguir os padroes de implementacao
        /// </summary>
        [Serializable]
        public class TypeModuleNotInheritedException : ModuleTypeInvalidException
        {
            ///<inheritdoc/>
            public TypeModuleNotInheritedException(Type moduleType) : base(string.Format(Constantes.ERRO_TIPO_NAO_HERDADO, moduleType.Name)) { }

            ///<inheritdoc/>
            [ExcludeFromCodeCoverage(Justification = Constantes.EXECEPTION_CODE_COVERAGE_JUSTIFICATION)]
            protected TypeModuleNotInheritedException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }

        /// <summary>
        /// Excecao para quando o modulo nao seguir os padroes de implementacao
        /// </summary>
        [Serializable]
        public class TypeModuleUnmarkedException : ModuleTypeInvalidException
        {
            /// <summary>
            /// Construtor padrão
            /// </summary>
            /// <param name="moduleType">Tipo do modulo</param>
            public TypeModuleUnmarkedException(Type moduleType) : base(string.Format(Constantes.ERRO_TIPO_NAO_MARCADO, moduleType.Name)) { }

            ///<inheritdoc/>
            [ExcludeFromCodeCoverage(Justification = Constantes.EXECEPTION_CODE_COVERAGE_JUSTIFICATION)]
            protected TypeModuleUnmarkedException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }
    }
}