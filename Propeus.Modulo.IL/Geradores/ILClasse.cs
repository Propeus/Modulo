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
using System.Security.Cryptography;
using System.Linq;

namespace Propeus.Modulo.IL.Geradores
{
    /// <summary>
    /// Gera uma classe em IL
    /// </summary>
    internal class ILClasse : IILExecutor, IDisposable
    {
        private Token[] _acessadores;

        /// <summary>
        /// Instancia para criar uma classe
        /// </summary>
        /// <param name="IlProxy">Gerador de IL atual</param>
        /// <param name="nome">Nome da classe</param>
        /// <param name="namespace">Namespace da classe</param>
        /// <param name="base">Objeto a ser extendido para classse</param>
        /// <param name="interfaces">Interface a ser implementado na classe</param>
        /// <param name="acessadores">Acessadores da classe</param>
        public ILClasse(ILBuilderProxy IlProxy, string nome, string @namespace, Type @base = null, Type[] interfaces = null, Token[] acessadores = null)
        {
            if (IlProxy is null)
            {
                throw new ArgumentNullException(nameof(IlProxy));
            }

            if (string.IsNullOrEmpty(nome))
            {
                throw new ArgumentException($"'{nameof(nome)}' não pode ser nulo nem vazio.", nameof(nome));
            }

            if (string.IsNullOrEmpty(@namespace))
            {
                throw new ArgumentException($"'{nameof(@namespace)}' não pode ser nulo nem vazio.", nameof(@namespace));
            }

            Proxy = IlProxy.Clone();



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
                acessadores = new Token[] { Token.Classe, Token.Publico };
            }

            Nome = nome;
            Namespace = @namespace;
            Base = @base;
            Interfaces = new List<Type>(interfaces);
            _acessadores = acessadores;
            Hash = GerarHashIlClasse(Nome, Namespace, Base, interfaces, acessadores)[..5];

            var builder = Proxy.ObterBuilder<ModuleBuilder>();
            List<TypeAttributes> typeAttributes = new List<TypeAttributes>();
            foreach (var item in acessadores)
            {
                typeAttributes.Add((TypeAttributes)Enum.Parse(typeof(TypeAttributes), item.ObterDescricaoEnum()));
            }
            TypeBuilder tiper = builder.DefineType(Namespace + "." + nome, typeAttributes.ToArray().ConcatenarEnum(), @base, interfaces);
            Proxy.RegistrarBuilders(tiper);


            Construtores = new List<ILMetodo>();
            Metodos = new List<ILMetodo>();
            Campos = new List<ILCampo>();
            Propriedades = new List<ILPropriedade>();
        }

        /// <summary>
        /// Identificador unico da classe
        /// </summary>
        public string Hash { get; }


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
        /// Acessadores da classe
        /// </summary>
        public Token[] GetAcessadores()
        {
            if (disposedValue)
                throw new ObjectDisposedException(GetType().FullName);

            return (Token[])_acessadores.Clone();
        }

        /// <summary>
        /// Tipo gerado da classe
        /// </summary>
        public Type TipoGerado { get; private set; }

        internal ILBuilderProxy Proxy { get; private set; }
        internal List<ILMetodo> Metodos { get; private set; }
        internal List<ILMetodo> Construtores { get; private set; }
        internal List<ILPropriedade> Propriedades { get; private set; }
        internal List<ILCampo> Campos { get; private set; }
        internal List<Type> Interfaces { get; set; }

        public bool Executado { get; private set; }


        /// <summary>
        /// Executa a montagem da classe
        /// </summary>
        public void Executar()
        {

            if (disposedValue)
                throw new ObjectDisposedException(GetType().FullName);

            if (Executado)
                return;

            if (TipoGerado is not null)
            {
                return;
            }

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
            Executado = true;
        }

        /// <summary>
        /// Obtem a construção da classe em IL
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (disposedValue)
                return string.Empty;

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

        /// <summary>
        /// Gera um hash unico para a classe dinamica
        /// </summary>
        /// <param name="nome"></param>
        /// <param name="namespace"></param>
        /// <param name="base"></param>
        /// <param name="interfaces"></param>
        /// <param name="acessadores"></param>
        /// <param name="versao"></param>
        /// <returns></returns>
        public static string GerarHashIlClasse(string nome, string @namespace = null, Type @base = null, Type[] interfaces = null, Token[] acessadores = null)
        {
            var sb = new StringBuilder();
            sb.Append(nome).Append(@namespace);

            if (@base != null)
            {
                sb.Append(@base.Name).Append(@base.Namespace);
            }

            if (interfaces != null)
            {
                foreach (Type @interface in interfaces)
                {
                    sb.Append(@interface.Name).Append(@interface.Namespace);
                }
            }

            if (acessadores != null)
            {
                foreach (var acessador in acessadores)
                {
                    sb.Append(acessador.ToString());
                }
            }

            var hash = sb.ToString().Hash();
            return hash;
        }

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Proxy.ObterBuilder<TypeBuilder>().Dispose();
                    Proxy.Dispose();

                    foreach (var campo in Campos)
                    {
                        campo.Dispose();
                    }
                    Campos.Clear();

                    foreach (var cosntrutor in Construtores)
                    {
                        cosntrutor.Dispose();
                    }
                    Construtores.Clear();

                    foreach (var metodo in Metodos)
                    {
                        metodo.Dispose();
                    }
                    Metodos.Clear();

                    foreach (var propriedade in Propriedades)
                    {
                        propriedade.Dispose();
                    }
                    Propriedades.Clear();


                    Proxy = null;
                    Campos = null;
                    Construtores = null;
                    Metodos = null;
                    Propriedades = null;
                    Interfaces = null;
                    _acessadores = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~ILClasse()
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