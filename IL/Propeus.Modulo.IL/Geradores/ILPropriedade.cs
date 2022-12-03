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
    public class ILPropriedade : IILExecutor
    {

        private readonly Type[] _parametros;

        /// <summary>
        /// Nome do metodo
        /// </summary>
        public string Nome { get; }
        public TokenEnum Acessador { get; }

        /// <summary>
        /// Retorno do metodo
        /// </summary>
        public Type Retorno { get; }

        public ILPropriedade(ILClasse iLClasse, string nome = null, AcessadorEnum acessador = AcessadorEnum.Publico, Type retorno = null, Type[] parametros = null)
        {
            if (iLClasse is null)
            {
                throw new ArgumentNullException(nameof(iLClasse));
            }

            Proxy = iLClasse.Proxy.Clone();
            Proxy.AdicionarBuilders(iLClasse.Proxy.ObterBuilder<TypeBuilder>());

            if (string.IsNullOrEmpty(nome))
            {
                nome = $"IL_Gerador_Propriedade_{Guid.NewGuid().ToString().Replace("-", "_", StringComparison.CurrentCulture)}";
            }

            if (retorno is null)
            {
                retorno = typeof(void);
            }

            Nome = nome;
            Acessador = (TokenEnum)acessador;
            Retorno = retorno;
            _parametros = parametros;

            var propertyBuilder = Proxy.ObterBuilder<TypeBuilder>().DefineProperty(nome, PropertyAttributes.HasDefault, retorno, parametros);
            Proxy.AdicionarBuilders(propertyBuilder);

            PilhaExecucao = new List<IILPilha>();

            Assinatura = string.Join(' ', acessador.ToString().ToLower(CultureInfo.CurrentCulture)) + " " + nome + string.Join(',', parametros.Select(p => p.FullName));
            Getter = default;
            Setter = default;

        }

        internal ILBuilderProxy Proxy { get; }
        internal List<IILPilha> PilhaExecucao { get; }
        internal string Assinatura { get; }

        internal ILMetodo Getter { get; set; }
        internal ILMetodo Setter { get; set; }

        public Type[] Parametros => _parametros;

        public void Executar()
        {
            if (!Getter.IsDefault())
            {
                Proxy.ObterBuilder<PropertyBuilder>().SetGetMethod(Getter.Proxy.ObterBuilder<MethodBuilder>());
            }
            else
            {
                throw new InvalidOperationException("Obrigatório existir o acessador get");
            }

            if (!Setter.IsDefault())
            {
                Proxy.ObterBuilder<PropertyBuilder>().SetSetMethod(Setter.Proxy.ObterBuilder<MethodBuilder>());
            }
            

            foreach (var executor in PilhaExecucao)
            {
                executor.Executar();
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(".property  ");

            sb.Append($"instance {Retorno.Name} {Nome}");

            if (Parametros != Array.Empty<Type>())
            {
                sb.Append("(");
            }

            for (int i = 0; i < Parametros.Length; i++)
            {
                string spl = ", ";
                if (i + 1 == Parametros.Length)
                {
                    spl = string.Empty;
                }

                sb.Append($"{Parametros[i].Name} {spl}");
            }

            if (Parametros != Array.Empty<Type>())
            {
                sb.Append(")");
            }

            sb.AppendLine("{");

            if (!Getter.IsDefault())
            {
                sb.Append($".get instance {Getter.Retorno.Name} {Getter.Nome}(");

                if (Getter.Parametros != Array.Empty<Type>())
                {
                    foreach (Type parametro in Getter.Parametros)
                    {
                        sb.Append($"{parametro}");
                    }
                }

                sb.AppendLine(")");
            }

            if (!Setter.IsDefault())
            {
                sb.Append($".set instance {Setter.Retorno.Name} {Setter.Nome}(");

                if (Setter.Parametros != Array.Empty<Type>())
                {
                    foreach (Type parametro in Setter.Parametros)
                    {
                        sb.Append($"{parametro}");
                    }
                }

                sb.AppendLine(")");
            }

            sb.AppendLine("}");

            return sb.ToString();
        }
    }
}
