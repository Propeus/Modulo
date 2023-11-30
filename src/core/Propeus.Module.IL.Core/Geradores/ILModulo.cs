using System.Reflection.Emit;
using System.Text;

using Propeus.Module.IL.Core.Enums;
using Propeus.Module.IL.Core.Geradores;
using Propeus.Module.IL.Core.Helpers;
using Propeus.Module.IL.Core.Interfaces;
using Propeus.Module.IL.Core.Proxy;

using static Propeus.Module.IL.Core.Proxy.ILBuilderProxy;

namespace Propeus.Module.IL.Geradores
{

    internal static partial class Constantes
    {
        public const string CONST_NME_CLASSE = "IL_Gerador_Classe_";
        public const string CONST_NME_CLASSE_PROXY = CONST_NME_CLASSE + "Proxy_";

        public const string CONST_NME_NAMESPACE_CLASSE = "Propeus.IL.Classes";
        public const string CONST_NME_NAMESPACE_CLASSE_PROXY = CONST_NME_NAMESPACE_CLASSE + ".ScopeBuilder";

        public const string CONST_NME_DELEGATE = "IL_Gerador_Delegate_";
        public const string CONST_NME_DELEGATE_PROXY = CONST_NME_DELEGATE + "Proxy_";

        public const string CONST_NME_NAMESPACE_DELEGATE = "Propeus.IL.Delegates";
        public const string CONST_NME_NAMESPACE_DELEGATE_PROXY = CONST_NME_NAMESPACE_DELEGATE + ".ScopeBuilder";

    }


    /// <summary>
    /// Gerador de provedores de classe
    /// </summary>
    public class ILModulo : IILExecutor, IDisposable
    {
        /// <summary>
        /// Classe de builders
        /// </summary>
        private readonly ILBuilderProxy _builderProxy;
        /// <summary>
        /// Dicionário de classes do modulo atual
        /// </summary>
        private Dictionary<string, ILClasseProvider> Classes { get; set; }
        /// <summary>
        /// Cria um modulo .NET para gerar as classes de proxy
        /// </summary>
        /// <param name="nomeModulo">Nome do modulo .NET</param>
        /// <param name="builderProxy">O proxy contendo os construtores de IL</param>
        /// <exception cref="InvalidOperationException"><see cref="AssemblyBuilder"/> ausente</exception>
        internal ILModulo(string nomeModulo, ILBuilderProxy builderProxy)
        {
            AssemblyBuilder? assmBuilder = builderProxy.GetBuilder<AssemblyBuilder>() ?? throw new InvalidOperationException("AssemblyBuilder ausente");
            Classes = new Dictionary<string, ILClasseProvider>();

            _builderProxy = builderProxy;
            builderProxy.RegisterBuilders(assmBuilder.DefineDynamicModule(nomeModulo));

        }
        /// <summary>
        /// Verifica se existe a classe no dicionário
        /// </summary>
        /// <param name="nomeClasse">Nome da classe</param>
        /// <param name="namespace">Namespace</param>
        /// <returns>Retorna <see langword="true"/> caso exista a classe no dicionário</returns>
        /// <exception cref="ObjectDisposedException">Classe com a chamada <see cref="Dispose()"/> acionado</exception>
        internal bool ExisteClasseProvider(string nomeClasse, string @namespace)
        {
            if (disposedValue)
                throw new ObjectDisposedException(GetType().FullName);

            return Classes.ContainsKey(@namespace + nomeClasse);
        }

        /// <summary>
        /// Obtém o provedor de classe informado
        /// </summary>
        /// <param name="nomeClasse">Nome da classe</param>
        /// <param name="namespace">Namespace</param>
        /// <returns>O provedor de classe</returns>
        /// <exception cref="ObjectDisposedException">Classe com a chamada <see cref="Dispose()"/> acionado</exception>
        /// <exception cref="KeyNotFoundException">Acionado quando não houver a chave no dicionário</exception>
        internal ILClasseProvider ObterClasseProvider(string nomeClasse, string @namespace)
        {
            if (disposedValue)
                throw new ObjectDisposedException(GetType().FullName);

            return Classes[@namespace + nomeClasse];
        }

        /// <summary>
        /// Cria um novo provedor de classe
        /// </summary>
        /// <param name="nomeClasse">Nome da classe</param>
        /// <param name="namespace">Namespace da classe</param>
        /// <param name="tipoBase">Tipo base da classe</param>
        /// <param name="interfaces">Interfaces da classe</param>
        /// <param name="modificadoresAcesso">Modificadores de acesso da classe</param>
        /// <param name="atributos">Atributos da classe</param>
        /// <returns>Retorna uma nova instancia do provedor de classe</returns>
        internal ILClasseProvider CriarClasseProvider(string nomeClasse, string @namespace, Type? tipoBase = null, Type[]? interfaces = null, Token[]? modificadoresAcesso = null, Type[]? atributos = null)
        {
            ILClasseProvider clsProvider = new ILClasseProvider(_builderProxy, nomeClasse, @namespace, tipoBase, interfaces, modificadoresAcesso, atributos);
            Classes.Add(@namespace + nomeClasse, clsProvider);
            return clsProvider;
        }
        ///<inheritdoc/>
        public void Apply()
        {

            foreach (KeyValuePair<string, ILClasseProvider> classe in Classes)
            {
                classe.Value.Apply();
            }
        }



        /// <summary>
        /// Retorna o conteúdo da classe em IL
        /// </summary>
        /// <returns></returns>
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

        private bool disposedValue;

        /// <summary>
        /// Libera todos os provedores de classe da memoria
        /// </summary>
        /// <param name="disposing">Indica se deve liberar todos os provedores de classe e limpar o dicionário</param>
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
                }

                disposedValue = true;
            }
        }

        /// <summary>
        /// Libera todos os provedores de classe da memoria
        /// </summary>
        public void Dispose()
        {
            // Não altere este código. Coloque o código de limpeza no método 'Dispose(bool disposing)'
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
