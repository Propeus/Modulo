using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Abstrato.Util;
using Propeus.Modulo.IL.Enums;
using Propeus.Modulo.IL.Interfaces;
using Propeus.Modulo.IL.Proxy;

using Propeus.Modulo.IL;

namespace Propeus.Modulo.IL.Geradores
{

    public static partial class Constantes
    {
        public const string CONST_NME_CLASSE = "IL_Gerador_Classe_";
        public const string CONST_NME_CLASSE_PROXY = CONST_NME_CLASSE + "PROXY_";

        public const string CONST_NME_NAMESPACE = "Propeus.IL.Classes";
        public const string CONST_NME_NAMESPACE_PROXY = CONST_NME_NAMESPACE + ".Proxy";

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
        private Dictionary<string, ILClasseProvider> Classes { get; set; }
        public bool Executado { get; private set; }

        internal ILModulo(ILGerador iLGerador, string nomeModulo)
        {
            this.iLGerador = iLGerador;
            this.nomeModulo = nomeModulo;
            this.Classes = new Dictionary<string, ILClasseProvider>();

            moduleBuilder = iLGerador.assemblyBuilder.DefineDynamicModule(nomeModulo);

        }


        internal ILClasseProvider CriarClasseProvider(string nomeClasse = Constantes.CONST_NME_CLASSE, string @namespace = Constantes.CONST_NME_NAMESPACE, Type @base = null, Type[] interfaces = null, Token[] acessadores = null)
        {
            if (Constantes.CONST_NME_CLASSE == nomeClasse)
            {
                nomeClasse = Constantes.GerarNome(Constantes.CONST_NME_CLASSE);
            }

            if (Constantes.CONST_NME_NAMESPACE == @namespace)
            {
                @namespace = Constantes.CONST_NME_NAMESPACE + "." + this.nomeModulo;
            }

            if (Classes.TryGetValue(@namespace + nomeClasse, out ILClasseProvider value))
            {
                return value;
            }
            else
            {
                ILBuilderProxy proxy = new ILBuilderProxy(new object[] { iLGerador.assemblyBuilder, moduleBuilder });

                var clsProvider = new ILClasseProvider(proxy, nomeClasse, @namespace, @base, interfaces, acessadores);
                Classes.Add(@namespace + nomeClasse, clsProvider);
                return clsProvider;
            }
        }

        public void Executar()
        {
            if (disposedValue)
                throw new ObjectDisposedException(GetType().FullName);

            if (Executado)
                return;

            foreach (var classe in Classes)
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
                    foreach (KeyValuePair<string, ILClasseProvider> classe in Classes)
                    {
                        classe.Value.Dispose();
                    }
                    Classes.Clear();
                    Classes = null;

                    moduleBuilder.Dispose();
                    moduleBuilder = null;

                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~ILModulo()
        // {
        //     // Não altere este código. Coloque o código de limpeza no método 'Dispose(bool disposing)'
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Não altere este código. Coloque o código de limpeza no método 'Dispose(bool disposing)'
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public override string ToString()
        {

            if (disposedValue)
                return string.Empty;

            StringBuilder sb = new StringBuilder();

            foreach (var classe in Classes)
            {
                sb.Append("Classe: ").AppendLine(classe.Key);
                sb.AppendLine(classe.Value.ToString());
            }

            return sb.ToString();
        }
    }
}
