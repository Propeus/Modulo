using Propeus.Modulo.IL.Enums;
using Propeus.Modulo.IL.Geradores;
using Propeus.Modulo.IL.Helpers;
using System;
using System.Reflection;

namespace Propeus.Modulo.IL.Playground
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ILGerador iLGerador = new ILGerador();
            ILClasse iLClasse = iLGerador.CriarClasse();
            iLClasse.CriarMetodo().CriarRetorno();
            iLClasse.CriarMetodo("teste", TokenEnum.Publico).CriarRetorno();
            iLClasse.CriarMetodo("teste1", Enums.TokenEnum.Protegido).CriarRetorno();
            iLClasse.CriarMetodo("teste2", Enums.TokenEnum.Privado).CriarRetorno();
            iLClasse.CriarMetodo("teste3", Enums.TokenEnum.Interno).CriarRetorno();

            iLClasse.CriarMetodo("teste4", Enums.TokenEnum.Publico, typeof(int)).CriarRetorno(0);
            iLClasse.CriarMetodo("teste5", Enums.TokenEnum.Publico, typeof(long)).CriarRetorno(300000000000000000);

            iLClasse.CriarMetodo("teste6", Enums.TokenEnum.Publico, typeof(float)).CriarRetorno(30000.24f);
            iLClasse.CriarMetodo("teste7", Enums.TokenEnum.Publico, typeof(double)).CriarRetorno(30000.24);

            iLClasse.CriarMetodo("teste8", Enums.TokenEnum.Publico, typeof(short)).CriarRetorno(30000);
            iLClasse.CriarMetodo("teste9", Enums.TokenEnum.Publico, typeof(decimal)).CriarRetorno(.79228162514264337593543950335M);
            iLClasse.CriarMetodo("teste10", Enums.TokenEnum.Publico, typeof(decimal)).CriarRetorno(-.79228162514264337593543950335M);

            iLClasse.CriarCampo(Enums.TokenEnum.Publico);
            iLClasse.CriarCampo(Enums.TokenEnum.Publico, typeof(int));
            iLClasse.CriarCampo(Enums.TokenEnum.Publico, typeof(int), "_fTeste1");
            iLClasse.CriarCampo(Enums.TokenEnum.Publico, typeof(double), "_fTeste2");
            iLClasse.CriarCampo(Enums.TokenEnum.Publico, typeof(float), "_fTeste3");
            iLClasse.CriarCampo(Enums.TokenEnum.Publico, typeof(long), "_fTeste4");
            iLClasse.CriarCampo(Enums.TokenEnum.Publico, typeof(decimal), "_fTeste5");
            iLClasse.CriarCampo(Enums.TokenEnum.Publico, typeof(string), "_fTeste6");
            var cmp = iLClasse.CriarCampo(Enums.TokenEnum.Publico, typeof(int), "_fTeste7");

            //Cria ctor
            iLClasse.CriarConstrutor()
                .CarregarArgumento()
                .ChamarFuncao(typeof(object).GetConstructors()[0])
                .CriarRetorno();

            //Cria mth com retorno
            var mth = iLClasse.CriarMetodo("teste11", TokenEnum.Publico, typeof(int[]));
            var arg = mth.CriarVariavel(typeof(int), "arr");
            mth.CriarArray(arg, 10);
            mth.CriarRetorno();

            //Retorna o type
            mth = iLClasse.CriarMetodo("teste12", TokenEnum.Publico, typeof(Type));
            mth.CarregarArgumento()
                .AtribuirValor(cmp, 1)
                .ArmazenarCampo(cmp);

            arg = mth.CriarVariavel(typeof(int), "arr");
            mth.CriarArray(arg, 10);
            mth.ArmazenarVariavel(arg)
                .CarregarArgumento()
                .CarregarCampo(cmp)
                .Converter(typeof(int))
                .ChamarFuncaoVirtual(typeof(Type).GetMethod("GetType", Array.Empty<Type>()));
            mth.CriarRetorno();

            //Invoca metodo de tipo
            mth = iLClasse.CriarMetodo("teste13", TokenEnum.Publico, typeof(MethodInfo[]));
            mth.CarregarArgumento()
                .AtribuirValor(cmp, 1)
                .ArmazenarCampo(cmp);

            arg = mth.CriarVariavel(typeof(int), "arr");
            mth.CriarArray(arg, 10);
            mth.ArmazenarVariavel(arg)
                .CarregarArgumento()
                .CarregarCampo(cmp)
                .Converter(typeof(int))
                .ChamarFuncaoVirtual(typeof(Type).GetMethod("GetType", Array.Empty<Type>()))
                .ChamarFuncaoVirtual(typeof(Type).GetMethod("GetMethods", Array.Empty<Type>()));
            mth.CriarRetorno();

            //Cria metodo com parametros
            mth = iLClasse.CriarMetodo("teste14", TokenEnum.Publico, typeof(int), new Type[] { typeof(int) })
                .CarregarArgumento(1)
                .CriarRetorno();

            //Cria propriedade
            var prop = iLClasse.CriarPropriedade(typeof(int), "PropTeste_int")
                .DefinirGetter(iLClasse)
                .DefinirSetter(iLClasse);
            prop = iLClasse.CriarPropriedade(typeof(string), "PropTeste_string")
           .DefinirGetter(iLClasse)
           .DefinirSetter(iLClasse);
            prop = iLClasse.CriarPropriedade(typeof(float), "PropTeste_float")
           .DefinirGetter(iLClasse)
           .DefinirSetter(iLClasse);
            prop = iLClasse.CriarPropriedade(typeof(double), "PropTeste_double")
           .DefinirGetter(iLClasse)
           .DefinirSetter(iLClasse);
            prop = iLClasse.CriarPropriedade(typeof(decimal), "PropTeste_decimal")
           .DefinirGetter(iLClasse)
           .DefinirSetter(iLClasse);

            iLClasse.Executar();
            Exibir(iLClasse);
            dynamic cls = ILGerador.ObterInstancia(iLClasse);
            cls.PropTeste_int = int.MaxValue;
            cls.PropTeste_string = "teswte";
            cls.PropTeste_float = float.MaxValue;
            cls.PropTeste_double = double.MaxValue;
            cls.PropTeste_decimal = decimal.MaxValue;

            Console.WriteLine(cls.PropTeste_int);
            Console.WriteLine(cls.PropTeste_string);
            Console.WriteLine(cls.PropTeste_float);
            Console.WriteLine(cls.PropTeste_double);
            Console.WriteLine(cls.PropTeste_decimal);
        }

        private static void Exibir(ILClasse iLClasse)
        {
            Console.Clear();
            Console.WriteLine(iLClasse.ToString());
        }
    }
}