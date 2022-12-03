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
    /// <summary>
    /// Gerador de metodos
    /// </summary>
    public struct ILMetodo : IILExecutor
    {
        private readonly TokenEnum[] _acessadores;
        private readonly Type[] _parametros;

        /// <summary>
        /// Cria um novo metodo
        /// </summary>
        /// <param name="iLClasse"></param>
        /// <param name="nome"></param>
        /// <param name="acessadores"></param>
        /// <param name="retorno"></param>
        /// <param name="parametros"></param>
        public ILMetodo(ILClasse iLClasse, string nome = null, TokenEnum[] acessadores = null, Type retorno = null, Type[] parametros = null)
        {
            if (iLClasse is null)
            {
                throw new ArgumentNullException(nameof(iLClasse));
            }

            Proxy = iLClasse.Proxy.Clone();
            Proxy.AdicionarBuilders(iLClasse.Proxy.ObterBuilder<TypeBuilder>());

            if (string.IsNullOrEmpty(nome))
            {
                nome = $"IL_Gerador_Metodo_{Guid.NewGuid().ToString().Replace("-", "_", StringComparison.CurrentCulture)}";
            }

            if (acessadores is null)
            {
                acessadores = new TokenEnum[] {TokenEnum.Publico,  TokenEnum.OcutarAssinatura };
            }

            if (retorno is null)
            {
                retorno = typeof(void);
            }

            if(parametros is null)
            {
                parametros = Array.Empty<Type>();
            }

            Nome = nome;
            Retorno = retorno;
            _acessadores = acessadores;
            _parametros = parametros;

            List<MethodAttributes> typeAttributes = new List<MethodAttributes>();
            foreach (var item in acessadores)
            {
                typeAttributes.Add((MethodAttributes)Enum.Parse(typeof(MethodAttributes), item.ObterDescricaoEnum()));
            }

            MethodBuilder mth = Proxy.ObterBuilder<TypeBuilder>().DefineMethod(nome,  typeAttributes.ToArray().ConcatenarEnum(),retorno,parametros);
            
            Proxy.AdicionarBuilders(mth);

            PilhaExecucao = new List<IILPilha>();

            Assinatura = string.Join(' ', _acessadores.Select(a => a.ToString().ToLower(CultureInfo.CurrentCulture))) + " " + nome + string.Join(',', parametros.Select(p => p.FullName));

            Variaveis = new List<ILVariavel>();
        }

        /// <summary>
        /// Nome do metodo
        /// </summary>
        public string Nome { get; }

        /// <summary>
        /// Retorno do metodo
        /// </summary>
        public Type Retorno { get; }

        public Type[] Parametros => _parametros;
        internal ILBuilderProxy Proxy { get; }
        internal List<IILPilha> PilhaExecucao { get; }
        internal List<ILVariavel> Variaveis { get; }
        internal string Assinatura { get; }

        /// <summary>
        /// Executa a construção do IL
        /// </summary>
        public void Executar()
        {
            foreach (var pilha in PilhaExecucao)
            {
                pilha.Executar();
            }
        }

        /// <summary>
        /// Exibe o metodo em IL
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("\t")
                .Append($".method ");

            foreach (var item in _acessadores)
            {
                sb.Append(item.ObterDescricaoEnum().ToLower(CultureInfo.CurrentCulture)).Append(" ");
            };

            sb.Append("instance ")
             .Append(Retorno.Name.ToLower(CultureInfo.CurrentCulture))
             .Append(" ")
             .Append(Nome)
             .Append(" ")
             .Append("cil")
             .Append(" ")
             .Append("managed")
             .Append(" ")
             .AppendLine()
             .Append("\t")
             .Append("{")
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

            sb.Append("\t")
                .AppendLine("}");

            return sb.ToString();
        }
    }
}