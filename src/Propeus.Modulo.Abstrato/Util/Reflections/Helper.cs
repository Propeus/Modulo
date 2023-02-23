using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using static Propeus.Modulo.Abstrato.Constantes;

namespace Propeus.Modulo.Abstrato.Util
{
    /// <summary>
    /// Classe de ajuda para reflection
    /// </summary>
    public static partial class Helper
    {

        /// <summary>
        /// Obtem o construtor que possua os mesmos tipos de parametro
        /// </summary>
        /// <param name="type">Tipo que sera obtido o construtor</param>
        /// <param name="parametros">Os tipos do parametro do construtor</param>
        /// <returns>Retorna o <see cref="ConstructorInfo"/> caso encontrado</returns>
        /// <exception cref="Exception">Caso nao existe o contrutor com os parametros informado</exception>
        public static ConstructorInfo ObterConstrutor(this Type type, Type[] parametros = null)
        {
            ConstructorInfo ctor = null;
            if (parametros is null || !parametros.Any())
            {
                ctor = type.GetConstructors().FirstOrDefault(ct => ct.GetParameters().Length == 0);

                if (ctor == null)
                {
                    throw new Exception("Nao existem construtores sem parametro para este tipo");
                }
            }
            else
            {
                ctor = type.GetConstructors().FirstOrDefault(ct =>
                                {
                                    IEnumerable<Type> tParametros = ct.ObterTipoParametros();
                                    return tParametros.ContainsAll(parametros);
                                });

                if (ctor == null)
                {
                    throw new Exception("Nao existem construtores com os parametros informado");
                }
            }


            return ctor;
        }

        /// <summary>
        /// Obtem os tipos dos parametros do construtor selecionado
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        public static IEnumerable<Type> ObterTipoParametros(this ConstructorInfo action)
        {
            return action is null
                ? throw new ArgumentNullException(nameof(action), ARGUMENTO_NULO)
                : (IEnumerable<Type>)action.GetParameters().Select(x => x.ParameterType).ToList();
        }

        /// <summary>
        /// Obtem os tipos dos parametros do metodo selecionado
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        public static IEnumerable<Type> ObterTipoParametros(this MethodInfo action)
        {
            return action is null
                ? throw new ArgumentNullException(nameof(action), ARGUMENTO_NULO)
                : (IEnumerable<Type>)action.GetParameters().Select(x => x.ParameterType).ToList();
        }

        //https://stackoverflow.com/questions/2503645/reflect-emit-dynamic-type-memory-blowup
        /// <summary>
        /// Libera objetos da memoria durante a utlizacao do <see cref="System.Reflection.Emit.TypeBuilder"/>
        /// </summary>
        /// <remarks>
        /// Este metodo e necessario para evitar vazamentos de memoria
        /// </remarks>
        /// <param name="tb">A instancia do <see cref="TypeBuilder"/></param>
        public static void Dispose(this TypeBuilder tb)
        {
            if (tb == null)
            {
                return;
            }

            Type tbType = typeof(TypeBuilder);
            FieldInfo tbMbList = tbType.GetField("m_listMethods", BindingFlags.Instance | BindingFlags.NonPublic); //List<MethodBuilder>
            FieldInfo tbDecType = tbType.GetField("m_DeclaringType", BindingFlags.Instance | BindingFlags.NonPublic);//TypeBuilder
            FieldInfo tbGenType = tbType.GetField("m_genTypeDef", BindingFlags.Instance | BindingFlags.NonPublic);//TypeBuilder
            FieldInfo tbDeclMeth = tbType.GetField("m_declMeth", BindingFlags.Instance | BindingFlags.NonPublic);//MethodBuilder
            FieldInfo tbMbCurMeth = tbType.GetField("m_currentMethod", BindingFlags.Instance | BindingFlags.NonPublic);//MethodBuilder
            _ = tbType.GetField("m_module", BindingFlags.Instance | BindingFlags.NonPublic);//ModuleBuilder
            FieldInfo tbGenTypeParArr = tbType.GetField("m_inst", BindingFlags.Instance | BindingFlags.NonPublic); //GenericTypeParameterBuilder[] 

            TypeBuilder tempDecType = tbDecType.GetValue(tb) as TypeBuilder;
            tempDecType.Dispose();
            tbDecType.SetValue(tb, null);
            tempDecType = tbGenType.GetValue(tb) as TypeBuilder;
            tempDecType.Dispose();
            tbDecType.SetValue(tb, null);

            MethodBuilder tempMeth = tbDeclMeth.GetValue(tb) as MethodBuilder;
            tempMeth.Dispose();
            tbDeclMeth.SetValue(tb, null);
            tempMeth = tbMbCurMeth?.GetValue(tb) as MethodBuilder;
            tempMeth.Dispose();
            tbMbCurMeth?.SetValue(tb, null);

            if (tbMbList.GetValue(tb) is IList mbList)
            {
                for (int i = 0; i < mbList.Count; i++)
                {
                    tempMeth = mbList[i] as MethodBuilder;
                    tempMeth.Dispose();
                    mbList[i] = null;
                }
                mbList.Clear();
                tbMbList.SetValue(tb, null);
            }
            //ModuleBuilder tempMod = tbMod.GetValue(tb) as ModuleBuilder;
            //tempMod.Dispose();
            //tbMod.SetValue(tb, null);

            tbGenTypeParArr.SetValue(tb, null);
        }
        /// <summary>
        /// Libera objetos da memoria durante a utlizacao do <see cref="System.Reflection.Emit.MethodBuilder"/>
        /// </summary>
        /// <remarks>
        /// Este metodo e necessario para evitar vazamentos de memoria
        /// </remarks>
        /// <param name="mb">A instancia do <see cref="MethodBuilder"/></param>
        public static void Dispose(this MethodBuilder mb)
        {
            if (mb == null)
            {
                return;
            }

            Type mbType = typeof(MethodBuilder);
            FieldInfo mbILGen = mbType.GetField("m_ilGenerator", BindingFlags.Instance | BindingFlags.NonPublic);
            //FieldInfo mbIAttr = mbType.GetField("m_iAttributes", BindingFlags.Instance | BindingFlags.NonPublic);
            FieldInfo mbMod = mbType.GetField("m_module", BindingFlags.Instance | BindingFlags.NonPublic); //ModuleBuilder 
            FieldInfo mbContType = mbType.GetField("m_containingType", BindingFlags.Instance | BindingFlags.NonPublic);
            FieldInfo mbLocSigHelp = mbType.GetField("m_localSignature", BindingFlags.Instance | BindingFlags.NonPublic);//SignatureHelper
            FieldInfo mbSigHelp = mbType.GetField("m_signature", BindingFlags.Instance | BindingFlags.NonPublic);//SignatureHelper

            ILGenerator tempIlGen = mbILGen.GetValue(mb) as ILGenerator;
            tempIlGen.Dispose();
            SignatureHelper tempmbSigHelp = mbLocSigHelp.GetValue(mb) as SignatureHelper;
            tempmbSigHelp.Dispose();
            tempmbSigHelp = mbSigHelp.GetValue(mb) as SignatureHelper;
            tempmbSigHelp.Dispose();

            //ModuleBuilder tempMod = mbMod.GetValue(mb) as ModuleBuilder;
            //tempMod.Dispose();
            //mbMod.SetValue(mb, null);

            mbILGen.SetValue(mb, null);
            mbContType.SetValue(mb, null);
            mbLocSigHelp.SetValue(mb, null);
            mbSigHelp.SetValue(mb, null);
            mbMod.SetValue(mb, null);
        }
        /// <summary>
        /// Libera objetos da memoria durante a utlizacao do <see cref="System.Reflection.Emit.SignatureHelper"/>
        /// </summary>
        /// <remarks>
        /// Este metodo e necessario para evitar vazamentos de memoria
        /// </remarks>
        /// <param name="sh">A instancia do <see cref="SignatureHelper"/></param>
        public static void Dispose(this SignatureHelper sh)
        {
            if (sh == null)
            {
                return;
            }

            Type shType = typeof(SignatureHelper);
            FieldInfo shModule = shType.GetField("m_module", BindingFlags.Instance | BindingFlags.NonPublic);
            //FieldInfo shSig = shType.GetField("m_signature", BindingFlags.Instance | BindingFlags.NonPublic);
            shModule.SetValue(sh, null);
            //shSig.SetValue(sh, null);
        }
        /// <summary>
        /// Libera objetos da memoria durante a utlizacao do <see cref="System.Reflection.Emit.ILGenerator"/>
        /// </summary>
        /// <remarks>
        /// Este metodo e necessario para evitar vazamentos de memoria
        /// </remarks>
        /// <param name="ilGen">A instancia do <see cref="ILGenerator"/></param>
        public static void Dispose(this ILGenerator ilGen)
        {
            if (ilGen == null)
            {
                return;
            }

            Type ilGenType = typeof(ILGenerator);
            FieldInfo ilSigHelp = ilGenType.GetField("m_localSignature", BindingFlags.Instance | BindingFlags.NonPublic);//SignatureHelper
            SignatureHelper sigTemp = ilSigHelp.GetValue(ilGen) as SignatureHelper;
            sigTemp.Dispose();
            ilSigHelp.SetValue(ilGen, null);
        }
        /// <summary>
        /// Libera objetos da memoria durante a utlizacao do <see cref="System.Reflection.Emit.ModuleBuilder"/>
        /// </summary>
        /// <remarks>
        /// Este metodo e necessario para evitar vazamentos de memoria
        /// </remarks>
        /// <param name="modBuild">A instancia do <see cref="ModuleBuilder"/></param>
        /// <param name="nome">Nome do tipo que possui o metodo</param>
        public static void Dispose(this ModuleBuilder modBuild, string nome = null)
        {
            if (modBuild == null)
            {
                return;
            }

            Type modBuildType = typeof(ModuleBuilder);
            FieldInfo modTypeBuildList = modBuildType.GetField("_typeBuilderDict", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);

            if (modTypeBuildList.GetValue(modBuild) is Dictionary<string, Type> modTypeList)
            {

                if (string.IsNullOrEmpty(nome))
                {
                    foreach (KeyValuePair<string, Type> item in modTypeList)
                    {
                        (item.Value as TypeBuilder).Dispose();
                    }
                    modTypeList.Clear();

                    modTypeBuildList.SetValue(modBuild, null);
                }
                else
                {
                    if (modTypeList.ContainsKey(nome))
                    {
                        (modTypeList[nome] as TypeBuilder).Dispose();
                        _ = modTypeList.Remove(nome);
                    }
                }


            }

        }
        //https://stackoverflow.com/questions/2503645/reflect-emit-dynamic-type-memory-blowup


    }
}