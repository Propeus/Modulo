using Propeus.Modulo.IL.Enums;
using Propeus.Modulo.IL.Geradores;
using Propeus.Modulo.IL.Pilhas;
using Propeus.Modulo.Util.Objetos;
using Propeus.Modulo.Util.Tipos;
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
        /// Cria um metodo auto gerado
        /// <para>
        /// Por padrão o metodo é publico sem retorno e sem parametro
        /// </para>
        /// </summary>
        /// <param name="iLClasse"></param>
        /// <returns></returns>
        public static ILMetodo CriarMetodo(this ILClasse iLClasse)
        {
            if (iLClasse is null)
            {
                throw new ArgumentNullException(nameof(iLClasse));
            }

            var mth = new ILMetodo(iLClasse);
            iLClasse.Metodos.Add(mth);
            return mth;
        }

        /// <summary>
        /// Cria um metodo
        /// <para>
        /// Por padrão o metodo com retorno e sem parametro
        /// </para>
        /// </summary>
        /// <param name="iLClasse"></param>
        /// <param name="nome"></param>
        /// <param name="retorno"></param>
        /// <returns></returns>
        public static ILMetodo CriarMetodo(this ILClasse iLClasse, string nome, Type retorno)
        {
            if (iLClasse is null)
            {
                throw new ArgumentNullException(nameof(iLClasse));
            }

            if (string.IsNullOrEmpty(nome))
            {
                throw new ArgumentException("Nome é obrigatório", nameof(nome));
            }

            if (retorno is null)
            {
                throw new ArgumentNullException(nameof(retorno));
            }

            var mth = new ILMetodo(iLClasse, nome, retorno: retorno);
            if (iLClasse.Metodos.Any(m => m.Assinatura == mth.Assinatura))
            {
                throw new InvalidOperationException("Já existe um metodo com a mesma assinatura");
            }
            iLClasse.Metodos.Add(mth);
            return mth;
        }

        /// <summary>
        /// Cria um metodo
        /// <para>
        /// Por padrão o metodo com parametro
        /// </para>
        /// </summary>
        /// <param name="iLClasse"></param>
        /// <param name="nome"></param>
        /// <param name="retorno"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public static ILMetodo CriarMetodo(this ILClasse iLClasse, string nome, Type retorno, Type[] parametros)
        {
            if (iLClasse is null)
            {
                throw new ArgumentNullException(nameof(iLClasse));
            }

            if (string.IsNullOrEmpty(nome))
            {
                throw new ArgumentException("Nome é obrigatório", nameof(nome));
            }

            if (retorno is null)
            {
                throw new ArgumentNullException(nameof(retorno));
            }

            if (parametros is null)
            {
                throw new ArgumentNullException(nameof(parametros));
            }

            var mth = new ILMetodo(iLClasse, nome,retorno: retorno, parametros: parametros);
            iLClasse.Metodos.Add(mth);
            return mth;
        }

        /// <summary>
        /// Cria um metodo
        /// <para>
        /// Por padrão o metodo sem retorno e sem parametro
        /// </para>
        /// </summary>
        /// <param name="iLClasse"></param>
        /// <param name="nome"></param>
        /// <param name="acessador"></param>
        /// <returns></returns>
        public static ILMetodo CriarMetodo(this ILClasse iLClasse, string nome, TokenEnum acessador)
        {
            if (iLClasse is null)
            {
                throw new ArgumentNullException(nameof(iLClasse));
            }

            if (string.IsNullOrEmpty(nome))
            {
                throw new ArgumentException("Nome é obrigatório", nameof(nome));
            }

            var mth = new ILMetodo(iLClasse, nome, new TokenEnum[] { acessador, TokenEnum.OcutarAssinatura });
            if (iLClasse.Metodos.Any(m => m.Assinatura == mth.Assinatura))
            {
                throw new InvalidOperationException("Já existe um metodo com a mesma assinatura");
            }
            iLClasse.Metodos.Add(mth);
            return mth;
        }

        /// <summary>
        /// Cria um metodo
        /// <para>
        /// Por padrão o metodo sem parametro
        /// </para>
        /// </summary>
        /// <param name="iLClasse"></param>
        /// <param name="nome"></param>
        /// <param name="acessador"></param>
        /// <param name="retorno"></param>
        /// <returns></returns>
        public static ILMetodo CriarMetodo(this ILClasse iLClasse, string nome, TokenEnum acessador, Type retorno)
        {
            if (iLClasse is null)
            {
                throw new ArgumentNullException(nameof(iLClasse));
            }

            if (string.IsNullOrEmpty(nome))
            {
                throw new ArgumentException("Nome é obrigatório", nameof(nome));
            }

            if (retorno is null)
            {
                throw new ArgumentNullException(nameof(retorno));
            }

            var mth = new ILMetodo(iLClasse, nome, new TokenEnum[] { acessador, TokenEnum.OcutarAssinatura }, retorno);
            iLClasse.Metodos.Add(mth);
            return mth;
        }

        /// <summary>
        /// Cria um metodo
        /// <para>
        /// Por padrão o metodo sem parametro
        /// </para>
        /// </summary>
        /// <param name="iLClasse"></param>
        /// <param name="nome"></param>
        /// <param name="acessador"></param>
        /// <param name="retorno"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public static ILMetodo CriarMetodo(this ILClasse iLClasse, string nome, TokenEnum acessador, Type retorno, Type[] parametros)
        {
            if (iLClasse is null)
            {
                throw new ArgumentNullException(nameof(iLClasse));
            }

            if (string.IsNullOrEmpty(nome))
            {
                throw new ArgumentException("Nome é obrigatório", nameof(nome));
            }

            if (retorno is null)
            {
                throw new ArgumentNullException(nameof(retorno));
            }

            if (parametros is null)
            {
                throw new ArgumentNullException(nameof(parametros));
            }

            var mth = new ILMetodo(iLClasse, nome, new TokenEnum[] { acessador, TokenEnum.OcutarAssinatura }, retorno, parametros);
            iLClasse.Metodos.Add(mth);
            return mth;
        }

        /// <summary>
        /// Cria um metodo
        /// <para>
        /// Por padrão o metodo sem retorno e sem parametro
        /// </para>
        /// </summary>
        /// <param name="iLClasse"></param>
        /// <param name="nome"></param>
        /// <param name="acessadores"></param>
        /// <returns></returns>
        public static ILMetodo CriarMetodo(this ILClasse iLClasse, string nome, TokenEnum[] acessadores)
        {
            if (iLClasse is null)
            {
                throw new ArgumentNullException(nameof(iLClasse));
            }

            if (string.IsNullOrEmpty(nome))
            {
                throw new ArgumentException("Nome é obrigatório", nameof(nome));
            }

            var mth = new ILMetodo(iLClasse, nome, acessadores);
            if (iLClasse.Metodos.Any(m => m.Assinatura == mth.Assinatura))
            {
                throw new InvalidOperationException("Já existe um metodo com a mesma assinatura");
            }
            iLClasse.Metodos.Add(mth);
            return mth;
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
        /// <returns></returns>
        public static ILMetodo CriarMetodo(this ILClasse iLClasse, string nome, TokenEnum[] acessadores, Type retorno)
        {
            if (iLClasse is null)
            {
                throw new ArgumentNullException(nameof(iLClasse));
            }

            if (string.IsNullOrEmpty(nome))
            {
                throw new ArgumentException("Nome é obrigatório", nameof(nome));
            }

            if (retorno is null)
            {
                throw new ArgumentNullException(nameof(retorno));
            }

            var mth = new ILMetodo(iLClasse, nome, acessadores , retorno);
            iLClasse.Metodos.Add(mth);
            return mth;
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
        public static ILMetodo CriarMetodo(this ILClasse iLClasse, string nome, TokenEnum[] acessadores, Type retorno, Type[] parametros)
        {
            if (iLClasse is null)
            {
                throw new ArgumentNullException(nameof(iLClasse));
            }

            if (string.IsNullOrEmpty(nome))
            {
                throw new ArgumentException("Nome é obrigatório", nameof(nome));
            }

            if (retorno is null)
            {
                throw new ArgumentNullException(nameof(retorno));
            }

            if (parametros is null)
            {
                throw new ArgumentNullException(nameof(parametros));
            }

            var mth = new ILMetodo(iLClasse, nome, acessadores, retorno, parametros);
            iLClasse.Metodos.Add(mth);
            return mth;
        }


        /// <summary>
        /// Cria um construtor padrão
        /// </summary>
        /// <param name="iLClasse"></param>
        /// <returns></returns>
        public static ILMetodo CriarConstrutor(this ILClasse iLClasse)
        {
            if (iLClasse is null)
            {
                throw new ArgumentNullException(nameof(iLClasse));
            }

            var mth = new ILMetodo(iLClasse, ".ctor", new TokenEnum[] { TokenEnum.Publico, TokenEnum.OcutarAssinatura, TokenEnum.NomeEspecial, TokenEnum.RotuloNomeEspecial });
            iLClasse.Construtores.Add(mth);
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
    }
}