using System.Reflection;

using Propeus.Module.IL.Core.Pilhas;
using Propeus.Module.IL.Core.Pilhas.Campos;
using Propeus.Module.IL.Geradores;

namespace Propeus.Module.IL.Core.API
{
    internal class MetodoAPI
    {
        #region Armazenar
        internal static void ArmazenarValorCampo(ILMetodo metodo, ILCampo campo)
        {
            metodo.PilhaExecucao.Add(new ILStfld(metodo._metodoBuilder, campo._campoBuilder));
        }
        #endregion

        #region Argumentos
        internal static void CarregarArgumento(ILMetodo metodo)
        {
            metodo.PilhaExecucao.Add(new ILLdarg(metodo._metodoBuilder));
        }
        internal static void CarregarArgumento(ILMetodo metodo, int indice)
        {
            metodo.PilhaExecucao.Add(new ILLdarg(metodo._metodoBuilder, indice));
        }

        #endregion

        #region Carregar
        internal static void CarregarValorCampo(ILMetodo metodo, ILCampo campo)
        {
            metodo.PilhaExecucao.Add(new ILLdfld(metodo._metodoBuilder, campo._campoBuilder));
        }

        #endregion

        #region Criar
        internal static void CriarRetorno(ILMetodo iLMetodo)
        {
            iLMetodo.PilhaExecucao.Add(new ILRet(iLMetodo._metodoBuilder));
        }

        internal static void CriarObjeto(ILMetodo iLMetodo, ConstructorInfo constructorInfo)
        {
            iLMetodo.PilhaExecucao.Add(new ILNewObj(iLMetodo._metodoBuilder, constructorInfo));
        }
        #endregion

        #region Chamar
        internal static void ChamarFuncao(ILMetodo iLMetodo, ConstructorInfo constructorInfo)
        {
            iLMetodo.PilhaExecucao.Add(new ILCall(iLMetodo._metodoBuilder, constructorInfo));
        }
        internal static void ChamarFuncao(ILMetodo iLMetodo, MethodInfo methodInfo)
        {
            iLMetodo.PilhaExecucao.Add(new ILCall(iLMetodo._metodoBuilder, methodInfo));
        }
        internal static void ChamarFuncaoVirtual(ILMetodo iLMetodo, MethodInfo methodInfo)
        {
            iLMetodo.PilhaExecucao.Add(new ILCallVirt(iLMetodo._metodoBuilder, methodInfo));
        }
        #endregion
    }
}
