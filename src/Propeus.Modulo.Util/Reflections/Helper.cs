using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using Propeus.Modulo.Util.Objetos;

namespace Propeus.Modulo.Util.Reflections
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
            return action is null
                ? throw new ArgumentNullException(nameof(action))
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
                ? throw new ArgumentNullException(nameof(action))
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
        public static void DisposeTypeBuilder(this TypeBuilder tb)
        {
            if (tb == null)
            {
                return;
            }

            Type tbType = typeof(TypeBuilder);

#pragma warning disable S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
            FieldInfo tbMbList = tbType.GetField("m_listMethods", BindingFlags.Instance | BindingFlags.NonPublic); //List<MethodBuilder>
            FieldInfo tbDecType = tbType.GetField("m_DeclaringType", BindingFlags.Instance | BindingFlags.NonPublic);//TypeBuilder
            FieldInfo tbGenType = tbType.GetField("m_genTypeDef", BindingFlags.Instance | BindingFlags.NonPublic);//TypeBuilder
            FieldInfo tbDeclMeth = tbType.GetField("m_declMeth", BindingFlags.Instance | BindingFlags.NonPublic);//MethodBuilder
            FieldInfo tbMbCurMeth = tbType.GetField("m_currentMethod", BindingFlags.Instance | BindingFlags.NonPublic);//MethodBuilder
            _ = tbType.GetField("m_module", BindingFlags.Instance | BindingFlags.NonPublic);//ModuleBuilder
            FieldInfo tbGenTypeParArr = tbType.GetField("m_inst", BindingFlags.Instance | BindingFlags.NonPublic); //GenericTypeParameterBuilder[] 
#pragma warning restore S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields

            TypeBuilder tempDecType = tbDecType.GetValue(tb) as TypeBuilder;
            tempDecType.DisposeTypeBuilder();
            tbDecType.SetValue(tb, null);
            tempDecType = tbGenType.GetValue(tb) as TypeBuilder;
            tempDecType.DisposeTypeBuilder();
            tbDecType.SetValue(tb, null);

            MethodBuilder tempMeth = tbDeclMeth.GetValue(tb) as MethodBuilder;
            tempMeth.DisposeMethod();
            tbDeclMeth.SetValue(tb, null);
            tempMeth = tbMbCurMeth?.GetValue(tb) as MethodBuilder;
            tempMeth.DisposeMethod();
            tbMbCurMeth?.SetValue(tb, null);

            if (tbMbList.GetValue(tb) is IList mbList)
            {
                for (int i = 0; i < mbList.Count; i++)
                {
                    tempMeth = mbList[i] as MethodBuilder;
                    tempMeth.DisposeMethod();
                    mbList[i] = null;
                }
                mbList.Clear();
                tbMbList.SetValue(tb, null);
            }

            tbGenTypeParArr.SetValue(tb, null);
        }
        /// <summary>
        /// Libera objetos da memoria durante a utlizacao do <see cref="System.Reflection.Emit.MethodBuilder"/>
        /// </summary>
        /// <remarks>
        /// Este metodo e necessario para evitar vazamentos de memoria
        /// </remarks>
        /// <param name="mb">A instancia do <see cref="MethodBuilder"/></param>
        public static void DisposeMethod(this MethodBuilder mb)
        {
            if (mb == null)
            {
                return;
            }

            Type mbType = typeof(MethodBuilder);
#pragma warning disable S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
            FieldInfo mbILGen = mbType.GetField("m_ilGenerator", BindingFlags.Instance | BindingFlags.NonPublic);
            FieldInfo mbMod = mbType.GetField("m_module", BindingFlags.Instance | BindingFlags.NonPublic); //ModuleBuilder 
            FieldInfo mbContType = mbType.GetField("m_containingType", BindingFlags.Instance | BindingFlags.NonPublic);
            FieldInfo mbLocSigHelp = mbType.GetField("m_localSignature", BindingFlags.Instance | BindingFlags.NonPublic);//SignatureHelper
            FieldInfo mbSigHelp = mbType.GetField("m_signature", BindingFlags.Instance | BindingFlags.NonPublic);//SignatureHelper
#pragma warning restore S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields

            ILGenerator tempIlGen = mbILGen.GetValue(mb) as ILGenerator;
            tempIlGen.DisposeILGenerator();
            SignatureHelper tempmbSigHelp = mbLocSigHelp.GetValue(mb) as SignatureHelper;
            tempmbSigHelp.DisposeSignature();
            tempmbSigHelp = mbSigHelp.GetValue(mb) as SignatureHelper;
            tempmbSigHelp.DisposeSignature();


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
        public static void DisposeSignature(this SignatureHelper sh)
        {
            if (sh == null)
            {
                return;
            }

            Type shType = typeof(SignatureHelper);
#pragma warning disable S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
            FieldInfo shModule = shType.GetField("m_module", BindingFlags.Instance | BindingFlags.NonPublic);
#pragma warning restore S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
            shModule.SetValue(sh, null);
        }
        /// <summary>
        /// Libera objetos da memoria durante a utlizacao do <see cref="System.Reflection.Emit.ILGenerator"/>
        /// </summary>
        /// <remarks>
        /// Este metodo e necessario para evitar vazamentos de memoria
        /// </remarks>
        /// <param name="ilGen">A instancia do <see cref="ILGenerator"/></param>
        public static void DisposeILGenerator(this ILGenerator ilGen)
        {
            if (ilGen == null)
            {
                return;
            }

            Type ilGenType = typeof(ILGenerator);
#pragma warning disable S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
            FieldInfo ilSigHelp = ilGenType.GetField("m_localSignature", BindingFlags.Instance | BindingFlags.NonPublic);//SignatureHelper
#pragma warning restore S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
            SignatureHelper sigTemp = ilSigHelp.GetValue(ilGen) as SignatureHelper;
            sigTemp.DisposeSignature();
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
        public static void DisposeModuleBuilder(this ModuleBuilder modBuild, string nome = null)
        {
            if (modBuild == null)
            {
                return;
            }

            Type modBuildType = typeof(ModuleBuilder);
#pragma warning disable S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
            FieldInfo modTypeBuildList = modBuildType.GetField("_typeBuilderDict", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
#pragma warning restore S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields

            if (modTypeBuildList.GetValue(modBuild) is Dictionary<string, Type> modTypeList)
            {

                if (string.IsNullOrEmpty(nome))
                {
                    foreach (KeyValuePair<string, Type> item in modTypeList)
                    {
                        (item.Value as TypeBuilder).DisposeTypeBuilder();
                    }
                    modTypeList.Clear();

                    modTypeBuildList.SetValue(modBuild, null);
                }
                else
                {
                    if (modTypeList.ContainsKey(nome))
                    {
                        (modTypeList[nome] as TypeBuilder).DisposeTypeBuilder();
                        _ = modTypeList.Remove(nome);
                    }
                }


            }

        }
        //https://stackoverflow.com/questions/2503645/reflect-emit-dynamic-type-memory-blowup

        public static string HashMetodoMethodInfo(this MethodInfo methodInfo)
        {
            return $"{methodInfo.ReturnType.FullName} {methodInfo.Name}({string.Join(',', methodInfo.GetParameters().Select(x => x.ParameterType.Name))})".Hash();
        }
    }
}