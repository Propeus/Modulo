using System;
using System.Reflection;

using Propeus.Modulo.Util.Tipos;

namespace Propeus.Modulo.Util.Objetos
{
    /// <summary>
    /// Classe de ajuda para tipos variados
    /// </summary>
    public static partial class Helper
    {

        /// <summary>
        /// Converte qualquer objeto para o tipo desejado
        /// </summary>
        /// <param name="para">Tipo a ser convertido</param>
        /// <param name="obj">Objeto a ser convertido</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        /// <exception cref="ArgumentException">Argumento invalido</exception>
        /// <exception cref="OverflowException"></exception>
        public static object To(this object obj, Type para)
        {
            //Verifica se o objeto é nulo
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
            if (obj is Type)
            {
                throw new InvalidCastException();
            }


            if (obj.GetType().IsAssignableTo(para))
            {
                return obj;
            }


            MethodInfo box_explicito_implicito = obj.GetType().GetMethod("op_Explicit", new[] { obj.GetType() })
                ?? obj.GetType().GetMethod("op_Implicit", new[] { obj.GetType() });

            //Ta dando ruim aqui

            return box_explicito_implicito?.ReturnType == para
                ? box_explicito_implicito.Invoke(obj, new object[] { obj })
                : throw new InvalidCastException();
        }
        //https://stackoverflow.com/questions/374651/how-to-check-if-an-object-is-nullable
        public static bool IsNullable<T>(this T obj)
        {
            if (obj == null) return true; // obvious
            Type type = typeof(T);
            if (!type.IsValueType) return true; // ref-type
            if (Nullable.GetUnderlyingType(type) != null) return true; // Nullable<T>
            return false; // value-type
        }

    }
}