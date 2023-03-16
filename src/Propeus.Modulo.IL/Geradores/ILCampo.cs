using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

using Propeus.Modulo.IL.Enums;
using Propeus.Modulo.IL.Interfaces;
using Propeus.Modulo.IL.Proxy;
using Propeus.Modulo.Util;

namespace Propeus.Modulo.IL.Geradores
{
    public static partial class Constantes
    {
        public const string CONST_NME_CAMPO = "IL_Gerador_{0}_Campo_";
        public const string CONST_NME_CAMPO_PROXY = CONST_NME_CAMPO + "Proxy_";

        public static string GerarNomeCampo(string nomeClasse)
        {
            return Constantes.GerarNome(string.Format(CONST_NME_CAMPO, nomeClasse));
        }
    }

    /// <summary>
    /// Cria um campo
    /// </summary>
    public class ILCampo : IILExecutor, IDisposable
    {

        internal FieldBuilder _campoBuilder;
        private bool _executado;

        public string Nome { get; private set; }
        public Type Retorno { get; private set; }

        public Token[] Acessadores { get; private set; }

        public ILCampo(ILBuilderProxy builderProxy, string nomeClasse, Token[] acessadores, Type tipo, string nome)
        {
            if (builderProxy is null)
            {
                throw new ArgumentNullException(nameof(builderProxy));
            }

            if (string.IsNullOrEmpty(nomeClasse))
            {
                throw new ArgumentException($"'{nameof(nomeClasse)}' não pode ser nulo nem vazio.", nameof(nomeClasse));
            }

            if (string.IsNullOrEmpty(nome))
            {
                throw new ArgumentException($"'{nameof(nome)}' não pode ser nulo nem vazio.", nameof(nome));
            }

            Nome = nome;
            Retorno = tipo ?? throw new ArgumentNullException(nameof(tipo));
            Acessadores = acessadores ?? throw new ArgumentNullException(nameof(acessadores));

            List<FieldAttributes> typeAttributes = new();
            foreach (Token item in acessadores)
            {
                typeAttributes.Add((FieldAttributes)Enum.Parse(typeof(FieldAttributes), item.ObterDescricaoEnum()));
            }
            _campoBuilder = builderProxy.ObterBuilder<TypeBuilder>().DefineField(nome, tipo, typeAttributes.ToArray().ConcatenarEnum());
        }


        ///<inheritdoc/>
        public void Executar()
        {
            if (disposedValue)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }

            _executado = true;
        }

        public override string ToString()
        {
            StringBuilder sb = new();

            _ = sb.Append("\t")
                .Append($".field ");

            foreach (Token item in Acessadores)
            {
                _ = sb.Append(item.ObterDescricaoEnum().ToLower(CultureInfo.CurrentCulture)).Append(" ");
            }

            if (Retorno != typeof(object) && Retorno != typeof(string) && Retorno.IsClass && !Retorno.IsPrimitive)
            {
                _ = sb.Append("class ");
            }

            _ = sb.Append(Retorno.Name.ToLower(CultureInfo.CurrentCulture));
            _ = sb.Append(" ");
            _ = sb.Append(Nome);
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
                    _campoBuilder = null;
                    Acessadores = null;
                    Retorno = null;
                    Nome = null;
                }

                disposedValue = true;
            }
        }


        public void Dispose()
        {
            Dispose(disposing: true);   
        }

    }
}