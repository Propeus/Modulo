using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;


namespace Propeus.Modulo.Util
{
    /// <summary>
    /// Classe de ajuda para tipos variados
    /// </summary>
    public static partial class Helper
    {
        /// <summary>
        /// Serializa um objeto em array de <see cref="byte" /></summary>
        /// <param name="obj">Qualuer objeto do tipo <see cref="object" /></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        /// <exception cref="SerializationException">Objeto não serializavel</exception>
        public static byte[] Serializar(this object obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if (obj is string)
            {
                return obj.ToString().ToArrayByte();
            }

            int size = Marshal.SizeOf(obj);
            byte[] bytes = new byte[size];
            nint ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(obj, ptr, false);
            Marshal.Copy(ptr, bytes, 0, size);
            Marshal.FreeHGlobal(ptr);

            return bytes;
        }

        /// <summary>
        /// Verifica se o objeto é herdado do tipo passado no parametro <paramref name="comparacao" /></summary>
        /// <param name="obj">Classe a ser verificado</param>
        /// <param name="comparacao">Tipo a ser comparado</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        /// <exception cref="ArgumentException">Argumento invalido</exception>
        public static bool Herdado(this object obj, Type comparacao)
        {
            return obj is null
                ? throw new ArgumentNullException(nameof(obj))
                : comparacao is null
                ? throw new ArgumentNullException(nameof(comparacao))
                : obj is Type ? Helper.Herdado(obj as Type, comparacao) : Helper.Herdado(obj.GetType(), comparacao);
        }

     
        /// <summary>
        /// Converte qualquer objeto para o tipo desejado
        /// </summary>
        /// <typeparam name="T">Tipo a ser convertido</typeparam>
        /// <param name="obj">Objeto a ser convertido</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        /// <exception cref="ArgumentException">Argumento invalido</exception>
        /// <exception cref="OverflowException"></exception>
        public static T To<T>(this object obj)
        {
            //Tentei remover essa macumba, porem, de alguma forma ele funciona melhor que o Convert e Cast juntos
            return (T)obj.To(typeof(T));
        }

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

            if ( obj is not Type ? obj.Equals(obj.GetType().Default()) : obj.Equals(((Type)obj).Default()))
            {
                throw new ArgumentNullException(nameof(para));
            }

            //Evita redundancia de conversão
            if (obj.GetType() == para)
            {
                return obj;
            }

            if (obj.Herdado(para))
            {
                return obj;
            }

            if (para.IsEnum)
            {
                return Enum.Parse(para, obj.ToString());
            }
            else if (para.IsPrimitive)
            {
                return Convert.ChangeType(obj, para);
            }
            else if (para.IsClass && obj.GetType().IsPrimitive)
            {
                return Convert.ChangeType(obj, para);
            }

            MethodInfo box_explicito_implicito = obj.GetType().GetMethod("op_Explicit", new[] { obj.GetType() })
                ?? obj.GetType().GetMethod("op_Implicit", new[] { obj.GetType() });


            return box_explicito_implicito?.ReturnType == para
                ? box_explicito_implicito.Invoke(obj, new object[] { obj })
                : throw new InvalidCastException();
        }

            
        /// <summary>
        /// Obtem o hash de um objeto serializavel
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="SerializationException"></exception>
        public static string Hash(this object obj)
        {
            return Helper.Hash(obj.Serializar());
        }
    }
}