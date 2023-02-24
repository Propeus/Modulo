using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

using Propeus.Modulo.Util;
using Propeus.Modulo.IL.Enums;
using Propeus.Modulo.IL.Interfaces;
using Propeus.Modulo.IL.Proxy;

namespace Propeus.Modulo.IL.Geradores
{
    /// <summary>
    /// Gera uma classe em IL
    /// </summary>
    public class ILClasse : IILExecutor, IDisposable
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

            Proxy = IlProxy.Clone();

            Construtores = new List<ILMetodo>();
            Metodos = new List<ILMetodo>();
            Campos = new List<ILCampo>();
            Propriedades = new List<ILPropriedade>();
            Delegates = new List<ILDelegate>();

            @base ??= typeof(object);
            interfaces ??= Array.Empty<Type>();
            acessadores ??= new Token[] { Token.Classe, Token.Publico };

            Nome = nome;
            Namespace = @namespace;
            Base = @base;
            Interfaces = new List<Type>(interfaces);
            _acessadores = acessadores;
            Hash = GerarHashIlClasse(Nome, Namespace, Base, interfaces, acessadores)[..5];

            List<TypeAttributes> typeAttributes = new();
            foreach (Token item in acessadores)
            {
                typeAttributes.Add((TypeAttributes)Enum.Parse(typeof(TypeAttributes), item.ObterDescricaoEnum()));
            }
            TypeBuilder typeBuilder;
            
            if (Proxy.ObterBuilder<TypeBuilder>() == null)
            {
                ModuleBuilder builder = Proxy.ObterBuilder<ModuleBuilder>();

                typeBuilder = Namespace != null
                    ? builder.DefineType(Namespace + "." + nome, typeAttributes.ToArray().ConcatenarEnum(), @base, interfaces)
                    : builder.DefineType(nome, typeAttributes.ToArray().ConcatenarEnum(), @base, interfaces);
            }
            else
            {
                TypeBuilder builder = Proxy.ObterBuilder<TypeBuilder>();
                typeBuilder = Namespace != null
                    ? builder.DefineNestedType(Namespace + "." + nome, typeAttributes.ToArray().ConcatenarEnum(), @base, interfaces)
                    : builder.DefineNestedType(nome, typeAttributes.ToArray().ConcatenarEnum(), @base, interfaces);
            }
            Proxy.RegistrarOuAtualizarBuilder(typeBuilder);
            



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
            return disposedValue ? throw new ObjectDisposedException(GetType().FullName) : (Token[])_acessadores.Clone();
        }

        /// <summary>
        /// Tipo gerado da classe
        /// </summary>
        public Type TipoGerado { get; private set; }

        internal ILBuilderProxy Proxy { get; private set; }
        internal List<ILMetodo> Metodos { get; private set; }
        internal List<ILMetodo> Construtores { get; private set; }
        internal List<ILPropriedade> Propriedades { get; private set; }
        internal List<ILDelegate> Delegates { get; private set; }
        internal List<ILCampo> Campos { get; private set; }
        internal List<Type> Interfaces { get; set; }

        private bool _executado;


        ///<inheritdoc/>
        public void Executar()
        {

            if (disposedValue)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }

            if (_executado)
            {
                return;
            }

            if (TipoGerado is not null)
            {
                return;
            }

            foreach (ILDelegate @delegate in Delegates)
            {
                @delegate.Executar();
            }

            foreach (ILCampo campo in Campos)
            {
                campo.Executar();
            }

            foreach (ILMetodo cosntrutor in Construtores)
            {
                cosntrutor.Executar();
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
            _executado = true;
        }

        /// <summary>
        /// Obtem a construção da classe em IL
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (disposedValue)
            {
                return string.Empty;
            }

            StringBuilder sb = new();

            _ = sb.Append($".");

            foreach (Token item in GetAcessadores())
            {
                _ = sb.Append(item.ObterDescricaoEnum().ToLower(CultureInfo.CurrentCulture)).Append(" ");
            };

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

            foreach (ILMetodo construtor in Construtores)
            {
                _ = sb.AppendLine(construtor.ToString());
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

        /// <summary>
        /// Gera um hash unico para a classe dinamica
        /// </summary>
        /// <param name="nome"></param>
        /// <param name="namespace"></param>
        /// <param name="base"></param>
        /// <param name="interfaces"></param>
        /// <param name="acessadores"></param>
        /// <returns></returns>
        public static string GerarHashIlClasse(string nome, string @namespace = null, Type @base = null, Type[] interfaces = null, Token[] acessadores = null)
        {
            StringBuilder sb = new();
            _ = sb.Append(nome).Append(@namespace);

            if (@base != null)
            {
                _ = sb.Append(@base.Name).Append(@base.Namespace);
            }

            if (interfaces != null)
            {
                foreach (Type @interface in interfaces)
                {
                    _ = sb.Append(@interface.Name).Append(@interface.Namespace);
                }
            }

            if (acessadores != null)
            {
                foreach (Token acessador in acessadores)
                {
                    _ = sb.Append(acessador.ToString());
                }
            }

            string hash = sb.ToString().Hash();
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

                    foreach (ILCampo campo in Campos)
                    {
                        campo.Dispose();
                    }
                    Campos.Clear();

                    foreach (ILMetodo cosntrutor in Construtores)
                    {
                        cosntrutor.Dispose();
                    }
                    Construtores.Clear();

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