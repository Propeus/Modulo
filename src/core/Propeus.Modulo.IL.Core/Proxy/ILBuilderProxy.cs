using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;

using Propeus.Module.IL.Core.Helpers;

namespace Propeus.Module.IL.Core.Proxy
{
    /// <summary>
    /// Abstração de <see cref="PropertyBuilder"/>, <see cref="MethodBuilder"/>, <see cref="FieldBuilder"/>, <see cref="EventBuilder"/> em um unico objeto
    /// </summary>
    internal class ILBuilderProxy : IDisposable
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
            Builders.Add(builder.GetType(), builder);
        }

        public ILBuilderProxy(object[] builders) : this()
        {
            RegistrarBuilders(builders);
        }

        /// <summary>
        /// Instncia a <see cref="ILBuilderProxy"/> adicionando o <see cref="MethodBuilder"/>
        /// </summary>
        /// <param name="builder"></param>
        public ILBuilderProxy(MethodBuilder builder) : this((object)builder)
        {
            ILGenerator = builder.GetILGenerator();
        }


        private readonly Dictionary<Type, object> Builders;
        private readonly Dictionary<string, string> Stack = new();
        internal ILGenerator ILGenerator { get; private set; }

        /// <summary>
        /// Obtem a builder passado no parametro do construtor
        /// </summary>
        /// <typeparam name="TBuilder"></typeparam>
        /// <returns></returns>
        public TBuilder ObterBuilder<TBuilder>()
        {
            return Builders.ContainsKey(typeof(TBuilder)) ? (TBuilder)Builders[typeof(TBuilder)] : default;
        }
        /// <summary>
        /// Adiciona um novo builder no proxy
        /// </summary>
        /// <param name="builders"></param>
        public void RegistrarBuilders(params object[] builders)
        {

            foreach (object builder in builders)
            {

                if (builder is null)
                {
                    continue;
                }

                Builders.Add(builder.GetType(), builder);

            }
        }

        internal void Emit(OpCode code)
        {
            ILGenerator.Emit(code);
            Stack.Add(ILGenerator.ILOffset.ToString(new CultureInfo("pt-BR")), code.ToString());
        }

        internal void Emit(OpCode code, int v)
        {
            ILGenerator.Emit(code, v);
            Stack.Add(ILGenerator.ILOffset.ToString(new CultureInfo("pt-BR")), $"{code} {v}");
        }

        internal void Emit(OpCode code, ConstructorInfo ctor)
        {
            ILGenerator.Emit(code, ctor);

            IEnumerable<Type> parametros = ctor.ObterTipoParametros();
            string strParametros = null;
            if (parametros.Any())
            {
                strParametros = string.Join(", ", parametros.Select(p => p.Name)).ToLower();
            }

            Stack.Add(ILGenerator.ILOffset.ToString(new CultureInfo("pt-BR")), $"{code} {ctor.DeclaringType.FullName}::{ctor.Name}({strParametros})");
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

        /// <summary>
        /// Cria uma nova instancia da classe passando somente o <see cref="AssemblyBuilder"/> e <see cref="ModuleBuilder"/>
        /// </summary>
        /// <returns></returns>
        public ILBuilderProxy Clone()
        {
            ILBuilderProxy proxy = new();
            proxy.RegistrarBuilders(ObterBuilder<AssemblyBuilder>(), ObterBuilder<ModuleBuilder>(), ObterBuilder<TypeBuilder>());
            return proxy;
        }

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach (KeyValuePair<Type, object> builder in Builders)
                    {
                        if (builder.Key == typeof(TypeBuilder))
                        {
                            (builder.Value as TypeBuilder)?.DisposeTypeBuilder();
                        }
                        else if (builder.Key == typeof(MethodBuilder))
                        {
                            (builder.Value as MethodBuilder)?.DisposeMethod();
                        }

                    }

                    Builders.Clear();
                    Stack.Clear();
                }

                disposedValue = true;
            }
        }


        public void Dispose()
        {
            // Não altere este código. Coloque o código de limpeza no método 'DisposeMethod(bool disposing)'
            Dispose(disposing: true);
        }


        public static implicit operator ILBuilderProxy(MethodBuilder methodBuilder)
        {
            return new ILBuilderProxy(methodBuilder);
        }
    }
}