using Propeus.Modulo.IL.Enums;
using Propeus.Modulo.IL.Interfaces;
using Propeus.Modulo.IL.Proxy;
using Propeus.Modulo.Util.Enumerados;
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
    /// Cria um campo
    /// </summary>
    public class ILCampo : IILExecutor
    {
        public ILCampo(ILClasse iLClasse, TokenEnum[] acessadores = null, Type retorno = null, string nome = null)
        {
            if (iLClasse is null)
            {
                throw new ArgumentNullException(nameof(iLClasse));
            }

            Proxy = iLClasse.Proxy.Clone(true);
            Proxy.AdicionarBuilders(iLClasse.Proxy.ObterBuilder<TypeBuilder>());

            if (string.IsNullOrEmpty(nome))
            {
                nome = $"IL_Gerador_Campo_{Guid.NewGuid().ToString().Replace("-", "_", StringComparison.CurrentCulture)}";
            }

            if (acessadores is null)
            {
                acessadores = new TokenEnum[] { TokenEnum.Publico, TokenEnum.OcutarAssinatura };
            }

            if (retorno is null)
            {
                retorno = typeof(object);
            }

            Nome = nome;
            Retorno = retorno;
            _acessadores = acessadores;

            List<FieldAttributes> typeAttributes = new List<FieldAttributes>();
            foreach (var item in acessadores)
            {
                typeAttributes.Add((FieldAttributes)Enum.Parse(typeof(FieldAttributes), item.ObterDescricaoEnum()));
            }
            var fld = Proxy.ObterBuilder<TypeBuilder>().DefineField(nome, retorno, typeAttributes.ToArray().ConcatenarEnum());
            Proxy.AdicionarBuilders(fld);

            Assinatura = string.Join(' ', _acessadores.Select(a => a.ToString().ToLower(CultureInfo.CurrentCulture))) + " " + nome;
        }

        public ILBuilderProxy Proxy { get; }
        public string Nome { get; }
        public Type Retorno { get; }
        public string Assinatura { get; }

        private TokenEnum[] _acessadores;

        public void Executar()
        {
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("\t")
                .Append($".field ");

            foreach (var item in _acessadores)
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
    }
}