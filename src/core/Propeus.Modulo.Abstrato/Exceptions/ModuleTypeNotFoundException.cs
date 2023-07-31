using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Propeus.Modulo.Abstrato.Exceptions
{
    /// <summary>
    /// Exceção para quando o tipo do modulo informado nao for encontrado no Assembly
    /// </summary>
    [Serializable]
    public class ModuleTypeNotFoundException : ModuleException
    {
        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="moduleName">Nome do modulo</param>
        public ModuleTypeNotFoundException(string moduleName) : base(string.Format(Constantes.ERRO_MODULO_NAO_ENCONTRADO, moduleName)) { }

        ///<inheritdoc/>
        [ExcludeFromCodeCoverage(Justification = Constantes.EXECEPTION_CODE_COVERAGE_JUSTIFICATION)]
        protected ModuleTypeNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}