using Microsoft.VisualStudio.TestTools.UnitTesting;

using Propeus.Modulo.Dinamico.Contracts;

namespace Propeus.Modulo.DinamicoTests
{
    [TestClass()]
    public class ModuleProviderTests
    {
        private IModuleWatcherContract provider;

        [TestInitialize]
        public void Init()
        {
            Abstrato.Interfaces.IModuleManager gen = Core.ModuleManagerExtensions.CreateModuleManager();
            provider = Dinamico.ModuleManagerExtensions.CreateModuleManager(gen).GetModule<IModuleWatcherContract>();
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