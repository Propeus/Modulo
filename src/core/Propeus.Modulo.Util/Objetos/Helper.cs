using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Propeus.Module.Utils.Objetos
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

            if (box_explicito_implicito is null)
            {
                throw new InvalidCastException();
            }
            else if (box_explicito_implicito.ReturnType == para)
            {
                return box_explicito_implicito.Invoke(obj, new object[] { obj });
            }
            else
            {
                throw new InvalidCastException();
            }

        }
        /// <summary>
        /// Verifica se o objeto informado é nulo
        /// </summary>
        /// <remarks>Codigo extraido de https://stackoverflow.com/questions/374651/how-to-check-if-an-object-is-nullable</remarks>
        /// <typeparam name="T">Tipo do objeto</typeparam>
        /// <param name="obj">Instancia do objeto</param>
        /// <returns></returns>
        [ExcludeFromCodeCoverage(Justification = "Codigo de terceiro")]
        public static bool IsNullable<T>(this T obj)
        {
            if (obj == null)
            {
                return true; // obvious
            }

            Type type = typeof(T);
            if (!type.IsValueType)
            {
                return true; // ref-type
            }

            if (Nullable.GetUnderlyingType(type) != null)
            {
                return true; // Nullable<T>
            }

            return false; // value-type
        }

        /// <summary>
        /// Realiza uma validação de tipo com os ParameterInfo 
        /// </summary>
        /// <param name="parameterConstructor">parametros do metodo</param>
        /// <param name="parameterUser">Valores do usuario</param>
        /// <returns>Um array com os parametros do construtor</returns>
        /// <exception cref="ArgumentException">Quando os valores do usaurio são incompativeis com os parametros do metodo</exception>
        public static object[] JoinParameterValue(ParameterInfo[] parameterConstructor, object[] parameterUser, Func<ParameterInfo,object> resolveParamter = null)
        {
            if(parameterUser == null || parameterUser.Length == Array.Empty<object>().Length )
            {
                parameterUser = new object[parameterConstructor.Length];
            }

            if (parameterConstructor.Length < parameterUser.Length)
            {
                throw new ArgumentException("The number of constructor parameters cannot be less than the number of user parameters.");
            }

            object[] result = new object[parameterConstructor.Length];
            int userIndex = 0;
            int paramIndex = 0;

            foreach (ParameterInfo constructorParam in parameterConstructor)
            {
                Type constructorParamType = constructorParam.ParameterType;

                bool foundMatch = false;

                for (int i = userIndex; i < parameterUser.Length; i++)
                {
                    Type userParamType = parameterUser[userIndex]?.GetType();

                    if (constructorParamType.IsAssignableFrom(userParamType))
                    {
                        result[paramIndex] = parameterUser[userIndex];
                        userIndex++;
                        paramIndex++;
                        foundMatch = true;
                        break;
                    }
                }


                // Se não houver correspondência, defina o valor padrão
                if (!foundMatch)
                {
                    result[userIndex] = resolveParamter?.Invoke(constructorParam);

                    if (parameterUser.Length > userIndex && parameterUser[userIndex] == null)
                    {
                        userIndex++;
                    }
                    paramIndex++;
                }
            }

            if (userIndex < parameterUser.Length)
            {
                throw new ArgumentException("A ordem dos argumentos não é compativel com o construtor atual");
            }

            return result;
        }

    }
}