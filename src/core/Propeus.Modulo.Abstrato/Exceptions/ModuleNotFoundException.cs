using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Propeus.Modulo.Abstrato.Exceptions
{
    /// <summary>
    /// Exceção para quando o modulo informado nao foi encontrado
    /// </summary>
    [Serializable]
    public class ModuleNotFoundException : ModuleException
    {

        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="type">Tipo do modulo</param>
        public ModuleNotFoundException(Type type) : base(string.Format(Constantes.ERRO_MODULO_NAO_ENCONTRADO,type.FullName))
        {
                
        }
        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="idModule">Id do modulo</param>
        public ModuleNotFoundException(string idModule) : base(string.Format(Constantes.ERRO_MODULO_NAO_ENCONTRADO, idModule))
        {
            
        }

        ///<inheritdoc/>
        [ExcludeFromCodeCoverage(Justification = Constantes.EXECEPTION_CODE_COVERAGE_JUSTIFICATION)]
        protected ModuleNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}