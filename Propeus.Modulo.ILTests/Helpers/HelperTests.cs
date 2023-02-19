using Microsoft.VisualStudio.TestTools.UnitTesting;

using Propeus.Modulo.IL.Enums;
using Propeus.Modulo.IL.Geradores;
using Propeus.Modulo.IL.Helpers;

namespace Propeus.Modulo.ILTests.Helpers
{
    [TestClass()]
    public class HelperTests
    {
        private ILGerador? iLGerador;
        private ILModulo? modulo;
        private ILClasseProvider? classe;

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
            ILMetodo mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(int), "Adicao", new ILParametro[] {
                new ILParametro("Adicao",typeof(int),"p1"),
                new ILParametro("Adicao",typeof(int),"p2")
            });

            _ = mth.Soma(mth.Parametros[0], mth.Parametros[1]);
            _ = mth.CriarRetorno();

            classe.Executar();
            dynamic calc = classe.ObterInstancia();
            Assert.IsNotNull(calc);
            Assert.AreEqual(calc.Adicao(1, 1), 2);
        }

        [TestMethod()]
        public void SubitrairTest()
        {
            ILMetodo mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(int), "Subtracao", new ILParametro[] {
                new ILParametro("Subtracao",typeof(int),"p1"),
                new ILParametro("Subtracao",typeof(int),"p2")
            });

            _ = mth.Subitrair(mth.Parametros[0], mth.Parametros[1]);
            _ = mth.CriarRetorno();

            classe.Executar();
            dynamic calc = classe.ObterInstancia();
            Assert.IsNotNull(calc);
            Assert.AreEqual(calc.Subtracao(1, 1), 0);
        }



        [TestMethod()]
        public void DividirTest()
        {
            ILMetodo mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(int), "Divisao", new ILParametro[] {
                new ILParametro("Divisao",typeof(int),"p1"),
                new ILParametro("Divisao",typeof(int),"p2")
            });

            _ = mth.Dividir(mth.Parametros[0], mth.Parametros[1]);
            _ = mth.CriarRetorno();

            classe.Executar();
            dynamic calc = classe.ObterInstancia();
            Assert.IsNotNull(calc);
            Assert.AreEqual(calc.Divisao(2, 1), 2);
        }

        [TestMethod()]
        public void MultiplicarTest()
        {
            ILMetodo mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(int), "Multiplicacao", new ILParametro[] {
                new ILParametro("Multiplicacao",typeof(int),"p1"),
                new ILParametro("Multiplicacao",typeof(int),"p2")
            });

            _ = mth.Multiplicar(mth.Parametros[0], mth.Parametros[1]);
            _ = mth.CriarRetorno();

            classe.Executar();
            dynamic calc = classe.ObterInstancia();
            Assert.IsNotNull(calc);
            Assert.AreEqual(calc.Multiplicacao(2, 1), 2);
        }

        [TestMethod()]
        public void IgualTest()
        {
            ILMetodo mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(int), "SeIgualValor", new ILParametro[] {
                new ILParametro("SeIgualValor",typeof(int),"p1"),
                new ILParametro("SeIgualValor",typeof(int),"p2")
            });
            _ = mth.Se(mth.Parametros[0], mth.Igual, mth.Parametros[1]);
            _ = mth.CarregarParametro(mth.Parametros[0]);
            _ = mth.CriarRetorno();
            _ = mth.SeFim();
            _ = mth.CarregarParametro(mth.Parametros[1]);
            _ = mth.CriarRetorno();

            mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(bool), "Igual", new ILParametro[] {
                new ILParametro("Igual",typeof(int),"p1"),
                new ILParametro("Igual",typeof(int),"p2")
            });
            _ = mth.Igual(mth.Parametros[0], mth.Parametros[1]);
            _ = mth.CriarRetorno();

            classe.Executar();
            dynamic calc = classe.ObterInstancia();
            Assert.IsNotNull(calc);
            Assert.AreEqual(true, calc.Igual(2, 2));
            Assert.AreEqual(2, calc.SeIgualValor(2, 2));
        }

        [TestMethod()]
        public void DiferenteTest()
        {
            ILMetodo mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(int), "SeDiferenteValor", new ILParametro[] {
                new ILParametro("SeDiferenteValor",typeof(int),"p1"),
                new ILParametro("SeDiferenteValor",typeof(int),"p2")
            });
            _ = mth.Se(mth.Parametros[0], mth.Diferente, mth.Parametros[1]);
            _ = mth.CarregarParametro(mth.Parametros[0]);
            _ = mth.CriarRetorno();
            _ = mth.SeFim();
            _ = mth.CarregarParametro(mth.Parametros[1]);
            _ = mth.CriarRetorno();

            mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(bool), "Diferente", new ILParametro[] {
                new ILParametro("Diferente",typeof(int),"p1"),
                new ILParametro("Diferente",typeof(int),"p2")
            });
            _ = mth.Diferente(mth.Parametros[0], mth.Parametros[1]);
            _ = mth.CriarRetorno();

            classe.Executar();
            dynamic calc = classe.ObterInstancia();
            Assert.IsNotNull(calc);
            Assert.AreEqual(true, calc.Diferente(2, 3));
            Assert.AreEqual(2, calc.SeDiferenteValor(2, 3));
        }

        [TestMethod()]
        public void MaiorQueTest()
        {
            ILMetodo mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(int), "SeMaiorValor", new ILParametro[] {
                new ILParametro("SeMaiorValor",typeof(int),"p1"),
                new ILParametro("SeMaiorValor",typeof(int),"p2")
            });
            _ = mth.Se(mth.Parametros[0], mth.MaiorQue, mth.Parametros[1]);
            _ = mth.CarregarParametro(mth.Parametros[0]);
            _ = mth.CriarRetorno();
            _ = mth.SeFim();
            _ = mth.CarregarParametro(mth.Parametros[1]);
            _ = mth.CriarRetorno();

            mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(bool), "MaiorQue", new ILParametro[] {
                new ILParametro("MaiorQue",typeof(int),"p1"),
                new ILParametro("MaiorQue",typeof(int),"p2")
            });
            _ = mth.MaiorQue(mth.Parametros[0], mth.Parametros[1]);
            _ = mth.CriarRetorno();

            classe.Executar();
            dynamic calc = classe.ObterInstancia();
            Assert.IsNotNull(calc);
            Assert.AreEqual(true, calc.MaiorQue(3, 2));
            Assert.AreEqual(3, calc.SeMaiorValor(2, 3));
        }

        [TestMethod()]
        public void MenorQueTest()
        {
            ILMetodo mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(int), "SeMenorValor", new ILParametro[] {
                new ILParametro("SeMenorValor",typeof(int),"p1"),
                new ILParametro("SeMenorValor",typeof(int),"p2")
            });
            _ = mth.Se(mth.Parametros[0], mth.MenorQue, mth.Parametros[1]);
            _ = mth.CarregarParametro(mth.Parametros[0]);
            _ = mth.CriarRetorno();
            _ = mth.SeFim();
            _ = mth.CarregarParametro(mth.Parametros[1]);
            _ = mth.CriarRetorno();

            mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(bool), "MenorQue", new ILParametro[] {
                new ILParametro("MenorQue",typeof(int),"p1"),
                new ILParametro("MenorQue",typeof(int),"p2")
            });
            _ = mth.MenorQue(mth.Parametros[0], mth.Parametros[1]);
            _ = mth.CriarRetorno();

            classe.Executar();
            dynamic calc = classe.ObterInstancia();
            Assert.IsNotNull(calc);
            Assert.AreEqual(true, calc.MenorQue(2, 3));
            Assert.AreEqual(2, calc.SeMenorValor(2, 3));
        }

        [TestMethod()]
        public void MaiorOuIgualQueTest()
        {
            ILMetodo mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(int), "SeMaiorValorOuIgual", new ILParametro[] {
                new ILParametro("SeMaiorValorOuIgual",typeof(int),"p1"),
                new ILParametro("SeMaiorValorOuIgual",typeof(int),"p2")
            });
            _ = mth.Se(mth.Parametros[0], mth.MaiorOuIgualQue, mth.Parametros[1]);
            _ = mth.CarregarParametro(mth.Parametros[0]);
            _ = mth.CriarRetorno();
            _ = mth.SeFim();
            _ = mth.CarregarParametro(mth.Parametros[1]);
            _ = mth.CriarRetorno();

            mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(bool), "MaiorOuIgualQue", new ILParametro[] {
                new ILParametro("MaiorOuIgualQue",typeof(int),"p1"),
                new ILParametro("MaiorOuIgualQue",typeof(int),"p2")
            });
            _ = mth.MaiorOuIgualQue(mth.Parametros[0], mth.Parametros[1]);
            _ = mth.CriarRetorno();

            classe.Executar();
            dynamic calc = classe.ObterInstancia();
            Assert.IsNotNull(calc);
            Assert.AreEqual(true, calc.MaiorOuIgualQue(4, 3));
            Assert.AreEqual(4, calc.SeMaiorValorOuIgual(4, 3));
        }

        [TestMethod()]
        public void MenorOuIgualQueTest()
        {
            ILMetodo mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(int), "SeMenorValorOuIgual", new ILParametro[] {
                new ILParametro("SeMenorValorOuIgual",typeof(int),"p1"),
                new ILParametro("SeMenorValorOuIgual",typeof(int),"p2")
            });
            _ = mth.Se(mth.Parametros[0], mth.MenorOuIgualQue, mth.Parametros[1]);
            _ = mth.CarregarParametro(mth.Parametros[0]);
            _ = mth.CriarRetorno();
            _ = mth.SeFim();
            _ = mth.CarregarParametro(mth.Parametros[1]);
            _ = mth.CriarRetorno();

            mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(bool), "MenorOuIgualQue", new ILParametro[] {
                new ILParametro("MenorOuIgualQue",typeof(int),"p1"),
                new ILParametro("MenorOuIgualQue",typeof(int),"p2")
            });
            _ = mth.MenorOuIgualQue(mth.Parametros[0], mth.Parametros[1]);
            _ = mth.CriarRetorno();

            classe.Executar();
            dynamic calc = classe.ObterInstancia();
            Assert.IsNotNull(calc);
            Assert.AreEqual(true, calc.MenorOuIgualQue(2, 3));
            Assert.AreEqual(2, calc.SeMenorValorOuIgual(2, 3));
        }







    }
}