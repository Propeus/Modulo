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

            ClasseAPI.CriarCampo(cls.Atual, new Token[] { Token.Privado }, tClasse, Constantes.CONST_NME_CLASSE_PROXY + tClasse.Name);
            ILCampo cmp = cls.Atual.Campos.Last();

            #region Construtores
            foreach (ConstructorInfo c in tClasse.GetConstructors())
            {

                ClasseAPI.CriarMetodo(cls.Atual, c.Attributes.DividirEnum().ParseEnum<MethodAttributes, Token>(), typeof(void), ".ctor", c.GetParameters().Select(p => new ILParametro(".ctor", p.ParameterType, p.IsOptional, p.DefaultValue, p.Name)).ToArray());
                ILMetodo ctorMth = cls.Atual.Metodos.Last();

                MetodoAPI.CarregarArgumento(ctorMth);
                MetodoAPI.ChamarFuncao(ctorMth, typeof(object).GetConstructors()[0]);
                for (int i = 0; i <= c.GetParameters().Length; i++)
                {
                    MetodoAPI.CarregarArgumento(ctorMth, i);
                }
                MetodoAPI.CriarObjeto(ctorMth, c);
                MetodoAPI.ArmazenarValorCampo(ctorMth, cmp);
                MetodoAPI.CriarRetorno(ctorMth);
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
                    List<Token> _acessadoresL = metodo.Attributes.DividirEnum().ParseEnum<MethodAttributes, Token>().ToList();
                    _ = _acessadoresL.Remove(Token.Abstrato);
                    _ = _acessadoresL.Remove(Token.ReusoSlot);
                    _ = _acessadoresL.Remove(Token.VtableLayoutMask);
                    _acessadoresL.Add(Token.Final);
                    _acessadoresL.Add(Token.NovoSlot);
                    _acessadores = _acessadoresL.ToArray();
                }
                else
                {
                    _acessadores = metodo.Attributes.DividirEnum().ParseEnum<MethodAttributes, Token>();
                }

                if (metodo.GetParameters().Length == 0)
                {
                    ClasseAPI.CriarMetodo(cls.Atual, _acessadores, metodo.ReturnType, metodo.Name);
                }
                else
                {
                    ClasseAPI.CriarMetodo(cls.Atual, _acessadores, metodo.ReturnType, metodo.Name, metodo.GetParameters().Select(p => new ILParametro(metodo.Name, p.ParameterType, p.IsOptional, p.DefaultValue, p.Name)).ToArray());
                }

                ILMetodo mth = cls.Atual.Metodos.Last();
                MetodoAPI.CarregarArgumento(mth);
                MetodoAPI.CarregarValorCampo(mth, cmp);
                for (int i = 1; i <= metodo.GetParameters().Length; i++)
                {
                    MetodoAPI.CarregarArgumento(mth, i);
                }
                MethodInfo cmpMetodo = tClasse.GetMethod(metodo.Name, mth.Parametros.Select(x => x.Tipo).ToArray());
                MetodoAPI.ChamarFuncao(mth, cmpMetodo);
                MetodoAPI.CriarRetorno(mth);
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

                ClasseAPI.CriarPropriedade(cls.Atual, propriedade.PropertyType, propriedade.Name, propriedadeParametros);
                ILPropriedade prop = cls.Atual.Propriedades.Last();

                prop.IsProxy = true;
                Token[] _acessadores;

                if (propriedadeKP.Value)
                {
                    List<Token> _acessadoresL = mth_info_get.Attributes.DividirEnum().ParseEnum<MethodAttributes, Token>().ToList();
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
                    List<Token> _acessadoresL = mth_info_get.Attributes.DividirEnum().ParseEnum<MethodAttributes, Token>().ToList();
                    _ = _acessadoresL.Remove(Token.Abstrato);
                    _ = _acessadoresL.Remove(Token.ReusoSlot);
                    _ = _acessadoresL.Remove(Token.VtableLayoutMask);
                    _acessadores = _acessadoresL.ToArray();

                }


                ClasseAPI.CriarMetodo(cls.Atual, _acessadores, prop.Retorno, Constantes.CONST_NME_PROPRIEDADE_METODO_GET + prop.Nome, propriedadeParametros.Select(p => new ILParametro(Constantes.CONST_NME_PROPRIEDADE_METODO_GET + prop.Nome, p)).ToArray());

                ILMetodo mth_get = cls.Atual.Metodos.Last();

                prop.Getter = mth_get;

                MetodoAPI.CarregarArgumento(mth_get);
                MetodoAPI.CarregarValorCampo(mth_get, cmp);
                for (int i = 1; i <= propriedadeParametros.Length; i++)
                {
                    MetodoAPI.CarregarArgumento(mth_get, i);
                }
                MetodoAPI.ChamarFuncaoVirtual(mth_get, mth_info_get);
                MetodoAPI.CriarRetorno(mth_get);

                if (mth_info_set != null)
                {
                    if (propriedadeKP.Value)
                    {
                        List<Token> _acessadoresL = mth_info_set.Attributes.DividirEnum().ParseEnum<MethodAttributes, Token>().ToList();
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
                        List<Token> _acessadoresL = mth_info_set.Attributes.DividirEnum().ParseEnum<MethodAttributes, Token>().ToList();
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
                        iLParametros.Add(new ILParametro(Constantes.CONST_NME_PROPRIEDADE_METODO_SET + prop.Nome, param.ParameterType, param.IsOptional, param.DefaultValue, param.Name));
                    }

                    ClasseAPI.CriarMetodo(cls.Atual, _acessadores, typeof(void), Constantes.CONST_NME_PROPRIEDADE_METODO_SET + prop.Nome, iLParametros.ToArray());
                    ILMetodo mth_set = cls.Atual.Metodos.Last();

                    prop.Setter = mth_set;

                    MetodoAPI.CarregarArgumento(mth_set);
                    MetodoAPI.CarregarValorCampo(mth_set, cmp);
                    for (int i = 1; i <= iLParametros.Count; i++)
                    {
                        MetodoAPI.CarregarArgumento(mth_set, i);
                    }
                    MetodoAPI.ChamarFuncaoVirtual(mth_set, mth_info_set);
                    MetodoAPI.CriarRetorno(mth_set);
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
                    cls.Atual.Proxy.ObterBuilder<TypeBuilder>().SetCustomAttribute(attributeBuilder);
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
            return iLClasseProvider.Atual.TipoGerado;
        }
    }
}
