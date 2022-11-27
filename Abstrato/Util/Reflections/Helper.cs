using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using static Propeus.Modulo.Abstrato.Constante;

namespace Propeus.Modulo.Abstrato.Util
{
    /// <summary>
    /// Classe de ajuda para reflection
    /// </summary>
    public static partial class Helper
    {
        /// <summary>
        /// Obtem os tipos dos parametros do construtor selecionado
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        public static IEnumerable<Type> ObterTipoParametros(this ConstructorInfo action)
        {
            if (action.IsNull())
            {
                throw new ArgumentNullException(nameof(action), ARGUMENTO_NULO);
            }

            return action.GetParameters().Select(x => x.ParameterType).ToList();
        }

        /// <summary>
        /// Obtem os tipos dos parametros do metodo selecionado
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        public static IEnumerable<Type> ObterTipoParametros(this MethodInfo action)
        {
            if (action.IsNull())
            {
                throw new ArgumentNullException(nameof(action), ARGUMENTO_NULO);
            }

            return action.GetParameters().Select(x => x.ParameterType).ToList();
        }

        /// <summary>
        /// Obtem os tipos dos parametros da propriedade selecionada
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        public static IEnumerable<Type> ObterTipoParametros(this PropertyInfo action)
        {
            if (action.IsNull())
            {
                throw new ArgumentNullException(nameof(action), ARGUMENTO_NULO);
            }
            return action.GetIndexParameters().Select(x => x.ParameterType).ToList();
        }

        /// <summary>
        /// Verifica se existe o procedimento informado no tipo <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        public static bool ExisteMetodo<T>(this Action action)
        {
            if (action.IsNull())
            {
                throw new ArgumentNullException(nameof(action), ARGUMENTO_NULO);
            }
            return typeof(T).ExisteMetodo(action);
        }

        /// <summary>
        /// Obtem o caminho completo do arquivo .exe ou .dll que esta sendo executado no momento
        /// </summary>
        /// <returns></returns>
        public static string ObterPathProgramaAtual()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppDomain.CurrentDomain.FriendlyName);
            if (File.Exists($"{path}.dll"))
            {
                return $"{path}.dll";
            }
            else if (File.Exists($"{path}.exe"))
            {
                return $"{path}.exe";
            }
            throw new FileNotFoundException(PROGRAMA_NAO_ENCONTRADO);
        }
    }
}