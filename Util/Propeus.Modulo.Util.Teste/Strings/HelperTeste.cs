using Microsoft.VisualStudio.TestTools.UnitTesting;
using Propeus.Modulo.Util.Teste.Modelo;
using System;

namespace Propeus.Modulo.Util.Strings.Teste
{
    [TestClass()]
    public class HelperTeste
    {
        [TestMethod]
        public void ToArrayByteTesteSucesso()
        {
            string parametro = "teste";
            byte[] resultado = parametro.ToArrayByte();
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void ToArrayByteTesteParametroNuloFalha()
        {
            string parametro = null;
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                parametro.ToArrayByte();
            });
        }

        [TestMethod]
        public void ToArrayByteTesteParametroVazioFalha()
        {
            string parametro = string.Empty;
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                parametro.ToArrayByte();
            });
        }

        [TestMethod]
        public void FirstSplitTesteSucesso()
        {
            string parametro = "teste | primeira | quebra";
            string[] resultado = parametro.FirstSplit('|');
            Assert.IsNotNull(resultado);
            Assert.AreEqual(2, resultado.Length);
            Assert.AreEqual(" primeira | quebra", resultado[1]);
        }

        [TestMethod]
        public void FirstSplitTesteParametroNuloStrFalha()
        {
            string parametro = null;
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                parametro.FirstSplit('|');
            });
        }

        [TestMethod]
        public void FirstSplitTesteParametroVazioStrFalha()
        {
            string parametro = string.Empty;
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                parametro.FirstSplit('|');
            });
        }

        [TestMethod]
        public void FirstSplitTesteParametroNuloSeparatorFalha()
        {
            string parametro = "teste | primeira | quebra";
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                parametro.FirstSplit(default);
            });
        }

        [TestMethod]
        public void FirstSplitTesteParametroVazioSeparatorFalha()
        {
            string parametro = string.Empty;
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                parametro.FirstSplit(char.MinValue);
            });
        }

        [TestMethod]
        public void FirstReplaceTesteSucesso()
        {
            string parametro = "teste | primeira | quebra";
            string resultado = parametro.FirstReplace("quebra", "substituicao");
            Assert.IsNotNull(resultado);
            Assert.AreEqual("teste | primeira | substituicao", resultado);
        }

        [TestMethod]
        public void FirstReplaceTesteParametroNuloStrFalha()
        {
            string parametro = null;
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                parametro.FirstReplace("quebra", "substituicao");
            });
        }

        [TestMethod]
        public void FirstReplaceTesteParametroVazioStrFalha()
        {
            string parametro = string.Empty;
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                parametro.FirstReplace(string.Empty, "substituicao");
            });
        }

        [TestMethod]
        public void FirstReplaceTesteParametroNuloAntigoFalha()
        {
            string parametro = "teste | primeira | quebra";
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                parametro.FirstReplace(null, "substituicao");
            });
        }

        [TestMethod]
        public void FirstReplaceTesteParametroVazioAntigoFalha()
        {
            string parametro = "teste | primeira | quebra";
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                parametro.FirstReplace(string.Empty, "substituicao");
            });
        }

        [TestMethod()]
        public void IsNullOrEmptyTesteSucesso()
        {
            bool resultado = "".IsNullOrEmpty();
            Assert.IsTrue(resultado);
        }

        [TestMethod]
        public void ExisteMetodoTesteSucesso()
        {
            bool resultado = "TesteSemRetorno".ExisteMetodo<ClasseHeranca>();
            Assert.IsTrue(resultado);
        }
    }
}