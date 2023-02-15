using Microsoft.VisualStudio.TestTools.UnitTesting;

using Propeus.Modulo.IL.Enums;
using Propeus.Modulo.IL.Geradores;
using Propeus.Modulo.IL.Helpers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Propeus.Modulo.IL.Helpers.Tests
{
    [TestClass()]
    public class HelperTests
    {
        private ILGerador iLGerador;
        private ILModulo modulo;
        private ILClasseProvider classe;

        [TestInitialize()]
        public void Init()
        {
            iLGerador = new ILGerador();
            modulo = iLGerador.CriarModulo();
            classe = modulo.CriarClasse("teste", "teste", null, null, new Token[] { Token.Publico });

        }

        [TestCleanup()]
        public void Finish()
        {
            iLGerador.Dispose();
        }

        //[TestMethod()]
        //public void ImplementarMetodoInterfaceTest()
        //{
        //    Assert.Fail();
        //}

        [TestMethod()]
        public void SomaTest()
        {
            var mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(int), "Adicao", new ILParametro[] {
                new ILParametro("Adicao",typeof(int),"p1"),
                new ILParametro("Adicao",typeof(int),"p2")
            });

            mth.Soma(mth.Parametros[0], mth.Parametros[1]);
            mth.CriarRetorno();

            classe.Executar();
            var calc = classe.ObterInstancia();
            Assert.IsNotNull(calc);
            Assert.AreEqual(calc.Adicao(1, 1), 2);
        }

        [TestMethod()]
        public void SubitrairTest()
        {
            var mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(int), "Subtracao", new ILParametro[] {
                new ILParametro("Subtracao",typeof(int),"p1"),
                new ILParametro("Subtracao",typeof(int),"p2")
            });

            mth.Subitrair(mth.Parametros[0], mth.Parametros[1]);
            mth.CriarRetorno();

            classe.Executar();
            var calc = classe.ObterInstancia();
            Assert.IsNotNull(calc);
            Assert.AreEqual(calc.Subtracao(1, 1), 0);
        }



        [TestMethod()]
        public void DividirTest()
        {
            var mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(int), "Divisao", new ILParametro[] {
                new ILParametro("Divisao",typeof(int),"p1"),
                new ILParametro("Divisao",typeof(int),"p2")
            });

            mth.Dividir(mth.Parametros[0], mth.Parametros[1]);
            mth.CriarRetorno();

            classe.Executar();
            var calc = classe.ObterInstancia();
            Assert.IsNotNull(calc);
            Assert.AreEqual(calc.Divisao(2, 1), 2);
        }

        [TestMethod()]
        public void MultiplicarTest()
        {
            var mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(int), "Multiplicacao", new ILParametro[] {
                new ILParametro("Multiplicacao",typeof(int),"p1"),
                new ILParametro("Multiplicacao",typeof(int),"p2")
            });

            mth.Multiplicar(mth.Parametros[0], mth.Parametros[1]);
            mth.CriarRetorno();

            classe.Executar();
            var calc = classe.ObterInstancia();
            Assert.IsNotNull(calc);
            Assert.AreEqual(calc.Multiplicacao(2, 1), 2);
        }

        [TestMethod()]
        public void IgualTest()
        {
            var mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(int), "SeIgualValor", new ILParametro[] {
                new ILParametro("SeIgualValor",typeof(int),"p1"),
                new ILParametro("SeIgualValor",typeof(int),"p2")
            });
            mth.Se().Igual(mth.Parametros[0], mth.Parametros[1]);
            mth.CarregarParametro(mth.Parametros[0]);
            mth.CriarRetorno();
            mth.SeFim();
            mth.CarregarParametro(mth.Parametros[1]);
            mth.CriarRetorno();

            mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(bool), "Igual", new ILParametro[] {
                new ILParametro("Igual",typeof(int),"p1"),
                new ILParametro("Igual",typeof(int),"p2")
            });
            mth.Igual(mth.Parametros[0], mth.Parametros[1]);
            mth.CriarRetorno();

            classe.Executar();
            var calc = classe.ObterInstancia();
            Assert.IsNotNull(calc);
            Assert.AreEqual(true, calc.Igual(2, 2));
            Assert.AreEqual(2, calc.SeIgualValor(2, 2));
        }

        [TestMethod()]
        public void DiferenteTest()
        {
            var mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(int), "SeDiferenteValor", new ILParametro[] {
                new ILParametro("SeDiferenteValor",typeof(int),"p1"),
                new ILParametro("SeDiferenteValor",typeof(int),"p2")
            });
            mth.Se().Diferente(mth.Parametros[0], mth.Parametros[1]);
            mth.CarregarParametro(mth.Parametros[0]);
            mth.CriarRetorno();
            mth.SeFim();
            mth.CarregarParametro(mth.Parametros[1]);
            mth.CriarRetorno();

            mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(bool), "Diferente", new ILParametro[] {
                new ILParametro("Diferente",typeof(int),"p1"),
                new ILParametro("Diferente",typeof(int),"p2")
            });
            mth.Diferente(mth.Parametros[0], mth.Parametros[1]);
            mth.CriarRetorno();

            classe.Executar();
            var calc = classe.ObterInstancia();
            Assert.IsNotNull(calc);
            Assert.AreEqual(true, calc.Diferente(2, 3));
            Assert.AreEqual(2, calc.SeDiferenteValor(2, 3));
        }

        [TestMethod()]
        public void MaiorQueTest()
        {
            var mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(int), "SeMaiorValor", new ILParametro[] {
                new ILParametro("SeMaiorValor",typeof(int),"p1"),
                new ILParametro("SeMaiorValor",typeof(int),"p2")
            });
            mth.Se().MaiorQue(mth.Parametros[0], mth.Parametros[1]);
            mth.CarregarParametro(mth.Parametros[0]);
            mth.CriarRetorno();
            mth.SeFim();
            mth.CarregarParametro(mth.Parametros[1]);
            mth.CriarRetorno();

            mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(bool), "MaiorQue", new ILParametro[] {
                new ILParametro("MaiorQue",typeof(int),"p1"),
                new ILParametro("MaiorQue",typeof(int),"p2")
            });
            mth.MaiorQue(mth.Parametros[0], mth.Parametros[1]);
            mth.CriarRetorno();

            classe.Executar();
            var calc = classe.ObterInstancia();
            Assert.IsNotNull(calc);
            Assert.AreEqual(true, calc.MaiorQue(3,2));
            Assert.AreEqual(3, calc.SeMaiorValor(2, 3));
        }

        [TestMethod()]
        public void MenorQueTest()
        {
            var mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(int), "SeMenorValor", new ILParametro[] {
                new ILParametro("SeMenorValor",typeof(int),"p1"),
                new ILParametro("SeMenorValor",typeof(int),"p2")
            });
            mth.Se().MenorQue(mth.Parametros[0], mth.Parametros[1]);
            mth.CarregarParametro(mth.Parametros[0]);
            mth.CriarRetorno();
            mth.SeFim();
            mth.CarregarParametro(mth.Parametros[1]);
            mth.CriarRetorno();

            mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(bool), "MenorQue", new ILParametro[] {
                new ILParametro("MenorQue",typeof(int),"p1"),
                new ILParametro("MenorQue",typeof(int),"p2")
            });
            mth.MenorQue(mth.Parametros[0], mth.Parametros[1]);
            mth.CriarRetorno();

            classe.Executar();
            var calc = classe.ObterInstancia();
            Assert.IsNotNull(calc);
            Assert.AreEqual(true, calc.MenorQue(2,3));
            Assert.AreEqual(2, calc.SeMenorValor(2, 3));
        }

        [TestMethod()]
        public void MaiorOuIgualQueTest()
        {
            var mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(int), "SeMaiorValorOuIgual", new ILParametro[] {
                new ILParametro("SeMaiorValorOuIgual",typeof(int),"p1"),
                new ILParametro("SeMaiorValorOuIgual",typeof(int),"p2")
            });
            mth.Se().MaiorOuIgualQue(mth.Parametros[0], mth.Parametros[1]);
            mth.CarregarParametro(mth.Parametros[0]);
            mth.CriarRetorno();
            mth.SeFim();
            mth.CarregarParametro(mth.Parametros[1]);
            mth.CriarRetorno();

            mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(bool), "MaiorOuIgualQue", new ILParametro[] {
                new ILParametro("MaiorOuIgualQue",typeof(int),"p1"),
                new ILParametro("MaiorOuIgualQue",typeof(int),"p2")
            });
            mth.MaiorOuIgualQue(mth.Parametros[0], mth.Parametros[1]);
            mth.CriarRetorno();

            classe.Executar();
            var calc = classe.ObterInstancia();
            Assert.IsNotNull(calc);
            Assert.AreEqual(true, calc.MaiorOuIgualQue(4, 3));
            Assert.AreEqual(4, calc.SeMaiorValorOuIgual(4, 3));
        }

        [TestMethod()]
        public void MenorOuIgualQueTest()
        {
            var mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(int), "SeMenorValorOuIgual", new ILParametro[] {
                new ILParametro("SeMenorValorOuIgual",typeof(int),"p1"),
                new ILParametro("SeMenorValorOuIgual",typeof(int),"p2")
            });
            mth.Se().MenorOuIgualQue(mth.Parametros[0], mth.Parametros[1]);
            mth.CarregarParametro(mth.Parametros[0]);
            mth.CriarRetorno();
            mth.SeFim();
            mth.CarregarParametro(mth.Parametros[1]);
            mth.CriarRetorno();

            mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(bool), "MenorOuIgualQue", new ILParametro[] {
                new ILParametro("MenorOuIgualQue",typeof(int),"p1"),
                new ILParametro("MenorOuIgualQue",typeof(int),"p2")
            });
            mth.MenorOuIgualQue(mth.Parametros[0], mth.Parametros[1]);
            mth.CriarRetorno();

            classe.Executar();
            var calc = classe.ObterInstancia();
            Assert.IsNotNull(calc);
            Assert.AreEqual(true, calc.MenorOuIgualQue(2, 3));
            Assert.AreEqual(2, calc.SeMenorValorOuIgual(2, 3));
        }







    }
}