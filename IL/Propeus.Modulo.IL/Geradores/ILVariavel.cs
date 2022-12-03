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
    public class ILVariavel : IILExecutor
    {
        public ILVariavel(ILMetodo iLMetodo, Type retorno = null, string nome = null)
        {
            if (iLMetodo.IsDefault())
            {
                throw new ArgumentNullException(nameof(iLMetodo));
            }

            Proxy = iLMetodo.Proxy.Clone(true);
            Proxy.AdicionarBuilders(iLMetodo.Proxy.ObterBuilder<MethodBuilder>());

            if (string.IsNullOrEmpty(nome))
            {
                nome = $"IL_Gerador_Variavel_{Guid.NewGuid().ToString().Replace("-", "_", StringComparison.CurrentCulture)}";
            }

            if (retorno is null)
            {
                retorno = typeof(object);
            }

            Nome = nome;
            Retorno = retorno;

            var fld = Proxy.DeclareLocal(retorno);
            Proxy.AdicionarBuilders(fld);
            Indice = fld.LocalIndex;

            Assinatura = retorno + nome;
        }

        public ILBuilderProxy Proxy { get; }
        public string Nome { get; }
        public Type Retorno { get; }
        public string Assinatura { get; }

        public int Indice { get; }

        public void Executar()
        {
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("[")
                .Append(Proxy.ObterBuilder<LocalBuilder>().LocalIndex)
                .Append("] ");

            sb.Append(Retorno.Name.ToLower(CultureInfo.CurrentCulture));
            sb.Append(" ");
            sb.AppendLine();

            return sb.ToString();
        }
    }
}