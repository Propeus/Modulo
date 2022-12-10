using Microsoft.VisualStudio.TestTools.UnitTesting;
using Propeus.Modulo.IL.Proxy;
using Propeus.Modulo.IL.Reservados;
using Propeus.Modulo.IL.Teste.Interfaces;
using Propeus.Modulo.IL.Teste.Modelos;
using Propeus.Modulo.IL.Token.Condicional;
using Propeus.Modulo.IL.Token.Especial;
using Propeus.Modulo.IL.Token.Variaveis;
using Propeus.Modulo.IL.Util;
using Propeus.Modulo.Util.Objetos;
using System.Linq;

namespace Propeus.Modulo.IL.Teste
{
    [TestCategory("Propeus.NetCore.IL")]
    [TestClass]
    public class Teste
    {
        private ILClasse iLMaker;
        private ILGerador iLGerador;
        private readonly string NomeClasseTeste = "Classe_Teste";
        private readonly string NomeClasseHerdadoTeste = "Classe_Teste_Herdado";

        public void InicializarIL()
        {
            iLGerador = new ILGerador();
        }

        [TestMethod]
        public void CriarDynamicProxy_ClasseTesteSemConstrutorSemInterface()
        {
            IClasseComInterface dyp = typeof(ClasseTesteSemConstrutorSemInterface).DynamicProxy<IClasseComInterface>();
            Assert.IsNotNull(dyp);
        }

        [TestMethod]
        public void CriarDynamicProxy_ClasseTesteComConstrutorSemInterface()
        {
            IClasseComInterface dyp = typeof(ClasseTesteComConstrutorSemInterface).DynamicProxy<IClasseComInterface>("OK!");
            Assert.IsNotNull(dyp);
        }

        [TestMethod]
        public void CriarDynamicProxy_ClasseTesteSemConstrutorComInterface()
        {
            IClasseComInterfaceNaoAssociado dyp = typeof(ClasseTesteSemConstrutorComInterface).DynamicProxy<IClasseComInterfaceNaoAssociado>();
            Assert.IsNotNull(dyp);
        }

        [TestMethod]
        public void CriarDynamicProxy_ClasseTesteComConstrutorComInterface()
        {
            IClasseComInterfaceNaoAssociado dyp = typeof(ClasseTesteComConstrutorComInterface).DynamicProxy<IClasseComInterfaceNaoAssociado>("Ok!");
            Assert.IsNotNull(dyp);
            Assert.IsNull(dyp["teste"]);
        }

  
        [TestMethod]
        public void CriarTipoHerdado_ClasseTesteSemConstrutorSemInterface()
        {
            InicializarIL();
            iLMaker = new ILClasse(iLGerador, NomeClasseHerdadoTeste, typeof(ClasseTesteSemConstrutorSemInterface));
            ILMetodo mth = iLMaker.ImplementarInterfaces(typeof(IClasseComInterface)).First().Metodos.First();

            mth.CriarVariavel("Var_Str", "Hello World!");
            mth.Retornar();

            IClasseComInterface obj = iLGerador.ObterInstancia<IClasseComInterface>(NomeClasseHerdadoTeste);
            Assert.IsNotNull(obj);
            string result = obj.Teste();
            Assert.IsNotNull(result);
            object tobj = obj;
            Assert.IsTrue(tobj.Herdado<ClasseTesteSemConstrutorSemInterface>());
            Assert.IsTrue(tobj.Herdado<IClasseComInterface>());
        }

        [TestMethod]
        public void CriarTipoHerdado_ClasseTesteComConstrutorSemInterface()
        {
            InicializarIL();
            iLMaker = new ILClasse(iLGerador, NomeClasseHerdadoTeste, typeof(ClasseTesteComConstrutorSemInterface));
            ILMetodo mth = iLMaker.ImplementarInterfaces(typeof(IClasseComInterface)).First().Metodos[0];

            mth.CriarVariavel("Var_Str", "Hello World!");
            mth.Retornar();

            IClasseComInterface obj = iLGerador.ObterInstancia<IClasseComInterface>(NomeClasseHerdadoTeste, "Ok!");
            Assert.IsNotNull(obj);
            string result = obj.Teste();
            Assert.IsNotNull(result);
            object tobj = obj;
            Assert.IsTrue(tobj.Herdado<ClasseTesteComConstrutorSemInterface>());
            Assert.IsTrue(tobj.Herdado<IClasseComInterface>());
        }

        [TestMethod]
        public void CriarTipoHerdado_ClasseTesteSemConstrutorComInterface()
        {
            InicializarIL();
            iLMaker = new ILClasse(iLGerador, NomeClasseHerdadoTeste, typeof(ClasseTesteSemConstrutorComInterface));
            iLMaker.ImplementarInterfaces(typeof(IClasseComInterfaceNaoAssociado));

            IClasseComInterfaceNaoAssociado obj = iLGerador.ObterInstancia<IClasseComInterfaceNaoAssociado>(NomeClasseHerdadoTeste);
            Assert.IsNotNull(obj);
            string result = obj.Teste();
            Assert.IsNotNull(result);
            Assert.IsTrue(obj.Herdado<ClasseTesteSemConstrutorComInterface>());
            Assert.IsTrue(obj.Herdado<IClasseComInterfaceNaoAssociado>());
            Assert.IsTrue(obj.Herdado<IClasseComInterface>());
        }

        [TestMethod]
        public void CriarTipoHerdado_ClasseTesteComConstrutorComInterface()
        {
            InicializarIL();
            iLMaker = new ILClasse(iLGerador, NomeClasseHerdadoTeste, typeof(ClasseTesteComConstrutorComInterface));
            iLMaker.ImplementarInterfaces(typeof(IClasseComInterfaceNaoAssociado));

            IClasseComInterfaceNaoAssociado obj = iLGerador.ObterInstancia<IClasseComInterfaceNaoAssociado>(NomeClasseHerdadoTeste, "Ok");
            Assert.IsNotNull(obj);
            string result = obj.Teste();
            Assert.IsNotNull(result);
            Assert.IsTrue(obj.Herdado<ClasseTesteComConstrutorComInterface>());
            Assert.IsTrue(obj.Herdado<IClasseComInterfaceNaoAssociado>());
            Assert.IsTrue(obj.Herdado<IClasseComInterface>());
        }

        [TestMethod]
        public void CriarTipoDinamico()
        {
            InicializarIL();
            iLMaker = iLGerador.GerarClasse(NomeClasseTeste);

            dynamic obj = iLGerador.ObterInstancia(NomeClasseTeste);
            Assert.IsNotNull(obj);
        }

        [TestMethod]
        public void CriarMetodoRetorno()
        {
            InicializarIL();
            iLMaker = iLGerador.GerarClasse(NomeClasseTeste);

            ILMetodo mth = iLMaker.GerarMetodoPublicoSemParametros("TesteMetodo", typeof(int));
            ILVariavel TesteVar = mth.CriarVariavel("TesteVar", 1);
            mth.ArmazenarVariavel(TesteVar);
            mth.CarregarVariavel(TesteVar);
            mth.Retornar();

            dynamic t = iLGerador.ObterInstancia(NomeClasseTeste);
            int valor = t.TesteMetodo();
            Assert.AreEqual(1, valor);
        }

        [TestMethod]
        public void CriarMetodoVoid()
        {
            InicializarIL();
            iLMaker = iLGerador.GerarClasse(NomeClasseTeste);

            ILMetodo mth = iLMaker.GerarMetodoPublicoSemRetornoSemParametros("TesteMetodo");
            mth.Retornar();

            dynamic t = iLGerador.ObterInstancia(NomeClasseTeste);
            t.TesteMetodo();
        }

        [TestMethod]
        public void CriarMetodoRetornoSoma()
        {
            InicializarIL();
            iLMaker = iLGerador.GerarClasse(NomeClasseTeste);

            ILMetodo mth = iLMaker.GerarMetodoPublicoSemParametros("TesteMetodo", typeof(int));
            mth.CriarVariavel("Var_1", 5);
            mth.CriarVariavel("Var_2", 5);
            mth.Soma();
            mth.Retornar();

            dynamic t = iLGerador.ObterInstancia(NomeClasseTeste);
            int valor = t.TesteMetodo();
            Assert.AreEqual(10, valor);
        }

        [TestMethod]
        public void CriarMetodoRetornoDivisao()
        {
            InicializarIL();
            iLMaker = iLGerador.GerarClasse(NomeClasseTeste);
            ILMetodo mth = iLMaker.GerarMetodoPublicoSemParametros("TesteMetodo", typeof(int));
            mth.CriarVariavel("Var_1", 5);
            mth.CriarVariavel("Var_2", 5);
            mth.Divisao();
            mth.Retornar();

            dynamic t = iLGerador.ObterInstancia(NomeClasseTeste);
            int valor = t.TesteMetodo();
            Assert.AreEqual(1, valor);
        }

        [TestMethod]
        public void CriarMetodoRetornoMultiplicacao()
        {
            InicializarIL();
            iLMaker = iLGerador.GerarClasse(NomeClasseTeste);

            ILMetodo mth = iLMaker.GerarMetodoPublicoSemParametros("TesteMetodo", typeof(int));
            mth.CriarVariavel("Var_1", 5);
            mth.CriarVariavel("Var_2", 5);
            mth.Multiplicacao();
            mth.Retornar();

            dynamic t = iLGerador.ObterInstancia(NomeClasseTeste);
            int valor = t.TesteMetodo();
            Assert.AreEqual(25, valor);
        }

        [TestMethod]
        public void CriarMetodoRetornoSubtracao()
        {
            InicializarIL();
            iLMaker = iLGerador.GerarClasse(NomeClasseTeste);

            ILMetodo mth = iLMaker.GerarMetodoPublicoSemParametros("TesteMetodo", typeof(int));
            mth.CriarVariavel("Var_1", 5);
            mth.CriarVariavel("Var_2", 5);
            mth.Subtracao();
            mth.Retornar();

            dynamic t = iLGerador.ObterInstancia(NomeClasseTeste);
            int valor = t.TesteMetodo();
            Assert.AreEqual(0, valor);
        }

        [TestMethod]
        public void TesteCondicionalSeSenaoSeMenorQueFalso()
        {
            InicializarIL();
            iLMaker = iLGerador.GerarClasse(NomeClasseTeste);

            ILMetodo mth = iLMaker.GerarMetodoPublicoSemParametros("TesteMetodo", typeof(int));
            ILVariavel v1 = mth.CriarVariavel("Var_1", 3);
            mth.ArmazenarVariavel(v1);
            ILVariavel v2 = mth.CriarVariavel("Var_2", 2);
            mth.ArmazenarVariavel(v2);
            mth.CarregarVariavel(v1);
            mth.CarregarVariavel(v2);
            _ = mth.Se();
            ILMaiorQue cond = mth.MaiorQue();
            mth.CarregarVariavel(v1);
            mth.Retornar();
            mth.Senao(cond);
            mth.CarregarVariavel(v1);
            mth.CarregarVariavel(v2);
            mth.Se();
            ILMenorQue cond2 = mth.MenorQue();
            mth.CarregarVariavel(v2);
            mth.Retornar();
            mth.MarcarPulo(cond2);
            mth.CriarVariavel("Var_3", 0);
            mth.Retornar();

            dynamic t = iLGerador.ObterInstancia(NomeClasseTeste);
            int valor = t.TesteMetodo();
            Assert.AreEqual(3, valor);
        }

        [TestMethod]
        public void TesteCondicionalSeSenaoSeMenorQueVerdadeiro()
        {
            InicializarIL();
            iLMaker = iLGerador.GerarClasse(NomeClasseTeste);

            ILMetodo mth = iLMaker.GerarMetodoPublicoSemParametros("TesteMetodo", typeof(int));
            ILVariavel v1 = mth.CriarVariavel("Var_1", 1);
            mth.ArmazenarVariavel(v1);
            ILVariavel v2 = mth.CriarVariavel("Var_2", 2);
            mth.ArmazenarVariavel(v2);
            mth.CarregarVariavel(v1);
            mth.CarregarVariavel(v2);
            _ = mth.Se();
            ILMaiorQue cond = mth.MaiorQue();
            mth.CarregarVariavel(v1);
            mth.Retornar();
            mth.Senao(cond);
            mth.CarregarVariavel(v1);
            mth.CarregarVariavel(v2);
            mth.Se();
            ILMenorQue cond2 = mth.MenorQue();
            mth.CarregarVariavel(v2);
            mth.Retornar();
            mth.MarcarPulo(cond2);
            mth.CriarVariavel("Var_3", 0);
            mth.Retornar();

            dynamic t = iLGerador.ObterInstancia(NomeClasseTeste);
            int valor = t.TesteMetodo();
            Assert.AreEqual(2, valor);
        }

        [TestMethod]
        public void TesteCondicionalSeMenorQueFalso()
        {
            InicializarIL();
            iLMaker = iLGerador.GerarClasse(NomeClasseTeste);

            ILMetodo mth = iLMaker.GerarMetodoPublicoSemParametros("TesteMetodo", typeof(int));
            ILVariavel v1 = mth.CriarVariavel("Var_1", 3);
            mth.ArmazenarVariavel(v1);
            ILVariavel v2 = mth.CriarVariavel("Var_2", 2);
            mth.ArmazenarVariavel(v2);
            mth.CarregarVariavel(v1);
            mth.CarregarVariavel(v2);
            _ = mth.Se();
            ILMenorQue cond = mth.MenorQue();
            mth.CarregarVariavel(v1);
            mth.Retornar();
            mth.MarcarPulo(cond);
            mth.CarregarVariavel(v2);
            mth.Retornar();

            dynamic t = iLGerador.ObterInstancia(NomeClasseTeste);
            int valor = t.TesteMetodo();
            Assert.AreEqual(2, valor);
        }

        [TestMethod]
        public void TesteCondicionalSeMenorQueVerdadeiro()
        {
            InicializarIL();
            iLMaker = iLGerador.GerarClasse(NomeClasseTeste);

            ILMetodo mth = iLMaker.GerarMetodoPublicoSemParametros("TesteMetodo", typeof(int));
            ILVariavel v1 = mth.CriarVariavel("Var_1", 1);
            mth.ArmazenarVariavel(v1);
            ILVariavel v2 = mth.CriarVariavel("Var_2", 2);
            mth.ArmazenarVariavel(v2);
            mth.CarregarVariavel(v1);
            mth.CarregarVariavel(v2);
            _ = mth.Se();
            ILMenorQue cond = mth.MenorQue();
            mth.CarregarVariavel(v1);
            mth.Retornar();
            mth.MarcarPulo(cond);
            mth.CarregarVariavel(v2);
            mth.Retornar();

            dynamic t = iLGerador.ObterInstancia(NomeClasseTeste);
            int valor = t.TesteMetodo();
            Assert.AreEqual(1, valor);
        }

        [TestMethod]
        public void TesteCondicionalSeSenaoMenorQueFalso()
        {
            InicializarIL();
            iLMaker = iLGerador.GerarClasse(NomeClasseTeste);

            ILMetodo mth = iLMaker.GerarMetodoPublicoSemParametros("TesteMetodo", typeof(int));
            ILVariavel v1 = mth.CriarVariavel("Var_1", 3);
            mth.ArmazenarVariavel(v1);
            ILVariavel v2 = mth.CriarVariavel("Var_2", 2);
            mth.ArmazenarVariavel(v2);
            mth.CarregarVariavel(v1);
            mth.CarregarVariavel(v2);
            _ = mth.Se();
            ILMenorQue cond = mth.MenorQue();
            mth.CarregarVariavel(v1);
            mth.Retornar();
            mth.Senao(cond);
            mth.CarregarVariavel(v2);
            mth.Retornar();
            mth.CriarVariavel("Var_3", 0);
            mth.Retornar();

            dynamic t = iLGerador.ObterInstancia(NomeClasseTeste);
            int valor = t.TesteMetodo();
            Assert.AreEqual(2, valor);
        }

        [TestMethod]
        public void TesteCondicionalSeSenaoMenorQueVerdadeiro()
        {
            InicializarIL();
            iLMaker = iLGerador.GerarClasse(NomeClasseTeste);

            ILMetodo mth = iLMaker.GerarMetodoPublicoSemParametros("TesteMetodo", typeof(int));
            ILVariavel v1 = mth.CriarVariavel("Var_1", 1);
            mth.ArmazenarVariavel(v1);
            ILVariavel v2 = mth.CriarVariavel("Var_2", 2);
            mth.ArmazenarVariavel(v2);
            mth.CarregarVariavel(v1);
            mth.CarregarVariavel(v2);
            _ = mth.Se();
            ILMenorQue cond = mth.MenorQue();
            mth.CarregarVariavel(v1);
            mth.Retornar();
            mth.Senao(cond);
            mth.CarregarVariavel(v2);
            mth.Retornar();
            mth.CriarVariavel("Var_3", 0);
            mth.Retornar();

            dynamic t = iLGerador.ObterInstancia(NomeClasseTeste);
            int valor = t.TesteMetodo();
            Assert.AreEqual(1, valor);
        }

        [TestMethod]
        public void TesteCondicionalSeSenaoMaiorQueFalso()
        {
            InicializarIL();
            iLMaker = iLGerador.GerarClasse(NomeClasseTeste);

            ILMetodo mth = iLMaker.GerarMetodoPublicoSemParametros("TesteMetodo", typeof(int));
            ILVariavel v1 = mth.CriarVariavel("Var_1", 2);
            mth.ArmazenarVariavel(v1);
            ILVariavel v2 = mth.CriarVariavel("Var_2", 3);
            mth.ArmazenarVariavel(v2);
            mth.CarregarVariavel(v1);
            mth.CarregarVariavel(v2);
            _ = mth.Se();
            ILMaiorQue cond = mth.MaiorQue();
            mth.CarregarVariavel(v1);
            mth.Retornar();
            mth.Senao(cond);
            mth.CarregarVariavel(v2);
            mth.Retornar();
            mth.CriarVariavel("Var_3", 0);
            mth.Retornar();

            dynamic t = iLGerador.ObterInstancia(NomeClasseTeste);
            int valor = t.TesteMetodo();
            Assert.AreEqual(3, valor);
        }

        [TestMethod]
        public void TesteCondicionalSeSenaoMaiorQueVerdadeiro()
        {
            InicializarIL();
            iLMaker = iLGerador.GerarClasse(NomeClasseTeste);

            ILMetodo mth = iLMaker.GerarMetodoPublicoSemParametros("TesteMetodo", typeof(int));
            ILVariavel v1 = mth.CriarVariavel("Var_1", 3);
            mth.ArmazenarVariavel(v1);
            ILVariavel v2 = mth.CriarVariavel("Var_2", 2);
            mth.ArmazenarVariavel(v2);
            mth.CarregarVariavel(v1);
            mth.CarregarVariavel(v2);
            _ = mth.Se();
            ILMaiorQue cond = mth.MaiorQue();
            mth.CarregarVariavel(v1);
            mth.Retornar();
            mth.Senao(cond);
            mth.CarregarVariavel(v2);
            mth.Retornar();
            mth.CriarVariavel("Var_3", 0);
            mth.Retornar();

            dynamic t = iLGerador.ObterInstancia(NomeClasseTeste);
            int valor = t.TesteMetodo();
            Assert.AreEqual(3, valor);
        }

        [TestMethod]
        public void TesteCondicionalSeMaiorQueFalso()
        {
            InicializarIL();
            iLMaker = iLGerador.GerarClasse(NomeClasseTeste);

            ILMetodo mth = iLMaker.GerarMetodoPublicoSemParametros("TesteMetodo", typeof(int));
            ILVariavel v1 = mth.CriarVariavel("Var_1", 1);
            mth.ArmazenarVariavel(v1);
            ILVariavel v2 = mth.CriarVariavel("Var_2", 2);
            mth.ArmazenarVariavel(v2);
            mth.CarregarVariavel(v1);
            mth.CarregarVariavel(v2);
            _ = mth.Se();
            ILMaiorQue cond = mth.MaiorQue();
            mth.CarregarVariavel(v1);
            mth.Retornar();
            mth.MarcarPulo(cond);
            mth.CarregarVariavel(v2);
            mth.Retornar();

            dynamic t = iLGerador.ObterInstancia(NomeClasseTeste);
            int valor = t.TesteMetodo();
            Assert.AreEqual(2, valor);
        }

        [TestMethod]
        public void TesteCondicionalSeMaiorQueVerdadeiro()
        {
            InicializarIL();
            iLMaker = iLGerador.GerarClasse(NomeClasseTeste);

            ILMetodo mth = iLMaker.GerarMetodoPublicoSemParametros("TesteMetodo", typeof(int));
            ILVariavel v1 = mth.CriarVariavel("Var_1", 3);
            mth.ArmazenarVariavel(v1);
            ILVariavel v2 = mth.CriarVariavel("Var_2", 2);
            mth.ArmazenarVariavel(v2);
            mth.CarregarVariavel(v1);
            mth.CarregarVariavel(v2);
            _ = mth.Se();
            ILMaiorQue cond = mth.MaiorQue();
            mth.CarregarVariavel(v1);
            mth.Retornar();
            mth.MarcarPulo(cond);
            mth.CarregarVariavel(v2);
            mth.Retornar();

            dynamic t = iLGerador.ObterInstancia(NomeClasseTeste);
            int valor = t.TesteMetodo();
            Assert.AreEqual(3, valor);
        }

        [TestMethod]
        public void TesteCondicionalSeSenaoSeMaiorQueFalso()
        {
            InicializarIL();
            iLMaker = iLGerador.GerarClasse(NomeClasseTeste);

            ILMetodo mth = iLMaker.GerarMetodoPublicoSemParametros("TesteMetodo", typeof(int));
            ILVariavel v1 = mth.CriarVariavel("Var_1", 1);
            mth.ArmazenarVariavel(v1);
            ILVariavel v2 = mth.CriarVariavel("Var_2", 2);
            mth.ArmazenarVariavel(v2);
            mth.CarregarVariavel(v1);
            mth.CarregarVariavel(v2);
            _ = mth.Se();
            ILMenorQue cond = mth.MenorQue();
            mth.CarregarVariavel(v2);
            mth.Retornar();
            mth.Senao(cond);
            mth.CarregarVariavel(v1);
            mth.CarregarVariavel(v2);
            mth.Se();
            ILMaiorQue cond2 = mth.MaiorQue();
            mth.CarregarVariavel(v1);
            mth.Retornar();
            mth.MarcarPulo(cond2);
            mth.CriarVariavel("Var_3", 0);
            mth.Retornar();

            dynamic t = iLGerador.ObterInstancia(NomeClasseTeste);
            int valor = t.TesteMetodo();
            Assert.AreEqual(2, valor);
        }

        [TestMethod]
        public void TesteCondicionalSeSenaoSeMaiorQueVerdadeiro()
        {
            InicializarIL();
            iLMaker = iLGerador.GerarClasse(NomeClasseTeste);

            ILMetodo mth = iLMaker.GerarMetodoPublicoSemParametros("TesteMetodo", typeof(int));
            ILVariavel v1 = mth.CriarVariavel("Var_1", 3);
            mth.ArmazenarVariavel(v1);
            ILVariavel v2 = mth.CriarVariavel("Var_2", 2);
            mth.ArmazenarVariavel(v2);
            mth.CarregarVariavel(v1);
            mth.CarregarVariavel(v2);
            _ = mth.Se();
            ILMenorQue cond = mth.MenorQue();
            mth.CarregarVariavel(v2);
            mth.Retornar();
            mth.Senao(cond);
            mth.CarregarVariavel(v1);
            mth.CarregarVariavel(v2);
            mth.Se();
            ILMaiorQue cond2 = mth.MaiorQue();
            mth.CarregarVariavel(v1);
            mth.Retornar();
            mth.MarcarPulo(cond2);
            mth.CriarVariavel("Var_3", 0);
            mth.Retornar();

            dynamic t = iLGerador.ObterInstancia(NomeClasseTeste);
            int valor = t.TesteMetodo();
            Assert.AreEqual(3, valor);
        }

        [TestMethod]
        public void TesteCondicionalSeSenaoIgualVerdadeiro()
        {
            InicializarIL();
            iLMaker = iLGerador.GerarClasse(NomeClasseTeste);

            ILMetodo mth = iLMaker.GerarMetodoPublicoSemParametros("TesteMetodo", typeof(int));
            ILVariavel v1 = mth.CriarVariavel("Var_1", 2);
            mth.ArmazenarVariavel(v1);
            ILVariavel v2 = mth.CriarVariavel("Var_2", 2);
            mth.ArmazenarVariavel(v2);
            mth.CarregarVariavel(v1);
            mth.CarregarVariavel(v2);
            _ = mth.Se();
            ILIgual cond = mth.Igual();
            mth.CarregarVariavel(v2);
            mth.Retornar();
            mth.Senao(cond);
            mth.CarregarVariavel(v1);
            mth.Retornar();
            mth.CriarVariavel("Var_3", 0);
            mth.Retornar();

            dynamic t = iLGerador.ObterInstancia(NomeClasseTeste);
            int valor = t.TesteMetodo();
            Assert.AreEqual(2, valor);
        }

        [TestMethod]
        public void TesteCondicionalSeSenaoIgualFalso()
        {
            InicializarIL();
            iLMaker = iLGerador.GerarClasse(NomeClasseTeste);

            ILMetodo mth = iLMaker.GerarMetodoPublicoSemParametros("TesteMetodo", typeof(int));
            ILVariavel v1 = mth.CriarVariavel("Var_1", 1);
            mth.ArmazenarVariavel(v1);
            ILVariavel v2 = mth.CriarVariavel("Var_2", 2);
            mth.ArmazenarVariavel(v2);
            mth.CarregarVariavel(v1);
            mth.CarregarVariavel(v2);
            _ = mth.Se();
            ILIgual cond = mth.Igual();
            mth.CarregarVariavel(v2);
            mth.Retornar();
            mth.Senao(cond);
            mth.CarregarVariavel(v1);
            mth.Retornar();
            mth.CriarVariavel("Var_3", 0);
            mth.Retornar();

            dynamic t = iLGerador.ObterInstancia(NomeClasseTeste);
            int valor = t.TesteMetodo();
            Assert.AreEqual(1, valor);
        }

        [TestMethod]
        public void TesteCondicionalSeSenaoSeDiferenteVerdadeiro()
        {
            InicializarIL();
            iLMaker = iLGerador.GerarClasse(NomeClasseTeste);

            ILMetodo mth = iLMaker.GerarMetodoPublicoSemParametros("TesteMetodo", typeof(int));
            ILVariavel v1 = mth.CriarVariavel("Var_1", 1);
            mth.ArmazenarVariavel(v1);
            ILVariavel v2 = mth.CriarVariavel("Var_2", 2);
            mth.ArmazenarVariavel(v2);
            mth.CarregarVariavel(v1);
            mth.CarregarVariavel(v2);
            _ = mth.Se();
            ILDiferente cond = mth.Diferente();
            mth.CarregarVariavel(v2);
            mth.Retornar();
            mth.Senao(cond);
            mth.CarregarVariavel(v1);
            mth.Retornar();
            mth.CriarVariavel("Var_3", 0);
            mth.Retornar();

            dynamic t = iLGerador.ObterInstancia(NomeClasseTeste);
            int valor = t.TesteMetodo();
            Assert.AreEqual(2, valor);
        }

        [TestMethod]
        public void TesteCondicionalSeSenaoSeDiferenteFalso()
        {
            InicializarIL();
            iLMaker = iLGerador.GerarClasse(NomeClasseTeste);

            ILMetodo mth = iLMaker.GerarMetodoPublicoSemParametros("TesteMetodo", typeof(int));
            ILVariavel v1 = mth.CriarVariavel("Var_1", 2);
            mth.ArmazenarVariavel(v1);
            ILVariavel v2 = mth.CriarVariavel("Var_2", 2);
            mth.ArmazenarVariavel(v2);
            mth.CarregarVariavel(v1);
            mth.CarregarVariavel(v2);
            _ = mth.Se();
            ILDiferente cond = mth.Diferente();
            mth.CarregarVariavel(v2);
            mth.Retornar();
            mth.Senao(cond);
            mth.CarregarVariavel(v1);
            mth.Retornar();
            mth.CriarVariavel("Var_3", 0);
            mth.Retornar();

            dynamic t = iLGerador.ObterInstancia(NomeClasseTeste);
            int valor = t.TesteMetodo();
            Assert.AreEqual(2, valor);
        }

        [TestMethod]
        public void TesteCondicionalRepeticaoEnquantoMenorQue()
        {
            InicializarIL();
            iLMaker = iLGerador.GerarClasse(NomeClasseTeste);

            ILMetodo mth = iLMaker.GerarMetodoPublicoSemParametros("TesteMetodo", typeof(int));
            ILVariavel v1 = mth.CriarVariavel("Var_1", 0);
            mth.ArmazenarVariavel(v1);
            ILVariavel v2 = mth.CriarVariavel("Var_2", 10);
            mth.ArmazenarVariavel(v2);
            ILIrPara jmp = mth.MarcarPulo();
            mth.CarregarVariavel(v1);
            mth.CarregarVariavel(v2);
            _ = mth.Enquanto();
            ILMenorQue mq = mth.MenorQue();
            mth.CarregarVariavel(v1);
            mth.CriarVariavel("Var_3", 1);
            mth.Soma();
            mth.ArmazenarVariavel(v1);
            mth.PularPara(jmp);
            mth.Senao(mq);
            mth.CarregarVariavel(v1);
            mth.Retornar();

            dynamic t = iLGerador.ObterInstancia(NomeClasseTeste);
            int valor = t.TesteMetodo();
            Assert.AreEqual(10, valor);
        }

        [TestMethod]
        public void TesteCondicionalRepeticaoEnquantoMaiorQue()
        {
            InicializarIL();
            iLMaker = iLGerador.GerarClasse(NomeClasseTeste);

            ILMetodo mth = iLMaker.GerarMetodoPublicoSemParametros("TesteMetodo", typeof(int));
            ILVariavel v1 = mth.CriarVariavel("Var_1", 10);
            mth.ArmazenarVariavel(v1);
            ILVariavel v2 = mth.CriarVariavel("Var_2", 0);
            mth.ArmazenarVariavel(v2);
            ILIrPara jmp = mth.MarcarPulo();
            mth.CarregarVariavel(v1);
            mth.CarregarVariavel(v2);
            _ = mth.Enquanto();
            ILMaiorQue mq = mth.MaiorQue();
            mth.CarregarVariavel(v1);
            mth.CriarVariavel("Var_3", 1);
            mth.Subtracao();
            mth.ArmazenarVariavel(v1);
            mth.PularPara(jmp);
            mth.Senao(mq);
            mth.CarregarVariavel(v1);
            mth.Retornar();

            dynamic t = iLGerador.ObterInstancia(NomeClasseTeste);
            int valor = t.TesteMetodo();
            Assert.AreEqual(0, valor);
        }

        [TestMethod]
        public void TesteCondicionalRepeticaoEnquantoDiferente()
        {
            InicializarIL();
            iLMaker = iLGerador.GerarClasse(NomeClasseTeste);

            ILMetodo mth = iLMaker.GerarMetodoPublicoSemParametros("TesteMetodo", typeof(int));
            ILVariavel v1 = mth.CriarVariavel("Var_1", 10);
            mth.ArmazenarVariavel(v1);
            ILVariavel v2 = mth.CriarVariavel("Var_2", 0);
            mth.ArmazenarVariavel(v2);
            ILIrPara jmp = mth.MarcarPulo();
            mth.CarregarVariavel(v1);
            mth.CarregarVariavel(v2);
            mth.Enquanto();
            ILDiferente mq = mth.Diferente();
            mth.CarregarVariavel(v1);
            mth.CriarVariavel("Var_3", 1);
            mth.Subtracao();
            mth.ArmazenarVariavel(v1);
            mth.PularPara(jmp);
            mth.Senao(mq);
            mth.CarregarVariavel(v1);
            mth.Retornar();

            dynamic t = iLGerador.ObterInstancia(NomeClasseTeste);
            int valor = t.TesteMetodo();
            Assert.AreEqual(0, valor);
        }

        [TestMethod]
        public void TesteCondicionalRepeticaoEnquantoIgual()
        {
            InicializarIL();
            iLMaker = iLGerador.GerarClasse(NomeClasseTeste);

            ILMetodo mth = iLMaker.GerarMetodoPublicoSemParametros("TesteMetodo", typeof(int));
            ILVariavel v1 = mth.CriarVariavel("Var_1", 0);
            mth.ArmazenarVariavel(v1);
            ILVariavel v2 = mth.CriarVariavel("Var_2", 0);
            mth.ArmazenarVariavel(v2);
            ILIrPara jmp = mth.MarcarPulo();
            mth.CarregarVariavel(v1);
            mth.CarregarVariavel(v2);
            _ = mth.Enquanto();
            ILIgual mq = mth.Igual();
            mth.CarregarVariavel(v1);
            mth.CriarVariavel("Var_3", 1);
            mth.Soma();
            mth.ArmazenarVariavel(v1);
            mth.PularPara(jmp);
            mth.Senao(mq);
            mth.CarregarVariavel(v1);
            mth.Retornar();

            dynamic t = iLGerador.ObterInstancia(NomeClasseTeste);
            int valor = t.TesteMetodo();
            Assert.AreEqual(1, valor);
        }
    }
}