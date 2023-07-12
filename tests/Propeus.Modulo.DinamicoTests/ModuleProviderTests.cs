using Microsoft.VisualStudio.TestTools.UnitTesting;

using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Dinamico;
using Propeus.Modulo.Dinamico.Modules;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Propeus.Modulo.DinamicoTests
{
    [TestClass()]
    public class ModuleProviderTests
    {
        private ModuleProviderModule provider;

        [TestInitialize]
        public void Init()
        {
            var gen = Core.ModuleManagerCoreExtensions.CreateModuleManagerDefault();
            gen.KeepAliveModuleAsync(gen.CreateModule<QueueMessageModule>()).Wait();
            provider = ModuleManagerExtensions.CreateModuleManager(gen).CreateModule<ModuleProviderModule>();
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