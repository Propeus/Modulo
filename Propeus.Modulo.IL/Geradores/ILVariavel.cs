using System;
using System.Globalization;
using System.Reflection.Emit;
using System.Text;

using Propeus.Modulo.IL.Interfaces;
using Propeus.Modulo.IL.Proxy;

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

    public class ILVariavel : IILExecutor, IDisposable
    {
        internal LocalBuilder _variavelBuilder;
        private bool _Executado;

        public int Indice { get; }
        public string Nome { get; }
        public Type Retorno { get; private set; }

        public ILVariavel(ILBuilderProxy builderProxy, string nomeMetodo, Type retorno = null, string nome = Constantes.CONST_NME_VARIAVEL)
        {
            if (builderProxy is null)
            {
                throw new ArgumentNullException(nameof(builderProxy));
            }

            if (string.IsNullOrEmpty(nomeMetodo))
            {
                throw new ArgumentException($"'{nameof(nomeMetodo)}' não pode ser nulo nem vazio.", nameof(nomeMetodo));
            }

            if (nome == Constantes.CONST_NME_VARIAVEL)
            {
                nome = Constantes.GerarNomeVariavel(nomeMetodo);
            }


            retorno ??= typeof(object);

            _variavelBuilder = builderProxy.DeclareLocal(retorno);

            Nome = nome;
            Retorno = retorno;
            Indice = _variavelBuilder.LocalIndex;

        }

        
     


        public void Executar()
        {
            if (disposedValue)
            {
                throw new ObjectDisposedException(GetType().Name);
            }

            _Executado = true;
        }

        public override string ToString()
        {
            StringBuilder sb = new();

            _ = sb.Append("[")
                .Append(Indice)
                .Append("] ");

            _ = sb.Append(Retorno.Name.ToLower(CultureInfo.CurrentCulture));
            _ = sb.Append(" ");
            _ = sb.AppendLine();

            return sb.ToString();
        }

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Retorno = null;
                    _variavelBuilder = null;
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