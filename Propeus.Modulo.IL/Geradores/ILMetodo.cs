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
        public const string CONST_NME_METODO = "IL_Gerador_{0}_Metodo_";

        public static string GerarNomeMetodo(string nomeClasse)
        {
            return Constantes.GerarNome(string.Format(CONST_NME_METODO, nomeClasse));
        }
    }

    /// <summary>
    /// Gerador de metodos
    /// </summary>
    public class ILMetodo : IILExecutor ,IDisposable
    {

        /// <summary>
        /// Cria um novo metodo
        /// </summary>
        /// <param name="builderProxy"></param>
        /// <param name="nomeCLasse"></param>
        /// <param name="nomeMetodo"></param>
        /// <param name="acessadores"></param>
        /// <param name="retorno"></param>
        /// <param name="parametros"></param>
        public ILMetodo(ILBuilderProxy builderProxy, string nomeCLasse, string nomeMetodo = Constantes.CONST_NME_METODO, Token[] acessadores = null, Type retorno = null, Type[] parametros = null)
        {
            if (builderProxy is null)
            {
                throw new ArgumentNullException(nameof(builderProxy));
            }

            if (string.IsNullOrEmpty(nomeCLasse))
            {
                throw new ArgumentException($"'{nameof(nomeCLasse)}' não pode ser nulo nem vazio.", nameof(nomeCLasse));
            }

            if (string.IsNullOrEmpty(nomeMetodo))
            {
                throw new ArgumentException($"'{nameof(nomeMetodo)}' não pode ser nulo nem vazio.", nameof(nomeMetodo));
            }

            Proxy = builderProxy.Clone();
            Proxy.RegistrarBuilders(builderProxy.ObterBuilder<TypeBuilder>());

            if (nomeMetodo == Constantes.CONST_NME_METODO)
            {
                nomeMetodo = Constantes.GerarNomeMetodo(nomeCLasse);
            }

            if (acessadores is null)
            {
                acessadores = new Token[] { Token.Publico, Token.OcutarAssinatura };
            }

            if (retorno is null)
            {
                retorno = typeof(void);
            }

            if (parametros is null)
            {
                parametros = Array.Empty<Type>();
            }

            Nome = nomeMetodo;
            Retorno = retorno;
            Acessadores = acessadores;
            Parametros = parametros;
            Executado = false;

            List<MethodAttributes> typeAttributes = new List<MethodAttributes>();
            foreach (var item in acessadores)
            {
                typeAttributes.Add((MethodAttributes)Enum.Parse(typeof(MethodAttributes), item.ObterDescricaoEnum()));
            }

            MethodBuilder mth = Proxy.ObterBuilder<TypeBuilder>().DefineMethod(nomeMetodo, typeAttributes.ToArray().ConcatenarEnum(), retorno, parametros);

            Proxy.RegistrarBuilders(mth);

            PilhaExecucao = new List<IILPilha>();

            Assinatura = string.Join(' ', Acessadores.Select(a => a.ToString().ToLower(CultureInfo.CurrentCulture))) + " " + nomeMetodo + string.Join(',', parametros.Select(p => p.FullName));

            Variaveis = new List<ILVariavel>();
        }

        /// <summary>
        /// Nome do metodo
        /// </summary>
        public string Nome { get; private set; }

        /// <summary>
        /// Retorno do metodo
        /// </summary>
        public Type Retorno { get; private set;  }

        public bool Executado { get; private set; }
        public Type[] Parametros { get; private set; }
        internal ILBuilderProxy Proxy { get; private set; }
        internal List<IILPilha> PilhaExecucao { get; private set; }
        internal List<ILVariavel> Variaveis { get; private set; }
        internal string Assinatura { get; private set; }
        public Token[] Acessadores { get; private set; }

        /// <summary>
        /// Executa a construção do IL
        /// </summary>
        public void Executar()
        {
            if (Executado)
                return;

            foreach (var pilha in PilhaExecucao)
            {
                pilha.Executar();
            }

            Executado = true;
        }

        /// <summary>
        /// Exibe o metodo em IL
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            
            StringBuilder sb = new StringBuilder();

            sb.Append('\t')
                .Append($".method ");

            foreach (var item in Acessadores)
            {
                sb.Append(item.ObterDescricaoEnum().ToLower(CultureInfo.CurrentCulture)).Append(' ');
            };

            sb.Append("instance ")
             .Append(Retorno.Name.ToLower(CultureInfo.CurrentCulture))
             .Append(' ')
             .Append(Nome)
             .Append(' ');

            if (Parametros.Length > 0)
            {
                sb.Append('(')
                    .AppendLine();

                foreach (var parametro in Parametros)
                {
                    sb.Append(parametro.Name.ToLower())
                        .Append(' ')
                        .Append(Guid.NewGuid().ToString().Replace('-','_').ToLower())
                        .AppendLine();
                }
                sb.Append(") ");
            }
            else
            {
                sb.Append("() ");
            }

            sb.Append("cil")
            .Append(' ')
            .Append("managed")
            .Append(' ')
            .AppendLine()
            .Append('\t')
            .Append('{')
            .AppendLine()
            .AppendLine();

            if (Variaveis.Count != 0)
            {
                sb.Append("\t\t")
                    .Append($".locals init ( ")
                    .AppendLine();

                for (int i = 0; i < Variaveis.Count; i++)
                {
                    sb.Append("\t\t\t").Append(Variaveis[i].ToString());

                    if (i + 1 != Variaveis.Count)
                        sb.AppendLine();
                }

                sb.Append("\t\t")
                    .Append(" )")
                    .AppendLine()
                    .AppendLine();
            }

            sb.Append(Proxy.ToString());

            sb.Append('\t')
                .AppendLine("}");

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

                    foreach (IILPilha item in PilhaExecucao)
                    {
                        item.Dispose();
                    }

                    PilhaExecucao.Clear();
                    PilhaExecucao = null;
                    foreach (var item in Variaveis)
                    {
                        item.Dispose();
                    }
                    Variaveis.Clear();
                    Variaveis = null;
                    Retorno = null;
                    Parametros = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~ILMetodo()
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