using System.Net.Security;
using System.Reflection.Emit;

using Propeus.Module.IL.Core.Enums;
using Propeus.Module.IL.Core.Helpers;
using Propeus.Module.IL.Core.Interfaces;
using Propeus.Module.IL.Core.Proxy;
using Propeus.Module.IL.Geradores;

using static Propeus.Module.IL.Core.Proxy.ILBuilderProxy;

namespace Propeus.Module.IL.Core.Geradores
{



    /// <summary>
    /// Provedor de construtor de classe em IL
    /// </summary>
    public class ILClasseProvider : IILExecutor, IDisposable
    {
        /// <summary>
        /// Versões da classe gerado
        /// </summary>
        private ILClasse[] _versoes;
        /// <summary>
        /// Versão atual da classe
        /// </summary>
        private ILClasse? _atual;
        /// <summary>
        /// Versão atual da classe
        /// </summary>
        internal ILClasse CurrentClass
        {
            get
            {
                if (_atual == null && !disposedValue)
                {
                    _ = NewVersion();
                }
                return _atual ?? throw new InvalidOperationException("Instancia do ILClasse está nulo");
            }

        }
        /// <summary>
        /// Versão do tipo
        /// </summary>
        /// <value>O valor inicial é 1</value>
        public int Version { get; private set; }
        /// <summary>
        /// Nome do tipo
        /// </summary>
        public string ClassName { get; }
        /// <summary>
        /// Namespace do tipo
        /// </summary>
        public string Namespace { get; private set; }
        /// <summary>
        /// Tipo base
        /// </summary>
        /// <value>Caso o valor seja nulo, será definido a herança padrão que é <see cref="object"/></value>
        public Type? TypeBase { get; private set; }
        /// <summary>
        /// Interfaces que o tipo herda
        /// </summary>
        public Type[]? Interfaces { get; private set; }
        /// <summary>
        /// Modificadores de acesso da classe
        /// </summary>
        public Token[]? Tokens { get; private set; }
        /// <summary>
        /// Atributos que compõem a classe 
        /// </summary>
        public Type[]? Attributes { get; private set; }
        /// <summary>
        /// Indica se a classe atual foi "compilado"
        /// </summary>
        /// <value>O valor inicial é <see langword="false"/></value>
        public bool Compiled { get; private set; }

        /// <summary>
        /// Classe de builders
        /// </summary>
        internal ILBuilderProxy Proxy { get; private set; }

        /// <summary>
        /// Cria uma nova instancia de provedor de classe
        /// </summary>
        /// <param name="IlProxy">Gerador de IL atual</param>
        /// <param name="nomeClasse">ClassName da classe</param>
        /// <param name="namespace">Namespace da classe</param>
        /// <param name="tipoBase">Objeto a ser estendido para classe</param>
        /// <param name="interfaces">Interface a ser implementado na classe</param>
        /// <param name="modificadorAcesso">Tokens da classe</param>
        /// <param name="atributos">Tipos herdados de <see cref="Attribute"/></param>
        internal ILClasseProvider(ILBuilderProxy IlProxy, string nomeClasse = Constantes.CONST_NME_CLASSE, string @namespace = Constantes.CONST_NME_NAMESPACE_CLASSE, Type? tipoBase = null, Type[]? interfaces = null, Token[]? modificadorAcesso = null, Type[]? atributos = null)
        {

            Namespace = @namespace;
            TypeBase = tipoBase;
            Interfaces = interfaces;
            Tokens = modificadorAcesso;
            Proxy = IlProxy;
            ClassName = nomeClasse;
            Compiled = false;
            _versoes = new ILClasse[3];
            Attributes = atributos;
        }

        /// <summary>
        /// Cria uma nova versão do <see cref="ILClasseProvider"/> do tipo 
        /// </summary>
        /// <param name="namespace">Namespace da classe</param>
        /// <param name="tipoBase">Objeto a ser estendido para classe</param>
        /// <param name="interfaces">Interface a ser implementado na classe</param>
        /// <param name="modificadorAcesso">Modificadores de acesso da classe</param>
        /// <returns></returns>
        /// <exception cref="ObjectDisposedException"></exception>
        public ILClasseProvider NewVersion(string? @namespace = null, Type? tipoBase = null, Type[]? interfaces = null, Token[]? modificadorAcesso = null)
        {
            if (disposedValue)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }

            Version++;


            if (@namespace is not null)
            {
                Namespace = @namespace;
            }
            if (tipoBase is not null)
            {
                TypeBase = tipoBase;
            }
            if (interfaces is not null)
            {
                if (Interfaces is not null)
                    Interfaces = Interfaces.FullJoin(interfaces).ToArray();
                else
                    Interfaces = interfaces;
            }
            if (modificadorAcesso is not null)
            {
                Tokens = modificadorAcesso.FullJoin(modificadorAcesso).ToArray();
            }

            if (Namespace is not null)
            {
                ILClasse classe = new ILClasse(Proxy.CreateScope(), ClassName, Namespace + ".V" + Version, TypeBase, Interfaces, Tokens, Attributes);
                InserirNovaVersaoArray(classe, Version);
                _atual = classe;
            }
            else
            {
                ILClasse classe = new ILClasse(Proxy.CreateScope(), ClassName, "V" + Version, TypeBase, Interfaces, Tokens, Attributes);
                InserirNovaVersaoArray(classe, Version);
                _atual = classe;
            }

            return this;
        }

        ///<inheritdoc/>
        public void Apply()
        {
            if (disposedValue)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }

            if (!Compiled && CurrentClass.DynamicTypeClass is null)
            {
                CurrentClass.Apply();
                Compiled = true;
            }
        }


        /// <summary>
        /// Adiciona a nova classe no histórico de versões 
        /// </summary>
        /// <param name="iLClasse">Estado da classe a ser mantido em histórico</param>
        /// <param name="versao">Versão atual da classe</param>
        private void InserirNovaVersaoArray(ILClasse iLClasse, int versao)
        {
            if(disposedValue)
                throw new ObjectDisposedException (GetType().FullName);

            if (versao - 1 >= _versoes.Length)
            {
                string nome = _versoes[0].Namespace + "." + _versoes[0].ClassName;
                Proxy.GetBuilder<ModuleBuilder>()?.DisposeModuleBuilder(nome);
                
                _versoes[0].Dispose();

                for (int i = 0; i < _versoes.Length - 1; i++)
                {
                    _versoes[i] = _versoes[i + 1];
                }
                _versoes[^1] = iLClasse;

            }
            else
            {
                _versoes[versao - 1] = iLClasse;

            }

            Compiled = false;

        }

        ///<inheritdoc/>
        public override string ToString()
        {
            if (_atual is null)
                return string.Empty;
            if (disposedValue)
                return string.Empty;

            return _atual.ToString();
        }

        private bool disposedValue;

        /// <summary>
        /// Limpa o histórico de classes e todas as suas referencias
        /// </summary>
        /// <param name="disposing">Indica se deve eliminar todos os objetos gerenciáveis</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    for (int i = 0; i < _versoes.Length; i++)
                    {
                        ILClasse item = _versoes[i];
                        item?.Dispose();
                    }
                  

                    TypeBase = null;
                    Interfaces = null;

                    Tokens = null;

                    Proxy.Dispose();
                }
                
                _versoes = Array.Empty<ILClasse>();

                disposedValue = true;
            }
        }
        /// <summary>
        /// Limpa o histórico de classes e todas as suas referencias
        /// </summary>
        public void Dispose()
        {
            // Não altere este código. Coloque o código de limpeza no método 'Dispose(bool disposing)'
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

      

    }
}
