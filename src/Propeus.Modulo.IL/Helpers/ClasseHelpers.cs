using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Principal;

using Propeus.Modulo.IL.Enums;
using Propeus.Modulo.IL.Geradores;
using Propeus.Modulo.IL.Proxy;
using Propeus.Modulo.Util;

namespace Propeus.Modulo.IL.Helpers
{
    public static class ClasseHelpers
    {
        private static ILClasseProvider Proxy(ILClasseProvider iLClasseProvider, Type classe, Type[] interfaces = null)
        {
            if (iLClasseProvider is null)
            {
                throw new ArgumentNullException(nameof(iLClasseProvider));
            }

            Type tClasse = classe;

            ILClasseProvider cls = iLClasseProvider;

            API.ClasseAPI.CriarCampo(cls.Atual, new Token[] { Enums.Token.Privado }, tClasse, Constantes.CONST_NME_CLASSE_PROXY + tClasse.Name);
            ILCampo cmp = cls.Atual.Campos.Last();

            #region Construtores
            foreach (ConstructorInfo c in tClasse.GetConstructors())
            {

                API.ClasseAPI.CriarMetodo(cls.Atual, c.Attributes.DividirEnum().ParseEnum<MethodAttributes, Token>(), typeof(void), ".ctor", c.GetParameters().Select(p => new ILParametro(".ctor", p.ParameterType, p.IsOptional, p.DefaultValue, p.Name)).ToArray());
                ILMetodo ctorMth = cls.Atual.Metodos.Last();

                API.MetodoAPI.CarregarArgumento(ctorMth);
                API.MetodoAPI.ChamarFuncao(ctorMth, typeof(object).GetConstructors()[0]);
                for (int i = 0; i <= c.GetParameters().Length; i++)
                {
                    API.MetodoAPI.CarregarArgumento(ctorMth, i);
                }
                API.MetodoAPI.CriarObjeto(ctorMth, c);
                API.MetodoAPI.ArmazenarValorCampo(ctorMth, cmp);
                API.MetodoAPI.CriarRetorno(ctorMth);
            }
            #endregion

            #region Metodos
      
            IEnumerable<MethodInfo> mthInterfaces = interfaces.SelectMany(i => i.GetMethods());
            MethodInfo[] mths = tClasse.GetMethods();

            IDictionary<MethodInfo, bool> methods = mthInterfaces.FullJoinDictionaryMethodInfo(mths);

            foreach (KeyValuePair<MethodInfo, bool> metodoKP in methods)
            {
                MethodInfo metodo = metodoKP.Key;

                if (metodo.Name.Contains("get_") || metodo.Name.Contains("set_"))
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
                    API.ClasseAPI.CriarMetodo(cls.Atual, _acessadores, metodo.ReturnType, metodo.Name);
                }
                else
                {
                    API.ClasseAPI.CriarMetodo(cls.Atual, _acessadores, metodo.ReturnType, metodo.Name, metodo.GetParameters().Select(p => new ILParametro(metodo.Name, p.ParameterType, p.IsOptional, p.DefaultValue, p.Name)).ToArray());
                }

                ILMetodo mth = cls.Atual.Metodos.Last();
                API.MetodoAPI.CarregarArgumento(mth);
                API.MetodoAPI.CarregarValorCampo(mth, cmp);
                for (int i = 1; i <= metodo.GetParameters().Length; i++)
                {
                    API.MetodoAPI.CarregarArgumento(mth, i);
                }
                MethodInfo cmpMetodo = tClasse.GetMethod(metodo.Name);
                API.MetodoAPI.ChamarFuncao(mth, cmpMetodo);
                API.MetodoAPI.CriarRetorno(mth);
            }
            #endregion

            #region Propriedades
            foreach (PropertyInfo propriedade in tClasse.GetProperties())
            {
                MethodInfo mth_info_get = propriedade.GetGetMethod();
                MethodInfo mth_info_set = propriedade.GetSetMethod();

                Type[] propriedadeParametros = propriedade.GetIndexParameters().Select(p => p.ParameterType).ToArray();
                API.ClasseAPI.CriarPropriedade(cls.Atual, propriedade.PropertyType, propriedade.Name, propriedadeParametros);
                ILPropriedade prop = cls.Atual.Propriedades.Last();

                prop.IsProxy = true;

                API.ClasseAPI.CriarMetodo(cls.Atual, mth_info_get.Attributes.DividirEnum().ParseEnum<MethodAttributes, Token>(), prop.Retorno, Constantes.CONST_NME_PROPRIEDADE_METODO_GET + prop.Nome, propriedadeParametros.Select(p => new ILParametro(Constantes.CONST_NME_PROPRIEDADE_METODO_GET + prop.Nome, p)).ToArray());
                ILMetodo mth_get = cls.Atual.Metodos.Last();

                prop.Getter = mth_get;

                API.MetodoAPI.CarregarArgumento(mth_get);
                API.MetodoAPI.CarregarValorCampo(mth_get, cmp);
                for (int i = 1; i <= propriedadeParametros.Length; i++)
                {
                    API.MetodoAPI.CarregarArgumento(mth_get, i);
                }
                API.MetodoAPI.ChamarFuncaoVirtual(mth_get, mth_info_get);
                API.MetodoAPI.CriarRetorno(mth_get);

                if (prop.Setter != null)
                {
                    API.ClasseAPI.CriarMetodo(cls.Atual, mth_info_set.Attributes.DividirEnum().ParseEnum<MethodAttributes, Token>(), typeof(void), Constantes.CONST_NME_PROPRIEDADE_METODO_SET + prop.Nome, new ILParametro[] { new ILParametro(Constantes.CONST_NME_PROPRIEDADE_METODO_SET + prop.Nome, prop.Retorno) });
                    ILMetodo mth_set = cls.Atual.Metodos.Last();

                    prop.Setter = mth_set;

                    API.MetodoAPI.CarregarArgumento(mth_set);
                    API.MetodoAPI.CarregarValorCampo(mth_set, cmp);
                    API.MetodoAPI.CarregarArgumento(mth_set, 1);
                    API.MetodoAPI.ChamarFuncaoVirtual(mth_set, mth_info_set);
                    API.MetodoAPI.CriarRetorno(mth_set);
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
                ConstructorInfo ctor = attr.GetType().GetConstructors().MinBy(x=> x.GetParameters().Length);
                if (ctor != null)
                {
                    CustomAttributeBuilder attributeBuilder = new(ctor, Array.Empty<object>());
                    cls.Atual.Proxy.ObterBuilder<TypeBuilder>().SetCustomAttribute(attributeBuilder);
                }

            }
            #endregion

            return iLClasseProvider;
        }

        public static ILClasseProvider CriarProxyClasse(this ILClasseProvider iLGerador, Type classe, Type[] interfaces = null)
        {
            return Proxy(iLGerador, classe, interfaces);
        }
        public static ILClasseProvider CriarProxyClasse(this ILModulo iLGerador, Type classe, Type[] interfaces = null)
        {
            if (iLGerador is null)
            {
                throw new ArgumentNullException(nameof(iLGerador));
            }

            Type tClasse = classe;

            interfaces = tClasse.GetInterfaces().FullJoin(interfaces).ToArray();

            ILClasseProvider cls = iLGerador.CriarClasseProvider(tClasse.Name, Constantes.CONST_NME_NAMESPACE_CLASSE_PROXY + '.' + tClasse.Namespace, null, interfaces);

            return Proxy(cls, classe, interfaces);
        }

        public static ILClasseProvider CriarProxyClasse<TClasse>(this ILModulo iLGerador, Type[] interfaces = null)
            where TClasse : class
        {
            return iLGerador.CriarProxyClasse(typeof(TClasse), interfaces);
        }
        public static ILClasseProvider CriarClasse(this ILModulo iLGerador, string nome, string @namespace, Type tipoBase, Type[] interfaces, Token[] token)
        {
            return iLGerador.CriarClasseProvider(nome, @namespace, tipoBase, interfaces, token);
        }

       
        public static ILDelegate CriarDelegate(this ILClasseProvider iLClasse, Type tipoSaida, string nomeDelegate, params ILParametro[] parametros)
        {

            ILBuilderProxy iLGerador = iLClasse.Atual.Proxy;
            ILDelegate cls = new(iLGerador, nomeDelegate, null, new Enums.Token[] { Token.PublicaAninhado, Token.Auto, Token.Ansi, Token.Selado });

            _ = cls.CriarConstrutor(
                   new Token[] { Token.Publico, Token.OcutarAssinatura, Token.NomeEspecial, Token.RotuloNomeEspecial },
                   new ILParametro(".ctor", typeof(object), nome: "object"),
                   new ILParametro(".ctor", typeof(nint), nome: "method"));

            _ = cls.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura, Token.NovoSlot, Token.Virtual }, tipoSaida,
                "Invoke", parametros);
            _ = cls.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura, Token.NovoSlot, Token.Virtual },
                typeof(IAsyncResult),
                "BeginInvoke",
                parametros.FullJoin(new ILParametro[] {
                new ILParametro("BeginInvoke", typeof(AsyncCallback)),
                new ILParametro("BeginInvoke", typeof(object)) }).ToArray());
            _ = cls.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura, Token.NovoSlot, Token.Virtual },
                typeof(int),
                "EndInvoke",
                new ILParametro("result", typeof(IAsyncResult)));

            iLClasse.Atual.Delegates.Add(cls);

            return cls;
        }
       
        public static dynamic ObterInstancia(this ILClasseProvider iLClasse, params object[] args)
        {
            return Activator.CreateInstance(iLClasse.Atual.TipoGerado, args);
        }
    
        public static Type ObterTipoGerado(this ILClasseProvider iLClasseProvider)
        {
            return iLClasseProvider.Atual.TipoGerado;
        }
    }
}
