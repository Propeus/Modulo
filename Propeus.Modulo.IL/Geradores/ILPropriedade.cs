using Propeus.Modulo.IL.Enums;
using Propeus.Modulo.IL.Interfaces;
using Propeus.Modulo.IL.Proxy;

using Propeus.Modulo.Abstrato.Util;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace Propeus.Modulo.IL.Geradores
{
    public static partial class Constantes
    {
        public const string CONST_NME_PROPRIEDADE = "IL_Gerador_{0}_Propriedade_";

        public const string CONST_NME_PROPRIEDADE_METODO_GET = "get_";
        public const string CONST_NME_PROPRIEDADE_METODO_SET = "set_";

        //Continuar construindo o propriedade helper

        public static string GerarNomePropriedade(string nomeClasse)
        {
            return Constantes.GerarNome(string.Format(CONST_NME_PROPRIEDADE, nomeClasse));
        }
    }

    public class ILPropriedade : IILExecutor, IDisposable
    {

        /// <summary>
        /// Nome do metodo
        /// </summary>
        public string Nome { get; private set; }
        /// <summary>
        /// Retorno do metodo
        /// </summary>
        public Type Retorno { get; private set; }
        public bool Executado { get; private set; }
        internal bool IsProxy { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builderProxy"></param>
        /// <param name="nomeClasse"></param>
        /// <param name="nomePropriedade"></param>
        /// <param name="retorno"></param>
        /// <param name="parametros"></param>
        public ILPropriedade(ILBuilderProxy builderProxy, string nomeClasse, string nomePropriedade = Constantes.CONST_NME_PROPRIEDADE, Type retorno = null, Type[] parametros = null)
        {
            if (builderProxy is null)
            {
                throw new ArgumentNullException(nameof(builderProxy));
            }

            if (string.IsNullOrEmpty(nomeClasse))
            {
                throw new ArgumentException($"'{nameof(nomeClasse)}' não pode ser nulo nem vazio.", nameof(nomeClasse));
            }

            if (string.IsNullOrEmpty(nomePropriedade))
            {
                throw new ArgumentException($"'{nameof(nomePropriedade)}' não pode ser nulo nem vazio.", nameof(nomePropriedade));
            }



            if (nomePropriedade == Constantes.CONST_NME_PROPRIEDADE)
            {
                nomePropriedade = Constantes.GerarNomePropriedade(nomeClasse);
            }



            Nome = nomePropriedade;
            Retorno = retorno ?? throw new ArgumentNullException(nameof(retorno));
            Parametros = parametros ?? Array.Empty<Type>();

            PilhaExecucao = new List<IILPilha>();

            Proxy = builderProxy.Clone();
            Proxy.RegistrarBuilders(builderProxy.ObterBuilder<TypeBuilder>());
            Proxy.RegistrarBuilders(Proxy.ObterBuilder<TypeBuilder>().DefineProperty(Nome, PropertyAttributes.HasDefault, retorno, parametros));

            Campo = default;
            Getter = default;
            Setter = default;

        }

        internal ILBuilderProxy Proxy { get; private set; }
        internal List<IILPilha> PilhaExecucao { get; private set; }


        internal ILCampo Campo { get; set; }
        internal ILMetodo Getter { get; set; }
        internal ILMetodo Setter { get; set; }

        public Type[] Parametros { get; private set; }

        public void Executar()
        {
            if (Executado)
                return;

            if (!IsProxy)
                Campo.Executar();

            if (Getter != null)
            {
                Getter.Executar();
                Proxy.ObterBuilder<PropertyBuilder>().SetGetMethod(Getter.Proxy.ObterBuilder<MethodBuilder>());
            }
            else
            {
                throw new InvalidOperationException("Obrigatório existir o acessador get");
            }

            if (Setter != null)
            {
                Setter.Executar();
                Proxy.ObterBuilder<PropertyBuilder>().SetSetMethod(Setter.Proxy.ObterBuilder<MethodBuilder>());
            }


            foreach (var executor in PilhaExecucao)
            {
                executor.Executar();
            }

            Executado = true;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(".property ");

            sb.Append($"instance {Retorno.Name} {Nome}");


            sb.Append("(");


            for (int i = 0; i < Parametros.Length; i++)
            {
                string spl = ", ";
                if (i + 1 == Parametros.Length)
                {
                    spl = string.Empty;
                }

                sb.Append($"{Parametros[i].Name} {spl}");
            }


            sb.Append(")");


            sb.AppendLine("{");

            if (Getter != null)
            {
                sb.Append($".get instance {Getter.Retorno.Name} {Getter.Nome}(");

                if (Getter.Parametros != Array.Empty<Type>())
                {
                    foreach (Type parametro in Getter.Parametros)
                    {
                        sb.Append($"{parametro}");
                    }
                }

                sb.AppendLine(")");
            }

            if (Setter != null)
            {
                sb.Append($".set instance {Setter.Retorno.Name} {Setter.Nome}(");

                if (Setter.Parametros != Array.Empty<Type>())
                {
                    foreach (Type parametro in Setter.Parametros)
                    {
                        sb.Append($"{parametro}");
                    }
                }

                sb.AppendLine(")");
            }

            sb.AppendLine("}");

            return sb.ToString();
        }

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Proxy.Dispose();
                    Proxy = null;
                    Getter?.Dispose();
                    Setter?.Dispose();
                    Campo?.Dispose();
                    foreach (var item in PilhaExecucao)
                    {
                        item.Dispose();
                    }
                    PilhaExecucao.Clear();
                    PilhaExecucao = null;
                    Parametros = null;
                    Retorno = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~ILPropriedade()
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