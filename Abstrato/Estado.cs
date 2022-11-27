using System;

namespace Propeus.Modulo.Abstrato
{
    /// <summary>
    /// Informa o estado do modulo
    /// </summary>
    [Flags]
    public enum Estado
    {
        /// <summary>
        /// Define que o modulo foi eliminado pelo gerenciador ou foi chamado o <see cref="IDisposable"/> externamente
        /// </summary>
        Desligado,
        /// <summary>
        /// Define que o modulo foi inicializado com sucesso.
        /// </summary>
        Inicializado,
        /// <summary>
        /// Define que durante a execução do modulo acionado alguma <see cref="Exception"/>
        /// </summary>
        Erro,
        /// <summary>
        /// Define que durante o desligamento do modulo houve alguma exceção
        /// </summary>
        DesligamentoForcado = Desligado|Erro
        
    }
}
