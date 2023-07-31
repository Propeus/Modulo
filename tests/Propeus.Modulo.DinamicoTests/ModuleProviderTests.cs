using Microsoft.VisualStudio.TestTools.UnitTesting;

using Propeus.Modulo.Dinamico.Contracts;

namespace Propeus.Modulo.DinamicoTests
{
    [TestClass()]
    public class ModuleProviderTests
    {
        private IModuleWatcherModule provider;

        [TestInitialize]
        public void Init()
        {
            Abstrato.Interfaces.IModuleManager gen = Core.ModuleManagerExtensions.CreateModuleManager();
            provider = Dinamico.ModuleManagerExtensions.CreateModuleManager(gen).GetModule<IModuleWatcherModule>();
        }
        [TestCleanup]
        public void Cleanup()
        {
            provider.Dispose();
        }

        [TestMethod()]
        public void LoadModulesTest()
        {
            Assert.IsNotNull(provider);
        }
    }
}