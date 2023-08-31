using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;

using Propeus.Module.IL.Core.Enums;
using Propeus.Module.IL.Core.Geradores;
using Propeus.Module.IL.Core.Helpers;
using Propeus.Module.IL.Core.Interfaces;
using Propeus.Module.IL.Core.Proxy;

namespace Propeus.Modulo.IL.Geradores
{

    internal static partial class Constantes
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

        private Dictionary<string, ILClasseProvider> Classes { get; set; }
        internal ILModulo(ILGerador iLGerador, string nomeModulo)
        {
            this.iLGerador = iLGerador;
            this.nomeModulo = nomeModulo;
            Classes = new Dictionary<string, ILClasseProvider>();

            moduleBuilder = iLGerador.assemblyBuilder.DefineDynamicModule(nomeModulo);

        }

        internal bool ExisteClasseProvider(string nomeClasse, string @namespace)
        {
            return Classes.ContainsKey(@namespace + nomeClasse);
        }

        internal ILClasseProvider ObterClasseProvider(string nomeClasse, string @namespace)
        {
            return Classes[@namespace + nomeClasse];
        }

        internal ILClasseProvider CriarClasseProvider(string nomeClasse, string @namespace, Type @base = null, Type[] interfaces = null, Token[] acessadores = null, Type[] atributos = null)
        {
            ILBuilderProxy proxy = new ILBuilderProxy(new object[] { iLGerador.assemblyBuilder, moduleBuilder });

            ILClasseProvider clsProvider = new ILClasseProvider(proxy, nomeClasse, @namespace, @base, interfaces, acessadores, atributos);
            Classes.Add(@namespace + nomeClasse, clsProvider);
            return clsProvider;
        }

        public void Executar()
        {

            foreach (KeyValuePair<string, ILClasseProvider> classe in Classes)
            {
                classe.Value.Executar();
            }
        }

        internal bool disposedValue;
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

                    moduleBuilder.DisposeModuleBuilder();
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

            foreach (KeyValuePair<string, ILClasseProvider> classe in Classes)
            {
                _ = sb.Append("Classe: ").AppendLine(classe.Key);
                _ = sb.AppendLine(classe.Value.ToString());
            }

            return sb.ToString();
        }
    }
}
