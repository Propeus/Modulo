using Propeus.Modulo.IL.Enums;
using Propeus.Modulo.IL.Geradores;
using Propeus.Modulo.IL.Helpers;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Propeus.Modulo.IL.Playground
{

    public interface Iteste
    {
        void M();
        void M2(int a);
        void Whoami();

        int teste { get; }
        int teste2 { get; set; }
    }

    public class Teste : Iteste
    {

        public Teste(string teste)
        {
            Console.WriteLine(teste);
        }

        public Teste()
        {
            Console.WriteLine("constutor sem parametro");
        }

        public void M()
        {
            Console.WriteLine("ok!");
        }

        public void M2(int a)
        {
            Console.WriteLine(a);
        }

        public void Whoami()
        {
            Console.WriteLine(this.GetType().ToString());
        }

        public int teste { get; }
        public int teste2 { get; set; }
    }

    public class Teste2 : Iteste
    {

        public Teste2(string teste)
        {
            Console.WriteLine(teste);
        }

        public Teste2()
        {
            Console.WriteLine("constutor sem parametro");
        }

        public void M()
        {
            Console.WriteLine("ok!");
        }

        public void M2(int a)
        {
            Console.WriteLine(a);
        }

        public void Whoami()
        {
            Console.WriteLine(this.GetType().ToString());
        }

        public int teste { get; }
        public int teste2 { get; set; }
    }

    internal class Program
    {
        private const string NamespaceModulo = "Propeus.IL.Teste.TPL.";

        static List<ILGerador> geradores = new List<ILGerador>();


        private static void Main(string[] args)
        {
            using (ILGerador iLGerador = new ILGerador())
            {
                var tp = iLGerador.CriarModulo().CriarProxyClasse<Teste>();
                tp.Executar();
                var t = (Iteste)tp.ObterInstancia();
                t.M();
                t.M2(10);
                t.Whoami();
                t.teste2 = 10;

            }
            


            using (ILGerador iLGerador = new ILGerador())
            {

                ILClasseProvider iLClasse = iLGerador.CriarModulo().CriarClasse();



                iLClasse.CriarMetodo().CriarRetorno();
              

                iLClasse.CriarMetodo("teste", new Token[] { Enums.Token.Publico }).CriarRetorno();
                
                iLClasse.CriarMetodo("teste1", new Token[] { Enums.Token.Protegido }).CriarRetorno();
                iLClasse.CriarMetodo("teste2", new Token[] { Enums.Token.Privado }).CriarRetorno();
                iLClasse.CriarMetodo("teste3", new Token[] { Enums.Token.Interno }).CriarRetorno();



                iLClasse.CriarMetodo("teste4", new Token[] { Enums.Token.Publico }, typeof(int)).CriarRetorno(0);
                iLClasse.CriarMetodo("teste5", new Token[] { Enums.Token.Publico }, typeof(long)).CriarRetorno(300000000000000000);

                iLClasse.CriarMetodo("teste6", new Token[] { Enums.Token.Publico }, typeof(float)).CriarRetorno(30000.24f);
                iLClasse.CriarMetodo("teste7", new Token[] { Enums.Token.Publico }, typeof(double)).CriarRetorno(30000.24);

                iLClasse.CriarMetodo("teste8", new Token[] { Enums.Token.Publico }, typeof(short)).CriarRetorno(30000);
                iLClasse.CriarMetodo("teste9", new Token[] { Enums.Token.Publico }, typeof(decimal)).CriarRetorno(.79228162514264337593543950335M);
                iLClasse.CriarMetodo("teste10", new Token[] { Enums.Token.Publico }, typeof(decimal)).CriarRetorno(-.79228162514264337593543950335M);

              

                //iLClasse.CriarCampo(new Token[] { Enums.Token.Publico });
                iLClasse.CriarCampo(new Token[] { Enums.Token.Publico }, typeof(int));
                iLClasse.CriarCampo(new Token[] { Enums.Token.Publico }, typeof(int), "_fTeste1");
                iLClasse.CriarCampo(new Token[] { Enums.Token.Publico }, typeof(double), "_fTeste2");
                iLClasse.CriarCampo(new Token[] { Enums.Token.Publico }, typeof(float), "_fTeste3");
                iLClasse.CriarCampo(new Token[] { Enums.Token.Publico }, typeof(long), "_fTeste4");
                iLClasse.CriarCampo(new Token[] { Enums.Token.Publico }, typeof(decimal), "_fTeste5");
                iLClasse.CriarCampo(new Token[] { Enums.Token.Publico }, typeof(string), "_fTeste6");
                var cmp = iLClasse.CriarCampo(new Token[] { Enums.Token.Publico }, typeof(int), "_fTeste7");



                //Cria ctor
                iLClasse.CriarConstrutor(new Token[] { Token.Publico })
                    .CarregarArgumento()
                    .ChamarFuncao(typeof(object).GetConstructors()[0])
                    .CriarRetorno();




                //Cria mth com retorno
                var mth = iLClasse.CriarMetodo("teste11", new Token[] { Token.Publico }, typeof(int[]));
                var arg = mth.CriarVariavel(typeof(int), "arr");
                mth.CriarArray(arg, 10);
                mth.CriarRetorno();



                //Retorna o type
                mth = iLClasse.CriarMetodo("teste12", new Token[] { Token.Publico }, typeof(Type));
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
                mth = iLClasse.CriarMetodo("teste13", new Token[] { Token.Publico }, typeof(MethodInfo[]));
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
                mth = iLClasse.CriarMetodo("teste14", new Token[] { Token.Publico }, typeof(int), new Type[] { typeof(int) })
                    .CarregarArgumento(1)
                    .CriarRetorno();



                //Cria propriedade
                var prop = iLClasse.CriarPropriedade(Token.Publico, typeof(int), "PropTeste_int");

                iLClasse.CriarPropriedade(Token.Publico, typeof(string), "PropTeste_string");
                iLClasse.CriarPropriedade(Token.Publico, typeof(float), "PropTeste_float");
                iLClasse.CriarPropriedade(Token.Publico, typeof(double), "PropTeste_double");
                iLClasse.CriarPropriedade(Token.Publico, typeof(decimal), "PropTeste_decimal");



                iLClasse.Executar();
                Exibir(iLClasse);
                dynamic cls = iLClasse.ObterInstancia();

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

                Console.WriteLine(cls.teste11());
                Console.WriteLine(cls.teste12());
                Console.WriteLine(cls.teste13());
            }
        }

        private static void Exibir(ILClasseProvider iLClasse)
        {
            Console.Clear();
            Console.WriteLine(iLClasse.ToString());
        }
    }
}