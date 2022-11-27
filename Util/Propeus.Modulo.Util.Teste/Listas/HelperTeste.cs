using Microsoft.VisualStudio.TestTools.UnitTesting;
using Propeus.Modulo.Util.Teste.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Propeus.Modulo.Util.Listas.Teste
{
    [TestCategory("Propeus.NetCore.Util.Enums.Lista")]
    [TestClass()]
    public class HelperTeste
    {
        [TestMethod()]
        public void JoinTesteSucesso()
        {
            IEnumerable<string> esquerda = new string[] { "A", "B", "C" };
            IEnumerable<string> direita = new string[] { "C", "D", "E" };
            IEnumerable<string> resultado = esquerda.Join(direita);
            Assert.IsTrue(resultado.Contains("C"));
            Assert.IsTrue(resultado.Count() == 1);
        }

        [TestMethod()]
        public void JoinTesteParametroEsquerdoVazioSucesso()
        {
            IEnumerable<string> esquerda = Array.Empty<string>();
            IEnumerable<string> direita = new string[] { "C", "D", "E" };
            IEnumerable<string> resultado = esquerda.Join(direita);
            Assert.IsTrue(!resultado.Contains("C"));
            Assert.IsTrue(!resultado.Any());
        }

        [TestMethod()]
        public void JoinTesteParametroDireitoVazioSucesso()
        {
            IEnumerable<string> esquerda = new string[] { "A", "B", "C" };
            IEnumerable<string> direita = Array.Empty<string>();
            IEnumerable<string> resultado = esquerda.Join(direita);
            Assert.IsTrue(!resultado.Contains("C"));
            Assert.IsTrue(!resultado.Any());
        }

        [TestMethod()]
        public void ContainsAllTesteTypeSucesso()
        {
            IEnumerable<Type> tipos = new Type[] { typeof(string), typeof(int), typeof(float) };
            bool resultado = tipos.ContainsAll(typeof(int), typeof(string));
            Assert.IsTrue(resultado);
        }

        [TestMethod()]
        public void ContainsAllTesteTypeSemParametroSucesso()
        {
            IEnumerable<Type> tipos = new Type[] { typeof(string), typeof(int), typeof(float) };
            bool resultado = tipos.ContainsAll();
            Assert.IsTrue(resultado);
        }

        [TestMethod()]
        public void ContainsAllTesteTypeHerdadoSucesso()
        {
            IEnumerable<Type> tipos = new Type[] { typeof(string), typeof(ClasseHeranca), typeof(float) };
            bool resultado = tipos.ContainsAll(typeof(ClasseAjudaAtributoTeste), typeof(string));
            Assert.IsFalse(resultado);
        }

        [TestMethod()]
        public void ContainsAllTesteIntSucesso()
        {
            IEnumerable<int> tipos = new int[] { 1, 5, 7 };
            bool resultado = tipos.ContainsAll(1, 7);
            Assert.IsTrue(resultado);
        }

        [TestMethod()]
        public void ContainsAllTesteIntSemParametroSucesso()
        {
            IEnumerable<int> tipos = new int[] { 1, 5, 7 };
            bool resultado = tipos.ContainsAll();
            Assert.IsTrue(resultado);
        }

        [TestMethod()]
        public void ContainsAllTesteStringSucesso()
        {
            IEnumerable<string> tipos = new string[] { "1", "5", "7" };
            bool resultado = tipos.ContainsAll("1", "7");
            Assert.IsTrue(resultado);
        }

        [TestMethod()]
        public void ContainsAllTesteStringSemParametroSucesso()
        {
            IEnumerable<string> tipos = new string[] { "1", "5", "7" };
            bool resultado = tipos.ContainsAll();
            Assert.IsTrue(resultado);
        }

        [TestMethod()]
        public void IsEmptyTesteNaoVazioSucesso()
        {
            Assert.IsFalse(new string[] { "t", "e", "s", "t", "e" }.IsEmpty());
        }

        [TestMethod()]
        public void IsEmptyTesteVazioSucesso()
        {
            Assert.IsTrue(Array.Empty<string>().IsEmpty());
        }

        [TestMethod()]
        public void IsNotEmptyTesteNaoVazioSucesso()
        {
            Assert.IsTrue(new string[] { "t", "e", "s", "t", "e" }.IsNotEmpty());
        }

        [TestMethod()]
        public void IsNotEmptyTesteVazioSucesso()
        {
            Assert.IsFalse(Array.Empty<string>().IsNotEmpty());
        }

        [TestMethod()]
        public void IsNullOrEmptyTesteIEnumerableFalseSucesso()
        {
            Assert.IsFalse(new string[] { "t", "e", "s", "t", "e" }.IsNullOrEmpty());
        }

        [TestMethod()]
        public void IsNullOrEmptyTesteIEnumerableTrueNuloSucesso()
        {
            Assert.IsTrue(default(IEnumerable<string>).IsNullOrEmpty());
        }

        [TestMethod]
        public void IsNullOrEmptyTesteIEnumerableTrueVazioSucesso()
        {
            Assert.IsTrue(Array.Empty<string>().IsNullOrEmpty());
        }

        [TestMethod()]
        public void IsNullOrEmptyTesteStringTrueSucesso()
        {
            Assert.IsTrue("".IsNullOrEmpty());
        }

        [TestMethod()]
        public void IsNullOrEmptyTesteStringFalseSucesso()
        {
            Assert.IsFalse("teste".IsNullOrEmpty());
        }

        [TestMethod()]
        public void IsNotNullOrEmptyTesteResultadoTrueNuloSucesso()
        {
            Assert.IsFalse(default(IEnumerable<string>).IsNotNullOrEmpty());
        }

        [TestMethod()]
        public void IsNotNullOrEmptyTesteResultadoTrueVazioSucesso()
        {
            Assert.IsFalse(Array.Empty<string>().IsNotNullOrEmpty());
        }

        [TestMethod]
        public void IsNotNullOrEmptyTesteResultadoFalseSucesso()
        {
            Assert.IsTrue(new string[] { "t", "e", "s", "t", "e" }.IsNotNullOrEmpty());
        }

        [TestMethod]
        public void JoinTesteParametroEsquerdoNuloFalha()
        {
            IEnumerable<string> esquerda = null;
            IEnumerable<string> direita = new string[] { "C", "D", "E" };
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                esquerda.Join(direita);
            });
        }

        [TestMethod]
        public void JoinTesteParametroDireitoNuloFalha()
        {
            IEnumerable<string> direita = null;
            IEnumerable<string> esquerda = new string[] { "C", "D", "E" };
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                esquerda.Join(direita);
            });
        }

        [TestMethod]
        public void ContainsAllTesteTypeParametroNuloFalha()
        {
            IEnumerable<Type> tipos = default;
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                tipos.ContainsAll(typeof(int), typeof(string));
            });
        }

        [TestMethod]
        public void ContainsAllTesteObjectParametroNuloFalha()
        {
            IEnumerable<int> tipos = default;
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                tipos.ContainsAll(1, 2, 3);
            });
        }

        [TestMethod]
        public void IsEmptyTesteParametroNuloFalha()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                default(IEnumerable<string>).IsEmpty();
            });
        }

        [TestMethod]
        public void IsNotEmptyTesteParametroNuloFalha()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                default(IEnumerable<string>).IsNotEmpty();
            });
        }
    }
}