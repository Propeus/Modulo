using System.ComponentModel.DataAnnotations;
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
        public const string CONST_NME_CAMPO = "IL_Gerador_{0}_Campo_";
        public const string CONST_NME_CAMPO_PROXY = CONST_NME_CAMPO + "Proxy_";
    }

    /// <summary>
    /// Cria um campo
    /// </summary>
    internal class ILFieldComponent : ILComponent, IILExecutor, IDisposable
    {
        /// <summary>
        /// Proxy builder do componente
        /// </summary>
        internal FieldBuilder? Builder => BuilderProxy.GetBuilder<FieldBuilder>();

        /// <summary>
        /// Cria um novo campo para a classe ou método informado
        /// </summary>
        /// <param name="builderProxy">Proxy de construtores de classe ou método</param>
        /// <param name="modifyAccess">Tokens de acessibilidade.</param>
        /// <param name="fieldType">Tipo do campo</param>
        /// <param name="fieldName">Nome do campo</param>
        /// <exception cref="ArgumentNullException">O tipo do campo é nulo</exception>
        /// <exception cref="ArgumentNullException">O modificador de acesso é nulo</exception>
        [SetsRequiredMembers]
        public ILFieldComponent(IILBuilderProxyScope builderProxy, Token[] modifyAccess, Type fieldType, string fieldName) : base(builderProxy, modifyAccess, fieldType, fieldName)
        {
            List<FieldAttributes> typeAttributes = new();
            foreach (Token item in modifyAccess)
            {
                typeAttributes.Add((FieldAttributes)Enum.Parse(typeof(FieldAttributes), item.GetEnumDescription()));
            }

            base.BuilderProxy = builderProxy;
            var _builder = builderProxy.GetBuilder<TypeBuilder>() ?? throw new InvalidOperationException($"O tipo {nameof(TypeBuilder)} não foi encontrado no proxy");
            builderProxy.RegisterBuilders(_builder.DefineField(fieldName, fieldType, typeAttributes.ToArray().JoinEnums()));
        }


        ///<inheritdoc/>
        public void Apply()
        {
            //Nao faz nada
        }

        /// <summary>
        /// Exibe informações em IL do campo atual
        /// </summary>
        /// <returns>Informações em IL do campo atual</returns>
        public override string ToString()
        {
            StringBuilder sb = new();

            _ = sb.Append('\t')
                .Append($".field ");

            foreach (Token item in this.ModifyAccess)
            {
                _ = sb.Append(item.GetEnumDescription().ToLower(CultureInfo.CurrentCulture)).Append(' ');
            }

            if (Type != typeof(object) && Type != typeof(string) && Type.IsClass && !Type.IsPrimitive)
            {
                _ = sb.Append("class ");
            }

            _ = sb.Append(Type.Name.ToLower(CultureInfo.CurrentCulture));
            _ = sb.Append(' ');
            _ = sb.Append(Name);
            _ = sb.AppendLine();

            return sb.ToString();
        }

        private bool disposedValue;

        /// <summary>
        /// Libera o proxy da memoria
        /// </summary>
        /// <param name="disposing">Caso true, libera o proxy da memoria</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    BuilderProxy.Dispose();
                }
                disposedValue = true;
            }
        }

        /// <summary>
        /// Libera o objeto atual da memoria
        /// </summary>
        public void Dispose()
        {
            // Não altere este código. Coloque o código de limpeza no método 'Dispose(bool disposing)'
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

    }
}