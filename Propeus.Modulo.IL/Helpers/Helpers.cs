﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Propeus.Modulo.Abstrato.Util;
using Propeus.Modulo.IL.Enums;

using Propeus.Modulo.IL.Geradores;

namespace Propeus.Modulo.IL.Helpers
{
    public static class Helpers
    {
        public static ILClasseProvider CriarProxyClasse<TClasse>(this ILModulo iLGerador)
            where TClasse : class
        {
            if (iLGerador is null)
            {
                throw new ArgumentNullException(nameof(iLGerador));
            }

            Type tClasse = typeof(TClasse);

            ILClasseProvider cls = iLGerador.CriarClasseProvider(tClasse.Name, tClasse.Namespace + ".IL.Proxy", interfaces: tClasse.GetInterfaces());

            API.ClasseAPI.CriarCampo(cls.Atual, new Token[] { Enums.Token.Privado }, tClasse, "IL_Gerador_Proxy_" + tClasse.Name);
            ILCampo cmp = cls.Atual.Campos.Last();

            foreach (var c in tClasse.GetConstructors())
            {

                API.ClasseAPI.CriarMetodo(cls.Atual, c.Attributes.DividirEnum().ParseEnum<MethodAttributes, Token>(), typeof(void), ".ctor", c.ObterTipoParametros().ToArray());
                var ctorMth = cls.Atual.Metodos.Last();

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

            foreach (var metodo in tClasse.GetMethods())
            {
                if (metodo.Name.Contains("get_") || metodo.Name.Contains("set_"))
                {
                    continue;
                }


                if (metodo.GetParameters().Length == 0)
                {
                    API.ClasseAPI.CriarMetodo(cls.Atual, metodo.Attributes.DividirEnum().ParseEnum<MethodAttributes, Token>(), metodo.ReturnType, metodo.Name);
                }
                else
                {
                    API.ClasseAPI.CriarMetodo(cls.Atual, metodo.Attributes.DividirEnum().ParseEnum<MethodAttributes, Token>(), metodo.ReturnType, metodo.Name, metodo.ObterTipoParametros().ToArray());
                }

                var mth = cls.Atual.Metodos.Last();
                API.MetodoAPI.CarregarArgumento(mth);
                API.MetodoAPI.CarregarValorCampo(mth, cmp);
                for (int i = 1; i <= metodo.GetParameters().Length; i++)
                {
                    API.MetodoAPI.CarregarArgumento(mth, i);
                }
                API.MetodoAPI.ChamarFuncaoVirtual(mth, metodo);
                API.MetodoAPI.CriarRetorno(mth);
            }

            foreach (var propriedade in tClasse.GetProperties())
            {
                var mth_info_get = propriedade.GetGetMethod();
                var mth_info_set = propriedade.GetSetMethod();

                var propriedadeParametros = propriedade.GetIndexParameters().Select(p => p.ParameterType).ToArray();
                API.ClasseAPI.CriarPropriedade(cls.Atual, propriedade.PropertyType, propriedade.Name, propriedadeParametros);
                var prop = cls.Atual.Propriedades.Last();

                API.ClasseAPI.CriarMetodo(cls.Atual, mth_info_get.Attributes.DividirEnum().ParseEnum<MethodAttributes, Token>(), prop.Retorno, Constantes.CONST_NME_PROPRIEDADE_METODO_GET + prop.Nome, propriedadeParametros);
                var mth_get = cls.Atual.Metodos.Last();

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
                    API.ClasseAPI.CriarMetodo(cls.Atual, mth_info_set.Attributes.DividirEnum().ParseEnum<MethodAttributes, Token>(), typeof(void), Constantes.CONST_NME_PROPRIEDADE_METODO_SET + prop.Nome, new Type[] { prop.Retorno });
                    var mth_set = cls.Atual.Metodos.Last();

                    API.MetodoAPI.CarregarArgumento(mth_set);
                    API.MetodoAPI.CarregarValorCampo(mth_set, cmp);
                    API.MetodoAPI.CarregarArgumento(mth_set, 1);
                    API.MetodoAPI.ChamarFuncaoVirtual(mth_set, mth_info_set);
                    API.MetodoAPI.CriarRetorno(mth_set);
                }

            }


            return cls;
        }
        public static ILClasseProvider CriarClasse(this ILModulo iLGerador, string nome, string @namespace, Type tipoBase, Type[] interfaces, Token[] token)
        {
            ILClasseProvider cls = iLGerador.CriarClasseProvider(nome, @namespace, tipoBase, interfaces, token);
            return cls;
        }

        public static dynamic ObterInstancia(this ILClasseProvider iLClasse, params object[] args)
        {
            return Activator.CreateInstance(iLClasse.Atual.TipoGerado, args);
        }
        public static TInterface ObterInstancia<TInterface>(this ILClasseProvider iLClasse, params object[] args)
        {
            if (iLClasse.Interfaces.Contains(typeof(TInterface)))
            {
                return (TInterface)Activator.CreateInstance(iLClasse.Atual.TipoGerado, args);
            }

            throw new InvalidCastException();
        }





    }
}