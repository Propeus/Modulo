using Microsoft.VisualStudio.TestTools.UnitTesting;
using Propeus.Modulo.Util.Objetos;
using Propeus.Modulo.Util.Teste.Modelo;
using System;

namespace Propeus.Modulo.Util.Vetores.Teste
{
    [TestClass()]
    public class HelperTeste
    {
        [TestMethod]
        public void DeserializarTesteByteGenericoSucesso()
        {
            ClasseParaSerializacaoTeste teste = new ClasseParaSerializacaoTeste();
            byte[] resultado = teste.Serializar();
            Assert.IsNotNull(resultado);

            ClasseParaSerializacaoTeste resultadoFinal = resultado.Deserializar<ClasseParaSerializacaoTeste>();
            Assert.IsNotNull(resultadoFinal);
        }

        [TestMethod]
        public void DeserializarTesteByteTypeSucesso()
        {
            ClasseParaSerializacaoTeste teste = new ClasseParaSerializacaoTeste();
            byte[] resultado = teste.Serializar();
            Assert.IsNotNull(resultado);

            object resultadoFinal = resultado.Deserializar(typeof(ClasseParaSerializacaoTeste));
            Assert.IsNotNull(resultadoFinal);
        }

        [TestMethod]
        public void HashTesteSucesso()
        {
            Assert.IsNotNull(new ClasseParaSerializacaoTeste().Serializar().Hash());
        }

        [TestMethod]
        public void DeserializarTesteByteGenericoFalha()
        {
            byte[] teste = default;

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                teste.Deserializar<ClasseParaSerializacaoTeste>();
            });
        }

        [TestMethod]
        public void DeserializarTesteByteTypeFalha()
        {
            byte[] teste = default;

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                teste.Deserializar(typeof(ClasseParaSerializacaoTeste));
            });
        }

        [TestMethod]
        public void HashTesteFalha()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                Array.Empty<byte>().Hash();
            });
        }
    }
}