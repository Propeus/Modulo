using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

using Propeus.Modulo.IL.Core.Enums;
using Propeus.Modulo.IL.Core.Helpers;
using Propeus.Modulo.IL.Core.Interfaces;
using Propeus.Modulo.IL.Core.Proxy;

namespace Propeus.Modulo.IL.Geradores
{
    internal static partial class Constantes
    {
        public const string CONST_NME_CAMPO = "IL_Gerador_{0}_Campo_";
        public const string CONST_NME_CAMPO_PROXY = CONST_NME_CAMPO + "Proxy_";
    }

    /// <summary>
    /// Cria um campo
    /// </summary>
    internal class ILCampo : IILExecutor, IDisposable
    {

        internal FieldBuilder _campoBuilder;

        public string Nome { get; private set; }
        public Type Retorno { get; private set; }

        public Token[] Acessadores { get; private set; }

        public ILCampo(ILBuilderProxy builderProxy, string nomeClasse, Token[] acessadores, Type tipo, string nome)
        {

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
            //Nao faz nada
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