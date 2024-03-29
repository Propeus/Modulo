﻿using System;
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


            return box_explicito_implicito?.ReturnType == para
                ? box_explicito_implicito.Invoke(obj, new object[] { obj })
                : throw new InvalidCastException();
        }

      
    }
}