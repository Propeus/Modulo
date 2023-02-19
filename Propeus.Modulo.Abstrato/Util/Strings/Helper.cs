using System.Text.RegularExpressions;

using Propeus.Modulo.Abstrato.Util.Tabelas;

using static Propeus.Modulo.Abstrato.Constantes;

namespace Propeus.Modulo.Abstrato.Util
{
    /// <summary>
    /// Classe de ajuda para string
    /// </summary>
    public static partial class Helper
    {
        /// <summary>
        /// Converte uma <see cref="string"/> em um array de bytes
        /// </summary>
        /// <param name="obj">Qualquer objeto do tipo <see cref="string"/></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        public static byte[] ToArrayByte(this string obj)
        {
            if (obj.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(obj), ARGUMENTO_NULO_OU_VAZIO);
            }

            byte[] arr = new byte[obj.Length];

            for (int i = 0; i < obj.Length; i++)
            {
                arr[i] = (byte)obj[i];
            }

            return arr;
        }

        /// <summary>
        /// Divide uma string na primeira ocorrência
        /// </summary>
        /// <param name="str">String a ser separado</param>
        /// <param name="separator">Delimitador que será utilizado para a quebra de string</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo ou vazio</exception>
        public static string[] FirstSplit(this string str, char separator)
        {
            if (str.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(str), ARGUMENTO_NULO_OU_VAZIO);
            }
            if (separator.IsNullOrDefault())
            {
                throw new ArgumentNullException(nameof(separator), ARGUMENTO_NULO_OU_VAZIO);
            }

            int i = str.IndexOf(separator);
            if (i > 0)
            {
                string[] spl = new string[]
                {
                    str[..i ],
                    str[(i+1)..]
                };

                return spl;
            }
            else
            {
                return Array.Empty<string>();
            }
        }

        /// <summary>
        /// Substitui a primeira ocorrencia de uma string pela mais recente
        /// </summary>
        /// <param name="str">Conteudo a ser manipulado</param>
        /// <param name="antigo">Conteudo antigo a ser substituido</param>
        /// <param name="novo">Novo conteudo a ser inserido</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo ou vazio</exception>
        public static string FirstReplace(this string str, string antigo, string novo)
        {
            if (str.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(str), ARGUMENTO_NULO_OU_VAZIO);
            }
            if (antigo.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(antigo), ARGUMENTO_NULO_OU_VAZIO);
            }

            Regex regex = new(Regex.Escape(antigo));
            string newText = regex.Replace(str, novo, 1);
            return newText;
        }

        /// <summary>
        /// Verifica se a string esta vazia ou nula
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// Verifica se existe o procedimento informado no tipo <typeparamref name="T"/> pelo nome fornecido no parametro <paramref name="nome"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="nome"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo ou vazio</exception>
        public static bool ExisteMetodo<T>(this string nome)
        {
            return nome.IsNullOrEmpty()
                ? throw new ArgumentNullException(nameof(nome), ARGUMENTO_NULO_OU_VAZIO)
                : typeof(T).GetMethods().Where(x => x.Name == nome).Aggregate((m1, m2) => { return m1.GetParameters().Count() > m2.GetParameters().Count() ? m1 : m2; }).IsNotNull();
        }

        public static string WriteTable(this object[] data)
        {
            if (data.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (data[0] == null)
            {
                return string.Empty;
            }

            System.Reflection.PropertyInfo[] propriedades = data[0].ObterPropriedadeInfoType().Select(t => t.Key).ToArray();

            ConsoleTable consoleTable = new(new ConsoleTableOptions
            {
                Columns = propriedades.Select(p => p.Name),
                EnableCount = false,
                NumberAlignment = Alignment.Right
            });

            foreach (object dataItem in data)
            {
                if (dataItem == null)
                {
                    continue;
                }

                object[] dataValor = new object[propriedades.Length];
                for (int i = 0; i < propriedades.Length; i++)
                {
                    dataValor[i] = dataItem.ObterValorPropriedade(propriedades[i]);
                }
                _ = consoleTable.AddRow(dataValor);
            }

            return consoleTable.ToString();

        }


    }
}