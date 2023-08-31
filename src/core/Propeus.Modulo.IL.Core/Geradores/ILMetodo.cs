using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

using Propeus.Module.IL.Core.Enums;
using Propeus.Module.IL.Core.Helpers;
using Propeus.Module.IL.Core.Interfaces;
using Propeus.Module.IL.Core.Proxy;

namespace Propeus.Modulo.IL.Geradores
{


    internal static partial class Constantes
    {
        public const string CONST_NME_METODO = "IL_Gerador_{0}_Metodo_";
    }

    /// <summary>
    /// Gerador de metodos
    /// </summary>
    internal class ILMetodo : IILExecutor, IDisposable
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

        internal List<IILPilha> PilhaExecucao { get; private set; }

        internal Stack<IILPilha> PilhasAuxiliares { get; private set; }

        internal MethodBuilder _metodoBuilder;

        private bool _executado;


        /// <summary>
        /// Cria um novo metodo
        /// </summary>
        /// <param name="builderProxy"></param>
        /// <param name="nomeClasse"></param>
        /// <param name="nomeMetodo"></param>
        /// <param name="acessadores"></param>
        /// <param name="retorno"></param>
        /// <param name="parametros"></param>
        public ILMetodo(ILBuilderProxy builderProxy, string nomeClasse, string nomeMetodo, Token[] acessadores, Type retorno, ILParametro[] parametros = null)
        {

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
                if (parametros[i].Opcional)
                {
                    if (parametros[i].DefaultValue is null)
                    {
                        _ = _metodoBuilder.DefineParameter(parametros[i].Indice, ParameterAttributes.In | ParameterAttributes.Optional | ParameterAttributes.HasDefault, parametros[i].Nome);
                    }
                    else
                    {
                        ParameterBuilder paramBuilder = _metodoBuilder.DefineParameter(parametros[i].Indice, ParameterAttributes.In | ParameterAttributes.Optional | ParameterAttributes.HasDefault, parametros[i].Nome);
                        paramBuilder.SetConstant(parametros[i].DefaultValue);
                    }
                }
                else
                {
                    _ = _metodoBuilder.DefineParameter(parametros[i].Indice, ParameterAttributes.In, parametros[i].Nome);
                }
            }
            PilhaExecucao = new List<IILPilha>();
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
            }

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
                    _ = sb.Append(parametro.Tipo.Name)
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


            _ = sb.AppendLine()
            .Append('\t')
            .Append('{')
            .AppendLine()
            .AppendLine();


            foreach (IILPilha pilha in PilhaExecucao)
            {
                if (!string.IsNullOrEmpty(pilha.ToString()))
                {
                    _ = sb.AppendLine(pilha.ToString());
                }
            }

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

                    foreach (IILPilha item in PilhaExecucao)
                    {
                        item.Dispose();
                    }

                    PilhaExecucao.Clear();
                    PilhaExecucao = null;

                    Retorno = null;
                    Parametros = null;
                    _metodoBuilder = null;
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