using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

using Propeus.Modulo.Abstrato.Util;
using Propeus.Modulo.IL.Enums;
using Propeus.Modulo.IL.Interfaces;
using Propeus.Modulo.IL.Proxy;

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
    public class ILMetodo : IILExecutor, IDisposable
    {

        /// <summary>
        /// Nome do metodo
        /// </summary>
        public string Nome { get; private set; }

        /// <summary>
        /// Retorno do metodo
        /// </summary>
        public Type Retorno { get; private set; }


        public ILParametro[] Parametros { get; private set; }
        public Token[] Acessadores { get; private set; }
        public MethodInfo MethodInfo => _metodoBuilder.ReflectedType?.GetMethod(Nome, Parametros.Converter<Type>().ToArray());


        internal List<IILPilha> PilhaExecucao { get; private set; }
        internal List<ILVariavel> Variaveis { get; private set; }
        internal Stack<IILPilha> PilhasAuxiliares { get; private set; }

        internal MethodBuilder _metodoBuilder;

        private bool _executado;


        /// <summary>
        /// Cria um novo metodo
        /// </summary>
        /// <param name="builderProxy"></param>
        /// <param name="nomeCLasse"></param>
        /// <param name="nomeMetodo"></param>
        /// <param name="acessadores"></param>
        /// <param name="retorno"></param>
        /// <param name="parametros"></param>
        public ILMetodo(ILBuilderProxy builderProxy, string nomeCLasse, string nomeMetodo = Constantes.CONST_NME_METODO, Token[] acessadores = null, Type retorno = null, ILParametro[] parametros = null)
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

            if (nomeMetodo == Constantes.CONST_NME_METODO)
            {
                nomeMetodo = Constantes.GerarNomeMetodo(nomeCLasse);
            }

            acessadores ??= new Token[] { Token.Publico, Token.OcutarAssinatura };

            retorno ??= typeof(void);

            parametros ??= Array.Empty<ILParametro>();

            Nome = nomeMetodo;
            Retorno = retorno;
            Acessadores = acessadores;
            Parametros = parametros;
            _executado = false;

            List<MethodAttributes> typeAttributes = new();
            foreach (Token item in acessadores)
            {
                typeAttributes.Add((MethodAttributes)Enum.Parse(typeof(MethodAttributes), item.ObterDescricaoEnum()));
            }

            _metodoBuilder = builderProxy.ObterBuilder<TypeBuilder>().DefineMethod(nomeMetodo, typeAttributes.ToArray().ConcatenarEnum(), retorno, parametros.Converter<Type>().ToArray());
            for (int i = 0; i < parametros.Length; i++)
            {
                parametros[i].Indice = i + 1;
            }
            PilhaExecucao = new List<IILPilha>();
            Variaveis = new List<ILVariavel>();
            PilhasAuxiliares = new Stack<IILPilha>();

        }



        ///<inheritdoc/>
        public void Executar()
        {
            if (_executado)
            {
                return;
            }

            foreach (IILPilha pilha in PilhaExecucao)
            {
                pilha.Executar();
            }

            _executado = true;
        }

        /// <summary>
        /// Exibe o metodo em IL
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {

            StringBuilder sb = new();

            _ = sb.Append('\t')
                .Append($".method ");

            foreach (Token item in Acessadores)
            {
                _ = sb.Append(item.ObterDescricaoEnum().ToLower(CultureInfo.CurrentCulture)).Append(' ');
            };

            _ = sb.Append("instance ")
             .Append(Retorno.Name.ToLower(CultureInfo.CurrentCulture))
             .Append(' ')
             .Append(Nome)
             .Append(' ');

            if (Parametros.Length > 0)
            {
                _ = sb.Append('(')
                    .AppendLine();

                foreach (ILParametro parametro in Parametros)
                {
                    _ = sb.Append(parametro.Tipo.Name.ToLower())
                        .Append(' ')
                        .Append(parametro.Nome)
                        .AppendLine();
                }
                _ = sb.Append(") ");
            }
            else
            {
                _ = sb.Append("() ");
            }

            //TODO: Preciso exibir o runtime managed
            //foreach (MethodImplAttributes item in _metodoBuilder.GetMethodImplementationFlags().ObterEnumsConcatenadoBitaBit())
            //{
            //    _ = sb.Append(' ').Append(item.ToString());
            //}

            _ = sb.AppendLine()
            .Append('\t')
            .Append('{')
            .AppendLine()
            .AppendLine();

            if (Variaveis.Count != 0)
            {
                _ = sb.Append("\t\t")
                    .Append($".locals init ( ")
                    .AppendLine();

                for (int i = 0; i < Variaveis.Count; i++)
                {
                    _ = sb.Append("\t\t\t").Append(Variaveis[i].ToString());

                    if (i + 1 != Variaveis.Count)
                    {
                        _ = sb.AppendLine();
                    }
                }

                _ = sb.Append("\t\t")
                    .Append(" )")
                    .AppendLine()
                    .AppendLine();
            }

            foreach (IILPilha pilha in PilhaExecucao)
            {
                if (!string.IsNullOrEmpty(pilha.ToString()))
                {
                    _ = sb.AppendLine(pilha.ToString());
                }
            }

            //sb.Append(Proxy.ToString());

            _ = sb.Append('\t')
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
                    //Proxy.Dispose();

                    foreach (IILPilha item in PilhaExecucao)
                    {
                        item.Dispose();
                    }

                    PilhaExecucao.Clear();
                    PilhaExecucao = null;
                    foreach (ILVariavel item in Variaveis)
                    {
                        item.Dispose();
                    }
                    Variaveis.Clear();
                    Variaveis = null;
                    Retorno = null;
                    Parametros = null;
                    _metodoBuilder = null;
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