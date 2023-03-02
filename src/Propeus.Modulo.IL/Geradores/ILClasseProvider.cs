using System;
using System.Linq;
using System.Reflection.Emit;

using Propeus.Modulo.Util;
using Propeus.Modulo.IL.Enums;
using Propeus.Modulo.IL.Interfaces;
using Propeus.Modulo.IL.Proxy;

namespace Propeus.Modulo.IL.Geradores
{


    public class ILClasseProvider : IILExecutor, IDisposable
    {

        private ILClasse[] _versoes;

        internal ILClasse Atual
        {
            get
            {
                if (atual == null)
                {
                    _ = NovaVersao();
                    return atual;
                }
                return atual;
            }
            private set => atual = value;
        }
        public int Versao { get; private set; }
        public string Nome { get; }
        public string Namespace { get; private set; }
        public Type Base { get; private set; }
        public Type[] Interfaces { get; private set; }
        public Token[] Acessadores { get; private set; }
        public ILBuilderProxy Proxy { get; private set; }
        public bool Executado { get; private set; }

        public ILClasseProvider(ILBuilderProxy proxy, string nome = Constantes.CONST_NME_CLASSE, string @namespace = Constantes.CONST_NME_NAMESPACE_CLASSE, Type @base = null, Type[] interfaces = null, Token[] acessadores = null)
        {
            if (string.IsNullOrEmpty(nome))
            {
                throw new ArgumentException($"'{nameof(nome)}' não pode ser nulo nem vazio.", nameof(nome));
            }

            if (Constantes.CONST_NME_CLASSE == nome)
            {
                nome = Constantes.GerarNome(Constantes.CONST_NME_CLASSE);
            }

            if (Constantes.CONST_NME_NAMESPACE_CLASSE == @namespace)
            {
                @namespace = Constantes.GerarNome(Constantes.CONST_NME_NAMESPACE_CLASSE);
            }

            Namespace = @namespace;
            Base = @base;
            Interfaces = interfaces;
            Acessadores = acessadores;
            Proxy = proxy;
            Nome = nome;
            Executado = false;
            _versoes = new ILClasse[3];
        }


        public ILClasseProvider NovaVersao(string @namespace = null, Type @base = null, Type[] interfaces = null, Token[] acessadores = null)
        {
            if (disposedValue)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }

            Versao++;


            if (@namespace is not null)
            {
                Namespace = @namespace;
            }
            if (@base is not null)
            {
                Base = @base;
            }
            if (interfaces is not null)
            {
                Interfaces = Interfaces.FullJoin(interfaces).ToArray();
            }
            if (acessadores is not null)
            {
                Acessadores = acessadores.FullJoin(acessadores).ToArray();
            }


            ILClasse classe = new(Proxy, Nome, Namespace + "V" + Versao, Base, Interfaces, Acessadores);
            InserirNovaVersaoArray(classe, Versao);
            Atual = classe;

            return this;
        }

        public void Executar()
        {
            if (disposedValue)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }

            if (!Executado && Atual.TipoGerado is null)
            {
                Atual.Executar();
                Executado = true;
            }
        }



        private void InserirNovaVersaoArray(ILClasse iLClasse, int versao)
        {
            if (disposedValue)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }

            if (versao - 1 >= _versoes.Length)
            {
                string nome = _versoes[0].Namespace + "." + _versoes[0].Nome;
                Proxy.ObterBuilder<ModuleBuilder>().Dispose(nome);
                _versoes[0].Dispose();
                _versoes[0] = null;

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

            Executado = false;

        }

        public override string ToString()
        {
            return disposedValue ? string.Empty : atual.ToString();
        }

        private bool disposedValue;
        private ILClasse atual;

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
                        _versoes[i] = null;
                    }
                    _versoes = null;

                    Base = null;
                    Interfaces = null;
                    Atual = null;
                    Acessadores = null;

                    Proxy.Dispose();
                    Proxy = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~ILClasseProvider()
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

    }
}
