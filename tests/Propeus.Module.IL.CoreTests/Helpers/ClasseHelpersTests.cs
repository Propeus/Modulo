using Microsoft.VisualStudio.TestTools.UnitTesting;

using Propeus.Module.IL.Core.Enums;
using Propeus.Module.IL.Core.Geradores;
using Propeus.Module.IL.Core.Helpers;
using Propeus.Module.IL.CoreTests.Contratos;
using Propeus.Module.IL.CoreTests.Modulos;

namespace Propeus.Module.IL.CoreTests.Helpers
{

    [TestClass()]
    public class ClasseHelpersTests
    {
        /// <summary>
        /// Neste teste é feito uma reconstrução do mesmo tipo repetidas vezes para analisar vazamentos de memoria e tempo de processamento
        /// </summary>
        [TestMethod()]
        public void CriarProxyClasseTest()
        {
            GeradorHelper.GetCurrentInstanceOrNew(out ILGerador iLGerador);

            ILClasseProvider? Proxy = iLGerador.Modulo.CriarProxyClasse(typeof(TesteModule));
            Proxy.Apply();
            iLGerador.Modulo.Apply();
            Type tpProxy = Proxy.ObterTipoGerado();
            Assert.IsNotNull(tpProxy);
            Assert.IsNotNull(Activator.CreateInstance(tpProxy));
            Assert.IsNotNull(Proxy.ToString());
            Assert.IsNotNull(iLGerador.Modulo.ToString());
            Assert.IsNotNull(Proxy);

            Proxy = Proxy.NewVersion("Teste.NovaVersao.Namespace", typeof(object), new Type[] { typeof(ITesteContrato) }, new Token[] { Token.Publico }).CriarProxyClasse(typeof(TesteModule));
            Assert.IsNotNull(Proxy);
            Proxy.Apply();
            tpProxy = Proxy.ObterTipoGerado();
            Assert.IsNotNull(tpProxy);


            Proxy.Apply();
            Proxy.Dispose();
            Assert.AreEqual(string.Empty, Proxy.ToString());
            Assert.ThrowsException<ObjectDisposedException>(() => { Proxy.NewVersion("Teste.NovaVersao.Namespace", typeof(object), new Type[] { typeof(ITesteContrato) }).CriarProxyClasse(typeof(TesteModule)); });
            Assert.ThrowsException<ObjectDisposedException>(() => { Proxy.Apply(); });
            Assert.ThrowsException<ObjectDisposedException>(() => { Proxy.NewVersion(); });
            ILModulo modulo = iLGerador.Modulo;
            modulo.Dispose();
            Assert.AreEqual(string.Empty, modulo.ToString());
        }

        /// <summary>
        /// Neste teste é validado o proposito desta biblioteca (ta estranho de ler essa palavra, sera que esta errado???)
        /// </summary>
        [TestMethod()]
        public void CriarProxyClasseTest2()
        {

            GeradorHelper.GetCurrentInstanceOrNew(out ILGerador iLGerador);
            ILClasseProvider Proxy = iLGerador.Modulo.CriarProxyClasse(typeof(TesteModule), new Type[] { typeof(ITesteContrato), typeof(IPropriedadeIndexadaContrato) });
            Proxy.Apply();
            Type tpProxy = Proxy.ObterTipoGerado();
            Assert.IsNotNull(tpProxy);
            Assert.IsNotNull(Activator.CreateInstance(tpProxy));
        }

        [TestCleanup()]
        public void End()
        {
            GeradorHelper.DisposeGerador();
        }
    }
}