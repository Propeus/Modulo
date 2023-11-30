using System.Reflection;
using System.Reflection.Emit;

using Propeus.Module.IL.Core.API;
using Propeus.Module.IL.Core.Enums;
using Propeus.Module.IL.Core.Geradores;
using Propeus.Module.IL.Geradores;

namespace Propeus.Module.IL.Core.Helpers
{
    public static class ClasseHelpers
    {
        private static ILClasseProvider Proxy(ILClasseProvider iLClasseProvider, Type classe, Type[] interfaces = null)
        {
            Type tClasse = classe;

            ILClasseProvider cls = iLClasseProvider;

            ClassApi.CreateField(cls.CurrentClass, new Token[] { Token.Privado }, tClasse, Constantes.CONST_NME_CLASSE_PROXY + tClasse.Name);
            ILFieldComponent cmp = cls.CurrentClass.Fields.Last();

            #region Construtores
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
            #endregion

            #region Metodos

            IEnumerable<MethodInfo> mthInterfaces = interfaces?.SelectMany(i => i.GetMethods());
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

            IEnumerable<PropertyInfo> propInterfaces = interfaces?.SelectMany(i => i.GetProperties());
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
                        _acessadoresL.Add(Token.Final);

                    if (!_acessadoresL.Contains(Token.NovoSlot))
                        _acessadoresL.Add(Token.NovoSlot);

                    if (!_acessadoresL.Contains(Token.Virtual))
                        _acessadoresL.Add(Token.Virtual);

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
                            _acessadoresL.Add(Token.Final);

                        if (!_acessadoresL.Contains(Token.NovoSlot))
                            _acessadoresL.Add(Token.NovoSlot);

                        if (!_acessadoresL.Contains(Token.Virtual))
                            _acessadoresL.Add(Token.Virtual);
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

                    var mth_info_set_params = mth_info_set.GetParameters();
                    List<ILParametro> iLParametros = new List<ILParametro>();
                    foreach (var param in mth_info_set_params)
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
                ConstructorInfo ctor = attr.GetType().GetConstructors().MinBy(x => x.GetParameters().Length);
                //TODO: Esta duplicando algo aqui
                if (ctor != null && attr.GetType().Name.Contains("Modulo"))
                {

                    object[] arrParams = ctor.GetParameters().Select(x => x.DefaultValue).ToArray();
                    CustomAttributeBuilder attributeBuilder = new CustomAttributeBuilder(ctor, arrParams);
                    cls.CurrentClass.Proxy.GetBuilder<TypeBuilder>().SetCustomAttribute(attributeBuilder);
                }

            }
            #endregion

            #region Operators

            #endregion

            return iLClasseProvider;
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
            Type tClasse = classe;
            if (iLGerador.ExisteClasseProvider(tClasse.Name, Constantes.CONST_NME_NAMESPACE_CLASSE_PROXY + '.' + tClasse.Namespace))
            {
                ILClasseProvider cls = iLGerador.ObterClasseProvider(tClasse.Name, Constantes.CONST_NME_NAMESPACE_CLASSE_PROXY + '.' + tClasse.Namespace);
                return cls;
            }
            else
            {
                ILClasseProvider cls = iLGerador.CriarClasseProvider(tClasse.Name, Constantes.CONST_NME_NAMESPACE_CLASSE_PROXY + '.' + tClasse.Namespace, null, interfaces, null, atributos);
                return Proxy(cls, classe, interfaces);

            }
        }

        public static Type ObterTipoGerado(this ILClasseProvider iLClasseProvider)
        {
            return iLClasseProvider.CurrentClass.DynamicTypeClass;
        }
    }
}
