using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

using Propeus.Modulo.IL.Core.Enums;
using Propeus.Modulo.IL.Core.Helpers;
using Propeus.Modulo.IL.Core.Interfaces;
using Propeus.Modulo.IL.Core.Proxy;
using Propeus.Modulo.IL.Geradores;

namespace Propeus.Modulo.IL.Core.Geradores
{
    /// <summary>
    /// Gera uma classe em IL
    /// </summary>
    internal class ILClasse : IILExecutor, IDisposable
    {
        private Token[] _acessadores;
        private readonly Type[] _atributos;

        /// <summary>
        /// Instancia para criar uma classe
        /// </summary>
        /// <param name="IlProxy">Gerador de IL atual</param>
        /// <param name="nome">Nome da classe</param>
        /// <param name="namespace">Namespace da classe</param>
        /// <param name="base">Objeto a ser extendido para classse</param>
        /// <param name="interfaces">Interface a ser implementado na classe</param>
        /// <param name="acessadores">Acessadores da classe</param>
        public ILClasse(ILBuilderProxy IlProxy, string nome, string @namespace, Type @base = null, Type[] interfaces = null, Token[] acessadores = null, Type[] atributos = null)
        {
            Proxy = IlProxy.Clone();

            Metodos = new List<ILMetodo>();
            Campos = new List<ILCampo>();
            Propriedades = new List<ILPropriedade>();

            @base ??= typeof(object);
            interfaces ??= Array.Empty<Type>();
            acessadores ??= new Token[] { Token.Classe, Token.Publico };

            Nome = nome;
            Namespace = @namespace;
            Base = @base;
            Interfaces = new List<Type>(interfaces);
            _acessadores = acessadores;
            _atributos = atributos;


            List<TypeAttributes> typeAttributes = new();
            foreach (Token item in acessadores)
            {
                typeAttributes.Add((TypeAttributes)Enum.Parse(typeof(TypeAttributes), item.ObterDescricaoEnum()));
            }
            TypeBuilder typeBuilder;

            ModuleBuilder builder = Proxy.ObterBuilder<ModuleBuilder>();

            typeBuilder = Namespace != null
                ? builder.DefineType(Namespace + "." + nome, typeAttributes.ToArray().ConcatenarEnum(), @base, interfaces)
                : builder.DefineType(nome, typeAttributes.ToArray().ConcatenarEnum(), @base, interfaces);


            if (_atributos != null)
            {
                foreach (Type item in _atributos)
                {
                    //Pode dar erro de construtor padrao até proque eu nao sei quais sao os valores de parametro. (Na real estou com preguiça de fazer essa parte)
                    CustomAttributeBuilder attrbuilder = new CustomAttributeBuilder(item.GetConstructors().First(), Array.Empty<object>());
                    typeBuilder.SetCustomAttribute(attrbuilder);
                }
            }
            Proxy.RegistrarBuilders(typeBuilder);
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
        /// Tipo gerado da classe
        /// </summary>
        public Type TipoGerado { get; private set; }

        internal ILBuilderProxy Proxy { get; private set; }
        internal List<ILMetodo> Metodos { get; private set; }
        internal List<ILPropriedade> Propriedades { get; private set; }
        internal List<ILCampo> Campos { get; private set; }
        internal List<Type> Interfaces { get; set; }


        ///<inheritdoc/>
        public void Executar()
        {
            foreach (ILCampo campo in Campos)
            {
                campo.Executar();
            }

            foreach (ILMetodo metodo in Metodos)
            {
                metodo.Executar();
            }

            foreach (ILPropriedade propriedade in Propriedades)
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
            StringBuilder sb = new();

            _ = sb.Append($".");

            foreach (Token item in (Token[])_acessadores.Clone())
            {
                _ = sb.Append(item.ObterDescricaoEnum().ToLower(CultureInfo.CurrentCulture)).Append(" ");
            }

            _ = sb.Append("auto ")
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

            foreach (ILCampo campo in Campos)
            {
                _ = sb.AppendLine(campo.ToString());
            }

            foreach (ILMetodo metodo in Metodos)
            {
                _ = sb.AppendLine(metodo.ToString());
            }

            foreach (ILPropriedade propriedade in Propriedades)
            {
                _ = sb.AppendLine(propriedade.ToString());
            }

            _ = sb.AppendLine("}");

            return sb.ToString();
        }


        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Proxy.ObterBuilder<TypeBuilder>().DisposeTypeBuilder();
                    Proxy.Dispose();

                    foreach (ILCampo campo in Campos)
                    {
                        campo.Dispose();
                    }
                    Campos.Clear();

                    foreach (ILMetodo metodo in Metodos)
                    {
                        metodo.Dispose();
                    }
                    Metodos.Clear();

                    foreach (ILPropriedade propriedade in Propriedades)
                    {
                        propriedade.Dispose();
                    }
                    Propriedades.Clear();

                }

                Proxy = null;
                Campos = null;
                Metodos = null;
                Propriedades = null;
                Interfaces = null;
                _acessadores = null;

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