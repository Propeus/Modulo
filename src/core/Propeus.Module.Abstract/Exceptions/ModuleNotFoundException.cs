using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Propeus.Module.Abstract.Exceptions
{
    /// <summary>
    /// Exceção para quando a instancia do modulo informado nao foi encontrado
    /// </summary>
    [Serializable]
    public class ModuleNotFoundException : ModuleException
    {

        /// <summary>
        /// Exceção quando a instancia do modulo não é encontrado pelo tipo
        /// </summary>
        /// <param name="type">Tipo do modulo</param>
        public ModuleNotFoundException(Type type) : base(string.Format(Constantes.ERRO_MODULO_NAO_ENCONTRADO, type.FullName))
        {

        }
        /// <summary>
        /// Exceção quando a instancia do modulo não é encontrado pelo ID
        /// </summary>
        /// <param name="idModule">Id do modulo</param>
        public ModuleNotFoundException(string idModule) : base(string.Format(Constantes.ERRO_MODULO_ID_NAO_ENCONTRADO, idModule))
        {

        }

        ///<inheritdoc/>
        [ExcludeFromCodeCoverage(Justification = Constantes.EXECEPTION_CODE_COVERAGE_JUSTIFICATION)]
        protected ModuleNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}