using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Propeus.Modulo.Util.Outros.Teste
{
    [TestClass()]
    public class HelperTeste
    {
        [TestMethod]
        public void NotTesteTrueSucesso()
        {
            Assert.IsFalse(true.Not());
        }

        [TestMethod]
        public void NotTesteFalseSucesso()
        {
            Assert.IsTrue(false.Not());
        }
    }
}