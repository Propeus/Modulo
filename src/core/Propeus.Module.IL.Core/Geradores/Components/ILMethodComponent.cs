using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

using Propeus.Module.IL.Core.Enums;
using Propeus.Module.IL.Core.Geradores.Components;
using Propeus.Module.IL.Core.Helpers;
using Propeus.Module.IL.Core.Interfaces;
using Propeus.Module.IL.Core.Proxy;

using static Propeus.Module.IL.Core.Proxy.ILBuilderProxy;

namespace Propeus.Module.IL.Geradores
{


    internal static partial class Constantes
    {
        public const string CONST_NME_METODO = "IL_Gerador_{0}_Metodo_";
    }

    /// <summary>
    /// Gerador de método
    /// </summary>
    internal sealed class ILMethodComponent : ILComponent, IILExecutor, IDisposable
    {
        /// <summary>
        /// Proxy builder do componente
        /// </summary>
        internal MethodBuilder? Builder => this.BuilderProxy.GetBuilder<MethodBuilder>();


        /// <summary>
        /// Cria um novo método
        /// </summary>
        /// <param name="iLBuilderProxy">Builder da classe</param>
        /// <param name="name">Nome do método a ser criado</param>
        /// <param name="modifyAccess">Modificador de acesso do método</param>
        /// <param name="type">Tipo do retorno do método</param>
        /// <param name="parameters">Parâmetros do método se houver</param>
        [SetsRequiredMembers]
        public ILMethodComponent(IILBuilderProxyScope iLBuilderProxy, Token[] modifyAccess, Type type, string name, ILParametro[]? parameters = null) : base(iLBuilderProxy, modifyAccess, type, name)
        {

            Parameters = parameters ?? Array.Empty<ILParametro>();
            _executado = false;

            List<MethodAttributes> typeAttributes = new();
            foreach (Token item in modifyAccess)
            {
                typeAttributes.Add((MethodAttributes)Enum.Parse(typeof(MethodAttributes), item.GetEnumDescription()));
            }

            TypeBuilder typeBuilder = this.BuilderProxy.GetBuilder<TypeBuilder>() ?? throw new InvalidOperationException($"O tipo {nameof(TypeBuilder)} não foi encontrado no proxy");
            MethodBuilder methodBuilder = typeBuilder.DefineMethod(name, typeAttributes.ToArray().JoinEnums(), type, Helpers.Cast<Type>(Parameters).ToArray());
            this.BuilderProxy.RegisterBuilders(methodBuilder);

            for (int i = 0; i < Parameters.Length; i++)
            {
                Parameters[i].Indice = i + 1;
                if (Parameters[i].Opcional)
                {
                    if (Parameters[i].DefaultValue is null)
                    {
                        _ = methodBuilder.DefineParameter(Parameters[i].Indice, ParameterAttributes.In | ParameterAttributes.Optional | ParameterAttributes.HasDefault, Parameters[i].Nome);
                    }
                    else
                    {
                        ParameterBuilder paramBuilder = methodBuilder.DefineParameter(Parameters[i].Indice, ParameterAttributes.In | ParameterAttributes.Optional | ParameterAttributes.HasDefault, Parameters[i].Nome);
                        paramBuilder.SetConstant(Parameters[i].DefaultValue);
                    }
                }
                else
                {
                    _ = methodBuilder.DefineParameter(Parameters[i].Indice, ParameterAttributes.In, Parameters[i].Nome);
                }
            }
            StackExecution = new List<IILPilha>();
        }


        /// <summary>
        /// Parâmetros do método
        /// </summary>
        public ILParametro[] Parameters { get; private set; }

        /// <summary>
        /// Pilha de execução do método
        /// </summary>
        internal List<IILPilha> StackExecution { get; private set; }

        bool _executado;


        ///<inheritdoc/>
        public void Apply()
        {
            if (_executado)
            {
                return;
            }

            foreach (IILPilha pilha in StackExecution)
            {
                pilha.Apply();
            }

            _executado = true;
        }

        /// <summary>
        /// Exibe o metodo em IL
        /// </summary>
        /// <returns>Retorna o codigo IL do método</returns>
        public override string ToString()
        {

            StringBuilder sb = new();

            _ = sb.Append('\t')
                .Append($".method ");

            foreach (Token item in this.ModifyAccess)
            {
                _ = sb.Append(item.GetEnumDescription().ToLower(CultureInfo.CurrentCulture)).Append(' ');
            }

            _ = sb.Append("instance ")
             .Append(Type.Name.ToLower(CultureInfo.CurrentCulture))
             .Append(' ')
             .Append(Name)
             .Append(' ');

            if (Parameters.Length > 0)
            {
                _ = sb.Append('(')
                    .AppendLine();

                foreach (ILParametro parametro in Parameters)
                {
                    _ = sb.Append(parametro.Tipo.Name)
                        .Append(' ')
                        .Append(parametro.Nome)
                        .AppendLine();
                }
                _ = sb.Append(") ");
            }
            else
            {
                _ = sb.Append("() ");
            }


            _ = sb.AppendLine()
            .Append('\t')
            .Append('{')
            .AppendLine()
            .AppendLine();


            foreach (IILPilha pilha in StackExecution)
            {
                if (!string.IsNullOrEmpty(pilha.ToString()))
                {
                    _ = sb.AppendLine(pilha.ToString());
                }
            }

            _ = sb.Append('\t')
                .AppendLine("}");

            return sb.ToString();
        }

        private bool disposedValue;

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach (IILPilha item in StackExecution)
                    {
                        item.Dispose();
                    }

                    StackExecution.Clear();
                    Builder?.DisposeMethod();

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