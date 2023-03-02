using System;

using Propeus.Modulo.IL.Interfaces;
using Propeus.Modulo.IL.Proxy;

namespace Propeus.Modulo.IL.Geradores
{
    public static partial class Constantes
    {
        public const string CONSTNMEPARAMETRO = "IL_Gerador_{0}_Parametro_";


        public static string GerarNomeParametro(string nomeMetodo)
        {
            return Constantes.GerarNome(string.Format(CONSTNMEPARAMETRO, nomeMetodo));
        }
    }

    public class ILParametro : IILExecutor, IDisposable
    {

        private bool disposedValue;
        private bool _Executado;

        public ILParametro(string nomeMetodo, Type tipo, bool opcional = false, object defaultValue = null, string nome = Constantes.CONST_NME_VARIAVEL)
        {
            if (nome == Constantes.CONST_NME_VARIAVEL)
            {
                nome = Constantes.GerarNomeParametro(nomeMetodo);
            }
            tipo ??= typeof(object);

            Nome = nome;
            Tipo = tipo;
            Opcional = opcional;
            DefaultValue = defaultValue;
        }

        public ILParametro(ILBuilderProxy builderProxy, string nomeMetodo, Type tipo = null, string nome = Constantes.CONST_NME_VARIAVEL)
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


            tipo ??= typeof(object);

            //_variavelBuilder = builderProxy.DeclareLocal(tipo);

            Nome = nome;
            Tipo = tipo;
            //Indice = _variavelBuilder.LocalIndex;
        }

        public Type Tipo { get; private set; }
        public bool Opcional { get; private set; }
        public object DefaultValue { get; private set; }
        public string Nome { get; private set; }
        public int Indice { get; internal set; }

        public void Executar()
        {
            if (disposedValue)
            {
                throw new ObjectDisposedException(GetType().Name);
            }

            _Executado = true;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~ILParametro()
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


        //public static implicit operator Type(ILParametro obj)
        //{
        //    return obj.ToType();
        //}
        public static explicit operator Type(ILParametro obj)
        {
            return obj.ToType();
        }

        public static implicit operator int(ILParametro obj)
        {
            return obj.ToInt32();
        }

        public static implicit operator string(ILParametro obj)
        {
            return obj.ToString();
        }

        public int ToInt32()
        {
            return Indice;
        }

        public override string ToString()
        {
            return Tipo.ToString() + " " + Nome;
        }

        public Type ToType()
        {
            return Tipo;
        }
    }
}
