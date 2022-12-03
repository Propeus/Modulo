using Propeus.Modulo.IL.Enums;
using Propeus.Modulo.IL.Interfaces;
using Propeus.Modulo.IL.Proxy;

using Propeus.Modulo.Abstrato.Util;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace Propeus.Modulo.IL.Geradores
{
    /// <summary>
    /// Gera uma classe em IL
    /// </summary>
    public class ILClasse : IILExecutor
    {
        private readonly Type[] _interfaces;
        private readonly TokenEnum[] _acessadores;

        private ILBuilderProxy _proxy;

        /// <summary>
        /// Instancia para criar uma classe
        /// </summary>
        /// <param name="iLGerador">Gerador de IL atual</param>
        /// <param name="nome">Nome da classe</param>
        /// <param name="namespace">Namespace da classe</param>
        /// <param name="base">Objeto a ser extendido para classse</param>
        /// <param name="interfaces">Interface a ser implementado na classe</param>
        /// <param name="acessadores">Acessadores da classe</param>
        public ILClasse(ILGerador iLGerador, string nome = null, string @namespace = null, Type @base = null, Type[] interfaces = null, TokenEnum[] acessadores = null)
        {
            if (iLGerador is null)
            {
                throw new ArgumentNullException(nameof(iLGerador));
            }

            _proxy = iLGerador.Proxy;

            if (string.IsNullOrEmpty(nome))
            {
                nome = $"IL_Gerador_Classe_{Guid.NewGuid().ToString().Replace("-", "_", StringComparison.CurrentCulture)}";
            }

            if (string.IsNullOrEmpty(@namespace))
            {
                @namespace = "Propeus.IL.Gerador." + nome;
            }

            if (@base is null)
            {
                @base = typeof(object);
            }

            if (interfaces is null)
            {
                interfaces = Array.Empty<Type>();
            }

            if (acessadores is null)
            {
                acessadores = new TokenEnum[] { TokenEnum.Classe, TokenEnum.Publico };
            }

            Nome = nome;
            Namespace = @namespace;
            Base = @base;
            _interfaces = interfaces;
            _acessadores = acessadores;

            var builder = Proxy.ObterBuilder<ModuleBuilder>();
            List<TypeAttributes> typeAttributes = new List<TypeAttributes>();
            foreach (var item in acessadores)
            {
                typeAttributes.Add((TypeAttributes)Enum.Parse(typeof(TypeAttributes), item.ObterDescricaoEnum()));
            }
            var tiper = builder.DefineType(Namespace, typeAttributes.ToArray().ConcatenarEnum(), @base, interfaces);
            Proxy.AdicionarBuilders(tiper);

            Construtores = new List<ILMetodo>();
            Metodos = new List<ILMetodo>();
            Campos = new List<ILCampo>();
            Propriedades = new List<ILPropriedade>();
        }

        /// <summary>
        /// Nome da classe
        /// </summary>
        public string Nome { get; }

        /// <summary>
        /// Namespace da classe
        /// </summary>
        public string Namespace { get; }

        /// <summary>
        /// Tipo extendido da classe
        /// </summary>
        public Type Base { get; }

        /// <summary>
        /// Interfaces implementadas na classe
        /// </summary>
        public Type[] GetInterfaces()
        {
            return (Type[])_interfaces.Clone();
        }

        /// <summary>
        /// Acessadores da classe
        /// </summary>
        public TokenEnum[] GetAcessadores()
        {
            return (TokenEnum[])_acessadores.Clone();
        }

        /// <summary>
        /// Tipo gerado da classe
        /// </summary>
        public Type TipoGerado { get; private set; }

        internal ILBuilderProxy Proxy { get => _proxy; }
        internal List<ILMetodo> Metodos { get; }
        internal List<ILMetodo> Construtores { get; }
        internal List<ILPropriedade> Propriedades { get; }
        internal List<ILCampo> Campos { get; }

        /// <summary>
        /// Executa a montagem da classe
        /// </summary>
        public void Executar()
        {
            foreach (var campo in Campos)
            {
                campo.Executar();
            }

            foreach (var cosntrutor in Construtores)
            {
                cosntrutor.Executar();
            }

            foreach (var metodo in Metodos)
            {
                metodo.Executar();
            }

            foreach (var propriedade in Propriedades)
            {
                propriedade.Executar();
            }

            TipoGerado = Proxy.ObterBuilder<TypeBuilder>().CreateType();
        }

        /// <summary>
        /// Obtem a construção da classe em IL
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($".");

            foreach (var item in GetAcessadores())
            {
                sb.Append(item.ObterDescricaoEnum().ToLower(CultureInfo.CurrentCulture)).Append(" ");
            };

            sb.Append("auto ")
             .Append("ansi ")
             .Append("beforefieldinit ")
             .Append(Nome)
             .Append(" extends ")
             .Append(Base.FullName)
             .AppendLine()
             .Append("{")
             .AppendLine()
             .AppendLine()
             ;

            foreach (var campo in Campos)
            {
                sb.AppendLine(campo.ToString());
            }

            foreach (var construtor in Construtores)
            {
                sb.AppendLine(construtor.ToString());
            }

            foreach (var metodo in Metodos)
            {
                sb.AppendLine(metodo.ToString());
            }

            foreach (var propriedade in Propriedades)
            {
                sb.AppendLine(propriedade.ToString());
            }

            sb.AppendLine("}");

            return sb.ToString();
        }
    }
}