using System;

namespace Propeus.Modulo.Abstrato.Interfaces
{
    /// <summary>
    /// Obtem informacoes de diagnostico do gerenciador
    /// </summary>
    public interface IGerenciadorDiagnostico
    {
        /// <summary>
        /// Retorna data e hora que o gerenciador iniciou
        /// </summary>
        DateTime DataInicio { get; }
        /// <summary>
        /// Data e hora do ultimo evento realizado no gerenciador
        /// </summary>
        /// <remarks>
        /// Os eventos sao o CRUD (Criar, Reiniciar, Atualizar ou Remover) do genreciador
        /// </remarks>
        DateTime UltimaAtualizacao { get; }

        /// <summary>
        /// Indica a quantidade de modulos inicializados pelo gerenciador
        /// </summary>
        int ModulosInicializados { get; }
    }
}
