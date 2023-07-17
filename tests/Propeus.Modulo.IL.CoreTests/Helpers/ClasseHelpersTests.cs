using Microsoft.VisualStudio.TestTools.UnitTesting;

using Propeus.Modulo.Abstrato.Attributes;
using Propeus.Modulo.IL.Core.Geradores;
using Propeus.Modulo.IL.Core.Helpers;

namespace Propeus.Modulo.IL.CoreTests.Helpers
{
    public interface ITeste
    {

        int testeProp { get; }
        int testeProp2 { get; set; }

        int TesteMetodo();
        int TesteMetodo2(int a);
    }

    [Module]
    public class Teste
    {

        public Teste()
        {

        }

        public Teste(int testecmp, int testeProp, int testeProp2)
        {
            Testecmp = testecmp;
            this.testeProp = testeProp;
            this.testeProp2 = testeProp2;
        }

        public int Testecmp;

        public int this[int valor]
        {
            get => valor;
            set => valor = value;
        }
        public int testeProp { get; }
        public int testeProp2 { get; set; }
        public int testeProp3 { get; }
        public int testeProp4 { get; set; }
        public int TesteMetodo()
        {
            return 0;
        }
        public int TesteMetodo2(int a)
        {
            return a;
        }
        public int TesteMetodo3(int a, int b, int c, int d, int e, int f)
        {
            return a + b + c + d + e + f;
        }

        public int TesteMetodo4(int a = 10, int b = 20)
        {
            return a + b;
        }

        public object TesteMetodo5(object a = null)
        {
            return a;
        }

    }

    [TestClass()]
    public class ClasseHelpersTests
    {
        [TestMethod()]
        public void CriarProxyClasseTest()
        {
            ILClasseProvider? Proxy = GeradorHelper.Modulo.CriarProxyClasse(typeof(Teste));
            Proxy.Executar();
            GeradorHelper.Modulo.Executar();
            Type tpProxy = Proxy.ObterTipoGerado();
            Assert.IsNotNull(tpProxy);
            Assert.IsNotNull(Activator.CreateInstance(tpProxy));
            Assert.IsNotNull(Proxy.ToString());
            Assert.IsNotNull(GeradorHelper.Modulo.ToString());
            Assert.IsNotNull(Proxy);
            for (int i = 0; i < 20; i++)
            {
                Proxy = Proxy.NovaVersao("Teste.NovaVersao.Namespace", typeof(object), new Type[] { typeof(ITeste) }, new Core.Enums.Token[] { Core.Enums.Token.Publico }).CriarProxyClasse(typeof(Teste));
                Assert.IsNotNull(Proxy);
                Proxy.Executar();
                tpProxy = Proxy.ObterTipoGerado();
                Assert.IsNotNull(tpProxy);
            }

            Proxy.Executar();
            Proxy.Dispose();
            Assert.AreEqual(string.Empty, Proxy.ToString());
            Assert.ThrowsException<ObjectDisposedException>(() => { Proxy.NovaVersao("Teste.NovaVersao.Namespace", typeof(object), new Type[] { typeof(ITeste) }).CriarProxyClasse(typeof(Teste)); });
            Assert.ThrowsException<ObjectDisposedException>(() => { Proxy.Executar(); });
            Assert.ThrowsException<ObjectDisposedException>(() => { Proxy.NovaVersao(); });
            Geradores.ILModulo modulo = GeradorHelper.Modulo;
            modulo.Dispose();
            Assert.AreEqual(string.Empty, modulo.ToString());
        }

        [TestMethod()]
        public void CriarProxyClasseTest2()
        {

            ILClasseProvider Proxy = GeradorHelper.Modulo.CriarProxyClasse(typeof(Teste), new Type[] { typeof(ITeste) });
            Proxy.Executar();
            Type tpProxy = Proxy.ObterTipoGerado();
            Assert.IsNotNull(tpProxy);
            Assert.IsNotNull(Activator.CreateInstance(tpProxy));
        }

        [TestCleanup()]
        public void End()
        {
            GeradorHelper.Gerador.Dispose();
        }
    }
}