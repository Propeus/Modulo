using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

using Propeus.Module.IL.Core.Enums;
using Propeus.Module.IL.Core.Helpers;
using Propeus.Module.IL.Core.Interfaces;
using Propeus.Module.IL.Core.Proxy;
using Propeus.Module.IL.Geradores;

using static Propeus.Module.IL.Core.Proxy.ILBuilderProxy;

namespace Propeus.Module.IL.Core.Geradores
{
    /// <summary>
    /// Gera uma classe em IL
    /// </summary>
    internal class ILClasse : IILExecutor, IDisposable
    {

        /// <summary>
        /// Modificadores de acesso
        /// <para>
        /// Para mais informações acesse este <see href="https://learn.microsoft.com/pt-br/dotnet/csharp/programming-guide/classes-and-structs/access-modifiers">Manual</see>
        /// </para>
        /// </summary>
        private readonly Token[] modificadorAcesso;

        /// <summary>
        /// Instancia para criar uma classe
        /// </summary>
        /// <param name="IlProxy">Gerador de IL atual</param>
        /// <param name="className">ClassName da classe</param>
        /// <param name="namespace">Namespace da classe</param>
        /// <param name="baseType">Objeto a ser estendido para classe</param>
        /// <param name="interfaces">Interface a ser implementado na classe</param>
        /// <param name="modificadorAcesso">Tokens da classe</param>
        /// <param name="atributos">Tipos herdados de <see cref="Attribute"/></param>
        public ILClasse(IILBuilderProxyScope IlProxy, string className, string @namespace, Type? baseType = null, Type[]? interfaces = null, Token[]? modificadorAcesso = null, Type[]? atributos = null)
        {
            Proxy = IlProxy;

            Methods = new List<ILMethodComponent>();
            Fields = new List<ILFieldComponent>();
            Properties = new List<ILPropertyComponent>();

            baseType ??= typeof(object);
            interfaces ??= Array.Empty<Type>();
            modificadorAcesso ??= new Token[] { Token.Classe, Token.Publico };

            ClassName = className;
            Namespace = @namespace;
            TypeBase = baseType;
            Interfaces = new List<Type>(interfaces);
            this.modificadorAcesso = modificadorAcesso;

            List<TypeAttributes> typeAttributes = new();
            foreach (Token item in modificadorAcesso)
            {
                typeAttributes.Add((TypeAttributes)Enum.Parse(typeof(TypeAttributes), item.GetEnumDescription()));
            }
            TypeBuilder typeBuilder;

            ModuleBuilder builder = Proxy.GetBuilder<ModuleBuilder>() ?? throw new InvalidOperationException($"O tipo {nameof(ModuleBuilder)} não foi encontrado no proxy");

            typeBuilder = Namespace != null
                ? builder.DefineType(Namespace + "." + className, typeAttributes.ToArray().JoinEnums(), baseType, interfaces)
                : builder.DefineType(className, typeAttributes.ToArray().JoinEnums(), baseType, interfaces);

            if (atributos != null)
            {
                foreach (Type atributo in atributos)
                {
                    //Pode dar erro de construtor padrão até porque eu nao sei quais sao os valores de parâmetro. (Na real estou com preguiça de fazer essa parte)
                    CustomAttributeBuilder atributoCopia = new CustomAttributeBuilder(atributo.GetConstructors().First(), Array.Empty<object>());
                    typeBuilder.SetCustomAttribute(atributoCopia);
                }
            }
            Proxy.RegisterBuilders(typeBuilder);
        }

        /// <summary>
        /// Nome da classe
        /// </summary>
        public string ClassName { get; }

        /// <summary>
        /// Namespace da classe
        /// </summary>
        public string? Namespace { get; }

        /// <summary>
        /// Tipo estendido da classe
        /// </summary>
        public Type TypeBase { get; }

        /// <summary>
        /// Tipo gerado da classe
        /// </summary>
        public Type? DynamicTypeClass { get; private set; }

        /// <summary>
        /// Classe de builders
        /// </summary>
        internal IILBuilderProxyScope Proxy { get; private set; }
        /// <summary>
        /// Lista de métodos da classe
        /// </summary>
        internal List<ILMethodComponent> Methods { get; private set; }
        /// <summary>
        /// Lista de propriedades da classe
        /// </summary>
        internal List<ILPropertyComponent> Properties { get; private set; }
        /// <summary>
        /// Lista de campos da classe
        /// </summary>
        internal List<ILFieldComponent> Fields { get; private set; }
        /// <summary>
        /// Lista de interfaces da classe
        /// </summary>
        internal List<Type> Interfaces { get; set; }


        ///<inheritdoc/>
        public void Apply()
        {
            foreach (ILFieldComponent campo in Fields)
            {
                campo.Apply();
            }

            foreach (ILMethodComponent metodo in Methods)
            {
                metodo.Apply();
            }

            foreach (ILPropertyComponent propriedade in Properties)
            {
                propriedade.Apply();
            }

            var _builder = Proxy.GetBuilder<TypeBuilder>() ?? throw new InvalidOperationException($"O tipo {nameof(TypeBuilder)} não foi encontrado no proxy");
            DynamicTypeClass = _builder.CreateType();
        }

        /// <summary>
        /// Obtém a construção da classe em IL
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            _ = sb.Append('.');

            foreach (Token item in (Token[])modificadorAcesso.Clone())
            {
                _ = sb.Append(item.GetEnumDescription().ToLower(CultureInfo.CurrentCulture)).Append(' ');
            }

            _ = sb.Append("auto ")
             .Append("ansi ")
             .Append("beforefieldinit ")
             .Append(ClassName)
             .Append(" extends ")
             .Append(TypeBase.FullName)
             .AppendLine()
             .Append('{')
             .AppendLine()
             .AppendLine()
             ;

            foreach (ILFieldComponent campo in Fields)
            {
                _ = sb.AppendLine(campo.ToString());
            }

            foreach (ILMethodComponent metodo in Methods)
            {
                _ = sb.AppendLine(metodo.ToString());
            }

            foreach (ILPropertyComponent propriedade in Properties)
            {
                _ = sb.AppendLine(propriedade.ToString());
            }

            _ = sb.AppendLine("}");

            return sb.ToString();
        }

        private bool disposedValue;
        /// <summary>
        /// Libera todos os construtores de classe, métodos, campos e propriedades da memoria
        /// </summary>
        /// <param name="disposing">Indica se deve ser liberado todos os objetos gerenciados</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                    Proxy.Dispose();

                    foreach (ILFieldComponent campo in Fields)
                    {
                        campo.Dispose();
                    }
                    Fields.Clear();

                    foreach (ILMethodComponent metodo in Methods)
                    {
                        metodo.Dispose();
                    }
                    Methods.Clear();

                    foreach (ILPropertyComponent propriedade in Properties)
                    {
                        propriedade.Dispose();
                    }
                    Properties.Clear();
                }


                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Não altere este código. Coloque o código de limpeza no método 'Dispose(bool disposing)'
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

    }
}