using Microsoft.VisualStudio.TestTools.UnitTesting;
using Propeus.Modulo.Util.Enumerados.Teste;
using Propeus.Modulo.Util.Teste.Interfaces;
using Propeus.Modulo.Util.Teste.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Propeus.Modulo.Util.Objetos.Teste
{
    [TestClass()]
    public class HelperTeste
    {
        [TestMethod]
        public void SerializarTesteSucesso()
        {
            ClasseParaSerializacaoTeste teste = new ClasseParaSerializacaoTeste();
            byte[] resultado = teste.Serializar();
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void HerdadoTesteStructTypeTrueSucesso()
        {
            StructTesteCast teste = new StructTesteCast();
            bool resultado = teste.Herdado(typeof(IInterfaceTeste));
            Assert.IsTrue(resultado);
        }

        [TestMethod]
        public void HerdadoTesteStructTypeFalseSucesso()
        {
            StructTesteCast teste = new StructTesteCast();
            bool resultado = teste.Herdado(typeof(IInterfaceDeInterface));
            Assert.IsFalse(resultado);
        }

        [TestMethod]
        public void HerdadoTesteObjectTypeTrueSucesso()
        {
            ClasseHeranca teste = new ClasseHeranca();
            bool resultado = teste.Herdado(typeof(ClasseAjudaAtributoTeste));
            Assert.IsTrue(resultado);
        }

        [TestMethod]
        public void HerdadoTesteObjectTypeFalseSucesso()
        {
            ClasseHeranca teste = new ClasseHeranca();
            bool resultado = teste.Herdado(typeof(ClasseNaoSerializavelTeste));
            Assert.IsFalse(resultado);
        }

        [TestMethod]
        public void HerdadoTesteObjectGenericoTrueSucesso()
        {
            ClasseHeranca teste = new ClasseHeranca();
            bool resultado = teste.Herdado<ClasseAjudaAtributoTeste>();
            Assert.IsTrue(resultado);
        }

        [TestMethod]
        public void HerdadoTesteObjectGenericoFalseSucesso()
        {
            ClasseHeranca teste = new ClasseHeranca();
            bool resultado = teste.Herdado<ClasseNaoSerializavelTeste>();
            Assert.IsFalse(resultado);
        }

        [TestMethod()]
        public void IsStructTesteTrueSucesso()
        {
            StructTesteCast teste = new StructTesteCast();
            Assert.IsTrue(teste.IsStruct());
        }

        [TestMethod()]
        public void IsStructTesteFalseSucesso()
        {
            ClasseHeranca teste = new ClasseHeranca();
            Assert.IsFalse(teste.IsStruct());
        }

        [TestMethod()]
        public void IsStructOrNullTesteStructTrueSucesso()
        {
            StructTesteCast teste = new StructTesteCast();
            Assert.IsTrue(teste.IsStructOrNull());
        }

        [TestMethod()]
        public void IsStructOrNullTesteClasseNullTrueSucesso()
        {
            ClasseHeranca teste = null;
            Assert.IsTrue(teste.IsStructOrNull());
        }

        [TestMethod()]
        public void IsStructOrNullTesteStructFalseSucesso()
        {
            ClasseHeranca teste = new ClasseHeranca();
            Assert.IsFalse(teste.IsStructOrNull());
        }

        [TestMethod()]
        public void IsStructOrNullTesteClasseNullFalseSucesso()
        {
            ClasseHeranca teste = new ClasseHeranca();
            Assert.IsFalse(teste.IsStructOrNull());
        }

        [TestMethod()]
        public void IsListTesteSucesso()
        {
            List<string> ls = new List<string>();
            Assert.IsTrue(ls.IsList());
        }

        [TestMethod()]
        public void IsListTesteIEnumerableSucesso()
        {
            IEnumerable<string> ls = new List<string>();
            Assert.IsTrue(ls.IsList());
        }

        [TestMethod()]
        public void IsDictionaryTesteSucesso()
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            Assert.IsTrue(dic.IsDictionary());
        }

        [TestMethod()]
        public void IsDictionaryTesteIDictionarySucesso()
        {
            IDictionary<int, string> dic = new Dictionary<int, string>();
            Assert.IsTrue(dic.IsDictionary());
        }

        [TestMethod]
        public void IsTestePrimitivoGenericoSucesso()
        {
            int teste = 5;
            Assert.IsTrue(teste.Is<int>());
        }

        [TestMethod]
        public void IsTesteClasseGenericoSucesso()
        {
            ClasseHeranca teste = new ClasseHeranca();
            Assert.IsTrue(teste.Is<ClasseHeranca>());
        }

        [TestMethod]
        public void IsTesteClasseTypeSucesso()
        {
            ClasseHeranca teste = new ClasseHeranca();
            Assert.IsTrue(teste.Is(typeof(ClasseHeranca)));
        }

        [TestMethod]
        public void IsTestePrimitivoTypeSucesso()
        {
            int teste = 5;
            Assert.IsTrue(teste.Is(typeof(int)));
        }

        [TestMethod]
        public void ToTesteClasseClasseGenericoSucesso()
        {
            ClasseHeranca teste = new ClasseHeranca();
            ClasseAjudaAtributoTeste resultado = teste.To<ClasseAjudaAtributoTeste>();
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void ToTesteClasseClasseGenericoRedundanciaSucesso()
        {
            ClasseHeranca teste = new ClasseHeranca();
            ClasseHeranca resultado = teste.To<ClasseHeranca>();
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void ToTesteIntEnumGenericoSucesso()
        {
            int teste = 1;
            EnumSimplesTeste resultado = teste.To<EnumSimplesTeste>();
            Assert.IsNotNull(resultado);
            Assert.AreEqual(EnumSimplesTeste.conc_2, resultado);
        }

        [TestMethod]
        public void ToTesteFloatIntGenericoSucesso()
        {
            float teste = 5f;
            int resultado = teste.To<int>();
            Assert.AreEqual(5, resultado);
        }

        [TestMethod]
        public void ToTesteIntFloatGenericoSucesso()
        {
            int teste = 5;
            float resultado = teste.To<float>();
            Assert.AreEqual(5f, resultado);
        }

        [TestMethod]
        public void ToTesteClasseInterfaceInterfaceGenericoSucesso()
        {
            ClasseHeranca teste = new ClasseHeranca();
            IInterfaceDeInterface resultado = teste.To<IInterfaceDeInterface>();
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void ToTesteClasseInterfaceGenericoSucesso()
        {
            ClasseHeranca teste = new ClasseHeranca();
            IInterfaceTeste resultado = teste.To<IInterfaceTeste>();
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void ToTesteInterfaceStructGenericoSucesso()
        {
            IInterfaceTeste teste = new StructTesteCast
            {
                Propriedade = "teste"
            };
            StructTesteCast resultado = teste.To<StructTesteCast>();
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void ToTesteStructInterfaceGenericoSucesso()
        {
            StructTesteCast teste = new StructTesteCast
            {
                Propriedade = "teste"
            };
            IInterfaceTeste resultado = teste.To<IInterfaceTeste>();
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void ToTesteIntStringGenericoSucesso()
        {
            int teste = 5;
            string resultado = teste.To<string>();
            Assert.AreEqual("5", resultado);
        }

        [TestMethod]
        public void ToTesteClasseClasseTypeSucesso()
        {
            ClasseHeranca teste = new ClasseHeranca();
            object resultado = teste.To(typeof(ClasseAjudaAtributoTeste));
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void ToTesteClasseClasseTypeRedundanciaSucesso()
        {
            ClasseHeranca teste = new ClasseHeranca();
            object resultado = teste.To(typeof(ClasseHeranca));
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void ToTesteIntEnumTypeSucesso()
        {
            int teste = 1;
            object resultado = teste.To(typeof(EnumSimplesTeste));
            Assert.IsNotNull(resultado);
            Assert.AreEqual(EnumSimplesTeste.conc_2, resultado);
        }

        [TestMethod]
        public void ToTesteIntFloatTypeSucesso()
        {
            int teste = 5;
            object resultado = teste.To(typeof(float));
            Assert.AreEqual(5f, resultado);
        }

        [TestMethod]
        public void ToTesteIntDoubleTypeSucesso()
        {
            int teste = 5;
            object resultado = teste.To(typeof(double));
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void ToTesteIntLongTypeSucesso()
        {
            int teste = 5;
            object resultado = teste.To(typeof(long));
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void ToTesteIntStringTypeSucesso()
        {
            int teste = 5;
            object resultado = teste.To(typeof(string));
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void ToTesteFloatIntTypeSucesso()
        {
            float teste = 5f;
            object resultado = teste.To(typeof(int));
            Assert.AreEqual(5, resultado);
        }

        [TestMethod]
        public void ToTesteFloatDoubleTypeSucesso()
        {
            float teste = 5f;
            object resultado = teste.To(typeof(double));
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void ToTesteFloatLongTypeSucesso()
        {
            float teste = 5f;
            object resultado = teste.To(typeof(long));
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void ToTesteFloatStringTypeSucesso()
        {
            float teste = 5f;
            object resultado = teste.To(typeof(string));
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void ToTesteDoubleIntTypeSucesso()
        {
            double teste = 5.35;
            object resultado = teste.To(typeof(int));
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void ToTesteDoubleFloatTypeSucesso()
        {
            double teste = 5.35;
            object resultado = teste.To(typeof(float));
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void ToTesteDoubleLongTypeSucesso()
        {
            double teste = 5.35;
            object resultado = teste.To(typeof(long));
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void ToTesteDoubleStringTypeSucesso()
        {
            double teste = 5.35;
            object resultado = teste.To(typeof(string));
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void ToTesteLongIntTypeSucesso()
        {
            long teste = 1524893;
            object resultado = teste.To(typeof(int));
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void ToTesteLongFlotTypeSucesso()
        {
            long teste = long.MaxValue;
            object resultado = teste.To(typeof(float));
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void ToTesteLongDoubleTypeSucesso()
        {
            long teste = long.MaxValue;
            object resultado = teste.To(typeof(double));
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void ToTesteLongStringTypeSucesso()
        {
            long teste = long.MaxValue;
            object resultado = teste.To(typeof(string));
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void ToTesteStringIntTypeSucesso()
        {
            string teste = 524853.ToString();
            object resultado = teste.To(typeof(int));
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void ToTesteStringFloatTypeSucesso()
        {
            string teste = long.MaxValue.ToString();
            object resultado = teste.To(typeof(float));
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void ToTesteStringDoubleTypeSucesso()
        {
            string teste = long.MaxValue.ToString();
            object resultado = teste.To(typeof(double));
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void ToTesteStringLongTypeSucesso()
        {
            string teste = long.MaxValue.ToString();
            object resultado = teste.To(typeof(long));
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void ToTesteClasseInterfaceInterfaceTypeSucesso()
        {
            ClasseHeranca teste = new ClasseHeranca();
            object resultado = teste.To(typeof(IInterfaceDeInterface));
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void ToTesteClasseInterfaceTypeSucesso()
        {
            ClasseHeranca teste = new ClasseHeranca();
            object resultado = teste.To(typeof(IInterfaceTeste));
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void ToTesteInterfaceStructTypeSucesso()
        {
            IInterfaceTeste teste = new StructTesteCast
            {
                Propriedade = "teste"
            };
            object resultado = teste.To(typeof(StructTesteCast));
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void ToTesteStructInterfaceTypeSucesso()
        {
            StructTesteCast teste = new StructTesteCast
            {
                Propriedade = "teste"
            };
            object resultado = teste.To(typeof(IInterfaceTeste));
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void AsTesteClasseSucesso()
        {
            ClasseHeranca teste = new ClasseHeranca();
            ClasseAjudaAtributoTeste resultado = teste.As<ClasseAjudaAtributoTeste>();
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void AsTesteFloatSucesso()
        {
            int teste = 5;
            float resultado = teste.As<float>();
            Assert.IsNotNull(resultado);
            Assert.AreEqual(5f, resultado);
        }

        [TestMethod]
        public void AsTesteIntSucesso()
        {
            float teste = 5f;
            int resultado = teste.As<int>();
            Assert.IsNotNull(resultado);
            Assert.AreEqual(5, resultado);
        }

        [TestMethod]
        public void AsTesteDoubleSucesso()
        {
            int teste = 5;
            double resultado = teste.As<double>();
            Assert.IsNotNull(resultado);
            Assert.AreEqual(5, resultado);
        }

        [TestMethod]
        public void AsTesteStringSucesso()
        {
            string teste = "5";
            int resultado = teste.As<int>();
            Assert.IsNotNull(resultado);
            Assert.AreEqual(5, resultado);
        }

        [TestMethod]
        public void AsTesteClasseNuloSucesso()
        {
            string teste = "5";
            ClasseHeranca resultado = teste.As<ClasseHeranca>();
            Assert.IsNull(resultado);
        }

        [TestMethod]
        public void AsTesteClasseValorPadraoSucesso()
        {
            string teste = "5";
            ClasseAjudaAtributoTeste resultado = teste.As(new ClasseAjudaAtributoTeste());
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void AsTesteIntValorPadraoSucesso()
        {
            ClasseHeranca teste = new ClasseHeranca();
            int resultado = teste.As(10);
            Assert.AreEqual(10f, resultado);
        }

        [TestMethod]
        public void AsTesteFloatValorPadraoSucesso()
        {
            ClasseHeranca teste = new ClasseHeranca();
            float resultado = teste.As(10f);
            Assert.AreEqual(10f, resultado);
        }

        [TestMethod]
        public void AsTesteStringValorPadraoSucesso()
        {
            ClasseHeranca teste = new ClasseHeranca();
            string resultado = teste.As("10");
            Assert.IsNotNull(resultado);
            Assert.AreEqual("10", resultado);
        }

        [TestMethod]
        public void AsTesteObjectStructSucesso()
        {
            object teste = new StructTesteCast
            {
                Propriedade = "teste"
            };
            object resultado = teste.As(como: typeof(StructTesteCast));
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void AsTesteStructInterfaceSucesso()
        {
            object teste = new StructTesteCast
            {
                Propriedade = "teste"
            };
            object resultado = teste.As(como: typeof(IInterfaceTeste));
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void AsTesteStructDefaultInterfaceSucesso()
        {
            object teste = new StructTesteCast();
            object result = teste.As(como: typeof(IInterfaceTeste));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AsTesteObjectIntSucesso()
        {
            object teste = 5f;
            object resultado = teste.As(como: typeof(int));
            Assert.AreEqual(5, resultado);
        }

        [TestMethod]
        public void AsTesteObjectFloatSucesso()
        {
            object teste = 5;
            object resultado = teste.As(como: typeof(float));
            Assert.AreEqual(5f, resultado);
        }

        [TestMethod]
        public void AsTesteObjectStringSucesso()
        {
            object teste = 5;
            object resultado = teste.As(como: typeof(string));
            Assert.AreEqual("5", resultado);
        }

        [TestMethod]
        public void AsTesteObjectClassSucesso()
        {
            object teste = new ClasseHeranca();
            object resultado = teste.As(como: typeof(ClasseAjudaAtributoTeste));
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void AsTesteObjectEnumSucesso()
        {
            object teste = 1;
            object resultado = teste.As(como: typeof(EnumSimplesTeste));
            Assert.IsNotNull(resultado);
            Assert.AreEqual(EnumSimplesTeste.conc_2, resultado);
        }

        [TestMethod]
        public void AsTesteObjectClassRedundanciaSucesso()
        {
            object teste = new ClasseHeranca();
            object resultado = teste.As(como: typeof(ClasseHeranca));
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void AsTesteObjectInterfaceSucesso()
        {
            object teste = new ClasseHeranca();
            object resultado = teste.As(como: typeof(IInterfaceTeste));
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void AsTesteObjectInterfaceInterfaceSucesso()
        {
            object teste = new ClasseHeranca();
            object resultado = teste.As(como: typeof(IInterfaceDeInterface));
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void AsTesteObjectClasseResultadoNuloSucesso()
        {
            object teste = typeof(ClasseHeranca);

            Assert.IsNull(teste.As(como: typeof(ClasseAjudaAtributoTeste)));
        }

        [TestMethod]
        public void IsNullOrDefaultTesteStringTrueSucesso()
        {
            Assert.IsTrue(default(string).IsNullOrDefault());
        }

        [TestMethod]
        public void IsNullOrDefaultTesteStringFalseSucesso()
        {
            Assert.IsFalse("teste".IsNullOrDefault());
        }

        [TestMethod]
        public void IsDefaultTesteStringTrueSucesso()
        {
            Assert.IsTrue(default(StructTesteCast).IsDefault());
        }

        [TestMethod]
        public void IsDefaultTesteStringFalseSucesso()
        {
            Assert.IsFalse("teste".IsDefault());
        }

        [TestMethod]
        public void IsNotNullTesteObjectTrueSucesso()
        {
            object teste = new ClasseHeranca();
            Assert.IsTrue(teste.IsNotNull());
        }

        [TestMethod]
        public void IsNotNullTesteObjectFalseSucesso()
        {
            object teste = null;
            Assert.IsFalse(teste.IsNotNull());
        }

        [TestMethod]
        public void IsNullTesteObjectFalseSucesso()
        {
            object teste = new ClasseHeranca();
            Assert.IsFalse(teste.IsNull());
        }

        [TestMethod]
        public void IsNullTesteObjectTrueSucesso()
        {
            object teste = null;
            Assert.IsTrue(teste.IsNull());
        }

        [TestMethod]
        public void HashTesteSucesso()
        {
            Assert.IsNotNull(new ClasseParaSerializacaoTeste().Hash());
        }

        [TestMethod]
        public void ObterInterfacesTesteObjectSucesso()
        {
            object teste = new ClasseHeranca();
            IEnumerable<Type> resultado = teste.ObterInterfaces();
            Assert.IsNotNull(resultado);
            Assert.IsTrue(resultado.Any());
        }

        [TestMethod]
        public void ObterMetodosTesteObjectSucesso()
        {
            object teste = new ClasseHeranca();
            IEnumerable<System.Reflection.MethodInfo> resultado = teste.ObterMetodos();
            Assert.IsNotNull(resultado);
            Assert.IsTrue(resultado.Any());
        }

        [TestMethod]
        public void ObterMetodosTesteObjectNomeSucesso()
        {
            object teste = new ClasseHeranca();
            IEnumerable<System.Reflection.MethodInfo> resultado = teste.ObterMetodos("Teste");
            Assert.IsNotNull(resultado);
            Assert.IsTrue(resultado.Any());
        }

        [TestMethod]
        public void ObterMetodoTesteObjectSemParametroSucesso()
        {
            ClasseHeranca teste = new ClasseHeranca();
            System.Reflection.MethodInfo resultado = teste.ObterMetodo("TesteSemRetorno");
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void ObterMetodoTesteObjectComParametroSucesso()
        {
            ClasseHeranca teste = new ClasseHeranca();
            System.Reflection.MethodInfo resultado = teste.ObterMetodo("TesteMetodoParametroTipoDiferentes", typeof(int), typeof(string), typeof(int));
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void InvocarMetodoTesteSucesso()
        {
            ClasseHeranca teste = new ClasseHeranca();
            object resultado = teste.InvocarMetodo("TesteMetodoRetorno", 1);
            Assert.IsNotNull(resultado);
            Assert.AreEqual(1, resultado);
        }

        [TestMethod]
        public void ObterParametrosMetodoTesteSucesso()
        {
            ClasseHeranca teste = new ClasseHeranca();
            Type[] resultado = teste.ObterParametrosMetodo("TesteMetodoRetorno");
            Assert.IsNotNull(resultado);
            Assert.AreEqual(1, resultado.Count());
        }

        [TestMethod]
        public void ExisteMetodo_Sucesso()
        {
            ClasseHeranca teste = new ClasseHeranca();
            bool resultado = teste.ExisteMetodo("TesteMetodoRetorno");
            Assert.IsTrue(resultado);
        }

        //----

        [TestMethod]
        public void SerializarTesteParametroNuloFalha()
        {
            ClasseParaSerializacaoTeste teste = default;
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                teste.Serializar();
            });
        }

        [TestMethod]
        public void SerializarTesteClasseNaoSerializavelFalha()
        {
            ClasseNaoSerializavelTeste teste = new ClasseNaoSerializavelTeste();
            Assert.ThrowsException<SerializationException>(() =>
            {
                teste.Serializar();
            });
        }

        [TestMethod]
        public void HerdadoTesteObjectTypeParametroNuloComparacaoFalha()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                new ClasseHeranca().Herdado(default);
            });
        }

        [TestMethod]
        public void HerdadoTesteObjectTypeParametroNuloObjFalha()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                default(ClasseHeranca).Herdado(default);
            });
        }

        [TestMethod]
        public void HerdadoTesteObjectGenericoParametroNuloObjFalha()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                default(string).Herdado<ClasseNaoSerializavelTeste>();
            });
        }

        [TestMethod]
        public void HerdadoTesteObjectGenericoParametroVazioObjFalha()
        {
            Assert.IsFalse(string.Empty.Herdado<ClasseNaoSerializavelTeste>());
        }

        [TestMethod]
        public void HerdadoTesteObjectGenericoStringObjFalha()
        {
            Assert.IsFalse("teste".Herdado<ClasseNaoSerializavelTeste>());
        }

        [TestMethod]
        public void HerdadoTesteObjectGenericoIntObjFalha()
        {
            Assert.IsFalse(01.Herdado<ClasseNaoSerializavelTeste>());
        }

        [TestMethod]
        public void ToTesteClasseClasseGenericoParametroNuloObjFalha()
        {
            ClasseHeranca teste = default;
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                ClasseAjudaAtributoTeste resultado = teste.To<ClasseAjudaAtributoTeste>();
            });
        }

        [TestMethod]
        public void ToTesteClasseClasseRedundanciaGenericoParametroNuloObjFalha()
        {
            ClasseHeranca teste = default;
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                ClasseHeranca resultado = teste.To<ClasseHeranca>();
            });
        }

        [TestMethod]
        public void ToTesteIntEnumGenericoParametroNuloObjFalha()
        {
            int? teste = default;
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                EnumSimplesTeste resultado = teste.To<EnumSimplesTeste>();
            });
        }

        [TestMethod]
        public void ToTesteFloatIntGenericoParametroNuloObjFalha()
        {
            float? teste = default;
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                int resultado = teste.To<int>();
            });
        }

        [TestMethod]
        public void ToTesteIntFloatGenericoParametroNuloObjFalha()
        {
            int? teste = default;
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                float resultado = teste.To<float>();
            });
        }

        [TestMethod]
        public void ToTesteClasseInterfaceInterfaceGenericoParametroNuloObjFalha()
        {
            ClasseHeranca teste = default;
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                IInterfaceDeInterface resultado = teste.To<IInterfaceDeInterface>();
            });
        }

        [TestMethod]
        public void ToTesteClasseInterfaceGenericoParametroNuloObjFalha()
        {
            ClasseHeranca teste = default;
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                IInterfaceTeste resultado = teste.To<IInterfaceTeste>();
            });
        }

        [TestMethod]
        public void ToTesteInterfaceStructGenericoParametroNuloObjFalha()
        {
            IInterfaceTeste teste = default;
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                StructTesteCast resultado = teste.To<StructTesteCast>();
            });
        }

        [TestMethod]
        public void ToTesteStructInterfaceGenericoParametroNuloObjFalha()
        {
            StructTesteCast? teste = default;
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                IInterfaceTeste resultado = teste.To<IInterfaceTeste>();
            });
        }

        [TestMethod]
        public void ToTesteIntStringGenericoParametroNuloObjFalha()
        {
            int? teste = default;
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                string resultado = teste.To<string>();
            });
        }

        [TestMethod]
        public void ToTesteClasseClasseTypeParametroNuloObjFalha()
        {
            ClasseHeranca teste = default;
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                object resultado = teste.To(typeof(ClasseAjudaAtributoTeste));
            });
        }

        [TestMethod]
        public void ToTesteClasseClasseRedundanciaTypeParametroNuloObjFalha()
        {
            ClasseHeranca teste = default;
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                object resultado = teste.To(typeof(ClasseHeranca));
            });
        }

        [TestMethod]
        public void ToTesteIntEnumTypeParametroNuloObjFalha()
        {
            int? teste = default;
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                object resultado = teste.To(typeof(EnumSimplesTeste));
            });
        }

        [TestMethod]
        public void ToTesteFloatIntTypeParametroNuloObjFalha()
        {
            float? teste = default;
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                object resultado = teste.To(typeof(int));
            });
        }

        [TestMethod]
        public void ToTesteIntFloatTypeParametroNuloObjFalha()
        {
            int? teste = default;
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                object resultado = teste.To(typeof(float));
            });
        }

        [TestMethod]
        public void ToTesteClasseInterfaceInterfaceTypeParametroNuloObjFalha()
        {
            ClasseHeranca teste = default;
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                object resultado = teste.To(typeof(IInterfaceDeInterface));
            });
        }

        [TestMethod]
        public void ToTesteClasseInterfaceTypeParametroNuloObjFalha()
        {
            ClasseHeranca teste = default;
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                object resultado = teste.To(typeof(IInterfaceTeste));
            });
        }

        [TestMethod]
        public void ToTesteInterfaceStructTypeParametroNuloObjFalha()
        {
            IInterfaceTeste teste = default;
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                object resultado = teste.To(typeof(StructTesteCast));
            });
        }

        [TestMethod]
        public void ToTesteStructInterfaceTypeParametroNuloObjFalha()
        {
            StructTesteCast? teste = default;
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                object resultado = teste.To(typeof(IInterfaceTeste));
            });
        }

        [TestMethod]
        public void ToTesteIntStringTypeParametroNuloObjFalha()
        {
            int? teste = default;
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                object resultado = teste.To(typeof(string));
            });
        }

        [TestMethod]
        public void ToTesteClasseTypeParametroNuloParaFalha()
        {
            ClasseHeranca teste = new ClasseHeranca { Propriedade = "5" };
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                object resultado = teste.To(null);
            });
        }

        [TestMethod]
        public void ToTesteIntTypeParametroNuloParaFalha()
        {
            int teste = 5;
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                object resultado = teste.To(null);
            });
        }

        [TestMethod]
        public void ToTesteFloatTypeParametroNuloParaFalha()
        {
            float teste = 5f;
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                object resultado = teste.To(null);
            });
        }

        [TestMethod]
        public void ToTesteInterfaceTypeParametroNuloParaFalha()
        {
            IInterfaceTeste teste = new StructTesteCast { Propriedade = "5" };
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                object resultado = teste.To(null);
            });
        }

        [TestMethod]
        public void ToTesteStructTypeParametroNuloParaFalha()
        {
            StructTesteCast teste = new StructTesteCast { Propriedade = "5" };
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                object resultado = teste.To(null);
            });
        }

        [TestMethod]
        public void AsTesteObjectClasseParametroNuloObjFalha()
        {
            object teste = null;
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                teste.As(como: typeof(ClasseHeranca));
            });
        }

        [TestMethod]
        public void AsTesteObjectClasseParametroNuloComoFalha()
        {
            object teste = typeof(ClasseHeranca);
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                teste.As(como: null);
            });
        }

        [TestMethod]
        public void HashTesteObjetoParametroNuloFalha()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                default(ClasseParaSerializacaoTeste).Hash();
            });
        }
    }
}