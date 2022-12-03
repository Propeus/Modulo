using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Abstrato.Util;
using Propeus.Modulo.Abstrato;
using System;
using System.Collections.Generic;

namespace Propeus.Modulo.Core
{

    /// <summary>
    /// Informa detalhes sobre o modulo instanciado
    /// </summary>
    public class ModuloTipo : BaseModelo, IModuloTipo
    {


        public ModuloTipo(IModuloInformacao moduloInformacao, string nomeModulo)
        {
            if (moduloInformacao is null)
            {
                throw new ArgumentNullException(nameof(moduloInformacao));
            }

            TipoModulo = moduloInformacao.ObterTipoModulo(nomeModulo);
            if (TipoModulo is null)
            {
                throw new InvalidProgramException($"O tipo '{nomeModulo}' não foi encontrado na dll {moduloInformacao.Caminho}");
            }
        }

        public ModuloTipo(IModulo modulo)
        {
            if (modulo is null)
            {
                throw new ArgumentNullException(nameof(modulo));
            }

            WeakReference = new WeakReference(modulo);
            TipoModulo = modulo.GetType();
            InstanciaUnica = modulo.InstanciaUnica;
            IdModulo = modulo.Id;
        }
        /// <summary>
        /// Informa se o modulo foi coletado pelo <see cref="GC"/>
        /// </summary>
        public bool Coletado => (WeakReference is null || !WeakReference.IsAlive);
        /// <summary>
        /// Informa se o modulo foi eliminado da aplicação
        /// </summary>
        public bool Elimindado => (Coletado || (WeakReference.Target as IModulo)?.Estado == Estado.Desligado || Modulo.Disposed);
        /// <summary>
        /// Informações sobre o modulo na visão do <see cref="GC"/>
        /// </summary>
        public WeakReference WeakReference { get; protected set; }
        /// <summary>
        /// Instancia do modulo
        /// </summary>
        public IModulo Modulo => WeakReference.Target as IModulo;
        /// <summary>
        /// Tipo do modulo
        /// </summary>
        public Type TipoModulo { get; }
        /// <summary>
        /// Informa se o modulo é instancia unica
        /// </summary>
        public bool InstanciaUnica { get; }
        public string IdModulo { get; }

        public List<Type> Contratos { get; } = new List<Type>();
        public Type TipoModuloDinamico { get; private set; }

        protected override void Dispose(bool disposing)
        {
            if (!Elimindado)
            {
                Modulo.Dispose();
            }
            WeakReference = null;
            base.Dispose(disposing);
        }

        public Type AdicionarContrato(Type tipo)
        {
            if (!Contratos.Contains(tipo))
            {
                Contratos.Add(tipo);
                TipoModuloDinamico = TipoModulo.DynamicProxyType(Contratos.ToArray(), tipo.ObterModuloContratoAtributo().Nome);
            }
            return TipoModuloDinamico;
        }

    }
}
