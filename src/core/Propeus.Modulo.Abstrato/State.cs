using System;

namespace Propeus.Module.Abstract
{
    /// <summary>
    /// Informa o estado do modulo
    /// </summary>
    public enum State
    {
        /// <summary>
        /// Indica que o modulo foi instanciado
        /// </summary>
        Created,
        /// <summary>
        /// Indica que o modulo foi instanciado e configurado, porem nao comecou a sua execucao
        /// </summary>
        Ready,
        /// <summary>
        /// Define que o modulo foi inicializado com sucesso.
        /// </summary>
        Initialized,
        /// <summary>
        /// Define que o modulo foi eliminado pelo gerenciador ou foi chamado o <see cref="IDisposable"/> externamente
        /// </summary>
        Off,
        /// <summary>
        /// Define que durante a execução do modulo acionado alguma <see cref="Exception"/>
        /// </summary>
        Error

    }
}
