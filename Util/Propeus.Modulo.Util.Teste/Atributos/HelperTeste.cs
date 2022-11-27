using Microsoft.VisualStudio.TestTools.UnitTesting;
using Propeus.Modulo.Util.Teste.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Propeus.Modulo.Util.Atributos.Teste
{
    [TestCategory("Propeus.NetCore.Util.Attributes")]
    [TestClass()]
    public class HelperTeste
    {
        [TestMethod()]
        public void ObterAtributoTesteIEnumerableSucesso()
        {
            TestClassAttribute resultado = GetType().GetCustomAttributesData().ObterAtributo<TestClassAttribute>();
            Assert.IsNotNull(resultado);
        }

        [TestMethod()]
        public void ObterAtributoTesteTypeSucesso()
        {
            TestClassAttribute resultado = GetType().ObterAtributo<TestClassAttribute>();
            Assert.IsNotNull(resultado);
        }

        [TestMethod()]
        public void ObterAtributoTesteObjectSucesso()
        {
            TestClassAttribute resultado = this.ObterAtributo<TestClassAttribute>();
            Assert.IsNotNull(resultado);
        }

        [TestMethod()]
        public void ObterAtributosTesteSucesso()
        {
            IEnumerable<TestClassAttribute> resultado = GetType().ObterAtributos<TestClassAttribute>();
            Assert.IsNotNull(resultado);
            Assert.IsTrue(resultado.Any());
        }

        [TestMethod()]
        public void PossuiAtributoTesteTypeSucesso()
        {
            Assert.IsTrue(GetType().PossuiAtributo<TestClassAttribute>());
        }

        [TestMethod()]
        public void PossuiAtributoTesteObjectSucesso()
        {
            Assert.IsTrue(this.PossuiAtributo<TestClassAttribute>());
        }

        [TestMethod()]
        public void PossuiAtributoTestePropertyInfoSucesso()
        {
            Assert.IsTrue(typeof(ClasseAjudaAtributoTeste).GetProperty("Propriedade").PossuiAtributo<System.ComponentModel.DescriptionAttribute>());
        }

        [TestMethod()]
        public void ObterAtributoTesteIEnumerableNaoEncontradoFalha()
        {
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                GetType().GetCustomAttributesData().ObterAtributo<TestMethodAttribute>();
            });
        }

        [TestMethod()]
        public void ObterAtributoTesteIEnumerableParametroNuloFalha()
        {
            IEnumerable<CustomAttributeData> tipoNulo = null;
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                tipoNulo.ObterAtributo<TestClassAttribute>();
            });
        }

        [TestMethod()]
        public void ObterAtributoTesteIEnumerableParametroVazioFalha()
        {
            IEnumerable<CustomAttributeData> tipoNulo = new List<CustomAttributeData>();
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                tipoNulo.ObterAtributo<TestClassAttribute>();
            });
        }

        [TestMethod()]
        public void ObterAtributoTesteTypeNaoEncontradoFalha()
        {
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                GetType().ObterAtributo<TestMethodAttribute>();
            });
        }

        [TestMethod()]
        public void ObterAtributoTesteTypeParametroNuloFalha()
        {
            Assert.ThrowsException<ArgumentException>(() =>
            {
                default(Type).ObterAtributo<TestMethodAttribute>();
            });
        }

        [TestMethod()]
        public void ObterAtributoTesteTypeAtributoEmAtributoFalha()
        {
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                typeof(TestMethodAttribute).ObterAtributo<TestMethodAttribute>();
            });
        }

        [TestMethod()]
        public void ObterAtributoTesteObjectNaoEncontradoFalha()
        {
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                this.ObterAtributo<TestMethodAttribute>();
            });
        }

        [TestMethod()]
        public void ObterAtributoTesteObjectParametroNuloFalha()
        {
            Assert.ThrowsException<ArgumentException>(() =>
            {
                default(object).ObterAtributo<TestMethodAttribute>();
            });
        }

        [TestMethod()]
        public void ObterAtributoTesteObjectAtributoEmAtributoFalha()
        {
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                Activator.CreateInstance(typeof(TestMethodAttribute)).ObterAtributo<TestMethodAttribute>();
            });
        }

        [TestMethod()]
        public void ObterAtributosTesteTypeNaoEncontradoFalha()
        {
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                GetType().ObterAtributos<TestMethodAttribute>();
            });
        }

        [TestMethod()]
        public void ObterAtributosTesteTypeParametroNuloFalha()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                default(Type).ObterAtributos<TestClassAttribute>();
            });
        }

        [TestMethod()]
        public void ObterAtributosTesteTypeAtributoEmAtributoFalha()
        {
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                typeof(TestMethodAttribute).ObterAtributos<TestMethodAttribute>();
            });
        }

        [TestMethod()]
        public void PossuiAtributoTesteTypeParametroNuloFalha()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                default(Type).PossuiAtributo<TestMethodAttribute>();
            });
        }

        [TestMethod()]
        public void PossuiAtributoTesteTypeAtributoEmAtributoFalha()
        {
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                typeof(TestMethodAttribute).PossuiAtributo<TestMethodAttribute>();
            });
        }

        [TestMethod()]
        public void PossuiAtributoTesteObjectParametroNuloFalha()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                default(object).PossuiAtributo<TestMethodAttribute>();
            });
        }

        [TestMethod()]
        public void PossuiAtributoTesteObjectAtributoEmAtributoFalha()
        {
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                Activator.CreateInstance(typeof(TestMethodAttribute)).PossuiAtributo<TestMethodAttribute>();
            });
        }

        [TestMethod()]
        public void PossuiAtributoTestePropertyParametroNuloFalha()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                default(PropertyInfo).PossuiAtributo<DescriptionAttribute>();
            });
        }
    }
}