using Microsoft.VisualStudio.TestTools.UnitTesting;

using Propeus.Module.Watcher.Contracts;

namespace Propeus.Modulo.DinamicoTests
{
    [TestClass()]
    public class ModuleProviderTests
    {
        private IModuleWatcherContract provider;

        [TestInitialize]
        public void Init()
        {
            Propeus.Module.Abstract.Interfaces.IModuleManager gen = Module.Manager.ModuleManagerExtensions.CreateModuleManager();
            provider = Module.Manager.Dinamic.ModuleManagerExtensions.CreateModuleManager(gen).GetModule<IModuleWatcherContract>();
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