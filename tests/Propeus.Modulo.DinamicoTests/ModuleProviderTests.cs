using Microsoft.VisualStudio.TestTools.UnitTesting;

using Propeus.Modulo.Dinamico;
using Propeus.Modulo.Dinamico.Modules;

namespace Propeus.Modulo.DinamicoTests
{
    [TestClass()]
    public class ModuleProviderTests
    {
        private ModuleWatcherModule provider;

        [TestInitialize]
        public void Init()
        {
            Abstrato.Interfaces.IModuleManager gen = Abstrato.ModuleManagerCoreExtensions.CreateModuleManager();
            provider = ModuleManagerExtensions.CreateModuleManager(gen).CreateModule<ModuleWatcherModule>();
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