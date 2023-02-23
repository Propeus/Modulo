using System;

namespace Propeus.Modulo.Abstrato.Interfaces
{
    /// <summary>
    /// Obtem informacoes de diagnostico do gerenciador
    /// </summary>
    public interface IGerenciadorDiagnostico
    {
        DateTime DataInicio { get; }
        DateTime UltimaAtualizacao { get; }

        int ModulosInicializados { get; }
    }
}
