using Propeus.Modulo.IL.Enums;
using Propeus.Modulo.IL.Geradores;
using Propeus.Modulo.IL.Helpers;

using System;
using System.Threading.Tasks;

namespace Propeus.Modulo.IL.Playground
{



    internal class Program
    {

        private static void Main(string[] args)
        {



            //using (ILGerador iLGerador = new ILGerador())
            //{
            //    var tp = iLGerador.CriarModulo().CriarProxyClasse<Teste>();
            //    tp.Executar();
            //    var t = (Iteste)tp.ObterInstancia();
            //    t.M();
            //    t.M2(10);
            //    t.Whoami();
            //    t.teste2 = 10;

            //}

            using (ILGerador iLGerador = new ILGerador())
            {

                var modulo = iLGerador.CriarModulo("CalculadoraModulo");
                Calculadora(modulo);
                Matematica(modulo);

            }

        }



        public static Task testeTask()
        {
            Console.WriteLine("ok");
            return Task.CompletedTask;
        }

        private static void Matematica(ILModulo modulo)
        {
            var classe = modulo.CriarClasse("MatematicaBase", "Propeus.IL.Exemplo", null, null, new Token[] { Token.Publico });

            ////Criar metodo de entre valores (teste de multiplos ifs)
            //var mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(bool), "EntreValores", new ILParametro[] {
            //    new ILParametro("EntreValores",typeof(int),"menor"),
            //    new ILParametro("EntreValores",typeof(int),"atual"),
            //    new ILParametro("EntreValores",typeof(int),"maior")
            //});
            //mth.Se().MenorOuIgualQue(mth.Parametros[0], mth.Parametros[1]).Ou().MaiorOuIgualQue(mth.Parametros[2], mth.Parametros[1]);
            //mth.CarregarTrue();
            //mth.CriarRetorno();
            //mth.SeFim();
            //mth.CarregarFalse();
            //mth.CriarRetorno();

            //Criar metodo de entre valores (teste de multiplos ifs)
            var mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(bool), "EntreValores2", new ILParametro[] {
                new ILParametro("EntreValores2",typeof(int),"menor"),
                new ILParametro("EntreValores2",typeof(int),"atual"),
                new ILParametro("EntreValores2",typeof(int),"maior")
            });
            mth.Se(mth.Parametros[0], mth.MenorOuIgualQue, mth.Parametros[1]);
            mth.Ou(mth.Parametros[2], mth.MenorOuIgualQue, mth.Parametros[1]);
            mth.E(mth.Parametros[2], mth.MaiorQue, mth.Parametros[0]);
            mth.CarregarTrue();
            mth.CriarRetorno();
            mth.SeFim();
            mth.CarregarFalse();
            mth.CriarRetorno();

            classe.Executar();
            Console.WriteLine(mth.ToString());
            var calc = classe.ObterInstancia();

            Console.WriteLine("Matematica");
            Console.WriteLine(calc.EntreValores2(5, 4, 10));


        }

        private static void Calculadora(ILModulo modulo)
        {
            var classe = modulo.CriarClasse("CalculadoraBase", "Propeus.IL.Exemplo", null, null, new Token[] { Token.Publico });

            var mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(int), "Adicao", new ILParametro[] {
                new ILParametro("Adicao",typeof(int),"p1"),
                new ILParametro("Adicao",typeof(int),"p2")
            });

            mth.Soma(mth.Parametros[0], mth.Parametros[1]);
            mth.CriarRetorno();

            mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(int), "Subtracao", new ILParametro[] {
                new ILParametro("Subtracao",typeof(int),"p1"),
                new ILParametro("Subtracao",typeof(int),"p2")
            });

            mth.Subitrair(mth.Parametros[0], mth.Parametros[1]);
            mth.CriarRetorno();

            mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(int), "Divisao", new ILParametro[] {
                new ILParametro("Divisao",typeof(int),"p1"),
                new ILParametro("Divisao",typeof(int),"p2")
            });

            mth.Dividir(mth.Parametros[0], mth.Parametros[1]);
            mth.CriarRetorno();

            mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(int), "Multiplicacao", new ILParametro[] {
                new ILParametro("Multiplicacao",typeof(int),"p1"),
                new ILParametro("Multiplicacao",typeof(int),"p2")
            });

            mth.Multiplicar(mth.Parametros[0], mth.Parametros[1]);
            mth.CriarRetorno();




            classe.Executar();
            var calc = classe.ObterInstancia();

            Console.WriteLine("Calculadora");
            Console.WriteLine(calc.Adicao(1, 1));
            Console.WriteLine(calc.Subtracao(2, 2));
            Console.WriteLine(calc.Divisao(3, 3));
            Console.WriteLine(calc.Multiplicacao(4, 4));
        }


    }
}