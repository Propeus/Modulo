using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using Propeus.Module.IL.Core.Geradores.Components;
using Propeus.Module.IL.Core.Pilhas;
using Propeus.Module.IL.Core.Pilhas.Campos;

namespace Propeus.Module.IL.Core.API
{
    /// <summary>
    /// Api para montagem de métodos via IL
    /// </summary>
    internal static class MetodoApi
    {
        #region Armazenar
        /// <summary>
        /// Cria uma instrução IL para armazenar um valor em pilha no iLCampo informado
        /// </summary>
        /// <param name="iLMetodo">Método a ser inserido a instrução</param>
        /// <param name="iLcampo">Campo a ser armazenado o valor em pilha</param>
        internal static void StoreValueToField(ILMethodComponent iLMetodo, ILFieldComponent iLcampo)
        {
            ArgumentNullException.ThrowIfNull(iLMetodo.Builder, nameof(iLMetodo));
            ArgumentNullException.ThrowIfNull(iLcampo.Builder, nameof(iLcampo));

            iLMetodo.StackExecution.Add(new ILStfld(iLMetodo.Builder,iLcampo.Builder));
        }
        #endregion

        #region Argumentos
        /// <summary>
        /// Carrega o valor do parâmetro de índice 0 na pilha de execução do método informado
        /// </summary>
        /// <param name="iLMetodo">Método a ser obtido o valor de parâmetro</param>
        internal static void LoadMethodArgument([NotNull] ILMethodComponent iLMetodo)
        {
            ArgumentNullException.ThrowIfNull(iLMetodo.Builder);

            iLMetodo.StackExecution.Add(new ILLdarg(iLMetodo.Builder));
        }
        /// <summary>
        /// Carrega o valor do parâmetro de índice informado na pilha de execução do método
        /// </summary>
        /// <param name="iLMetodo">Método a ser obtido o valor de parâmetro</param>
        /// <param name="indice">Indice de parâmetro para obter o valor</param>
        internal static void LoadMethodArgument(ILMethodComponent iLMetodo, int indice)
        {
            ArgumentNullException.ThrowIfNull(iLMetodo.Builder, nameof(iLMetodo));

            iLMetodo.StackExecution.Add(new ILLdarg(iLMetodo.Builder, indice));
        }

        #endregion

        #region Carregar
        /// <summary>
        /// Carrega o valor do campo informado na pilha de execução do método
        /// </summary>
        /// <param name="iLMetodo">Método a ser manipulado o valor do campo</param>
        /// <param name="iLCampo">Campo a ser obtido o valor</param>
        internal static void LoadField(ILMethodComponent iLMetodo, ILFieldComponent iLCampo)
        {
            ArgumentNullException.ThrowIfNull(iLMetodo.Builder, nameof(iLMetodo));
            ArgumentNullException.ThrowIfNull(iLCampo.Builder, nameof(iLCampo));

            iLMetodo.StackExecution.Add(new ILLdfld(iLMetodo.Builder, iLCampo.Builder));
        }

        #endregion

        #region Criar
        /// <summary>
        /// Adiciona a instrução <see langword="return"/> na pilha de execução do método
        /// </summary>
        /// <param name="iLMetodo">Método a ser inserido a instrução</param>
        internal static void CreateReturn(ILMethodComponent iLMetodo)
        {
            ArgumentNullException.ThrowIfNull(iLMetodo.Builder, nameof(iLMetodo));

            iLMetodo.StackExecution.Add(new ILRet(iLMetodo.Builder));
        }
        /// <summary>
        /// Adiciona a instrução <see langword="new"/> na pilha de execução
        /// </summary>
        /// <param name="iLMetodo">Método a ser inserido a instrução</param>
        /// <param name="constructorInfo">Informação do construtor do objeto a ser instanciado</param>
        internal static void CreateNewInstanceObject(ILMethodComponent iLMetodo, ConstructorInfo constructorInfo)
        {
            ArgumentNullException.ThrowIfNull(iLMetodo.Builder, nameof(iLMetodo));

            iLMetodo.StackExecution.Add(new ILNewObj(iLMetodo.Builder, constructorInfo));
        }
        #endregion

        #region Chamar
        /// <summary>
        /// Adiciona uma instrução IL para chamada de construtor dentro do método atual
        /// </summary>
        /// <param name="iLMetodo">Método a ser inserido a instrução</param>
        /// <param name="constructorInfo">Informação do construtor do objeto a ser chamado</param>
        internal static void CallMethod(ILMethodComponent iLMetodo, ConstructorInfo constructorInfo)
        {
            ArgumentNullException.ThrowIfNull(iLMetodo.Builder, nameof(iLMetodo));

            iLMetodo.StackExecution.Add(new ILCall(iLMetodo.Builder, constructorInfo));
        }
        /// <summary>
        /// Adiciona uma instrução IL para chamada de método dentro do método atual
        /// </summary>
        /// <param name="iLMetodo">Método a ser inserido a instrução</param>
        /// <param name="methodInfo">Informação do método do objeto a ser chamado</param>
        internal static void CallMethod(ILMethodComponent iLMetodo, MethodInfo methodInfo)
        {
            ArgumentNullException.ThrowIfNull(iLMetodo.Builder, nameof(iLMetodo));

            iLMetodo.StackExecution.Add(new ILCall(iLMetodo.Builder, methodInfo));
        }
        /// <summary>
        /// Adiciona uma instrução IL para chamada de método virtual dentro do método atual
        /// </summary>
        /// <param name="iLMetodo">Método a ser inserido a instrução</param>
        /// <param name="methodInfo">Informação do método virtual do objeto a ser chamado</param>
        internal static void CallVirtualMethod(ILMethodComponent iLMetodo, MethodInfo methodInfo)
        {
            ArgumentNullException.ThrowIfNull(iLMetodo.Builder, nameof(iLMetodo));
            
            iLMetodo.StackExecution.Add(new ILCallVirt(iLMetodo.Builder, methodInfo));
        }
        #endregion
    }
}
