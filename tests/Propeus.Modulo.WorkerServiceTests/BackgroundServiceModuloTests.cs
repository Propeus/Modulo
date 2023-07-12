using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Abstrato.Modulos;
using Propeus.Modulo.Dinamico;
using Propeus.Modulo.WorkerService;

namespace Propeus.Modulo.WorkerServiceTests
{
    [Module]
    public class ModuloTesteWorker : BackgroundServiceModulo
    {
        public ModuloTesteWorker(bool instanciaUnica = false) : base()
        {
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine("Worker running at: {0}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }


    [TestClass()]
    public class BackgroundServiceModuloTests
    {
        private IModuleManager gerenciador;

        [TestInitialize]
        public void Begin()
        {
            gerenciador = Propeus.Modulo.Dinamico.ModuleManagerExtensions.CreateModuleManagerDefault(Core.ModuleManagerCoreExtensions.CreateModuleManagerDefault());
        }

        [TestCleanup]
        public void End()
        {
            gerenciador.Dispose();
        }

        [TestMethod()]
        public void BackgroundServiceModuloTest()
        {
            _ = gerenciador.CreateModule<ModuloTesteWorker>();
            ModuloTesteWorker worker = gerenciador.GetModule<ModuloTesteWorker>();
            Assert.AreEqual(Abstrato.State.Created, worker.State);
        }

        [TestMethod()]
        public void ToStringTest()
        {
            _ = gerenciador.CreateModule<ModuloTesteWorker>();
            ModuloTesteWorker worker = gerenciador.GetModule<ModuloTesteWorker>();
            Assert.IsNotNull(worker.ToString());
        }

        [TestMethod()]
        public void DisposeTest()
        {
            _ = gerenciador.CreateModule<ModuloTesteWorker>();
            ModuloTesteWorker worker = gerenciador.GetModule<ModuloTesteWorker>();
            Assert.AreEqual(Abstrato.State.Created, worker.State);
            gerenciador.RemoveModule(worker);
            Assert.AreEqual(Abstrato.State.Off, worker.State);
        }
    }
}