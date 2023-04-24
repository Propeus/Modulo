using System;

namespace Propeus.Modulo.Abstrato
{
    /// <summary>
    /// Informa o estado do modulo
    /// </summary>
    public enum Estado
    {
        /// <summary>
        /// Indica que o modulo foi instanciado
        /// </summary>
        Criado,
        /// <summary>
        /// Indica que o modulo foi instanciado e configurado, porem nao comecou a sua execucao
        /// </summary>
        Pronto,
        /// <summary>
        /// Define que o modulo foi inicializado com sucesso.
        /// </summary>
        Inicializado,
        /// <summary>
        /// Define que o modulo foi eliminado pelo gerenciador ou foi chamado o <see cref="IDisposable"/> externamente
        /// </summary>
        Desligado,
        /// <summary>
        /// Define que durante a execução do modulo acionado alguma <see cref="Exception"/>
        /// </summary>
        Erro

    }
}
