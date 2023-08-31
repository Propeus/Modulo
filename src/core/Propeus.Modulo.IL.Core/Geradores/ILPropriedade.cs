using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

using Propeus.Module.IL.Core.Interfaces;
using Propeus.Module.IL.Core.Proxy;

namespace Propeus.Modulo.IL.Geradores
{
    internal static partial class Constantes
    {
        public const string CONST_NME_PROPRIEDADE = "IL_Gerador_{0}_Propriedade_";

        public const string CONST_NME_PROPRIEDADE_METODO_GET = "get_";
        public const string CONST_NME_PROPRIEDADE_METODO_SET = "set_";

    }

    internal class ILPropriedade : IILExecutor, IDisposable
    {
        private PropertyBuilder _propriedadeBuilder;

        /// <summary>
        /// Nome do metodo
        /// </summary>
        public string Nome { get; }
        /// <summary>
        /// Retorno do metodo
        /// </summary>
        public Type Retorno { get; private set; }

        internal bool IsProxy { get; set; }
        internal ILCampo Campo { get; set; }
        internal ILMetodo Getter { get; set; }
        internal ILMetodo Setter { get; set; }

        public Type[] Parametros { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builderProxy"></param>
        /// <param name="nomeClasse"></param>
        /// <param name="nomePropriedade"></param>
        /// <param name="retorno"></param>
        /// <param name="parametros"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public ILPropriedade(ILBuilderProxy builderProxy, string nomeClasse, string nomePropriedade, Type retorno = null, Type[] parametros = null)
        {



            Nome = nomePropriedade;
            Retorno = retorno ?? throw new ArgumentNullException(nameof(retorno));
            Parametros = parametros ?? Array.Empty<Type>();


            _propriedadeBuilder = builderProxy.ObterBuilder<TypeBuilder>().DefineProperty(Nome, PropertyAttributes.HasDefault, retorno, parametros);

            Campo = default;
            Getter = default;
            Setter = default;

        }

        ///<inheritdoc/>
        public void Executar()
        {

            if (Getter != null)
            {
                Getter.Executar();
                _propriedadeBuilder.SetGetMethod(Getter._metodoBuilder);
            }

            if (Setter != null)
            {
                Setter.Executar();
                _propriedadeBuilder.SetSetMethod(Setter._metodoBuilder);
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new();

            _ = sb.Append(".property ");
            _ = sb.Append($"instance {Retorno.Name} {Nome}");
            _ = sb.Append("(");
            for (int i = 0; i < Parametros.Length; i++)
            {
                string spl = ", ";
                if (i + 1 == Parametros.Length)
                {
                    spl = string.Empty;
                }

                _ = sb.Append($"{Parametros[i].Name} {spl}");
            }


            _ = sb.Append(")");


            _ = sb.AppendLine("{");

            if (Getter != null)
            {
                _ = sb.Append($".get instance {Getter.Retorno.Name} {Getter.Nome}(");

                if (Getter.Parametros.Length != 0)
                {
                    foreach (Type parametro in Getter.Parametros)
                    {
                        _ = sb.Append($"{parametro}");
                    }
                }

                _ = sb.AppendLine(")");
            }

            if (Setter != null)
            {
                _ = sb.Append($".set instance {Setter.Retorno.Name} {Setter.Nome}(");

                if (Setter.Parametros.Length != 0)
                {
                    foreach (Type parametro in Setter.Parametros)
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

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                    Getter?.Dispose();
                    Setter?.Dispose();
                    Campo?.Dispose();

                    Parametros = null;
                    Retorno = null;

                    _propriedadeBuilder = null;
                }
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