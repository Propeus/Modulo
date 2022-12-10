using Propeus.Modulo.Abstrato.Util;
using Propeus.Modulo.IL.Enums;
using Propeus.Modulo.IL.Geradores;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Propeus.Modulo.IL.Helpers
{
    public static partial class Helper
    {
        /// <summary>
        /// Gera uma classe basica
        /// </summary>
        /// <param name="iLGerador">Gerador de IL atual</param>
        /// <param name="nome">Nome da classe</param>
        /// <param name="namespace">Namespace da classe</param>
        /// <param name="base">Objeto a ser extendido para classse</param>
        /// <param name="interfaces">Interface a ser implementado na classe</param>
        /// <param name="acessadores">Acessadores da classe</param>
        /// <returns></returns>
        public static ILClasseProvider CriarClasse(this ILModulo iLGerador, string nome = Constantes.CONST_NME_CLASSE, string @namespace = Constantes.CONST_NME_NAMESPACE, Type @base = null, Type[] interfaces = null, Token[] acessadores = null)
        {
            if (iLGerador is null)
            {
                throw new ArgumentNullException(nameof(iLGerador));
            }


            var clsProvider = iLGerador.CriarProvider(nome, @namespace, @base, interfaces, acessadores);

            return clsProvider;
        }

        public static ILClasseProvider CriarProxyClasse<TClasse>(this ILModulo iLGerador)
       where TClasse : class
        {
            if (iLGerador is null)
            {
                throw new ArgumentNullException(nameof(iLGerador));
            }

            var tClasse = typeof(TClasse);

            var cls = iLGerador.CriarClasse(tClasse.Name, tClasse.Namespace + ".IL.Proxy", interfaces: tClasse.GetInterfaces());


            ILCampo cmp = cls.CriarCampo(new Token[] { Enums.Token.Privado }, tClasse, "IL_Gerador_Proxy_" + tClasse.Name);

            foreach (var c in tClasse.GetConstructors())
            {

                ILMetodo mth = cls.CriarConstrutor(c.Attributes.DividirEnum().ParseEnum<MethodAttributes, Token>(), c.ObterTipoParametros().ToArray())
                       .CarregarArgumento()
                       .ChamarFuncao(typeof(object).GetConstructors()[0])
                       .CarregarArgumentoAgregado(indiceFinal: c.GetParameters().Length)
                       .CriarInstancia(tClasse, c.ObterTipoParametros().ToArray())
                       .ArmazenarCampo(cmp)
                       .CriarRetorno();
            }

            foreach (var metodo in tClasse.GetMethods())
            {
                if(metodo.Name.Contains("get_") || metodo.Name.Contains("set_"))
                {
                    continue;
                }
                

                ILMetodo mth = default;
                if (metodo.GetParameters().Length == 0)
                {
                    MethodAttributes a = metodo.Attributes;
                    mth = cls.CriarMetodo(nome: metodo.Name, acessadores: metodo.Attributes.DividirEnum().ParseEnum<MethodAttributes, Token>(), retorno: metodo.ReturnType);
                }
                else
                {
                    mth = cls.CriarMetodo(nome: metodo.Name, acessadores: metodo.Attributes.DividirEnum().ParseEnum<MethodAttributes, Token>(), retorno: metodo.ReturnType, parametros: metodo.ObterTipoParametros().ToArray());
                }

                mth.CarregarArgumento()
                   .CarregarCampo(cmp)
                   .CarregarArgumentoAgregado(1, metodo.GetParameters().Length)
                   .ChamarFuncaoVirtual(metodo)
                   .CriarRetorno();
            }

            foreach (var propriedade in tClasse.GetProperties())
            {
                var mth_get = propriedade.GetGetMethod();
                var mth_set = propriedade.GetSetMethod();

                var prop = cls.CriarPropriedade(propriedade.PropertyType, propriedade.Name, mth_get.ObterTipoParametros().ToArray(), mth_get.Attributes.DividirEnum().ParseEnum<MethodAttributes, Token>(), mth_set?.Attributes.DividirEnum().ParseEnum<MethodAttributes, Token>());
                prop.IsProxy = true;
                prop.Getter
                    .CarregarArgumento()
                   .CarregarCampo(cmp)
                   .CarregarArgumentoAgregado(1, mth_get.GetParameters().Length)
                   .ChamarFuncaoVirtual(mth_get)
                   .CriarRetorno();

                if (prop.Setter != null)
                {
                    prop.Setter
                        .CarregarArgumento()
                   .CarregarCampo(cmp)
                   .CarregarArgumentoAgregado(1, mth_set.GetParameters().Length)
                   .ChamarFuncaoVirtual(mth_set)
                   .CriarRetorno();
                }

            }


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