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
using Propeus.Modulo.IL.API;


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

                var modulo = iLGerador.CriarModulo();
                Calculadora(modulo);

            }
        }

        private static void Calculadora(ILModulo modulo)
        {
            //Criar Calculadora
            var classe = modulo.CriarClasse("teste", "teste", null, null, new Token[] { Token.Publico });

            //Criar metodo de soma
            var mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(int), "Adicao", new ILParametro[] {
                new ILParametro("Adicao",typeof(int),"p1"),
                new ILParametro("Adicao",typeof(int),"p2")
            });

            mth.Soma(mth.Parametros[0], mth.Parametros[1]);
            mth.CriarRetorno();

            //Criar metodo de subtracao
            mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(int), "Subtracao", new ILParametro[] {
                new ILParametro("Subtracao",typeof(int),"p1"),
                new ILParametro("Subtracao",typeof(int),"p2")
            });

            mth.Subitrair(mth.Parametros[0], mth.Parametros[1]);
            mth.CriarRetorno();
            //Criar metodo de divisao
            mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(int), "Divisao", new ILParametro[] {
                new ILParametro("Divisao",typeof(int),"p1"),
                new ILParametro("Divisao",typeof(int),"p2")
            });

            mth.Dividir(mth.Parametros[0], mth.Parametros[1]);
            mth.CriarRetorno();
            //Criar metodo de multipilcacao
            mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(int), "Multiplicacao", new ILParametro[] {
                new ILParametro("Multiplicacao",typeof(int),"p1"),
                new ILParametro("Multiplicacao",typeof(int),"p2")
            });

            mth.Multiplicar(mth.Parametros[0], mth.Parametros[1]);
            mth.CriarRetorno();

            //Criar metodo de maior valor
            mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(int), "SeMaiorValor", new ILParametro[] {
                new ILParametro("SeMaiorValor",typeof(int),"p1"),
                new ILParametro("SeMaiorValor",typeof(int),"p2")
            });
            mth.Se().MaiorQue(mth.Parametros[0], mth.Parametros[1]);
            mth.CarregarParametro(mth.Parametros[0]);
            mth.CriarRetorno();
            mth.SeFim();
            mth.CarregarParametro(mth.Parametros[1]);
            mth.CriarRetorno();

            //Criar metodo de menor valor
            mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(int), "SeMenorValor", new ILParametro[] {
                new ILParametro("SeMenorValor",typeof(int),"p1"),
                new ILParametro("SeMenorValor",typeof(int),"p2")
            });
            mth.Se().MenorQue(mth.Parametros[0], mth.Parametros[1]);
            mth.CarregarParametro(mth.Parametros[0]);
            mth.CriarRetorno();
            mth.SeFim();
            mth.CarregarParametro(mth.Parametros[1]);
            mth.CriarRetorno();

            //Criar metodo de maior valor ou igual
            mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(int), "SeMaiorValorOuIgual", new ILParametro[] {
                new ILParametro("SeMaiorValorOuIgual",typeof(int),"p1"),
                new ILParametro("SeMaiorValorOuIgual",typeof(int),"p2")
            });
            mth.Se().MaiorOuIgualQue(mth.Parametros[0], mth.Parametros[1]);
            mth.CarregarParametro(mth.Parametros[0]);
            mth.CriarRetorno();
            mth.SeFim();
            mth.CarregarParametro(mth.Parametros[1]);
            mth.CriarRetorno();

            //Criar metodo de menor valor ou igual
            mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(int), "SeMenorValorOuIgual", new ILParametro[] {
                new ILParametro("SeMenorValorOuIgual",typeof(int),"p1"),
                new ILParametro("SeMenorValorOuIgual",typeof(int),"p2")
            });
            mth.Se().MenorOuIgualQue(mth.Parametros[0], mth.Parametros[1]);
            mth.CarregarParametro(mth.Parametros[0]);
            mth.CriarRetorno();
            mth.SeFim();
            mth.CarregarParametro(mth.Parametros[1]);
            mth.CriarRetorno();

            //Criar metodo de maior valor
            mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(int), "SeIgualValor", new ILParametro[] {
                new ILParametro("SeIgualValor",typeof(int),"p1"),
                new ILParametro("SeIgualValor",typeof(int),"p2")
            });
            mth.Se().Diferente(mth.Parametros[0], mth.Parametros[1]);
            mth.CarregarParametro(mth.Parametros[0]);
            mth.CriarRetorno();
            mth.SeFim();
            mth.CarregarParametro(mth.Parametros[1]);
            mth.CriarRetorno();

            //Criar metodo de menor valor
            mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(int), "SeDiferenteValor", new ILParametro[] {
                new ILParametro("SeDiferenteValor",typeof(int),"p1"),
                new ILParametro("SeDiferenteValor",typeof(int),"p2")
            });
            mth.Se().Diferente(mth.Parametros[0], mth.Parametros[1]);
            mth.CarregarParametro(mth.Parametros[0]);
            mth.CriarRetorno();
            mth.SeFim();
            mth.CarregarParametro(mth.Parametros[1]);
            mth.CriarRetorno();


            //Criar metodo de igual valor
            mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(bool), "Igual", new ILParametro[] {
                new ILParametro("Igual",typeof(int),"p1"),
                new ILParametro("Igual",typeof(int),"p2")
            });
            mth.Igual(mth.Parametros[0], mth.Parametros[1]);
            mth.CriarRetorno();
            //Criar metodo de valor diferente
            mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(bool), "Diferente", new ILParametro[] {
                new ILParametro("Diferente",typeof(int),"p1"),
                new ILParametro("Diferente",typeof(int),"p2")
            });
            mth.Diferente(mth.Parametros[0], mth.Parametros[1]);
            mth.CriarRetorno();
            //Criar metodo de valor igual ou maior
            mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(bool), "MaiorOuIgualQue", new ILParametro[] {
                new ILParametro("Diferente",typeof(int),"p1"),
                new ILParametro("Diferente",typeof(int),"p2")
            });
            mth.MaiorOuIgualQue(mth.Parametros[0], mth.Parametros[1]);
            mth.CriarRetorno();
            //Criar metodo de valor igual ou menor
            mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(bool), "MenorOuIgualQue", new ILParametro[] {
                new ILParametro("Diferente",typeof(int),"p1"),
                new ILParametro("Diferente",typeof(int),"p2")
            });
            mth.MenorOuIgualQue(mth.Parametros[0], mth.Parametros[1]);
            mth.CriarRetorno();
            //Criar metodo de valor maior
            mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(bool), "MaiorQue", new ILParametro[] {
                new ILParametro("Diferente",typeof(int),"p1"),
                new ILParametro("Diferente",typeof(int),"p2")
            });
            mth.MaiorQue(mth.Parametros[0], mth.Parametros[1]);
            mth.CriarRetorno();
            //Criar metodo de valor menor
            mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(bool), "MenorQue", new ILParametro[] {
                new ILParametro("Diferente",typeof(int),"p1"),
                new ILParametro("Diferente",typeof(int),"p2")
            });
            mth.MenorQue(mth.Parametros[0], mth.Parametros[1]);
            mth.CriarRetorno();
            //Criar metodo de entre valores (teste de multiplos ifs)
            mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(bool), "EntreValores", new ILParametro[] {
                new ILParametro("EntreValores",typeof(int),"menor"),
                new ILParametro("EntreValores",typeof(int),"atual"),
                new ILParametro("EntreValores",typeof(int),"maior")
            });
            mth.Se().MenorOuIgualQue(mth.Parametros[0], mth.Parametros[1]);
            mth.Se().MaiorOuIgualQue(mth.Parametros[2], mth.Parametros[1]);
            mth.CarregarTrue();
            mth.CriarRetorno();
            mth.SeFim();
            mth.SeFim();
            mth.CarregarFalse();
            mth.CriarRetorno();

            classe.Executar();
            var calc = classe.ObterInstancia();
            classe.ToString();

            Console.WriteLine("Aritimetico");
            Console.WriteLine(calc.Adicao(1, 1));
            Console.WriteLine(calc.Subtracao(2, 2));
            Console.WriteLine(calc.Divisao(3, 3));
            Console.WriteLine(calc.Multiplicacao(4, 4));
            
            Console.WriteLine("Condicional");

            Console.WriteLine(calc.SeMaiorValor(4, 8));
            Console.WriteLine(calc.SeMenorValor(4, 8));
            Console.WriteLine(calc.SeMenorValorOuIgual(4, 8));
            Console.WriteLine(calc.SeMaiorValorOuIgual(4, 8));
            Console.WriteLine(calc.SeIgualValor(4, 8));
            Console.WriteLine(calc.SeDiferenteValor(4, 8));
            Console.WriteLine(calc.EntreValores(0, 8,10));

            Console.WriteLine("Logico");

            Console.WriteLine(calc.Igual(8, 8));
            Console.WriteLine(calc.Diferente(7, 8));
            Console.WriteLine(calc.MaiorOuIgualQue(8, 8));
            Console.WriteLine(calc.MenorOuIgualQue(8, 8));
            Console.WriteLine(calc.MaiorQue(8, 7));
            Console.WriteLine(calc.MenorQue(7, 8));
            //Console.WriteLine(calc.teste());
        }


    }
}