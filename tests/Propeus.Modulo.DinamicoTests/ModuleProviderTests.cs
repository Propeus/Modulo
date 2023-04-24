using Microsoft.VisualStudio.TestTools.UnitTesting;

using Propeus.Modulo.Abstrato;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Propeus.Modulo.Dinamico.Tests
{
    [TestClass()]
    public class ModuleProviderTests
    {
        private ModuleProvider provider;

        [TestInitialize]
        public void Init()
        {
            provider = ModuleProvider.Provider;
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
            provider.LoadModules();
        }
    }
}