using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using static Propeus.Modulo.Abstrato.Constante;

namespace Propeus.Modulo.Abstrato.Util
{
    /// <summary>
    /// Classe de ajuda para o tipo <see cref="Type"/>
    /// </summary>
    public static partial class Helper
    {
        /// <summary>
        /// Verifica se o tipo é herdado do tipo passado no parametro <paramref name="comparacao"/>
        /// </summary>
        /// <param name="obj">Tipo a ser verificado</param>
        /// <param name="comparacao">Tipo a ser comparado</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        /// <exception cref="ArgumentException">Argumento invalido</exception>
        public static bool Herdado(this Type obj, Type comparacao)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj), ARGUMENTO_NULO);
            }
            if (obj.IsVoid())
            {
                throw new ArgumentException(string.Format(ARGUMENTO_NAO_PODE_SER_DO_TIPO, typeof(void).Name), nameof(obj));
            }

            if (comparacao.IsNull())
            {
                throw new ArgumentNullException(nameof(comparacao), ARGUMENTO_NULO);
            }
            if (comparacao.IsVoid())
            {
                throw new ArgumentException(string.Format(ARGUMENTO_NAO_PODE_SER_DO_TIPO, typeof(void).Name), nameof(comparacao));
            }

            bool r = false;
            if (comparacao.IsClass)
            {
                Type type = obj;
                List<Type> heranca = new List<Type>();
                while (type != null)
                {
                    heranca.Add(type);
                    type = type.BaseType;
                }
                r = heranca.Any(t => t.Name == comparacao.Name);
                return r;
            }
            else if (comparacao.IsInterface)
            {
                Type type = obj;
                List<Type> heranca = new List<Type>();
                while (type != null)
                {
                    heranca.AddRange(type.GetInterfaces());
                    type = type.BaseType;
                }
                r = heranca.Contains(comparacao);
                return r;
            }
            return r;
        }

        /// <summary>
        /// Verifica se o tipo é herdado do tipo passado no parametro <typeparamref name="T"/>
        /// </summary>
        /// <param name="type">Tipo a ser verificado</param>
        /// <typeparam name="T">Tipo a ser comparado</typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        /// <exception cref="ArgumentException">Argumento invalido</exception>
        public static bool Herdado<T>(this Type type)
        {
            return Herdado(type, typeof(T));
        }

        /// <summary>
        /// Verifica se o tipo <paramref name="obj"/> é igual ou herdado de <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Tipo a ser comparado</typeparam>
        /// <param name="obj">Tipo a ser validado</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        /// <exception cref="ArgumentException">Argumento invalido</exception>
        public static bool Is<T>(this Type obj)
        {
            return obj.Is(typeof(T));
        }

        /// <summary>
        ///  Verifica se o tipo do parâmetro <paramref name="obj"/> é igual ou herdado de <paramref name="comparacao"/>
        /// </summary>
        /// <param name="obj">Tipo a ser validado</param>
        /// <param name="comparacao">Tipo a ser comparado</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        /// <exception cref="ArgumentException">Argumento invalido</exception>
        public static bool Is(this Type obj, Type comparacao)
        {
            if (obj.IsNull())
            {
                throw new ArgumentNullException(nameof(obj), ARGUMENTO_NULO);
            }

            if (comparacao.IsNull())
            {
                throw new ArgumentNullException(nameof(comparacao), ARGUMENTO_NULO);
            }

            if (obj.IsPrimitive || comparacao.IsPrimitive)
            {
                return (obj == comparacao);
            }

            if (obj == comparacao)
            {
                return true;
            }

            return (obj.Herdado(comparacao));
        }

        /// <summary>
        /// Verifica se o tipo é void
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsVoid(this Type type)
        {
            return type == typeof(void);
        }

        /// <summary>
        /// Verifica se o tipo é diferente de void
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNotVoid(this Type type)
        {
            return !type.IsVoid();
        }

        /// <summary>
        /// Verifica se o tipo não é nulo e diferente de void
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNotNullAndNotVoid(this Type type)
        {
            return type.IsNotNull() && !type.IsVoid();
        }

        /// <summary>
        /// Obtem o valor padrão do tipo passado no parametro <paramref name="obj"/>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object Default(this Type obj)
        {
            if (obj.IsClass || obj.IsInterface)
            {
                return null;
            }
            else if (obj.IsEnum)
            {
                return Enum.GetValues(obj).GetValue(0);
            }
            else if (obj.IsPrimitive || obj.IsValueType)
            {
                return Activator.CreateInstance(obj);
            }

            throw new InvalidCastException(string.Format(VALOR_PADRAO_NAO_ENCONTRADO, nameof(obj)));
        }

        /// <summary>
        /// Obtem as interfaces do tipo selecionado
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        public static IEnumerable<Type> ObterInterfaces(this Type obj)
        {
            if (obj.IsNull())
            {
                throw new ArgumentNullException(nameof(obj), ARGUMENTO_NULO);
            }
            return obj.GetInterfaces();
        }

        /// <summary>
        /// Obtem todos os metodos do tipo que possuem o mesmo nome
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="nome"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo ou vazio</exception>
        public static IEnumerable<MethodInfo> ObterMetodos(this Type obj, string nome)
        {
            if (obj.IsNull())
            {
                throw new ArgumentNullException(nameof(obj), ARGUMENTO_NULO);
            }
            if (nome.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(nome), ARGUMENTO_NULO_OU_VAZIO);
            }

            return obj.ObterMetodos().Where(m => m.Name == nome);
        }

        /// <summary>
        /// Obtem todos os metodos do tipo.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        public static IEnumerable<MethodInfo> ObterMetodos(this Type obj)
        {
            if (obj.IsNull())
            {
                throw new ArgumentNullException(nameof(obj), ARGUMENTO_NULO);
            }

            return obj.GetMethods().Where(x => !x.IsSpecialName && !x.Name.Contains("get_") && !x.Name.Contains("set_"));
        }

        /// <summary>
        /// Verifica se existe o metodo informado
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="mth"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        public static bool ExisteMetodo(this Type obj, MethodInfo mth)
        {
            if (obj.IsNull())
            {
                throw new ArgumentNullException(nameof(obj), ARGUMENTO_NULO);
            }
            if (mth.IsNull())
            {
                throw new ArgumentNullException(nameof(mth), ARGUMENTO_NULO);
            }

            return obj.GetMethods().Where(x => x.Name == mth.Name && x.GetParameters().Count() == mth.GetParameters().Count()).IsNotEmpty();
        }

        /// <summary>
        /// Verifica se existe o procedimento informado
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        public static bool ExisteMetodo(this Type obj, Action action)
        {
            if (obj.IsNull())
            {
                throw new ArgumentNullException(nameof(obj), ARGUMENTO_NULO);
            }
            if (action.IsNull())
            {
                throw new ArgumentNullException(nameof(action), ARGUMENTO_NULO);
            }
            return obj.GetMethods().Where(x => x.Name == action.Method.Name).Aggregate((m1, m2) => { return m1.GetParameters().Count() > m2.GetParameters().Count() ? m1 : m2; }).IsNotNull();
        }

        /// <summary>
        /// Obtem o metodo com maior quantidade de parametros.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="nome"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo ou vazio</exception>
        public static MethodInfo ObterMetodoComMaiorParametros(this Type obj, string nome)
        {
            if (obj.IsNull())
            {
                throw new ArgumentNullException(nameof(obj), ARGUMENTO_NULO);
            }
            if (nome.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(nome), ARGUMENTO_NULO_OU_VAZIO);
            }

            return obj.ObterMetodos(nome).Aggregate((m1, m2) => { return m1.GetParameters().Count() > m2.GetParameters().Count() ? m1 : m2; });
        }

        /// <summary>
        /// Obtem o metodo que possua o mesmo nome, quantidade e tipo de parametros
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="nome"></param>
        /// <param name="params"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo ou vazio</exception>
        public static MethodInfo ObterMetodo(this Type obj, string nome, params Type[] @params)
        {
            if (obj.IsNull())
            {
                throw new ArgumentNullException(nameof(obj), ARGUMENTO_NULO);
            }
            if (nome.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(nome), ARGUMENTO_NULO_OU_VAZIO);
            }

            return obj.ObterMetodos(nome).FirstOrDefault(m => m.ObterTipoParametros().ContainsAll(@params));
        }

        /// <summary>
        /// Obtem o metodo que possua a mesma assinatura
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="mth"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        public static MethodInfo ObterMetodo(this Type obj, MethodInfo mth)
        {
            if (obj.IsNull())
            {
                throw new ArgumentNullException(nameof(obj), ARGUMENTO_NULO);
            }
            if (mth.IsNull())
            {
                throw new ArgumentNullException(nameof(mth), ARGUMENTO_NULO);
            }

            return obj.ObterMetodo(mth.Name, mth.ObterTipoParametros().ToArray());
        }

        /// <summary>
        /// Obtem a propriedade que possua a mesma assinatura
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="prop"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        public static PropertyInfo ObterPropriedade(this Type obj, PropertyInfo prop)
        {
            if (obj.IsNull())
            {
                throw new ArgumentNullException(nameof(obj), ARGUMENTO_NULO);
            }
            if (prop.IsNull())
            {
                throw new ArgumentNullException(nameof(prop), ARGUMENTO_NULO);
            }
            return obj.GetProperties().FirstOrDefault(p => p.Name == prop.Name && p.GetIndexParameters().SequenceEqual(prop.GetIndexParameters()));
        }

        /// <summary>
        /// Obtem o evento que possua a mesma assinatura
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="evt"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        public static EventInfo ObterEvento(this Type obj, EventInfo evt)
        {
            if (obj.IsNull())
            {
                throw new ArgumentNullException(nameof(obj), ARGUMENTO_NULO);
            }
            if (evt.IsNull())
            {
                throw new ArgumentNullException(nameof(evt), ARGUMENTO_NULO);
            }

            return obj.GetEvents().FirstOrDefault(p => p.Name == evt.Name && p.EventHandlerType == evt.EventHandlerType);
        }
    }
}