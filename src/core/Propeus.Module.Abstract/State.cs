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
        /// <remarks>
        /// Este estado representa a criação do objeto em si, sem nenhuma configuração
        /// </remarks>
        Created,
        /// <summary>
        /// Indica que o modulo foi instanciado e configurado, porem nao começou a sua execução
        /// </summary>
        /// <remarks>
        /// Neste estado se diz que todos os parametros, metodos e configurações foram realizados, pronto para ser inicializado.
        /// </remarks>
        Ready,
        /// <summary>
        /// Define que o modulo foi inicializado com sucesso.
        /// </summary>
        /// <remarks>
        /// Neste estado o modulo possui todas as suas dependencias inicializadas e configurdas, neste ponto o modulo esta pronto para ser usado
        /// </remarks>
        Running,
        /// <summary>
        /// Define que o modulo foi eliminado pelo gerenciador ou foi chamado o <see cref="IDisposable"/> externamente
        /// </summary>
        /// <remarks>
        /// Neste estado o modulo libera todas as suas dependencias e esta incapacitado de realizar qualquer tipo de operação
        /// </remarks>
        Off,
        /// <summary>
        /// Define que durante a execução do modulo acionado alguma <see cref="Exception"/>
        /// </summary>
        /// <remarks>
        /// Neste estado, o modulo informa que houve algum erro porem suas operações continuam a funcionar, o seu estado ira sempre permanecer como <see cref="State.Error"/> até que se desligue (<see cref="State.Off"/>)
        /// </remarks>
        Error

    }
}
