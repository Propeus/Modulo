using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

using Propeus.Modulo.Abstrato.Util;

namespace Propeus.Modulo.IL.Proxy
{
    /// <summary>
    /// Abstração de <see cref="PropertyBuilder"/>, <see cref="MethodBuilder"/>, <see cref="FieldBuilder"/>, <see cref="EventBuilder"/> em um unico objeto
    /// </summary>
    public class ILBuilderProxy
    {
        /// <summary>
        /// Construtor simples
        /// </summary>
        public ILBuilderProxy()
        {
            Builders = new Dictionary<Type, object>();
        }

        private ILBuilderProxy(object builder) : this()
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            Builders.Add(builder.GetType(), builder);
        }

        /// <summary>
        /// Instncia a <see cref="ILBuilderProxy"/> adicionando o <see cref="PropertyBuilder"/>
        /// </summary>
        /// <param name="builder"></param>
        public ILBuilderProxy(PropertyBuilder builder) : this((object)builder)
        {
        }

        /// <summary>
        /// Instncia a <see cref="ILBuilderProxy"/> adicionando o <see cref="MethodBuilder"/>
        /// </summary>
        /// <param name="builder"></param>
        public ILBuilderProxy(MethodBuilder builder) : this((object)builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            ILGenerator = builder.GetILGenerator();
        }

        /// <summary>
        /// Instncia a <see cref="ILBuilderProxy"/> adicionando o <see cref="FieldBuilder"/>
        /// </summary>
        /// <param name="builder"></param>
        public ILBuilderProxy(FieldBuilder builder) : this((object)builder)
        {
        }

        /// <summary>
        /// Instncia a <see cref="ILBuilderProxy"/> adicionando o <see cref="EventBuilder"/>
        /// </summary>
        /// <param name="builder"></param>
        public ILBuilderProxy(EventBuilder builder) : this((object)builder)
        {
        }

        /// <summary>
        /// Instncia a <see cref="ILBuilderProxy"/> adicionando o <see cref="ConstructorBuilder"/>
        /// </summary>
        /// <param name="builder"></param>
        public ILBuilderProxy(ConstructorBuilder builder) : this((object)builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            ILGenerator = builder.GetILGenerator();
        }

        /// <summary>
        /// Instncia a <see cref="ILBuilderProxy"/> adicionando o <see cref="TypeBuilder"/>
        /// </summary>
        /// <param name="builder"></param>
        public ILBuilderProxy(TypeBuilder builder) : this((object)builder)
        {
        }

        /// <summary>
        /// Instncia a <see cref="ILBuilderProxy"/> adicionando o <see cref="AssemblyBuilder"/>
        /// </summary>
        /// <param name="builder"></param>
        public ILBuilderProxy(AssemblyBuilder builder) : this((object)builder)
        {
        }

        private readonly Dictionary<Type, object> Builders;
        private Dictionary<string, string> Stack = new Dictionary<string, string>();
        internal ILGenerator ILGenerator { get; private set; }

        /// <summary>
        /// Obtem a builder passado no parametro do construtor
        /// </summary>
        /// <typeparam name="TBuilder"></typeparam>
        /// <returns></returns>
        public TBuilder ObterBuilder<TBuilder>()
        {
            return (TBuilder)Builders[typeof(TBuilder)];
        }

        /// <summary>
        /// Adiciona um novo builder no proxy
        /// </summary>
        /// <param name="builders"></param>
        public void AdicionarBuilders(params object[] builders)
        {
            if (builders is null)
            {
                throw new ArgumentNullException(nameof(builders));
            }

            foreach (object builder in builders)
            {
                Builders.Add(builder.GetType(), builder);

                if (builder is MethodBuilder)
                {
                    ILGenerator = (builder as MethodBuilder).GetILGenerator();
                }
                else if (builder is ConstructorBuilder)
                {
                    ILGenerator = (builder as ConstructorBuilder).GetILGenerator();
                }
            }
        }

        internal void DefinirGeneratorPadrao(object builder)
        {
            if (builder is MethodBuilder)
            {
                ILGenerator = (builder as MethodBuilder).GetILGenerator();
            }
            else if (builder is ConstructorBuilder)
            {
                ILGenerator = (builder as ConstructorBuilder).GetILGenerator();
            }
            else
            {
                //TODO: Adicionar uma mensagem aqui
                throw new InvalidCastException();
            }
        }

        internal void Emit(OpCode code, Type valor)
        {
            ILGenerator.Emit(code, valor);
            Stack.Add(ILGenerator.ILOffset.ToString(new CultureInfo("pt-BR")), $"{code} [{valor.Namespace}]{valor.Name.ToLower(CultureInfo.CurrentCulture)}");
        }

        internal void Emit(OpCode code)
        {
            ILGenerator.Emit(code);
            Stack.Add(ILGenerator.ILOffset.ToString(new CultureInfo("pt-BR")), code.ToString());
        }

        internal void Emit(OpCode code, Label jump)
        {
            ILGenerator.Emit(code, jump);
            Stack.Add(ILGenerator.ILOffset.ToString(new CultureInfo("pt-BR")), $"{code} {jump}");
        }

        internal void Emit(OpCode code, LocalBuilder variavelLocal)
        {
            ILGenerator.Emit(code, variavelLocal);
            Stack.Add(ILGenerator.ILOffset.ToString(new CultureInfo("pt-BR")), $"{code} {variavelLocal}");
        }

        internal void Emit(OpCode code, int v)
        {
            ILGenerator.Emit(code, v);
            Stack.Add(ILGenerator.ILOffset.ToString(new CultureInfo("pt-BR")), $"{code} {v}");
        }

        internal void Emit(OpCode code, long v)
        {
            ILGenerator.Emit(code, v);
            Stack.Add(ILGenerator.ILOffset.ToString(new CultureInfo("pt-BR")), $"{code} {v}");
        }

        internal void Emit(OpCode code, float v)
        {
            ILGenerator.Emit(code, v);
            Stack.Add(ILGenerator.ILOffset.ToString(new CultureInfo("pt-BR")), $"{code} {v}");
        }

        internal void Emit(OpCode code, double v)
        {
            ILGenerator.Emit(code, v);
            Stack.Add(ILGenerator.ILOffset.ToString(new CultureInfo("pt-BR")), $"{code} {v}");
        }

        internal void Emit(OpCode code, ConstructorInfo ctor)
        {
            ILGenerator.Emit(code, ctor);
            Stack.Add(ILGenerator.ILOffset.ToString(new CultureInfo("pt-BR")), $"{code} {ctor.DeclaringType.FullName}::{ctor.Name}");
        }

        internal void Emit(OpCode code, MethodInfo mth)
        {
            ILGenerator.Emit(code, mth);
            Stack.Add(ILGenerator.ILOffset.ToString(new CultureInfo("pt-BR")), $"{code} {mth.ReturnType.Name.ToLower(CultureInfo.CurrentCulture)} {mth.DeclaringType.FullName}::{mth.Name}");
        }

        internal void Emit(OpCode code, FieldBuilder fieldBuilder)
        {
            ILGenerator.Emit(code, fieldBuilder);
            Stack.Add(ILGenerator.ILOffset.ToString(new CultureInfo("pt-BR")), $"{code} {fieldBuilder.FieldType.Name.ToLower(CultureInfo.CurrentCulture)} {fieldBuilder.DeclaringType.FullName}::{fieldBuilder.Name}");
        }

        internal void Emit(OpCode code, string v)
        {
            ILGenerator.Emit(code, v);
            Stack.Add(ILGenerator.ILOffset.ToString(new CultureInfo("pt-BR")), $"{code} {v}");
        }

        internal void MarkLabel(Label jump)
        {
            ILGenerator.MarkLabel(jump);
        }

        internal Label DefineLabel()
        {
            return ILGenerator.DefineLabel();
        }

        internal LocalBuilder DeclareLocal(Type tipo)
        {
            return ILGenerator.DeclareLocal(tipo);
        }

        internal FieldBuilder DefineField(string fieldName, Type type, FieldAttributes attributes)
        {
            FieldBuilder result = Builders[typeof(TypeBuilder)].To<TypeBuilder>().DefineField(fieldName, type, attributes);
            return result;
        }

        /// <summary>
        /// Cria uma nova instancia da classe passando somente o <see cref="AssemblyBuilder"/> e <see cref="ModuleBuilder"/>
        /// </summary>
        /// <returns></returns>
        public ILBuilderProxy Clone(bool stack = false)
        {
            ILBuilderProxy proxy = new ILBuilderProxy();
            proxy.AdicionarBuilders(ObterBuilder<AssemblyBuilder>(), ObterBuilder<ModuleBuilder>());
            if (stack)
                proxy.Stack = Stack;

            return proxy;
        }

        /// <summary>
        /// Exibe a pilha de execução do IL
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> item in Stack)
            {
                sb.Append("\t")
                    .Append("\t")
                    .Append(item.Key)
                    .Append(" ")
                    .Append(item.Value)
                    .AppendLine();
            }
            return sb.ToString();
        }
    }
}