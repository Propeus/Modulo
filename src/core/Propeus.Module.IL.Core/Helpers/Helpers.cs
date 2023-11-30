using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Reflection.Emit;

using Propeus.Module.IL.Core.Helpers;
using Propeus.Module.Utils.Objetos;

namespace Propeus.Module.IL.Core.Helpers
{
    internal static class Helpers
    {
        /// <summary>
        /// Converte o enum para o tipo informado usando o atributo <see cref="DescriptionAttribute"/> para obter o novo nomeEnum
        /// </summary>
        /// <typeparam name="TAntigoEnum">Tipo do enum atual</typeparam>
        /// <typeparam name="TNovoEnum">Tipo do novo enum</typeparam>
        /// <param name="enum">Array de enum antigo</param>
        /// <returns>Array de enum novos</returns>
        /// <exception cref="InvalidCastException">O nome do novo enum nao existe</exception>
        public static TNovoEnum[] ParseEnum<TAntigoEnum, TNovoEnum>(this TAntigoEnum[] @enum) where TNovoEnum : struct, Enum
            where TAntigoEnum : struct, Enum
        {
            TNovoEnum[] vnEnums = Enum.GetValues<TNovoEnum>();
            TNovoEnum[] nEnums = new TNovoEnum[@enum.Length];

            for (int i = 0; i < @enum.Length; i++)
            {
                foreach (TNovoEnum vnEnum in vnEnums)
                {
                    string dsc_vnEnum = vnEnum.GetEnumDescription();
                    if (dsc_vnEnum is not null && dsc_vnEnum.Equals(@enum[i].ToString().Trim(), StringComparison.CurrentCultureIgnoreCase))
                    {
                        nEnums[i] = vnEnum;
                    }
                }

            }

            return nEnums;

        }

        /// <summary>
        /// Converte uma cadeia de <see cref="Enum"/> em um array
        /// </summary>
        /// <remarks>
        /// Este método so funciona para <see cref="Enum"/> concatenados por ','
        /// </remarks>
        /// <typeparam name="TEnum">Tipo do enum que sera dividido</typeparam>
        /// <param name="enum">ObjectBuilder do enum a ser dividido</param>
        /// <returns>Array de enum</returns>
        public static TEnum[] SplitEnum<TEnum>(this TEnum @enum) where TEnum : struct
        {
            string? nomeEnum = @enum.ToString();
            if (!string.IsNullOrEmpty(nomeEnum))
            {
                string[] nomesEnums = nomeEnum.Split(',');
                TEnum[] enums = new TEnum[nomesEnums.Length];

                for (int i = 0; i < nomesEnums.Length; i++)
                {
                    enums[i] = Enum.Parse<TEnum>(nomesEnums[i]);
                }

                return enums;
            }
            else
            {
                throw new ArgumentException("O nome do enums é nulo ou vazio");
            }

        }

        /// <summary>
        /// Concatena um array de enum em um único Enum
        /// </summary>
        /// <remarks>
        /// Este método realiza a função do método <see cref="SplitEnum{TEnum}(TEnum)"/> de forma reversa, 
        /// ou seja, ele concatena todos os valores utilizando ','
        /// </remarks>
        /// <typeparam name="TEnum"><see cref="Enum"/> qualquer.</typeparam>
        /// <param name="enums">Array de <see cref="Enum"/></param>
        /// <returns>O enum concatenado</returns>
        /// <exception cref="InvalidCastException">O tipo não é <see cref="Enum"/></exception>
        /// <exception cref="ArgumentException">Argumento <paramref name="enums"/> vazio ou nulo</exception>
        public static TEnum JoinEnums<TEnum>(this TEnum[] enums) where TEnum : struct, Enum
        {
            string nomesEnums = string.Join(",", enums);
            TEnum @enum = (TEnum)Enum.Parse(typeof(TEnum), nomesEnums);
            return @enum;
        }

        /// <summary>
        /// Obtém a descrição do enum
        /// </summary>
        /// <typeparam name="TEnum"><see cref="Enum"/> a ser obtido a descrição</typeparam>
        /// <param name="enum">ObjectBuilder do <see cref="Enum"/> que será obtido a descrição </param>
        /// <returns>Descrição do enum</returns>
        /// <exception cref="InvalidEnumArgumentException"><see cref="Enum"/> sem descrição</exception>
        /// <exception cref="ArgumentException">Argumento <paramref name="enum"/> nulo</exception>
        public static string GetEnumDescription<TEnum>(this TEnum @enum) where TEnum : struct, Enum
        {
            string? enumName = @enum.ToString();
            if (string.IsNullOrEmpty(enumName))
            {
                throw new ArgumentException("O nome do enum está vazio ou nulo");
            }
            Type typeEnum = @enum.GetType();
            FieldInfo? filedEnum = typeEnum.GetField(enumName);
            if (filedEnum is null)
            {
                throw new NotImplementedException();
            }
            DescriptionAttribute[] descriptionAttribute = filedEnum.GetCustomAttributes(typeof(DescriptionAttribute), true) as DescriptionAttribute[] ?? Array.Empty<DescriptionAttribute>();
            string descriptionEnum = descriptionAttribute.First().Description;
            return descriptionEnum;
        }


        /**
         * Listas
         * **/

        /// <summary>
        /// Junta as duas listas sem repetir os objetos.
        /// </summary>
        /// <typeparam name="T">Tipo da lista</typeparam>
        /// <param name="esquerda">Lista da esquerda</param>
        /// <param name="direita">Lista da direita</param>
        /// <returns>Retorna os valores das duas listas sem repetir os valores</returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        public static IEnumerable<T> FullJoin<T>(this IEnumerable<T> esquerda, IEnumerable<T> direita)
        {

            if (direita is null)
            {
                return esquerda;
            }

            ICollection<T> join = new HashSet<T>();
            foreach (T ia in esquerda)
            {
                join.Add(ia);
            }
            foreach (T ib in direita)
            {
                if (!join.Contains(ib))
                {
                    join.Add(ib);
                }
            }
            return join;
        }

        /// <summary>
        /// Junta as duas listas sem repetir os objetos.
        /// </summary>
        /// <remarks>
        /// No dicionário, esquerda retorna como true e direita como false
        /// </remarks>
        /// <param name="esquerda">Lista da esquerda</param>
        /// <param name="direita">Lista da direita</param>
        /// <returns>Retorna os valores das duas listas sem repetir os valores</returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        public static IDictionary<MethodInfo, bool> FullJoinDictionaryMethodInfo(this IEnumerable<MethodInfo> esquerda, IEnumerable<MethodInfo> direita)
        {

            IDictionary<MethodInfo, bool> join = new Dictionary<MethodInfo, bool>();
            IDictionary<string, MethodInfo> joinKV = new Dictionary<string, MethodInfo>();

            foreach (MethodInfo ia in esquerda)
            {
                if (!joinKV.ContainsKey(ia.HashMethodInfo()))
                {
                    join.Add(ia, true);
                    joinKV.Add(ia.HashMethodInfo(), ia);
                }
            }
            foreach (MethodInfo ib in direita)
            {
                if (!joinKV.ContainsKey(ib.HashMethodInfo()))
                {
                    join.Add(ib, false);
                    joinKV.Add(ib.HashMethodInfo(), ib);
                }
            }

            joinKV.Clear();

            return join;
        }

        public static IDictionary<PropertyInfo, bool> FullJoinDictionaryPropertyInfo(this IEnumerable<PropertyInfo> esquerda, IEnumerable<PropertyInfo> direita)
        {

            IDictionary<PropertyInfo, bool> join = new Dictionary<PropertyInfo, bool>();
            IDictionary<string, PropertyInfo> joinKV = new Dictionary<string, PropertyInfo>();

            foreach (PropertyInfo ia in esquerda)
            {
                if (!joinKV.ContainsKey(ia.HashPropertyInfo()))
                {
                    join.Add(ia, true);
                    joinKV.Add(ia.HashPropertyInfo(), ia);
                }
            }
            foreach (PropertyInfo ib in direita)
            {
                if (!joinKV.ContainsKey(ib.HashPropertyInfo()))
                {
                    join.Add(ib, false);
                    joinKV.Add(ib.HashPropertyInfo(), ib);
                }
            }

            joinKV.Clear();

            return join;
        }

        /// <summary>
        /// Converte uma lista de objetos para uma lista do tipo especificado
        /// </summary>
        /// <typeparam name="TSaida">Tipo a ser convertido</typeparam>
        /// <param name="obj">Lista de objetos a ser convertido</param>
        /// <returns>Lista de objeto convertido</returns>
        public static IEnumerable<TSaida> Cast<TSaida>(this IEnumerable obj)
        {
            foreach (object entrada in obj)
            {
                yield return entrada.To<TSaida>();
            }
        }


        /**
         * Objetos
         * **/

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

        /**
         * Reflections
         * **/
        /// <summary>
        /// Obtém os tipos dos parâmetros do construtor selecionado
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        public static IEnumerable<Type> GetTypeParams(this ConstructorInfo action)
        {
            return action is null
                ? throw new ArgumentNullException(nameof(action))
                : (IEnumerable<Type>)action.GetParameters().Select(x => x.ParameterType).ToList();
        }

        //https://stackoverflow.com/questions/2503645/reflect-emit-dynamic-type-memory-blowup
        /// <summary>
        /// Libera objetos da memoria durante a utilização do <see cref="TypeBuilder"/>
        /// </summary>
        /// <remarks>
        /// Este método e necessário para evitar vazamentos de memoria
        /// </remarks>
        /// <param name="tb">A instancia do <see cref="TypeBuilder"/></param>
        public static void DisposeTypeBuilder(this TypeBuilder tb)
        {
            if (tb == null)
            {
                return;
            }

            Type tbType = typeof(TypeBuilder);

#pragma warning disable S3011, CS8600, CS8602, CS8604 // Reflection should not be used to increase accessibility of classes, methods, or fields
            FieldInfo tbMbList = tbType.GetField("m_listMethods", BindingFlags.Instance | BindingFlags.NonPublic); //List<MethodBuilder>
            FieldInfo tbDecType = tbType.GetField("m_DeclaringType", BindingFlags.Instance | BindingFlags.NonPublic);//TypeBuilder
            FieldInfo tbGenType = tbType.GetField("m_genTypeDef", BindingFlags.Instance | BindingFlags.NonPublic);//TypeBuilder
            FieldInfo tbDeclMeth = tbType.GetField("m_declMeth", BindingFlags.Instance | BindingFlags.NonPublic);//MethodBuilder
            FieldInfo tbMbCurMeth = tbType.GetField("m_currentMethod", BindingFlags.Instance | BindingFlags.NonPublic);//MethodBuilder
            _ = tbType.GetField("m_module", BindingFlags.Instance | BindingFlags.NonPublic);//ModuleBuilder
            FieldInfo tbGenTypeParArr = tbType.GetField("m_inst", BindingFlags.Instance | BindingFlags.NonPublic); //GenericTypeParameterBuilder[] 
#pragma warning restore S3011, CS8600 // Reflection should not be used to increase accessibility of classes, methods, or fields

#pragma warning disable CS8600, CS8602, CS8604
            TypeBuilder tempDecType = tbDecType.GetValue(tb) as TypeBuilder;
            tempDecType.DisposeTypeBuilder();
            tbDecType.SetValue(tb, null);
            tempDecType = tbGenType.GetValue(tb) as TypeBuilder;
            tempDecType.DisposeTypeBuilder();
            tbDecType.SetValue(tb, null);
#pragma warning restore CS8600, CS8602, CS8604

#pragma warning disable CS8600, CS8602, CS8604
            MethodBuilder tempMeth = tbDeclMeth.GetValue(tb) as MethodBuilder;
            tempMeth.DisposeMethod();
            tbDeclMeth.SetValue(tb, null);
            tempMeth = tbMbCurMeth?.GetValue(tb) as MethodBuilder;
            tempMeth.DisposeMethod();
            tbMbCurMeth?.SetValue(tb, null);
#pragma warning restore CS8600, CS8602, CS8604

#pragma warning disable CS8600, CS8602, CS8604
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
#pragma warning restore CS8600, CS8602, CS8604 
        }

        /// <summary>
        /// Libera objetos da memoria durante a utilização do <see cref="MethodBuilder"/>
        /// </summary>
        /// <remarks>
        /// Este método é necessário para evitar vazamentos de memoria
        /// </remarks>
        /// <param name="mb">A instancia do <see cref="MethodBuilder"/></param>
        public static void DisposeMethod(this MethodBuilder mb)
        {
            if (mb == null)
            {
                return;
            }

            Type mbType = typeof(MethodBuilder);
#pragma warning disable S3011, CS8600, CS8602, CS8604// Reflection should not be used to increase accessibility of classes, methods, or fields
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
#pragma warning restore CS8600, CS8604

            mbILGen.SetValue(mb, null);
            mbContType.SetValue(mb, null);
            mbLocSigHelp.SetValue(mb, null);
            mbSigHelp.SetValue(mb, null);
            mbMod.SetValue(mb, null);
#pragma warning restore CS8602

        }
        /// <summary>
        /// Libera objetos da memoria durante a utilização do <see cref="SignatureHelper"/>
        /// </summary>
        /// <remarks>
        /// Este método é necessário para evitar vazamentos de memoria
        /// </remarks>
        /// <param name="sh">A instancia do <see cref="SignatureHelper"/></param>
        public static void DisposeSignature(this SignatureHelper sh)
        {
            if (sh == null)
            {
                return;
            }

            Type shType = typeof(SignatureHelper);
#pragma warning disable S3011, CS8600, CS8602 // Reflection should not be used to increase accessibility of classes, methods, or fields
            FieldInfo shModule = shType.GetField("m_module", BindingFlags.Instance | BindingFlags.NonPublic);
#pragma warning restore S3011,CS8600// Reflection should not be used to increase accessibility of classes, methods, or fields
            shModule.SetValue(sh, null);
#pragma warning restore CS8602
        }
        /// <summary>
        /// Libera objetos da memoria durante a utilização do <see cref="ILGenerator"/>
        /// </summary>
        /// <remarks>
        /// Este método é necessário para evitar vazamentos de memoria
        /// </remarks>
        /// <param name="ilGen">A instancia do <see cref="ILGenerator"/></param>
        public static void DisposeILGenerator(this ILGenerator ilGen)
        {
            if (ilGen == null)
            {
                return;
            }

            Type ilGenType = typeof(ILGenerator);
#pragma warning disable S3011,CS8600,CS8602,CS8604 // Reflection should not be used to increase accessibility of classes, methods, or fields
            FieldInfo ilSigHelp = ilGenType.GetField("m_localSignature", BindingFlags.Instance | BindingFlags.NonPublic);//SignatureHelper
#pragma warning restore S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
            SignatureHelper sigTemp = ilSigHelp.GetValue(ilGen) as SignatureHelper;
#pragma warning restore CS8600,CS8602 // Reflection should not be used to increase accessibility of classes, methods, or fields
            sigTemp.DisposeSignature();
#pragma warning restore CS8604 // Reflection should not be used to increase accessibility of classes, methods, or fields

            ilSigHelp.SetValue(ilGen, null);
        }
        /// <summary>
        /// Libera objetos da memoria durante a utilização do <see cref="ModuleBuilder"/>
        /// </summary>
        /// <remarks>
        /// Este método é necessário para evitar vazamentos de memoria
        /// </remarks>
        /// <param name="modBuild">A instancia do <see cref="ModuleBuilder"/></param>
        /// <param name="nome">ClassName do tipo que possui o método</param>
        public static void DisposeModuleBuilder(this ModuleBuilder modBuild, string? nome = null)
        {

            Type modBuildType = typeof(ModuleBuilder);
#pragma warning disable S3011,CS8600,CS8602,CS8604 // Reflection should not be used to increase accessibility of classes, methods, or fields
            FieldInfo modTypeBuildList = modBuildType.GetField("_typeBuilderDict", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
#pragma warning restore S3011,CS8600 // Reflection should not be used to increase accessibility of classes, methods, or fields

            if (modTypeBuildList.GetValue(modBuild) is Dictionary<string, Type> modTypeList)
            {
#pragma warning restore CS8602

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
#pragma warning restore CS8604
        }
        //https://stackoverflow.com/questions/2503645/reflect-emit-dynamic-type-memory-blowup

        public static string HashMethodInfo(this MethodInfo methodInfo)
        {
            return $"{methodInfo.ReturnType.FullName} {methodInfo.Name}({string.Join(',', methodInfo.GetParameters().Select(x => x.ParameterType.Name))})";
        }
        public static string HashPropertyInfo(this PropertyInfo methodInfo)
        {
            return $"{methodInfo.PropertyType.FullName} {methodInfo.Name}({string.Join(',', methodInfo.GetIndexParameters().Select(x => x.ParameterType.Name))})";
        }
    }
}
