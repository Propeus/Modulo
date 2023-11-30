using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;

using Propeus.Module.IL.Core.Helpers;
using Propeus.Module.IL.Geradores;

namespace Propeus.Module.IL.Core.Proxy
{
    /// <summary>
    /// Abstração de <see cref="PropertyBuilder"/>, <see cref="MethodBuilder"/>, <see cref="FieldBuilder"/>, <see cref="EventBuilder"/> em um único objeto
    /// </summary>
    internal class ILBuilderProxy : IDisposable
    {
        /// <summary>
        /// Define o tamanho inicial do dicionário.
        /// </summary>
        private const int INITIAL_CAPACITY_DICTIONARY = 200;

        public interface IILBuilderProxyScope : IDisposable
        {
            TBuilder? GetBuilder<TBuilder>();
            void RegisterBuilders(params object?[] builders);
            IILBuilderProxyScope CreateScope();
        }

        internal class ILBuilderProxyScope : IILBuilderProxyScope
        {

            private string _key;
            private ILBuilderProxy? _parent;


            public ILBuilderProxyScope(ILBuilderProxy iLBuilderProxy)
            {
                _key = Guid.NewGuid().ToString();
                _parent = iLBuilderProxy;
            }
            private ILBuilderProxyScope(string key, ILBuilderProxy iLBuilderProxy)
            {
                _key = key + "::" + Guid.NewGuid().ToString();
                _parent = iLBuilderProxy;
            }

            public TBuilder? GetBuilder<TBuilder>()
            {
                if (disposedValue)
                    throw new ObjectDisposedException(GetType().Name);

                Type currentTipeBuilder = typeof(TBuilder);

                if (_parent == null)
                    throw new InvalidOperationException("O ILBuilderProxy não foi encontrado");

                string parentId = _key.Split("::")[0];

                if (_parent.Builders.TryGetValue(parentId + currentTipeBuilder.Name, out var builder))
                {
                    return (TBuilder)builder;
                }
                else if (_parent.Builders.TryGetValue(_key + currentTipeBuilder.Name, out builder))
                {
                    return (TBuilder)builder;
                }
                else if (_parent.Builders.TryGetValue(currentTipeBuilder.Name, out builder))
                {
                    return (TBuilder)builder;
                }

                return default;
            }

            /// <summary>
            /// Adiciona um ou mais builders no proxy
            /// </summary>
            /// <remarks>
            /// O array pode conter diversos builders de tipos diferentes
            /// </remarks>
            /// <param name="builders">Um array de builders que seja <see cref="PropertyBuilder"/>, <see cref="MethodBuilder"/>, <see cref="FieldBuilder"/> ou <see cref="EventBuilder"/></param>
            /// <exception cref="ArgumentException">Caso ja exista um mesmo tipo registrado</exception>
            public void RegisterBuilders(params object?[] builders)
            {
                if (disposedValue)
                    throw new ObjectDisposedException(GetType().Name);

                if (_parent == null)
                    throw new InvalidOperationException("O ILBuilderProxy não foi encontrado");

                foreach (object? builder in builders)
                {

                    if (builder is null)
                    {
                        continue;
                    }

                    _parent.Builders.Add(_key + builder.GetType().Name, builder);

                }
            }

            private bool disposedValue;

            protected virtual void Dispose(bool disposing)
            {
                if (!disposedValue)
                {
                    if (disposing && _parent != null)
                    {

                        for (int i = 0; i < _parent.Builders.Count; i++)
                        {
                            string currentKey = _parent.Builders.ElementAt(i).Key;
                            string realKey = "";

                            if (currentKey.Length > _key.Length)
                            {
                                realKey = currentKey.Remove(0, _key.Length);
                                if (_parent.Builders.ContainsKey(_key + realKey))
                                {
                                    if (realKey == typeof(MethodBuilder).Name)
                                    {
                                        (_parent.Builders[_key + realKey] as MethodBuilder)?.DisposeMethod();
                                    }
                                    if (_parent.Builders.Remove(_key + realKey))
                                    {
                                        i = 0;
                                    }
                                }
                            }

                        }

                    }

                    _parent = null;
                    disposedValue = true;
                }
            }

            public void Dispose()
            {
                // Não altere este código. Coloque o código de limpeza no método 'Dispose(bool disposing)'
                Dispose(disposing: true);
                GC.SuppressFinalize(this);
            }

            public IILBuilderProxyScope CreateScope()
            {
                if (disposedValue)
                    throw new ObjectDisposedException(GetType().Name);
                if (_parent == null)
                    throw new InvalidOperationException("O ILBuilderProxy não foi encontrado");

                return new ILBuilderProxyScope(_key, _parent);
            }
        }

        private LinkedList<IILBuilderProxyScope> _proxyList;

        /// <summary>
        /// Inicializa o builder vazio
        /// </summary>
        public ILBuilderProxy()
        {
            Builders = new Dictionary<string, object>(INITIAL_CAPACITY_DICTIONARY);
            _proxyList = new LinkedList<IILBuilderProxyScope>();
        }
        /// <summary>
        /// Inicializa o builder com ao menos um objeto
        /// </summary>
        /// <param name="builder">Um builder que seja <see cref="PropertyBuilder"/>, <see cref="MethodBuilder"/>, <see cref="FieldBuilder"/> ou <see cref="EventBuilder"/></param>
        private ILBuilderProxy(object builder) : this()
        {
            Builders.Add(builder.GetType().Name, builder);
        }
        /// <summary>
        /// Inicializa o builder com um array de builders
        /// </summary>
        /// <remarks>
        /// O array pode conter diversos builders de tipos diferentes
        /// </remarks>
        /// <param name="builders">Um array de builders que seja <see cref="PropertyBuilder"/>, <see cref="MethodBuilder"/>, <see cref="FieldBuilder"/> ou <see cref="EventBuilder"/></param>
        public ILBuilderProxy(object[] builders) : this()
        {
            RegisterBuilders(builders);
        }
        /// <summary>
        /// Instância a <see cref="ILBuilderProxy"/> adicionando o <see cref="MethodBuilder"/>
        /// </summary>
        /// <param name="builder">Um builder com escopo para método</param>
        public ILBuilderProxy(MethodBuilder builder) : this((object)builder)
        {
            ILGenerator = builder.GetILGenerator();
        }


        /// <summary>
        /// Dicionário de builders
        /// </summary>
        private readonly Dictionary<string, object> Builders;
        /// <summary>
        /// Dicionário com a pilha de instruções mapeadas
        /// </summary>
        private readonly Dictionary<string, string?> Stack = new Dictionary<string, string?>();

        /// <summary>
        /// Gerador de objetos a partir de instruções IL
        /// </summary>
        internal ILGenerator? ILGenerator { get; private set; }

        /// <summary>
        /// Obtém a builder passado no parâmetro do construtor
        /// </summary>
        /// <typeparam name="TBuilder">Tipo do builder podendo ser <see cref="PropertyBuilder"/>, <see cref="MethodBuilder"/>, <see cref="FieldBuilder"/> ou <see cref="EventBuilder"/></typeparam>
        /// <returns>Retorna o builder quando encontrado caso contrário <see langword="default"/></returns>
        public TBuilder? GetBuilder<TBuilder>()
        {
            if (Builders.TryGetValue(typeof(TBuilder).Name, out object? builder))
            {
                return (TBuilder?)builder;
            }
            return default;
        }

        /// <summary>
        /// Adiciona um ou mais builders no proxy
        /// </summary>
        /// <remarks>
        /// O array pode conter diversos builders de tipos diferentes
        /// </remarks>
        /// <param name="builders">Um array de builders que seja <see cref="PropertyBuilder"/>, <see cref="MethodBuilder"/>, <see cref="FieldBuilder"/> ou <see cref="EventBuilder"/></param>
        /// <exception cref="ArgumentException">Caso ja exista um mesmo tipo registrado</exception>
        public void RegisterBuilders(params object?[] builders)
        {

            foreach (object? builder in builders)
            {

                if (builder is null)
                {
                    continue;
                }

                Builders.Add(builder.GetType().Name, builder);

            }
        }

        /**
         * NOTA: Antes que reclame o porque do if nos métodos "Emit", o Sonar e o visual studio enche o saco com propriedade possivelmente nula.
         * Se cria um métodos so para isso, e referencia nos métodos emit o aviso volta a encher o saco.
         * **/

        /// <summary>
        /// Registra no gerador de objetos a instrução IL
        /// </summary>
        /// <param name="code">Instrução IL</param>
        /// <exception cref="InvalidOperationException">Gerador de IL nulo</exception>
        internal void Emit(OpCode code)
        {
            if (ILGenerator == null)
                throw new InvalidOperationException("Forneça um gerador para executar comandos emit");

            ILGenerator.Emit(code);
            Stack.Add(ILGenerator.ILOffset.ToString(new CultureInfo("pt-BR")), code.ToString());
        }
        /// <summary>
        /// Registra no gerador de objetos a instrução IL
        /// </summary>
        /// <remarks>
        /// Normalmente este método é usado para execução de instruções que envolvem índice o armazenamento de valor
        /// </remarks>
        /// <param name="code">Instrução IL</param>
        /// <param name="value">Valor a ser aplicado na instrução IL</param>
        /// <exception cref="InvalidOperationException">Gerador de IL nulo</exception>
        internal void Emit(OpCode code, int value)
        {
            if (ILGenerator == null)
                throw new InvalidOperationException("Forneça um gerador para executar comandos emit");

            ILGenerator.Emit(code, value);
            Stack.Add(ILGenerator.ILOffset.ToString(new CultureInfo("pt-BR")), $"{code} {value}");
        }
        /// <summary>
        /// Registra no gerador de objetos a instrução IL
        /// </summary>
        /// <remarks>
        /// Normalmente este método é usado para execução de instruções que envolvem inicialização de objetos
        /// </remarks>
        /// <param name="code">Instrução IL</param>
        /// <param name="constructorInfo">Informações do construtor</param>
        /// <exception cref="ArgumentNullException">Parâmetro "constructorInfo" está nulo</exception>
        /// <exception cref="ArgumentNullException">A propriedade "DeclaringType" do parâmetro "constructorInfo" está nulo</exception>
        /// <exception cref="InvalidOperationException">Gerador de IL nulo</exception>
        internal void Emit(OpCode code, ConstructorInfo constructorInfo)
        {
            if (constructorInfo is null)
            {
                throw new ArgumentNullException(nameof(constructorInfo));
            }

            if (constructorInfo.DeclaringType == null)
            {
                throw new InvalidOperationException("DeclaringType está nulo");
            }

            if (ILGenerator == null)
                throw new InvalidOperationException("Forneça um gerador para executar comandos emit");

            ILGenerator.Emit(code, constructorInfo);

            IEnumerable<Type> constructorArguments = constructorInfo.GetTypeParams();
            string nameArguments = string.Empty;
            if (constructorArguments.Any())
            {
                nameArguments = string.Join(", ", constructorArguments.Select(p => p.Name)).ToLower();
            }


            Stack.Add(ILGenerator.ILOffset.ToString(new CultureInfo("pt-BR")), $"{code} {constructorInfo.DeclaringType.FullName}::{constructorInfo.Name}({nameArguments})");
        }

        /// <summary>
        /// Registra no gerador de objetos a instrução IL
        /// </summary>
        /// <remarks>
        /// Normalmente este método é usado para execução de instruções que envolvem chamada de métodos
        /// </remarks>
        /// <param name="code">Instrução IL</param>
        /// <param name="methodInfo">Informações do método</param>
        /// <exception cref="ArgumentNullException">Parâmetro "methodInfo" está nulo</exception>
        /// <exception cref="ArgumentNullException">A propriedade "DeclaringType" do parâmetro "methodInfo" está nulo</exception>
        /// <exception cref="InvalidOperationException">Gerador de IL nulo</exception>
        internal void Emit(OpCode code, MethodInfo methodInfo)
        {
            if (methodInfo is null)
            {
                throw new ArgumentNullException(nameof(methodInfo));
            }

            if (methodInfo.DeclaringType == null)
            {
                throw new InvalidOperationException("DeclaringType está nulo");
            }

            if (ILGenerator == null)
                throw new InvalidOperationException("Forneça um gerador para executar comandos emit");


            ILGenerator.Emit(code, methodInfo);


            Stack.Add(ILGenerator.ILOffset.ToString(new CultureInfo("pt-BR")), $"{code} {methodInfo.ReturnType.Name.ToLower(CultureInfo.CurrentCulture)} {methodInfo.DeclaringType.FullName}::{methodInfo.Name}");
        }
        /// <summary>
        /// Registra no gerador de objetos a instrução IL
        /// </summary>
        /// <remarks>
        /// Normalmente este método é usado para execução de instruções que envolvem construção de campos
        /// </remarks>
        /// <param name="code">Instrução IL</param>
        /// <param name="fieldBuilder">Informações do método</param>
        /// <exception cref="ArgumentNullException">Parâmetro "fieldBuilder" está nulo</exception>
        /// <exception cref="ArgumentNullException">A propriedade "DeclaringType" do parâmetro "fieldBuilder" está nulo</exception>
        /// <exception cref="InvalidOperationException">Gerador de IL nulo</exception>
        internal void Emit(OpCode code, FieldBuilder fieldBuilder)
        {
            if (fieldBuilder is null)
            {
                throw new ArgumentNullException(nameof(fieldBuilder));
            }

            if (fieldBuilder.DeclaringType == null)
            {
                throw new InvalidOperationException("DeclaringType está nulo");
            }

            if (ILGenerator == null)
                throw new InvalidOperationException("Forneça um gerador para executar comandos emit");



            ILGenerator.Emit(code, fieldBuilder);
            Stack.Add(ILGenerator.ILOffset.ToString(new CultureInfo("pt-BR")), $"{code} {fieldBuilder.FieldType.Name.ToLower(CultureInfo.CurrentCulture)} {fieldBuilder.DeclaringType.FullName}::{fieldBuilder.Name}");
        }


        /// <summary>
        /// Cria uma nova instancia da classe passando somente o <see cref="AssemblyBuilder"/>, <see cref="ModuleBuilder"/> e <see cref="TypeBuilder"/>
        /// </summary>
        /// <returns>Retorna um clone deste objeto com os builders principais </returns>
        public IILBuilderProxyScope CreateScope()
        {
            IILBuilderProxyScope proxy = new ILBuilderProxyScope(this);
            _proxyList.AddLast(proxy);
            return proxy;
        }

        /// <summary>
        /// Realiza uma conversão implícita do MethodBuilder para o ILBuilderProxy
        /// </summary>
        /// <remarks>
        /// Basicamente o MethodBuilder é incluído dentro de uma nova instancia de ILBuilderProxy
        /// </remarks>
        /// <param name="methodBuilder">Construtor de métodos</param>
        /// <returns>Uma nova instancia do ILBuilderProxy com o MethodBuilder incluso</returns>
        public static implicit operator ILBuilderProxy(MethodBuilder methodBuilder)
        {
            return new ILBuilderProxy(methodBuilder);
        }

        private bool disposedValue;

        /// <summary>
        /// Libera todas as referencias de construtores da memoria
        /// </summary>
        /// <param name="disposing">Indica se deve ser liberado objetos gerenciados</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach (var proxy in _proxyList)
                    {
                        proxy.Dispose();
                    }
                    _proxyList.Clear();

                    foreach (KeyValuePair<string, object> builder in Builders)
                    {
                        if (builder.Key == typeof(TypeBuilder).Name)
                        {
                            (builder.Value as TypeBuilder)?.DisposeTypeBuilder();
                        }
                        else if (builder.Key == typeof(MethodBuilder).Name)
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

        /// <summary>
        /// Libera todas as referencias de construtores da memoria
        /// </summary>
        public void Dispose()
        {
            // Não altere este código. Coloque o código de limpeza no método 'Dispose(bool disposing)'
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}