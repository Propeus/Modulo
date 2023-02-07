using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Propeus.Modulo.Abstrato.Util;
using Propeus.Modulo.IL.Geradores;
using Propeus.Modulo.IL.Pilhas;
using Propeus.Modulo.IL.Pilhas.Aritimetico;
using Propeus.Modulo.IL.Pilhas.Campos;
using Propeus.Modulo.IL.Pilhas.Tipos;
using Propeus.Modulo.IL.Pilhas.Tipos.TiposPrimitivos;
using Propeus.Modulo.IL.Pilhas.Variaveis;

namespace Propeus.Modulo.IL.API
{
    internal class MetodoAPI
    {
        #region Armazenar
        public static void ArmazenarValorCampo(ILMetodo metodo, ILCampo campo)
        {
            metodo.PilhaExecucao.Add(new ILStfld(metodo._metodoBuilder, campo._campoBuilder));
        }
        public static void ArmazenarValorVariavel(ILMetodo metodo, ILVariavel variavel)
        {
            metodo.PilhaExecucao.Add(new ILStLoc(metodo._metodoBuilder, variavel.Indice));
        }
        #endregion

        #region Argumentos
        public static void CarregarArgumento(ILMetodo metodo)
        {
            metodo.PilhaExecucao.Add(new ILLdarg(metodo._metodoBuilder));
        }
        public static void CarregarArgumento(ILMetodo metodo, int indice)
        {
            metodo.PilhaExecucao.Add(new ILLdarg(metodo._metodoBuilder, indice));
        }

        #endregion

        #region Carregar
        public static void CarregarValorBoolean(ILMetodo metodo, bool valor)
        {
            metodo.PilhaExecucao.Add(new ILBoolean(metodo._metodoBuilder, valor));
        }
        public static void CarregarValorByte(ILMetodo metodo, byte valor)
        {
            metodo.PilhaExecucao.Add(new ILByte(metodo._metodoBuilder, valor));
        }
        public static void CarregarValorChar(ILMetodo metodo, char valor)
        {
            metodo.PilhaExecucao.Add(new ILChar(metodo._metodoBuilder, valor));
        }
        public static void CarregarValorDecimal(ILMetodo metodo, decimal valor)
        {
            metodo.PilhaExecucao.Add(new ILDecimal(metodo._metodoBuilder, valor));
        }
        public static void CarregarValorDouble(ILMetodo metodo, double valor)
        {
            metodo.PilhaExecucao.Add(new ILDouble(metodo._metodoBuilder, valor));
        }
        public static void CarregarValorFloat(ILMetodo metodo, float valor)
        {
            metodo.PilhaExecucao.Add(new ILFloat(metodo._metodoBuilder, valor));
        }
        public static void CarregarValorInt(ILMetodo metodo, int valor)
        {
            metodo.PilhaExecucao.Add(new ILInt(metodo._metodoBuilder, valor));
        }
        public static void CarregarValorLong(ILMetodo metodo, long valor)
        {
            metodo.PilhaExecucao.Add(new ILLong(metodo._metodoBuilder, valor));
        }
        public static void CarregarValorSbyte(ILMetodo metodo, sbyte valor)
        {
            metodo.PilhaExecucao.Add(new ILSbyte(metodo._metodoBuilder, valor));
        }
        public static void CarregarValorShort(ILMetodo metodo, short valor)
        {
            metodo.PilhaExecucao.Add(new ILShort(metodo._metodoBuilder, valor));
        }
        public static void CarregarValorUint(ILMetodo metodo, uint valor)
        {
            metodo.PilhaExecucao.Add(new ILUint(metodo._metodoBuilder, valor));
        }
        public static void CarregarValorUlong(ILMetodo metodo, ulong valor)
        {
            metodo.PilhaExecucao.Add(new ILUlong(metodo._metodoBuilder, valor));
        }
        public static void CarregarValorUshort(ILMetodo metodo, ushort valor)
        {
            metodo.PilhaExecucao.Add(new ILUshort(metodo._metodoBuilder, valor));
        }
        public static void CarregarValorString(ILMetodo metodo, string valor)
        {
            metodo.PilhaExecucao.Add(new ILString(metodo._metodoBuilder, valor));
        }

        public static void CarregarValorCampo(ILMetodo metodo, ILCampo campo)
        {
            metodo.PilhaExecucao.Add(new ILLdfld(metodo._metodoBuilder, campo._campoBuilder));
        }

        #endregion

        #region Criar
        public static void CriarRetorno(ILMetodo iLMetodo)
        {
            iLMetodo.PilhaExecucao.Add(new ILRet(iLMetodo._metodoBuilder));
        }
        public static void CriarVariavel(ILMetodo iLMetodo, Type tipo, string nome = Constantes.CONST_NME_VARIAVEL)
        {
            iLMetodo.Variaveis.Add(new ILVariavel(iLMetodo._metodoBuilder, iLMetodo.Nome, tipo, nome));
        }
        public static void CriarObjeto(ILMetodo iLMetodo, ConstructorInfo constructorInfo)
        {
            iLMetodo.PilhaExecucao.Add(new ILNewObj(iLMetodo._metodoBuilder, constructorInfo));
        }
        public static void CriarArray(ILMetodo iLMetodo, Type tipo)
        {
            iLMetodo.PilhaExecucao.Add(new ILNewArr(iLMetodo._metodoBuilder, tipo));
        }
        #endregion

        #region Chamar
        public static void ChamarFuncao(ILMetodo iLMetodo, ConstructorInfo constructorInfo)
        {
            iLMetodo.PilhaExecucao.Add(new ILCall(iLMetodo._metodoBuilder, constructorInfo));
        }
        public static void ChamarFuncaoVirtual(ILMetodo iLMetodo, MethodInfo methodInfo)
        {
            iLMetodo.PilhaExecucao.Add(new ILCallVirt(iLMetodo._metodoBuilder, methodInfo));
        }
        #endregion

        #region Aritimetico
        public static void Soma(ILMetodo iLMetodo)
        {
            iLMetodo.PilhaExecucao.Add(new ILAdd(iLMetodo._metodoBuilder));
        }
        #endregion
    }
}
