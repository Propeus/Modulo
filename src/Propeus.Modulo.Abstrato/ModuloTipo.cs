using System;
using System.Text;

using Propeus.Modulo.Abstrato.Interfaces;

namespace Propeus.Modulo.Abstrato
{

    /// <summary>
    /// Informa detalhes sobre o modulo instanciado
    /// </summary>
    public class ModuloTipo : BaseModelo, IModuloTipo
    {


        public ModuloTipo(IModulo modulo)
        {
            if (modulo is null)
            {
                throw new ArgumentNullException(nameof(modulo));
            }

            WeakReference = new WeakReference(modulo);
            InstanciaUnica = modulo.InstanciaUnica;
            IdModulo = modulo.Id;
            Versao = modulo.Versao;
        }

        public override string Versao { get; }
        /// <summary>
        /// Informa se o modulo foi coletado pelo <see cref="GC"/>
        /// </summary>
        public bool Coletado => WeakReference is null || !WeakReference.IsAlive;
        /// <summary>
        /// Informa se o modulo foi eliminado da aplicação
        /// </summary>
        public bool Elimindado => Coletado || (WeakReference.Target as IModulo)?.Estado == Estado.Desligado || Modulo.Disposed;
        /// <summary>
        /// Informações sobre o modulo na visão do <see cref="GC"/>
        /// </summary>
        public WeakReference? WeakReference { get; protected set; }
        /// <summary>
        /// Instancia do modulo
        /// </summary>
        public IModulo Modulo => WeakReference.Target as IModulo;
        /// <summary>
        /// Tipo do modulo
        /// </summary>
        public Type TipoModulo => WeakReference?.Target?.GetType();

        /// <summary>
        /// Informa se o modulo é instancia unica
        /// </summary>
        public bool InstanciaUnica { get; }
        public string IdModulo { get; }

        public Type TipoModuloDinamico { get; protected internal set; }

        public override string ToString()
        {
            StringBuilder sb = new(base.ToString());

            if (Modulo != null)
            {
                sb.Append(Modulo).AppendLine();
            }
            _ = sb.Append("Coletado pelo G.C.: ").Append(Coletado).AppendLine();
            _ = sb.Append("Objeto eliminado: ").Append(Elimindado).AppendLine();


            return sb.ToString();
        }

        protected override void Dispose(bool disposing)
        {
            if (!Elimindado)
            {
                Modulo.Dispose();
            }
            WeakReference = null;
            base.Dispose(disposing);
        }



    }
}
