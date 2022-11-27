using Microsoft.VisualStudio.TestTools.UnitTesting;
using Propeus.Modulo.Util.Teste.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Propeus.Modulo.Util.Genericos.Teste
{
    [TestClass()]
    public class HelperTeste
    {
        [TestMethod]
        public void CopiarPropriedadesTesteSucesso()
        {
            ClasseParaSerializacaoTeste cls_a = new ClasseParaSerializacaoTeste() { Teste = "teste" };
            ClasseParaSerializacaoInterfaceTeste cls_b = cls_a.CopiarPropriedades<ClasseParaSerializacaoTeste, ClasseParaSerializacaoInterfaceTeste>();
            Assert.IsNotNull(cls_b);
            Assert.AreEqual("teste", cls_b.Teste);
        }

        [TestMethod]
        public void InserirValorPropriedadeTesteFalseSucesso()
        {
            ClasseParaSerializacaoTeste cls_a = default;
            ClasseParaSerializacaoTeste cls_b = new ClasseParaSerializacaoTeste() { Teste = "teste" };
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                cls_a.InserirValorPropriedade(cls_b.ObterPropriedadeInfoType().First().Key, "teste2");
            });
        }

        [TestMethod]
        public void InserirValorPropriedadeTesteTrueSucesso()
        {
            ClasseParaSerializacaoTeste cls_a = new ClasseParaSerializacaoTeste() { Teste = "teste" };
            ClasseParaSerializacaoTeste cls_b = new ClasseParaSerializacaoTeste() { Teste = "teste" };
            cls_a.InserirValorPropriedade(cls_b.ObterPropriedadeInfoType().First().Key, "teste2");
            Assert.IsNotNull(cls_a.Teste);
            Assert.AreEqual("teste2", cls_a.Teste);
        }

        [TestMethod]
        public void ObterValorPropriedadeTesteSucesso()
        {
            ClasseParaSerializacaoTeste cls_a = new ClasseParaSerializacaoTeste() { Teste = "teste" };
            object resultado = cls_a.ObterValorPropriedade(cls_a.ObterPropriedadeInfoType().First().Key);
            Assert.IsNotNull(resultado);
            Assert.AreEqual("teste", resultado);
        }

        [TestMethod]
        public void ObterPropriedadeInfoTypeTesteSucesso()
        {
            ClasseParaSerializacaoTeste cls_a = new ClasseParaSerializacaoTeste() { Teste = "teste" };
            IDictionary<System.Reflection.PropertyInfo, Type> resultado = cls_a.ObterPropriedadeInfoType();
            Assert.IsNotNull(resultado);
            Assert.IsTrue(resultado.Any());
        }

        [TestMethod]
        public void IsNullTesteTrueSucesso()
        {
            Assert.IsTrue(default(ClasseHeranca).IsNull());
        }

        [TestMethod]
        public void IsNotNullTesteTrueSucesso()
        {
            Assert.IsTrue("teste".IsNotNull());
        }

        [TestMethod]
        public void IsNotNullTesteFalseSucesso()
        {
            Assert.IsFalse(default(string).IsNotNull());
        }

        [TestMethod]
        public void CopiarPropriedadesTesteFalha()
        {
            ClasseParaSerializacaoTeste cls_a = default;
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                cls_a.CopiarPropriedades<ClasseParaSerializacaoTeste, ClasseParaSerializacaoInterfaceTeste>();
            });
        }

        [TestMethod]
        public void ObterPropriedadeInfoTypeTesteParametroNuloFalha()
        {
            ClasseParaSerializacaoTeste cls_a = default;
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                cls_a.ObterPropriedadeInfoType();
            });
        }

        [TestMethod]
        public void ObterValorPropriedadeParametroNuloObjFalha()
        {
            ClasseParaSerializacaoTeste cls_a = default;
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                cls_a.ObterValorPropriedade(cls_a.ObterPropriedadeInfoType().First().Key);
            });
        }

        [TestMethod]
        public void ObterValorPropriedadeParametroNuloPropertyFalha()
        {
            ClasseParaSerializacaoTeste cls_a = new ClasseParaSerializacaoTeste() { Teste = "teste" };
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                cls_a.ObterValorPropriedade(null);
            });
        }

        [TestMethod]
        public void InserirValorPropriedadeParametroNuloPropertyFalha()
        {
            ClasseParaSerializacaoTeste cls_a = new ClasseParaSerializacaoTeste() { Teste = "teste" };

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                cls_a.InserirValorPropriedade(null, "teste2");
            });
        }
    }
}