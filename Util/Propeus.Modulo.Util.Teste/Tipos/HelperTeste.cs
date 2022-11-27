using Microsoft.VisualStudio.TestTools.UnitTesting;
using Propeus.Modulo.Util.Enumerados.Teste;
using Propeus.Modulo.Util.Teste.Interfaces;
using Propeus.Modulo.Util.Teste.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Propeus.Modulo.Util.Tipos.Teste
{
    [TestClass()]
    public class HelperTeste
    {
        [TestMethod]
        public void HerdadoTesteTypeTrueSucesso()
        {
            bool resultado = typeof(ClasseHeranca).Herdado(typeof(ClasseAjudaAtributoTeste));
            Assert.IsTrue(resultado);
        }

        [TestMethod]
        public void HerdadoTesteTypeFalseSucesso()
        {
            bool resultado = typeof(ClasseHeranca).Herdado(typeof(ClasseNaoSerializavelTeste));
            Assert.IsFalse(resultado);
        }

        [TestMethod]
        public void HerdadoTesteTypeGenericoTrueSucesso()
        {
            bool resultado = typeof(ClasseHeranca).Herdado<ClasseAjudaAtributoTeste>();
            Assert.IsTrue(resultado);
        }

        [TestMethod]
        public void HerdadoTesteTypeGenericoFalseSucesso()
        {
            bool resultado = typeof(ClasseHeranca).Herdado<ClasseNaoSerializavelTeste>();
            Assert.IsFalse(resultado);
        }

        [TestMethod]
        public void IsTesteTypeGenericoSucesso()
        {
            Type teste = 5.GetType();
            Assert.IsTrue(teste.Is<int>());
        }

        [TestMethod]
        public void IsTesteTypeTypeSucesso()
        {
            Type teste = 5.GetType();
            Assert.IsTrue(teste.Is(typeof(int)));
        }

        [TestMethod]
        public void IsVoidTesteTrueSucesso()
        {
            Assert.IsTrue(typeof(void).IsVoid());
        }

        [TestMethod]
        public void IsVoidTesteFalseSucesso()
        {
            Assert.IsFalse(typeof(string).IsVoid());
        }

        [TestMethod]
        public void IsNotVoidTesteFalseSucesso()
        {
            Assert.IsFalse(typeof(void).IsNotVoid());
        }

        [TestMethod]
        public void IsNotVoidTesteTrueSucesso()
        {
            Assert.IsTrue(typeof(string).IsNotVoid());
        }

        [TestMethod]
        public void IsNotNullAndNotVoidTesteFalseSucesso()
        {
            Assert.IsFalse(typeof(void).IsNotNullAndNotVoid());
        }

        [TestMethod]
        public void IsNotNullAndNotVoidTesteTrueSucesso()
        {
            Assert.IsTrue(typeof(string).IsNotNullAndNotVoid());
        }

        [TestMethod]
        public void IsNotNullAndNotVoidTesteParametroNuloSucesso()
        {
            Assert.IsFalse(default(Type).IsNotNullAndNotVoid());
        }

        [TestMethod]
        public void DefaultTesteClasseSucesso()
        {
            Assert.AreEqual(default(ClasseHeranca), typeof(ClasseHeranca).Default());
        }

        [TestMethod]
        public void DefaultTesteInterfaceSucesso()
        {
            Assert.AreEqual(default(IInterfaceTeste), typeof(IInterfaceTeste).Default());
        }

        [TestMethod]
        public void DefaultTesteEnumSucesso()
        {
            Assert.AreEqual(default(EnumSimplesTeste), typeof(EnumSimplesTeste).Default());
        }

        [TestMethod]
        public void DefaultTestePrimitivoSucesso()
        {
            Assert.AreEqual(default(int), typeof(int).Default());
            Assert.AreEqual(default(float), typeof(float).Default());
            Assert.AreEqual(default(double), typeof(double).Default());
            Assert.AreEqual(default(decimal), typeof(decimal).Default());
            Assert.AreEqual(default(long), typeof(long).Default());
            Assert.AreEqual(default(string), typeof(string).Default());
            Assert.AreEqual(default(bool), typeof(bool).Default());
            Assert.AreEqual(default(char), typeof(char).Default());
        }

        [TestMethod]
        public void DefaultTesteStructSucesso()
        {
            Assert.AreEqual(default(StructTesteCast), typeof(StructTesteCast).Default());
        }

        [TestMethod]
        public void ObterInterfacesTesteSucesso()
        {
            Type teste = typeof(ClasseHeranca);
            IEnumerable<Type> resultado = teste.ObterInterfaces();
            Assert.IsNotNull(resultado);
            Assert.IsTrue(resultado.Any());
        }

        [TestMethod]
        public void ObterMetodosTesteStringSucesso()
        {
            Type teste = typeof(ClasseHeranca);
            IEnumerable<System.Reflection.MethodInfo> resultado = teste.ObterMetodos("Teste");
            Assert.IsNotNull(resultado);
            Assert.IsTrue(resultado.Any());
        }

        [TestMethod]
        public void ObterMetodosTesteTypeSucesso()
        {
            Type teste = typeof(ClasseHeranca);
            IEnumerable<System.Reflection.MethodInfo> resultado = teste.ObterMetodos();
            Assert.IsNotNull(resultado);
            Assert.IsTrue(resultado.Any());
        }

        [TestMethod]
        public void ExisteMetodoTesteClasseMethodInfoSucesso()
        {
            Type teste = typeof(ClasseHeranca);
            System.Reflection.MethodInfo act = teste.GetMethod("TesteSemRetorno");
            bool resultado = teste.ExisteMetodo(act);
            Assert.IsTrue(resultado);
        }

        [TestMethod]
        public void ExisteMetodoTesteClasseActionSucesso()
        {
            Type teste = typeof(ClasseHeranca);
            Action act = (Action)teste.GetMethod("TesteSemRetorno").CreateDelegate(typeof(Action), new ClasseHeranca());
            bool resultado = teste.ExisteMetodo(act);
            Assert.IsTrue(resultado);
        }

        [TestMethod]
        public void ObterMetodoComMaiorParametrosTesteSucesso()
        {
            Type teste = typeof(ClasseHeranca);
            System.Reflection.MethodInfo resultado = teste.ObterMetodoComMaiorParametros("TesteMetodoParametroOverload");
            Assert.IsNotNull(resultado);
            Assert.AreEqual(3, resultado.GetParameters().Count());
        }

        [TestMethod]
        public void ObterMetodoTesteTypeSemParametroSucesso()
        {
            Type teste = typeof(ClasseHeranca);
            System.Reflection.MethodInfo resultado = teste.ObterMetodo("TesteSemRetorno");
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void ObterMetodoTesteTypeComParametroSucesso()
        {
            Type teste = typeof(ClasseHeranca);
            System.Reflection.MethodInfo resultado = teste.ObterMetodo("TesteMetodoParametroTipoDiferentes", typeof(int), typeof(string), typeof(int));
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void ObterMetodoTesteMethodInfoSemParametroSucesso()
        {
            Type teste = typeof(ClasseHeranca);
            System.Reflection.MethodInfo resultado = teste.ObterMetodo("TesteSemRetorno");
            Assert.IsNotNull(resultado);

            resultado = teste.ObterMetodo(resultado);
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void ObterMetodoTesteMethodInfoComParametroSucesso()
        {
            Type teste = typeof(ClasseHeranca);
            System.Reflection.MethodInfo resultado = teste.ObterMetodo("TesteMetodoParametroTipoDiferentes", typeof(int), typeof(string), typeof(int));
            Assert.IsNotNull(resultado);

            resultado = teste.ObterMetodo(resultado);
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void ObterPropriedadeTesteSucesso()
        {
            Type teste = typeof(ClasseHeranca);
            System.Reflection.PropertyInfo resultado = teste.ObterPropriedade(teste.GetProperties().First());
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void ObterEventoTesteSucesso()
        {
            Type teste = typeof(ClasseHeranca);
            System.Reflection.EventInfo resultado = teste.ObterEvento(teste.GetEvents().First());
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void HerdadoTesteTypeGenericoTipoVoidFalha()
        {
            Assert.ThrowsException<ArgumentException>(() =>
            {
                typeof(void).Herdado<ClasseNaoSerializavelTeste>();
            });
        }

        [TestMethod]
        public void IsTesteTypeGenericoArgumentoNuloFalha()
        {
            Type teste = default;
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                teste.Is<int>();
            });
        }

        [TestMethod]
        public void IsTesteTypeTypeParametroNuloObjFalha()
        {
            Type teste = default;
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                teste.Is(typeof(int));
            });
        }

        [TestMethod]
        public void IsTesteTypeTypeParametroNuloComparacaoFalha()
        {
            Type teste = typeof(int);
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                teste.Is(null);
            });
        }
    }
}