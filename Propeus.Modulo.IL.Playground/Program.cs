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
            var mth = classe.CriarMetodo(new Token[] { Token.Publico }, typeof(int), "soma", new ILParametro[] { 
                new ILParametro("some",typeof(int),"p1"),
                new ILParametro("some",typeof(int),"p2")
            });

            mth.Soma(mth.Parametros[0], mth.Parametros[1]);
            mth.CriarRetorno();

            //Criar metodo de subtracao
            //Criar metodo de divisao
            //Criar metodo de multipilcacao
            classe.Executar();
            var calc = classe.ObterInstancia();
            classe.ToString();
            Console.WriteLine(calc.soma(1, 1));
        }


    }
}