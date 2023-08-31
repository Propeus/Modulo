using System;
using System.Linq;
using System.Reflection.Emit;

using Propeus.Module.IL.Core.Enums;
using Propeus.Module.IL.Core.Helpers;
using Propeus.Module.IL.Core.Interfaces;
using Propeus.Module.IL.Core.Proxy;
using Propeus.Modulo.IL.Geradores;

namespace Propeus.Module.IL.Core.Geradores
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
        public Type[] Atributos { get; private set; }
        internal ILBuilderProxy Proxy { get; private set; }
        public bool Executado { get; private set; }

        internal ILClasseProvider(ILBuilderProxy proxy, string nome = Constantes.CONST_NME_CLASSE, string @namespace = Constantes.CONST_NME_NAMESPACE_CLASSE, Type @base = null, Type[] interfaces = null, Token[] acessadores = null, Type[] atributos = null)
        {

            Namespace = @namespace;
            Base = @base;
            Interfaces = interfaces;
            Acessadores = acessadores;
            Proxy = proxy;
            Nome = nome;
            Executado = false;
            _versoes = new ILClasse[3];
            Atributos = atributos;
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


            ILClasse classe = new ILClasse(Proxy, Nome, Namespace + ".V" + Versao, Base, Interfaces, Acessadores, Atributos);
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
            if (versao - 1 >= _versoes.Length)
            {
                string nome = _versoes[0].Namespace + "." + _versoes[0].Nome;
                Proxy.ObterBuilder<ModuleBuilder>().DisposeModuleBuilder(nome);
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

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Não altere este código. Coloque o código de limpeza no método 'DisposeMethod(bool disposing)'
            Dispose(disposing: true);
        }

    }
}
