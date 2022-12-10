using Propeus.Modulo.IL.Enums;
using Propeus.Modulo.IL.Geradores;
using Propeus.Modulo.IL.Pilhas;

using Propeus.Modulo.Abstrato.Util;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace Propeus.Modulo.IL.Helpers
{
    /// <summary>
    /// Classe de ajuda para montagem de classes
    /// </summary>
    public static partial class Helper
    {

        /// <summary>
        /// Retorna os metodos nao implementados (por outras interfaces ou por proxy)
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <param name="iLClasse"></param>
        /// <returns></returns>
        public static IEnumerable<ILMetodo> ImplementarMetodoInterface<TInterface>(this ILClasseProvider iLClasse)
        {
            var tpInterface = typeof(TInterface);

            if (!iLClasse.Atual.Interfaces.Contains(tpInterface))
            {
                _ = iLClasse.NovaVersao(interfaces: new Type[] { tpInterface });
            }

            var metodos = tpInterface.GetMethods();

            //var hasProxy = clsBuilder.Campos.Any(c => c.Nome == "IL_Gerador_Proxy_" + clsBuilder.Nome);


            foreach (var mth in metodos)
            {
                if (!iLClasse.Atual.Metodos.Any(m => m.Assinatura == mth.Name && m.Parametros == mth.ObterTipoParametros().ToArray()))
                {
                    yield return iLClasse.CriarMetodo(mth.Name, mth.Attributes.DividirEnum().ParseEnum<MethodAttributes, Token>(), mth.ReturnType, mth.ObterTipoParametros().ToArray());
                }


            }

        }

        /// <summary>
        /// Cria um metodo
        /// <para>
        /// Por padrão o metodo sem parametro
        /// </para>
        /// </summary>
        /// <param name="iLClasse"></param>
        /// <param name="nome"></param>
        /// <param name="acessadores"></param>
        /// <param name="retorno"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public static ILMetodo CriarMetodo(this ILClasseProvider iLClasse, string nome = Constantes.CONST_NME_METODO, Token[] acessadores = null, Type retorno = null, Type[] parametros = null)
        {
            if (iLClasse is null)
            {
                throw new ArgumentNullException(nameof(iLClasse));
            }

            ILMetodo mth;



            //if (acessadores is not null)
            //{

            //mth = new ILMetodo(iLClasse, nome, acessadores.Join(new Token[] { Token.OcutarAssinatura }).ToArray(), retorno, parametros);
            //}
            //else
            //{
            mth = new ILMetodo(iLClasse.Atual.Proxy, iLClasse.Nome, nome, acessadores, retorno, parametros);
            //}
            if (nome.Trim() != ".ctor")
            {
                iLClasse.Atual.Metodos.Add(mth);
            }

            return mth;
        }

        /// <summary>
        /// Cria um construtor padrão
        /// </summary>
        /// <param name="iLClasse"></param>
        /// <param name="acessadores"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public static ILMetodo CriarConstrutor(this ILClasseProvider iLClasse, Token[] acessadores = null, Type[] parametros = null)
        {
            if (iLClasse is null)
            {
                throw new ArgumentNullException(nameof(iLClasse));
            }

            var mth = iLClasse.CriarMetodo(".ctor", new Token[] { Token.OcutarAssinatura, Token.NomeEspecial, Token.RotuloNomeEspecial }.ConcatDistinct(acessadores).ToArray(), null, parametros);
            iLClasse.Atual.Construtores.Add(mth);
            return mth;
        }

        /// <summary>
        /// Carrega um argumento para a pilha de execução
        /// </summary>
        /// <param name="iLMetodo"></param>
        /// <param name="indice"></param>
        /// <returns></returns>
        public static ILMetodo CarregarArgumento(this ILMetodo iLMetodo, int indice = 0)
        {
            if (iLMetodo.IsDefault())
            {
                throw new ArgumentNullException(nameof(iLMetodo));
            }

            iLMetodo.PilhaExecucao.Add(new ILLdarg(iLMetodo.Proxy, indice));
            return iLMetodo;
        }

        /// <summary>
        /// Carrega um conjunto de indice de argumento para a pilha de execução
        /// </summary>
        /// <param name="iLMetodo"></param>
        /// <param name="indice"></param>
        /// <returns></returns>
        public static ILMetodo CarregarArgumentoAgregado(this ILMetodo iLMetodo, int indiceInicial = 0, int indiceFinal = 0)
        {
            if (iLMetodo.IsDefault())
            {
                throw new ArgumentNullException(nameof(iLMetodo));
            }

            for (int indice = indiceInicial; indice <= indiceFinal; indice++)
            {
                iLMetodo.PilhaExecucao.Add(new ILLdarg(iLMetodo.Proxy, indice));
            }
            return iLMetodo;
        }

        /// <summary>
        /// Carrega um argumento para a pilha de execução
        /// </summary>
        /// <param name="iLMetodo"></param>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static ILMetodo CarregarArgumento(this ILMetodo iLMetodo, string valor)
        {
            if (iLMetodo.IsDefault())
            {
                throw new ArgumentNullException(nameof(iLMetodo));
            }

            if (valor is null)
            {
                throw new ArgumentNullException(nameof(valor));
            }

            iLMetodo.PilhaExecucao.Add(new ILLdstr(iLMetodo.Proxy, valor));
            return iLMetodo;
        }

        /// <summary>
        /// Carrega um campo para a pilha de execução
        /// </summary>
        /// <param name="iLMetodo"></param>
        /// <param name="iLCampo"></param>
        /// <returns></returns>
        public static ILMetodo CarregarCampo(this ILMetodo iLMetodo, ILCampo iLCampo)
        {
            if (iLMetodo.IsDefault())
            {
                throw new ArgumentNullException(nameof(iLMetodo));
            }

            if (iLCampo.IsDefault())
            {
                throw new ArgumentNullException(nameof(iLCampo));
            }

            iLMetodo.PilhaExecucao.Add(new ILLdfld(iLMetodo.Proxy, iLCampo.Proxy.ObterBuilder<FieldBuilder>()));
            return iLMetodo;
        }

        /// <summary>
        /// Carrega um campo para a pilha de execução
        /// </summary>
        /// <param name="iLMetodo"></param>
        /// <param name="iLVariavel"></param>
        /// <returns></returns>
        public static ILMetodo CarregarVariavel(this ILMetodo iLMetodo, ILVariavel iLVariavel)
        {
            if (iLMetodo.IsDefault())
            {
                throw new ArgumentNullException(nameof(iLMetodo));
            }

            if (iLVariavel is null)
            {
                throw new ArgumentNullException(nameof(iLVariavel));
            }

            iLMetodo.PilhaExecucao.Add(new ILLdLoc(iLMetodo.Proxy, iLVariavel.Indice));
            return iLMetodo;
        }

        /// <summary>
        /// Armazena o valor na variavel
        /// </summary>
        /// <param name="iLMetodo"></param>
        /// <param name="iLVariavel"></param>
        /// <returns></returns>
        public static ILMetodo ArmazenarVariavel(this ILMetodo iLMetodo, ILVariavel iLVariavel)
        {
            if (iLMetodo.IsDefault())
            {
                throw new ArgumentNullException(nameof(iLMetodo));
            }

            if (iLVariavel is null)
            {
                throw new ArgumentNullException(nameof(iLVariavel));
            }

            iLMetodo.PilhaExecucao.Add(new ILStLoc(iLMetodo.Proxy, iLVariavel.Indice));
            return iLMetodo;
        }

        /// <summary>
        /// Armazena o valor na variavel
        /// </summary>
        /// <param name="iLMetodo"></param>
        /// <param name="iLCampo"></param>
        /// <returns></returns>
        public static ILMetodo ArmazenarCampo(this ILMetodo iLMetodo, ILCampo iLCampo)
        {
            if (iLMetodo.IsDefault())
            {
                throw new ArgumentNullException(nameof(iLMetodo));
            }

            if (iLCampo.IsDefault())
            {
                throw new ArgumentNullException(nameof(iLCampo));
            }

            iLMetodo.PilhaExecucao.Add(new ILStfld(iLMetodo.Proxy, iLCampo.Proxy.ObterBuilder<FieldBuilder>()));
            return iLMetodo;
        }

        /// <summary>
        /// Chama uma função de um objeto
        /// </summary>
        /// <param name="iLMetodo"></param>
        /// <param name="constructorInfo"></param>
        /// <returns></returns>
        public static ILMetodo ChamarFuncao(this ILMetodo iLMetodo, ConstructorInfo constructorInfo)
        {
            if (iLMetodo.IsDefault())
            {
                throw new ArgumentNullException(nameof(iLMetodo));
            }

            if (constructorInfo is null)
            {
                throw new ArgumentNullException(nameof(constructorInfo));
            }

            iLMetodo.PilhaExecucao.Add(new ILCall(iLMetodo.Proxy, constructorInfo));

            return iLMetodo;
        }

        /// <summary>
        /// Chama uma função de um objeto
        /// </summary>
        /// <param name="iLMetodo"></param>
        /// <param name="iLVariavel"></param>
        /// <returns></returns>
        public static ILMetodo ChamarFuncao(this ILMetodo iLMetodo, ILVariavel iLVariavel)
        {
            if (iLMetodo.IsDefault())
            {
                throw new ArgumentNullException(nameof(iLMetodo));
            }

            if (iLVariavel is null)
            {
                throw new ArgumentNullException(nameof(iLVariavel));
            }

            iLMetodo.PilhaExecucao.Add(new ILCall(iLMetodo.Proxy, iLVariavel.Proxy.ObterBuilder<FieldBuilder>()));

            return iLMetodo;
        }

        /// <summary>
        /// Chama uma função de um objeto
        /// </summary>
        /// <param name="iLMetodo"></param>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        public static ILMetodo ChamarFuncaoVirtual(this ILMetodo iLMetodo, MethodInfo methodInfo)
        {
            if (iLMetodo.IsDefault())
            {
                throw new ArgumentNullException(nameof(iLMetodo));
            }

            if (methodInfo is null)
            {
                throw new ArgumentNullException(nameof(methodInfo));
            }

            iLMetodo.PilhaExecucao.Add(new ILCallVirt(iLMetodo.Proxy, methodInfo));

            return iLMetodo;
        }

        /// <summary>
        /// Cria o 'return'
        /// </summary>
        /// <param name="iLMetodo"></param>
        /// <returns></returns>
        public static ILMetodo CriarRetorno(this ILMetodo iLMetodo)
        {
            if (iLMetodo.IsDefault())
            {
                throw new ArgumentNullException(nameof(iLMetodo));
            }

            iLMetodo.PilhaExecucao.Add(new ILRetornar(iLMetodo.Proxy));

            return iLMetodo;
        }

        /// <summary>
        /// Cria o 'return'
        /// </summary>
        /// <param name="iLMetodo"></param>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static ILMetodo CriarRetorno(this ILMetodo iLMetodo, int valor)
        {
            if (iLMetodo.IsDefault())
            {
                throw new ArgumentNullException(nameof(iLMetodo));
            }

            iLMetodo.PilhaExecucao.Add(new ILInt32(iLMetodo.Proxy, valor));
            iLMetodo.PilhaExecucao.Add(new ILRetornar(iLMetodo.Proxy));
            return iLMetodo;
        }

        /// <summary>
        /// Cria o 'return'
        /// </summary>
        /// <param name="iLMetodo"></param>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static ILMetodo CriarRetorno(this ILMetodo iLMetodo, long valor)
        {
            if (iLMetodo.IsDefault())
            {
                throw new ArgumentNullException(nameof(iLMetodo));
            }

            iLMetodo.PilhaExecucao.Add(new ILInt64(iLMetodo.Proxy, valor));
            iLMetodo.PilhaExecucao.Add(new ILRetornar(iLMetodo.Proxy));
            return iLMetodo;
        }

        /// <summary>
        /// Cria o 'return'
        /// </summary>
        /// <param name="iLMetodo"></param>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static ILMetodo CriarRetorno(this ILMetodo iLMetodo, float valor)
        {
            if (iLMetodo.IsDefault())
            {
                throw new ArgumentNullException(nameof(iLMetodo));
            }

            iLMetodo.PilhaExecucao.Add(new ILFloat32(iLMetodo.Proxy, valor));
            iLMetodo.PilhaExecucao.Add(new ILRetornar(iLMetodo.Proxy));
            return iLMetodo;
        }

        /// <summary>
        /// Cria o 'return'
        /// </summary>
        /// <param name="iLMetodo"></param>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static ILMetodo CriarRetorno(this ILMetodo iLMetodo, double valor)
        {
            if (iLMetodo.IsDefault())
            {
                throw new ArgumentNullException(nameof(iLMetodo));
            }

            iLMetodo.PilhaExecucao.Add(new ILFloat64(iLMetodo.Proxy, valor));
            iLMetodo.PilhaExecucao.Add(new ILRetornar(iLMetodo.Proxy));
            return iLMetodo;
        }

        /// <summary>
        /// Cria o 'return'
        /// </summary>
        /// <param name="iLMetodo"></param>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static ILMetodo CriarRetorno(this ILMetodo iLMetodo, short valor)
        {
            if (iLMetodo.IsDefault())
            {
                throw new ArgumentNullException(nameof(iLMetodo));
            }

            iLMetodo.PilhaExecucao.Add(new ILInt16(iLMetodo.Proxy, valor));
            iLMetodo.PilhaExecucao.Add(new ILRetornar(iLMetodo.Proxy));
            return iLMetodo;
        }

        /// <summary>
        /// Cria o 'return'
        /// </summary>
        /// <param name="iLMetodo"></param>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static ILMetodo CriarRetorno(this ILMetodo iLMetodo, decimal valor)
        {
            if (iLMetodo.IsDefault())
            {
                throw new ArgumentNullException(nameof(iLMetodo));
            }

            iLMetodo.PilhaExecucao.Add(new ILValueType(iLMetodo.Proxy, valor));
            iLMetodo.PilhaExecucao.Add(new ILRetornar(iLMetodo.Proxy));
            return iLMetodo;
        }

        /// <summary>
        /// Converte o valor da pilha de execução no tipo especificado
        /// </summary>
        /// <param name="iLMetodo"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ILMetodo Converter(this ILMetodo iLMetodo, Type type)
        {
            if (iLMetodo.IsDefault())
            {
                throw new ArgumentNullException(nameof(iLMetodo));
            }

            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            iLMetodo.PilhaExecucao.Add(new ILBox(iLMetodo.Proxy, type));
            return iLMetodo;
        }

        /// <summary>
        /// Cria a instancia de um objeto
        /// </summary>
        /// <param name="iLMetodo"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ILMetodo CriarInstancia(this ILMetodo iLMetodo, Type type, Type[] parametros = null)
        {
            if (iLMetodo.IsDefault())
            {
                throw new ArgumentNullException(nameof(iLMetodo));
            }

            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            iLMetodo.PilhaExecucao.Add(new ILNewObj(iLMetodo.Proxy, type.ObterConstrutor(parametros)));
            return iLMetodo;
        }

    }
}