using System.Reflection.Emit;
using System.Text;

using Propeus.Modulo.IL.Enums;
using Propeus.Modulo.IL.Interfaces;
using Propeus.Modulo.IL.Proxy;

namespace Propeus.Modulo.IL.Geradores
{

    public static partial class Constantes
    {
        public const string CONST_NME_CLASSE = "IL_Gerador_Classe_";
        public const string CONST_NME_CLASSE_PROXY = CONST_NME_CLASSE + "Proxy_";

        public const string CONST_NME_NAMESPACE_CLASSE = "Propeus.IL.Classes";
        public const string CONST_NME_NAMESPACE_CLASSE_PROXY = CONST_NME_NAMESPACE_CLASSE + ".Proxy";

        public const string CONST_NME_DELEGATE = "IL_Gerador_Delegate_";
        public const string CONST_NME_DELEGATE_PROXY = CONST_NME_DELEGATE + "Proxy_";

        public const string CONST_NME_NAMESPACE_DELEGATE = "Propeus.IL.Delegates";
        public const string CONST_NME_NAMESPACE_DELEGATE_PROXY = CONST_NME_NAMESPACE_DELEGATE + ".Proxy";

    }


    /// <summary>
    /// Gera o modulo que sera resposavel pelas classes
    /// </summary>
    public class ILModulo : IILExecutor, IDisposable
    {
        private ModuleBuilder moduleBuilder;

        /// <summary>
        /// Obtem um clone do <see cref="ILBuilderProxy"/>
        /// </summary>
        private Dictionary<string, ILDelegate> Delegates { get; set; }
        private Dictionary<string, ILClasseProvider> Classes { get; set; }
        public bool Executado { get; private set; }

        internal ILModulo(ILGerador iLGerador, string nomeModulo)
        {
            this.iLGerador = iLGerador;
            this.nomeModulo = nomeModulo;
            Delegates = new Dictionary<string, ILDelegate>();
            Classes = new Dictionary<string, ILClasseProvider>();

            moduleBuilder = iLGerador.assemblyBuilder.DefineDynamicModule(nomeModulo);

        }

        internal ILDelegate CriarDelegate(string nomeClasse = Constantes.CONST_NME_DELEGATE, string @namespace = Constantes.CONST_NME_NAMESPACE_DELEGATE, Token[] acessadores = null)
        {
            if (Constantes.CONST_NME_CLASSE == nomeClasse)
            {
                nomeClasse = Constantes.GerarNome(Constantes.CONST_NME_CLASSE);
            }

            if (Constantes.CONST_NME_NAMESPACE_DELEGATE == @namespace)
            {
                @namespace = Constantes.CONST_NME_NAMESPACE_DELEGATE + "." + nomeModulo;
            }
            else
            {
                @namespace ??= Constantes.CONST_NME_NAMESPACE_DELEGATE;
            }

            if (Delegates.TryGetValue(@namespace + nomeClasse, out ILDelegate value))
            {
                return value;
            }
            else
            {
                ILBuilderProxy proxy = new(new object[] { iLGerador.assemblyBuilder, moduleBuilder });

                ILDelegate clsProvider = new(proxy, nomeClasse, @namespace, acessadores);
                Delegates.Add(@namespace + nomeClasse, clsProvider);
                return clsProvider;
            }
        }

        internal ILClasseProvider CriarClasseProvider(string nomeClasse = Constantes.CONST_NME_CLASSE, string @namespace = Constantes.CONST_NME_NAMESPACE_CLASSE, Type @base = null, Type[] interfaces = null, Token[] acessadores = null)
        {
            if (Constantes.CONST_NME_CLASSE == nomeClasse)
            {
                nomeClasse = Constantes.GerarNome(Constantes.CONST_NME_CLASSE);
            }

            if (Constantes.CONST_NME_NAMESPACE_CLASSE == @namespace)
            {
                @namespace = Constantes.CONST_NME_NAMESPACE_CLASSE + "." + nomeModulo;
            }

            if (Classes.TryGetValue(@namespace + nomeClasse, out ILClasseProvider value))
            {
                return value;
            }
            else
            {
                ILBuilderProxy proxy = new ILBuilderProxy(new object[] { iLGerador.assemblyBuilder, moduleBuilder });

                ILClasseProvider clsProvider = new ILClasseProvider(proxy, nomeClasse, @namespace, @base, interfaces, acessadores);
                Classes.Add(@namespace + nomeClasse, clsProvider);
                return clsProvider;
            }
        }

        internal ILClasseProvider ObterCLasseProvider(string nomeClasse = Constantes.CONST_NME_CLASSE, string @namespace = Constantes.CONST_NME_NAMESPACE_CLASSE)
        {
            return Classes.TryGetValue(@namespace + nomeClasse, out ILClasseProvider iLClasseProvider) ? iLClasseProvider : null;
        }

        public void Executar()
        {
            if (disposedValue)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }

            if (Executado)
            {
                return;
            }

            foreach (KeyValuePair<string, ILDelegate> @delegate in Delegates)
            {
                @delegate.Value.Executar();
            }

            foreach (KeyValuePair<string, ILClasseProvider> classe in Classes)
            {
                classe.Value.Executar();
            }

            Executado = true;
        }

        private bool disposedValue;
        private readonly ILGerador iLGerador;
        private readonly string nomeModulo;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                    foreach (KeyValuePair<string, ILDelegate> @delegate in Delegates)
                    {
                        @delegate.Value.Dispose();
                    }
                    Delegates.Clear();
                    Delegates = null;

                    foreach (KeyValuePair<string, ILClasseProvider> classe in Classes)
                    {
                        classe.Value.Dispose();
                    }
                    Classes.Clear();
                    Classes = null;

                    Propeus.Modulo.Util.Reflections.Helper.DisposeModuleBuilder(moduleBuilder);
                    moduleBuilder = null;

                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Não altere este código. Coloque o código de limpeza no método 'DisposeMethod(bool disposing)'
            Dispose(disposing: true);
        }

        public override string ToString()
        {

            if (disposedValue)
            {
                return string.Empty;
            }

            StringBuilder sb = new();

            foreach (KeyValuePair<string, ILDelegate> @delegate in Delegates)
            {
                _ = sb.Append("Delegate: ").AppendLine(@delegate.Key);
                _ = sb.AppendLine(@delegate.Value.ToString());
            }

            foreach (KeyValuePair<string, ILClasseProvider> classe in Classes)
            {
                _ = sb.Append("Classe: ").AppendLine(classe.Key);
                _ = sb.AppendLine(classe.Value.ToString());
            }

            return sb.ToString();
        }
    }
}
