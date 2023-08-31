using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Propeus.Modulo.Abstrato.Exceptions
{
    /// <summary>
    /// Exceção para quando há mais de um modulo valido e de mesmo nome
    /// </summary>
    [Serializable]
    public class ModuleAmbiguousException : ModuleException
    {
        private const string Message = "Há mais de um modulo com o nome {0}. Acesse o dicionário de dados para verificar os tipos encontrados";

        /// <summary>
        /// Exceção para quando há mais de um modulo valido e de mesmo nome
        /// </summary>
        public ModuleAmbiguousException(IEnumerable<Type> modules) : base(string.Format(Message, modules.First().Name))
        {
            Data.Add("Modulos duplicados", modules);
        }

        ///<inheritdoc/>
        [ExcludeFromCodeCoverage(Justification = Constantes.EXECEPTION_CODE_COVERAGE_JUSTIFICATION)]
        protected ModuleAmbiguousException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
