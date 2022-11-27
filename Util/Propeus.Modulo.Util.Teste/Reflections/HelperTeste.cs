using Microsoft.VisualStudio.TestTools.UnitTesting;
using Propeus.Modulo.Util.Teste.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Propeus.Modulo.Util.Reflections.Teste
{
    [TestClass()]
    public class HelperTeste
    {
        [TestMethod]
        public void ObterTipoParametrosTesteConstrutorSucesso()
        {
            IEnumerable<Type> resultado = typeof(ClasseHeranca).GetConstructors().Last().ObterTipoParametros();
            Assert.IsNotNull(resultado);
            Assert.IsTrue(resultado.Any());
        }

        [TestMethod]
        public void ObterTipoParametrosTesteFuncaoSucesso()
        {
            IEnumerable<Type> resultado = typeof(ClasseHeranca).GetMethod("Teste").ObterTipoParametros();
            Assert.IsNotNull(resultado);
            Assert.IsTrue(resultado.Any());
        }

        [TestMethod]
        public void ObterTipoParametrosTestePropriedadeSucesso()
        {
            IEnumerable<Type> resultado = typeof(ClasseHeranca).GetProperty("Item").ObterTipoParametros();
            Assert.IsNotNull(resultado);
            Assert.IsTrue(resultado.Any());
        }

        [TestMethod]
        public void ExisteMetodoTesteSucesso()
        {
            Type teste = typeof(ClasseHeranca);
            Action act = (Action)teste.GetMethod("TesteSemRetorno").CreateDelegate(typeof(Action), new ClasseHeranca());
            bool resultado = act.ExisteMetodo<ClasseHeranca>();
            Assert.IsTrue(resultado);
        }

        [TestMethod]
        public void ObterPathProgramaAtualTesteFalha()
        {
            Assert.IsNotNull(Reflections.Helper.ObterPathProgramaAtual());
        }

        [TestMethod]
        public void ObterTipoParametrosTesteConstrutorParametroNuloActionFalha()
        {
            _ = Assert.ThrowsException<ArgumentNullException>(() =>
              {
                  _ = default(ConstructorInfo).ObterTipoParametros();
              });
        }

        [TestMethod]
        public void ObterTipoParametrosTesteMetodoParametroNuloActionFalha()
        {
            _ = Assert.ThrowsException<ArgumentNullException>(() =>
            {
                _ = default(MethodInfo).ObterTipoParametros();
            });
        }

        [TestMethod]
        public void ObterTipoParametrosTestePrpopriedadeParametroNuloActionFalha()
        {
            _ = Assert.ThrowsException<ArgumentNullException>(() =>
            {
                _ = default(PropertyInfo).ObterTipoParametros();
            });
        }

        [TestMethod]
        public void ExisteMetodoTesteFalha()
        {
            _ = Assert.ThrowsException<ArgumentNullException>(() =>
            {
                _ = default(Action).ExisteMetodo<ClasseHeranca>();
            });
        }

        [TestMethod]
        public void ObterPathProgramaAtualTesteSucesso()
        {
            Assert.IsNotNull(Reflections.Helper.ObterPathProgramaAtual());
        }
    }
}