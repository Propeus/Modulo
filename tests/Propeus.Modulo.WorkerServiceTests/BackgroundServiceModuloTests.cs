using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Propeus.Module.Abstract;
using Propeus.Module.Abstract.Attributes;
using Propeus.Module.Abstract.Interfaces;
using Propeus.Module.Manager;
using Propeus.Module.WorkerService;

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
            gerenciador = Propeus.Module.Manager.Dinamic.ModuleManagerExtensions.CreateModuleManager(ModuleManagerExtensions.CreateModuleManager());
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
            Assert.AreEqual(State.Initialized, worker.State);
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
            Assert.AreEqual(State.Initialized, worker.State);
            gerenciador.RemoveModule(worker);
            Assert.AreEqual(State.Off, worker.State);
        }
    }
}