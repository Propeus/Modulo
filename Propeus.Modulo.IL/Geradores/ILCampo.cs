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
using Microsoft.VisualBasic;

using Propeus.Modulo.IL;

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
        public ILCampo(ILBuilderProxy builderProxy, string nomeClasse, Token[] acessadores = null, Type retorno = null, string nome = Constantes.CONST_NME_CAMPO)
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

            Proxy = builderProxy.Clone(true);
            Proxy.RegistrarBuilders(builderProxy.ObterBuilder<TypeBuilder>());

            if (nome == Constantes.CONST_NME_CAMPO)
            {
                nome = Constantes.GerarNomeCampo(nomeClasse);
            }

            if (acessadores is null)
            {
                acessadores = new Token[] { Token.Publico, Token.OcutarAssinatura };
            }

            if (retorno is null)
            {
                retorno = typeof(object);
            }

            Nome = nome;
            Retorno = retorno;
            Acessadores = acessadores;

            List<FieldAttributes> typeAttributes = new List<FieldAttributes>();
            foreach (var item in acessadores)
            {
                typeAttributes.Add((FieldAttributes)Enum.Parse(typeof(FieldAttributes), item.ObterDescricaoEnum()));
            }
            var fld = Proxy.ObterBuilder<TypeBuilder>().DefineField(nome, retorno, typeAttributes.ToArray().ConcatenarEnum());
            Proxy.RegistrarBuilders(fld);

            Assinatura = string.Join(' ', Acessadores.Select(a => a.ToString().ToLower(CultureInfo.CurrentCulture))) + " " + nome;
        }

        public ILBuilderProxy Proxy { get; private set; }
        public string Nome { get; private set; }
        public Type Retorno { get; private set; }
        public string Assinatura { get; private set; }
        public Token[] Acessadores { get; private set; }
        public bool Executado { get; private set; }

        public void Executar()
        {
            if (disposedValue)
                throw new ObjectDisposedException(GetType().FullName);

            Executado = true;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("\t")
                .Append($".field ");

            foreach (var item in Acessadores)
            {
                sb.Append(item.ObterDescricaoEnum().ToLower(CultureInfo.CurrentCulture)).Append(" ");
            };

            if (Retorno != typeof(object) && Retorno != typeof(string) && Retorno.IsClass && !Retorno.IsPrimitive)
            {
                sb.Append("class ");
            }

            sb.Append(Retorno.Name.ToLower(CultureInfo.CurrentCulture));
            sb.Append(" ");
            sb.Append(Nome);
            sb.AppendLine();

            return sb.ToString();
        }

        public override bool Equals(object obj)
        {
            return obj is ILCampo && Assinatura == ((ILCampo)obj).Assinatura;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(ILCampo left, ILCampo right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ILCampo left, ILCampo right)
        {
            return !(left == right);
        }


        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Proxy.Dispose();
                    Proxy = null;
                    Acessadores = null;
                    Retorno = null;
                    Assinatura = null;
                    Nome = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~ILCampo()
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