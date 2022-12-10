using Propeus.Modulo.IL.Enums;
using Propeus.Modulo.IL.Interfaces;
using Propeus.Modulo.IL.Proxy;

using Propeus.Modulo.Abstrato.Util;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace Propeus.Modulo.IL.Geradores
{

    public static partial class Constantes
    {
        public const string CONST_NME_VARIAVEL = "IL_Gerador_{0}_Variavel_";

     
        public static string GerarNomeVariavel(string nomeClasse)
        {
            return Constantes.GerarNome(string.Format(CONST_NME_PROPRIEDADE, nomeClasse));
        }
    }

    public class ILVariavel : IILExecutor,IDisposable
    {
        public ILVariavel(ILBuilderProxy builderProxy, string nomeMetodo ,Type retorno = null, string nome = Constantes.CONST_NME_VARIAVEL)
        {
            if (builderProxy is null)
            {
                throw new ArgumentNullException(nameof(builderProxy));
            }

            if (string.IsNullOrEmpty(nomeMetodo))
            {
                throw new ArgumentException($"'{nameof(nomeMetodo)}' não pode ser nulo nem vazio.", nameof(nomeMetodo));
            }

            Proxy = builderProxy.Clone(true);
            Proxy.RegistrarBuilders(builderProxy.ObterBuilder<MethodBuilder>());

            if(nome == Constantes.CONST_NME_VARIAVEL)
            {
                nome = Constantes.GerarNomeVariavel(nomeMetodo);
            }


            if (retorno is null)
            {
                retorno = typeof(object);
            }

            Nome = nome;
            Retorno = retorno;

            var fld = Proxy.DeclareLocal(retorno);
            Proxy.RegistrarBuilders(fld);
            Indice = fld.LocalIndex;

            Assinatura = retorno + nome;
        }

        public ILBuilderProxy Proxy { get; private set; }
        public string Nome { get; private set; }
        public Type Retorno { get; private set; }
        public string Assinatura { get; private set; }
        public bool Executado { get; private set; }

        public int Indice { get; }

        public void Executar()
        {
            if (disposedValue)
                throw new ObjectDisposedException(GetType().Name);

            Executado = true;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("[")
                .Append(Proxy.ObterBuilder<LocalBuilder>().LocalIndex)
                .Append("] ");

            sb.Append(Retorno.Name.ToLower(CultureInfo.CurrentCulture));
            sb.Append(" ");
            sb.AppendLine();

            return sb.ToString();
        }

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Proxy.Dispose();
                    Proxy=null;
                    Retorno= null;
                    Assinatura= null;
                    Nome= null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~ILVariavel()
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