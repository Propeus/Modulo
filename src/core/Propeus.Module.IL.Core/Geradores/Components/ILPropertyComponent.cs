using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

using Propeus.Module.IL.Core.Enums;
using Propeus.Module.IL.Core.Geradores.Components;
using Propeus.Module.IL.Core.Interfaces;
using Propeus.Module.IL.Core.Proxy;

using static Propeus.Module.IL.Core.Proxy.ILBuilderProxy;

namespace Propeus.Module.IL.Geradores
{
    internal static partial class Constantes
    {
        public const string CONST_NME_PROPRIEDADE = "IL_Gerador_{0}_Propriedade_";

        public const string CONST_NME_PROPRIEDADE_METODO_GET = "get_";
        public const string CONST_NME_PROPRIEDADE_METODO_SET = "set_";

    }
    /// <summary>
    /// Gerador de propriedade
    /// </summary>
    internal class ILPropertyComponent : ILComponent, IILExecutor, IDisposable
    {
        /// <summary>
        /// Proxy builder do componente
        /// </summary>
        internal PropertyBuilder? Builder => this.BuilderProxy.GetBuilder<PropertyBuilder>();

        /// <summary>
        /// Indica se o componente atual é um proxy para outra propriedade
        /// </summary>
        internal bool IsProxy { get; set; }
        /// <summary>
        /// Componente de método para o acessado get
        /// </summary>
        internal ILMethodComponent? Getter { get; set; }
        /// <summary>
        /// Componente de método para o acessado set
        /// </summary>
        internal ILMethodComponent? Setter { get; set; }

        /// <summary>
        /// Lista de parâmetros da propriedade
        /// </summary>
        private ILParametro[] Parameters { get;  set; }

        /// <summary>
        /// Cria uma nova propriedade
        /// </summary>
        /// <param name="iLBuilderProxy">Builder da classe</param>
        /// <param name="name">Nome do método a ser criado</param>
        /// <param name="modifyAccess">Modificador de acesso do método</param>
        /// <param name="type">Tipo do retorno do método</param>
        /// <param name="parameters">Parâmetros do método se houver</param>
        /// <exception cref="NotImplementedException">O builder proxy nao possui o <see cref="TypeBuilder"/> </exception>
        [SetsRequiredMembers]
        public ILPropertyComponent(IILBuilderProxyScope iLBuilderProxy, Token[] modifyAccess, Type type, string name, ILParametro[]? parameters = null) : base(iLBuilderProxy, modifyAccess, type, name)
        {
           
            Parameters = parameters ?? Array.Empty<ILParametro>();


            var typeBuilder = this.BuilderProxy.GetBuilder<TypeBuilder>() ?? throw new NotImplementedException();
            var propertyBuilder = typeBuilder.DefineProperty(name, PropertyAttributes.HasDefault, type, parameters?.Select(x=> x.Tipo).ToArray());
            this.BuilderProxy.RegisterBuilders(propertyBuilder);

       
            Getter = default;
            Setter = default;
        }

        ///<inheritdoc/>
        /// <exception cref="ObjectDisposedException">Componente liberado da memoraria</exception>
        /// <exception cref="InvalidOperationException">O Builder está nulo</exception>
        /// <exception cref="InvalidOperationException">O Builder do Getter está nulo</exception>
        /// <exception cref="InvalidOperationException">O Builder do Setter está nulo</exception>
        public void Apply()
        {
            if(disposedValue)
                throw new ObjectDisposedException(GetType().FullName);

            if(Builder is null)
                throw new InvalidOperationException(nameof(Builder));

            if (Getter != null)
            {
                if (Getter.Builder is null)
                    throw new InvalidOperationException(nameof(Getter.Builder));

                Getter.Apply();
                Builder.SetGetMethod(Getter.Builder);
            }

            if (Setter != null)
            {
                if (Setter.Builder is null)
                    throw new InvalidOperationException(nameof(Setter.Builder));

                Setter.Apply();
                Builder.SetSetMethod(Setter.Builder);
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new();

            _ = sb.Append(".property ");
            _ = sb.Append($"instance {Type.Name} {Name}");
            _ = sb.Append("(");
            for (int i = 0; i < Parameters.Length; i++)
            {
                string spl = ", ";
                if (i + 1 == Parameters.Length)
                {
                    spl = string.Empty;
                }

                _ = sb.Append($"{Parameters[i].Nome} {spl}");
            }


            _ = sb.Append(")");


            _ = sb.AppendLine("{");

            if (Getter != null)
            {
                _ = sb.Append($".get instance {Getter.Type.Name} {Getter.Name}(");

                if (Getter.Parameters.Length != 0)
                {
                    foreach (Type parametro in Getter.Parameters)
                    {
                        _ = sb.Append($"{parametro}");
                    }
                }

                _ = sb.AppendLine(")");
            }

            if (Setter != null)
            {
                _ = sb.Append($".set instance {Setter.Type.Name} {Setter.Name}(");

                if (Setter.Parameters.Length != 0)
                {
                    foreach (Type parametro in Setter.Parameters)
                    {
                        _ = sb.Append($"{parametro}");
                    }
                }

                _ = sb.AppendLine(")");
            }

            _ = sb.AppendLine("}");

            return sb.ToString();
        }

        private bool disposedValue;

        /// <summary>
        /// Descarta todos os objetos referentes a propriedade
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Getter?.Dispose();
                    Setter?.Dispose();
                }

                Parameters = Array.Empty<ILParametro>();
                
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