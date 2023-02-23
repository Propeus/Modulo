using System;
namespace Propeus.Modulo.Abstrato.Interfaces
{
    /// <summary>
    /// Informa detalhes sobre o modulo instanciado
    /// </summary>
    public interface IModuloTipo : IBaseModelo
    {
        /// <summary>
        /// Informa se o modulo foi coletado pelo <see cref="GC"/>
        /// </summary>
        bool Coletado { get; }

        /// <summary>
        /// Informa se o modulo foi eliminado da aplicação
        /// </summary>
        bool Elimindado { get; }
        /// <summary>
        /// Informa o id gerado para o modulo instanciado
        /// </summary>
        string IdModulo { get; }
        /// <summary>
        /// Informa se o modulo é instancia unica
        /// </summary>
        bool InstanciaUnica { get; }
        /// <summary>
        /// Instancia do modulo
        /// </summary>
        IModulo Modulo { get; }
        /// <summary>
        /// Tipo do modulo
        /// </summary>
        Type TipoModulo { get; }
        /// <summary>
        /// Tipo do modulo gerado automaticamente junto com as intefaces delaclarados na propriedade <see cref="Contratos"/>
        /// </summary>
        Type TipoModuloDinamico { get; }
        /// <summary>
        /// Informações sobre o modulo na visão do <see cref="GC"/>
        /// </summary>
        WeakReference WeakReference { get; }
    }
}
