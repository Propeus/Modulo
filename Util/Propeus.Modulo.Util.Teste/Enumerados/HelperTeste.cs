using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel;

namespace Propeus.Modulo.Util.Enumerados.Teste
{
    [TestCategory("Propeus.NetCore.Util.Enums")]
    [TestClass()]
    public class HelperTeste
    {
        [TestMethod()]
        public void ConcatenarEnumTesteSucesso()
        {
            EnumSimplesTeste[] teste = new EnumSimplesTeste[] { EnumSimplesTeste.conc_1, EnumSimplesTeste.conc_2 };
            EnumSimplesTeste resultado = teste.ConcatenarEnum();
            Assert.AreEqual(EnumSimplesTeste.conc_1 | EnumSimplesTeste.conc_2, resultado);
        }

        [TestMethod()]
        public void ObterDescricaoEnumTesteSucesso()
        {
            string descricao = EnumSimplesTeste.conc_1.ObterDescricaoEnum();
            Assert.IsNotNull(descricao);
        }

        [TestMethod()]
        public void ConcatenarEnumTesteArgumentoVazioFalha()
        {
            EnumSimplesTeste[] teste = Array.Empty<EnumSimplesTeste>();
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                teste.ConcatenarEnum();
            });
        }

        [TestMethod()]
        public void ConcatenarEnumTesteArgumentoNuloFalha()
        {
            EnumSimplesTeste[] teste = null;
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                teste.ConcatenarEnum();
            });
        }

        [TestMethod()]
        public void ConcatenarEnumTesteArgumentoTipoInvalidoFalha()
        {
            object[] teste = new string[] { "teste", "unitario" };
            Assert.ThrowsException<ArgumentException>(() =>
            {
                teste.ConcatenarEnum();
            });
        }

        [TestMethod()]
        public void ObterDescricaoEnumTesteArgumentoNuloFalha()
        {
            EnumSimplesTeste[] teste = null;
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                teste.ConcatenarEnum();
            });
        }

        [TestMethod()]
        public void ObterDescricaoEnumTesteSemDescricaoFalha()
        {
            Assert.ThrowsException<InvalidEnumArgumentException>(() =>
            {
                EnumSimplesTeste.conc_3.ObterDescricaoEnum();
            });
        }
    }
}