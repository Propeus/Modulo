using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

using Propeus.Modulo.Abstrato.Interfaces;

namespace Propeus.Modulo.Abstrato.Exceptions
{
    /// <summary>
    /// Exceção para tentativa de criação de um novo modulo de instancia unica
    /// </summary>
    [Serializable]
    public class ModuleSingleInstanceException : ModuleException
    {

        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="moduleType">Instancia do modulo</param>
        public ModuleSingleInstanceException(Type moduleType) : base(string.Format( Constantes.ERRO_MODULO_INSTANCIA_UNICA,moduleType.Name)) { }

        ///<inheritdoc/>
        [ExcludeFromCodeCoverage(Justification = Constantes.EXECEPTION_CODE_COVERAGE_JUSTIFICATION)]
        protected ModuleSingleInstanceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}