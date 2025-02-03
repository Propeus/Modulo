using Microsoft.VisualStudio.TestTools.UnitTesting;

using Propeus.Module.Abstract.Interfaces;
using Propeus.Module.Manager.Dynamic;
using Propeus.Module.WatcherDynamicModule.Contracts;

namespace Propeus.Module.Manager.DynamicTests
{
    [TestClass()]
    public class ModuleProviderTests
    {
        private IModuleWatcherContract provider;

        [TestInitialize]
        public void Init()
        {
            IModuleManager gen = ModuleManagerExtensions.CreateModuleManager();
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