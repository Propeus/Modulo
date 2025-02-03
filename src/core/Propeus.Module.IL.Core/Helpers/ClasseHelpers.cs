using System.Reflection;
using System.Reflection.Emit;

using Propeus.Module.IL.Core.API;
using Propeus.Module.IL.Core.Enums;
using Propeus.Module.IL.Core.Geradores;
using Propeus.Module.IL.Core.Geradores.Components;

namespace Propeus.Module.IL.Core.Helpers
{
    /// <summary>
    /// Classe de ajuda para criar novas clases dinamicamente
    /// </summary>
    public static class ClasseHelpers
    {
        //Valor para verificar se tem algum atributo com o sufixo ou prefixo 'Module'
        private const string STR_TYPE_ATTRIBUTE = "Module";



        private static ILClasseProvider Proxy(ILClasseProvider iLClasseProvider, Type classe, Type[]? interfaces = null, bool proxyTemporario = false)
        {
            Type tClasse = classe;

            ILClasseProvider cls = iLClasseProvider;

            ClassApi.CreateField(cls.CurrentClass, new Token[] { Token.Privado }, tClasse, Constantes.CONST_NME_CLASSE_PROXY + tClasse.Name);
            ILFieldComponent cmp = cls.CurrentClass.Fields.Last();


            #region Construtores
            if (!proxyTemporario)
            {
                foreach (ConstructorInfo c in tClasse.GetConstructors())
                {

                    ClassApi.CreateMethod(cls.CurrentClass, c.Attributes.SplitEnum().ParseEnum<MethodAttributes, Token>(), typeof(void), ".ctor", c.GetParameters().Select(p => new ILParametro(".ctor", p.ParameterType, p.IsOptional, p.DefaultValue, p.Name)).ToArray());
                    ILMethodComponent ctorMth = cls.CurrentClass.Methods.Last();

                    MetodoApi.LoadMethodArgument(ctorMth);
                    MetodoApi.CallMethod(ctorMth, typeof(object).GetConstructors()[0]);
                    for (int i = 0; i <= c.GetParameters().Length; i++)
                    {
                        MetodoApi.LoadMethodArgument(ctorMth, i);
                    }
                    MetodoApi.CreateNewInstanceObject(ctorMth, c);
                    MetodoApi.StoreValueToField(ctorMth, cmp);
                    MetodoApi.CreateReturn(ctorMth);
                }
            }
            else
            {
                ClassApi.CreateMethod(cls.CurrentClass, new Token[] { Token.Publico, Token.OcutarAssinatura, Token.NomeEspecial, Token.RotuloNomeEspecial }, typeof(void), ".ctor", new ILParametro[] { new ILParametro(".ctor", tClasse, nome: "_cmpProxy") });
                ILMethodComponent ctorMth = cls.CurrentClass.Methods.Last();

                MetodoApi.LoadMethodArgument(ctorMth);
                MetodoApi.CallMethod(ctorMth, typeof(object).GetConstructors()[0]);

                MetodoApi.LoadMethodArgument(ctorMth, 0);
                MetodoApi.LoadMethodArgument(ctorMth, 1);

                MetodoApi.StoreValueToField(ctorMth, cmp);
                MetodoApi.CreateReturn(ctorMth);
            }
            #endregion

            #region Metodos
            IEnumerable<MethodInfo> mthInterfaces = Array.Empty<MethodInfo>();
            if (interfaces is not null)
            {
                mthInterfaces = interfaces.SelectMany(i => i.GetMethods());
            }

            MethodInfo[] mths = tClasse.GetMethods();

            IDictionary<MethodInfo, bool> methods = mthInterfaces.FullJoinDictionaryMethodInfo(mths);

            foreach (KeyValuePair<MethodInfo, bool> metodoKP in methods)
            {
                MethodInfo metodo = metodoKP.Key;

                if (metodo.Name.StartsWith("get_") || metodo.Name.StartsWith("set_"))
                {
                    continue;
                }
                Token[] _acessadores;
                if (metodoKP.Value)
                {
                    List<Token> _acessadoresL = metodo.Attributes.SplitEnum().ParseEnum<MethodAttributes, Token>().ToList();
                    _ = _acessadoresL.Remove(Token.Abstrato);
                    _ = _acessadoresL.Remove(Token.ReusoSlot);
                    _ = _acessadoresL.Remove(Token.VtableLayoutMask);
                    _acessadoresL.Add(Token.Final);
                    _acessadoresL.Add(Token.NovoSlot);
                    _acessadores = _acessadoresL.ToArray();
                }
                else
                {
                    _acessadores = metodo.Attributes.SplitEnum().ParseEnum<MethodAttributes, Token>();
                }

                if (metodo.GetParameters().Length == 0)
                {
                    ClassApi.CreateMethod(cls.CurrentClass, _acessadores, metodo.ReturnType, metodo.Name);
                }
                else
                {
                    ClassApi.CreateMethod(cls.CurrentClass, _acessadores, metodo.ReturnType, metodo.Name, metodo.GetParameters().Select(p => new ILParametro(metodo.Name, p.ParameterType, p.IsOptional, p.DefaultValue, p.Name)).ToArray());
                }

                ILMethodComponent mth = cls.CurrentClass.Methods.Last();
                MetodoApi.LoadMethodArgument(mth);
                MetodoApi.LoadField(mth, cmp);
                for (int i = 1; i <= metodo.GetParameters().Length; i++)
                {
                    MetodoApi.LoadMethodArgument(mth, i);
                }
                MethodInfo cmpMetodo = tClasse.GetMethod(metodo.Name, mth.Parameters.Select(x => x.Tipo).ToArray());
                MetodoApi.CallMethod(mth, cmpMetodo);
                MetodoApi.CreateReturn(mth);
            }
            #endregion

            #region Propriedades
            IEnumerable<PropertyInfo> propInterfaces = Array.Empty<PropertyInfo>();
            if (interfaces is not null)
            {
                propInterfaces = interfaces.SelectMany(i => i.GetProperties());
            }

            PropertyInfo[] props = tClasse.GetProperties();

            IDictionary<PropertyInfo, bool> properts = propInterfaces.FullJoinDictionaryPropertyInfo(props);

            foreach (KeyValuePair<PropertyInfo, bool> propriedadeKP in properts)
            {
                PropertyInfo propriedade = propriedadeKP.Key;

                MethodInfo mth_info_get = propriedade.GetGetMethod();
                MethodInfo mth_info_set = propriedade.GetSetMethod();

                Type[] propriedadeParametros = propriedade.GetIndexParameters().Select(p => p.ParameterType).ToArray();

                ClassApi.CreateProperty(cls.CurrentClass, propriedade.PropertyType, propriedade.Name, propriedadeParametros);
                ILPropertyComponent prop = cls.CurrentClass.Properties.Last();

                prop.IsProxy = true;
                Token[] _acessadores;

                if (propriedadeKP.Value)
                {
                    List<Token> _acessadoresL = mth_info_get.Attributes.SplitEnum().ParseEnum<MethodAttributes, Token>().ToList();
                    _ = _acessadoresL.Remove(Token.Abstrato);
                    _ = _acessadoresL.Remove(Token.ReusoSlot);
                    _ = _acessadoresL.Remove(Token.VtableLayoutMask);

                    if (!_acessadoresL.Contains(Token.Final))
                    {
                        _acessadoresL.Add(Token.Final);
                    }

                    if (!_acessadoresL.Contains(Token.NovoSlot))
                    {
                        _acessadoresL.Add(Token.NovoSlot);
                    }

                    if (!_acessadoresL.Contains(Token.Virtual))
                    {
                        _acessadoresL.Add(Token.Virtual);
                    }

                    _acessadores = _acessadoresL.ToArray();
                }
                else
                {
                    List<Token> _acessadoresL = mth_info_get.Attributes.SplitEnum().ParseEnum<MethodAttributes, Token>().ToList();
                    _ = _acessadoresL.Remove(Token.Abstrato);
                    _ = _acessadoresL.Remove(Token.ReusoSlot);
                    _ = _acessadoresL.Remove(Token.VtableLayoutMask);
                    _acessadores = _acessadoresL.ToArray();

                }


                ClassApi.CreateMethod(cls.CurrentClass, _acessadores, prop.Type, Constantes.CONST_NME_PROPRIEDADE_METODO_GET + prop.Name, propriedadeParametros.Select(p => new ILParametro(Constantes.CONST_NME_PROPRIEDADE_METODO_GET + prop.Name, p)).ToArray());

                ILMethodComponent mth_get = cls.CurrentClass.Methods.Last();

                prop.Getter = mth_get;

                MetodoApi.LoadMethodArgument(mth_get);
                MetodoApi.LoadField(mth_get, cmp);
                for (int i = 1; i <= propriedadeParametros.Length; i++)
                {
                    MetodoApi.LoadMethodArgument(mth_get, i);
                }
                MetodoApi.CallVirtualMethod(mth_get, mth_info_get);
                MetodoApi.CreateReturn(mth_get);

                if (mth_info_set != null)
                {
                    if (propriedadeKP.Value)
                    {
                        List<Token> _acessadoresL = mth_info_set.Attributes.SplitEnum().ParseEnum<MethodAttributes, Token>().ToList();
                        _ = _acessadoresL.Remove(Token.Abstrato);
                        _ = _acessadoresL.Remove(Token.ReusoSlot);
                        _ = _acessadoresL.Remove(Token.VtableLayoutMask);
                        if (!_acessadoresL.Contains(Token.Final))
                        {
                            _acessadoresL.Add(Token.Final);
                        }

                        if (!_acessadoresL.Contains(Token.NovoSlot))
                        {
                            _acessadoresL.Add(Token.NovoSlot);
                        }

                        if (!_acessadoresL.Contains(Token.Virtual))
                        {
                            _acessadoresL.Add(Token.Virtual);
                        }

                        _acessadores = _acessadoresL.ToArray();
                    }
                    else
                    {
                        List<Token> _acessadoresL = mth_info_set.Attributes.SplitEnum().ParseEnum<MethodAttributes, Token>().ToList();
                        _ = _acessadoresL.Remove(Token.Abstrato);
                        _ = _acessadoresL.Remove(Token.ReusoSlot);
                        _ = _acessadoresL.Remove(Token.VtableLayoutMask);
                        _acessadoresL.Add(Token.OcutarAssinatura);
                        _acessadoresL.Add(Token.NomeEspecial);
                        _acessadores = _acessadoresL.ToArray();
                    }

                    ParameterInfo[] mth_info_set_params = mth_info_set.GetParameters();
                    List<ILParametro> iLParametros = new List<ILParametro>();
                    foreach (ParameterInfo param in mth_info_set_params)
                    {
                        iLParametros.Add(new ILParametro(Constantes.CONST_NME_PROPRIEDADE_METODO_SET + prop.Name, param.ParameterType, param.IsOptional, param.DefaultValue, param.Name));
                    }

                    ClassApi.CreateMethod(cls.CurrentClass, _acessadores, typeof(void), Constantes.CONST_NME_PROPRIEDADE_METODO_SET + prop.Name, iLParametros.ToArray());
                    ILMethodComponent mth_set = cls.CurrentClass.Methods.Last();

                    prop.Setter = mth_set;

                    MetodoApi.LoadMethodArgument(mth_set);
                    MetodoApi.LoadField(mth_set, cmp);
                    for (int i = 1; i <= iLParametros.Count; i++)
                    {
                        MetodoApi.LoadMethodArgument(mth_set, i);
                    }
                    MetodoApi.CallVirtualMethod(mth_set, mth_info_set);
                    MetodoApi.CreateReturn(mth_set);
                }

            }
            #endregion

            #region Delegates
            //Implemnetar proxy de delegate
            #endregion

            #region Atributos
            IEnumerable<Attribute> attrs = tClasse.GetCustomAttributes();
            foreach (Attribute attr in attrs)
            {
                Type attributeType = attr.GetType();

                if (attributeType.Name.Contains(STR_TYPE_ATTRIBUTE))
                {
                    //CustomAttributeBuilder attributeBuilder = BuildCustomAttribute(attr);
                    ConstructorInfo[] attributeConstructors = attributeType.GetConstructors();
                    ConstructorInfo attributeConstructor = attributeConstructors.MinBy(x => x.GetParameters().Length);

                    var attributePropertiesInfo = attributeType.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(x => x.SetMethod is not null).ToArray();
                    var attributePropertiesValue = attributePropertiesInfo.Select(prop => prop.GetValue(attr, null)).ToArray();

                    CustomAttributeBuilder attributeBuilder = new CustomAttributeBuilder(attributeConstructor, Array.Empty<object>(), attributePropertiesInfo, attributePropertiesValue);

                    cls.CurrentClass.Proxy.GetBuilder<TypeBuilder>().SetCustomAttribute(attributeBuilder);
                }

            }
            #endregion

            #region Operators

            #endregion

            return iLClasseProvider;
        }

        /// <summary>
        /// Cria uma nova classe temporaria com base em uma existente
        /// </summary>
        /// <param name="iLGerador">Gerador de classes</param>
        /// <param name="classe">Classe a ser copiado</param>
        /// <param name="interfaces">Interfaces a serem implementados</param>
        /// <param name="atributos">Atributos a serem acrescentado na classe temporaria</param>
        /// <returns>Retorna um provedor de classe</returns>
        public static ILClasseProvider CriarClasseTemporaria(this ILModulo iLGerador, Type classe, Type[] interfaces = null, Type[] atributos = null)
        {
            //Type tClasse = classe;
            //string guidClasse = Guid.NewGuid().ToString().Replace("-", "_");
            //string nome_classe = tClasse.Name + "_TEMP_" + guidClasse;
            //string nome_classe = 

            ILClasseProvider cls = iLGerador.CriarClasseProvider(classe.Name + Constantes.CONST_SUFIXO_CLASSE_TEMPORARIA, Constantes.CONST_NME_NAMESPACE_CLASSE_PROXY + '.' + classe.Namespace, null, interfaces, null, atributos);
            return Proxy(cls, classe, interfaces, proxyTemporario: true);

        }
        /// <summary>
        /// Cria uma nova classe temporaria com base em uma existente
        /// </summary>
        /// <param name="iLGerador">Gerador de classes</param>
        /// <param name="classe">Classe a ser copiado</param>
        /// <param name="interfaces">Interfaces a serem implementados</param>
        /// <param name="atributos">Atributos a serem acrescentado na classe temporaria</param>
        /// <returns>Retorna um provedor de classe</returns>
        public static ILClasseProvider CriarOuObterClasseTemporaria(this ILModulo iLGerador, Type classe, Type[] interfaces = null, Type[] atributos = null)
        {
            if (iLGerador.ExisteClasseProvider(classe.Name + Constantes.CONST_SUFIXO_CLASSE_TEMPORARIA, Constantes.CONST_NME_NAMESPACE_CLASSE_PROXY + '.' + classe.Namespace))
            {
                ILClasseProvider cls = iLGerador.ObterClasseProvider(classe.Name + Constantes.CONST_SUFIXO_CLASSE_TEMPORARIA, Constantes.CONST_NME_NAMESPACE_CLASSE_PROXY + '.' + classe.Namespace);
                return cls;
            }
            else
            {
                ILClasseProvider cls = iLGerador.CriarClasseProvider(classe.Name + Constantes.CONST_SUFIXO_CLASSE_TEMPORARIA, Constantes.CONST_NME_NAMESPACE_CLASSE_PROXY + '.' + classe.Namespace, null, interfaces, null, atributos);
                return Proxy(cls, classe, interfaces,true);

            }
        }

        public static ILClasseProvider CriarProxyClasse(this ILClasseProvider iLGerador, Type classe, Type[] interfaces = null)
        {
            Type tClasse = classe;

            interfaces = tClasse.GetInterfaces().FullJoin(interfaces).FullJoin(iLGerador.Interfaces).ToArray();

            return Proxy(iLGerador, classe, interfaces);
        }
        public static ILClasseProvider CriarProxyClasse(this ILModulo iLGerador, Type classe, Type[] interfaces = null, Type[] atributos = null)
        {
            Type tClasse = classe;

            interfaces = tClasse.GetInterfaces().FullJoin(interfaces).ToArray();

            ILClasseProvider cls = iLGerador.CriarClasseProvider(tClasse.Name, Constantes.CONST_NME_NAMESPACE_CLASSE_PROXY + '.' + tClasse.Namespace, null, interfaces, null, atributos);

            return Proxy(cls, classe, interfaces);
        }
        public static ILClasseProvider CriarOuObterProxyClasse(this ILModulo iLGerador, Type classe, Type[] interfaces = null, Type[] atributos = null)
        {

            if (iLGerador.ExisteClasseProvider(classe.Name, Constantes.CONST_NME_NAMESPACE_CLASSE_PROXY + '.' + classe.Namespace))
            {
                ILClasseProvider cls = iLGerador.ObterClasseProvider(classe.Name, Constantes.CONST_NME_NAMESPACE_CLASSE_PROXY + '.' + classe.Namespace);
                return cls;
            }
            else
            {
                ILClasseProvider cls = iLGerador.CriarClasseProvider(classe.Name, Constantes.CONST_NME_NAMESPACE_CLASSE_PROXY + '.' + classe.Namespace, null, interfaces, null, atributos);
                return Proxy(cls, classe, interfaces);

            }
        }
        public static ILClasseProvider ObterProxyClasse(this ILModulo iLGerador, Type classe)
        {
            ILClasseProvider cls = iLGerador.ObterClasseProvider(classe.Name, Constantes.CONST_NME_NAMESPACE_CLASSE_PROXY + '.' + classe.Namespace);
            return cls;
        }
        public static bool ExisteProxyClasse(this ILModulo iLGerador, Type classe)
        {
            return iLGerador.ExisteClasseProvider(classe.Name, Constantes.CONST_NME_NAMESPACE_CLASSE_PROXY + '.' + classe.Namespace);
        }

        public static Type ObterTipoGerado(this ILClasseProvider iLClasseProvider)
        {
            return iLClasseProvider.CurrentClass.DynamicTypeClass;
        }
    }
}
