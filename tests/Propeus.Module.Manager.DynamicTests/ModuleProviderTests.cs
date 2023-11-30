using Microsoft.VisualStudio.TestTools.UnitTesting;

using Propeus.Module.Abstract.Interfaces;
using Propeus.Module.Manager;
using Propeus.Module.Manager.Dynamic;
using Propeus.Module.Watcher.Contracts;

namespace Propeus.Module.DinamicoTests
{
    [TestClass()]
    public class ModuleProviderTests
    {
        private IModuleWatcherContract provider;

        [TestInitialize]
        public void Init()
        {
            IModuleManager gen = Propeus.Module.Manager.ModuleManagerExtensions.CreateModuleManager();
            provider = gen.CreateModuleManager().GetModule<IModuleWatcherContract>();
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