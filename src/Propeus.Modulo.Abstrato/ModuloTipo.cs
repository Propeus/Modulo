using System;
using System.Text;

using Propeus.Modulo.Abstrato.Interfaces;

namespace Propeus.Modulo.Abstrato
{

    /// <summary>
    /// Informa detalhes sobre o modulo instanciado
    /// </summary>
    public class ModuloTipo : ModeloBase, IModuloTipo
    {

        /// <summary>
        /// Inicializa o objeto informando a instancia do modulo
        /// </summary>
        /// <param name="modulo">Instancia do modulo</param>
        /// <exception cref="ArgumentNullException">Modulo nao pode ser nulo</exception>
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

        /// <summary>
        /// Versao do modulo
        /// </summary>
        public override string Versao { get; }
        ///<inheritdoc/>
        public bool Coletado => WeakReference is null || !WeakReference.IsAlive;
        ///<inheritdoc/>
        public bool Elimindado => Coletado || (WeakReference.Target as IModulo)?.Estado == Estado.Desligado;
        ///<inheritdoc/>
        public WeakReference WeakReference { get; protected set; }
        ///<inheritdoc/>
        public IModulo Modulo => WeakReference.Target as IModulo;
        ///<inheritdoc/>
        public Type TipoModulo => WeakReference?.Target?.GetType();

        ///<inheritdoc/>
        public bool InstanciaUnica { get; }
        ///<inheritdoc/>
        public string IdModulo { get; }

        /// <summary>
        /// Exibe informacoes detalhadas sobre o modulo e seu estado no .NET
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new(base.ToString());

            if (Modulo != null)
            {
                _ = sb.Append(Modulo).AppendLine();
            }
            _ = sb.Append("Coletado pelo G.C.: ").Append(Coletado).AppendLine();
            _ = sb.Append("Objeto eliminado: ").Append(Elimindado).AppendLine();


            return sb.ToString();
        }

        ///<inheritdoc/>
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
