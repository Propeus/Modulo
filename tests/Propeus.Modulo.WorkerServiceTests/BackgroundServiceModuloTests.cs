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
    [Modulo]
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
        private IGerenciador gerenciador;

        [TestInitialize]
        public void Begin()
        {
            gerenciador = Gerenciador.Atual(Core.Gerenciador.Atual);
        }

        [TestCleanup]
        public void End()
        {
            gerenciador.Dispose();
            Core.Gerenciador.Atual.Dispose();
        }

        [TestMethod()]
        public void BackgroundServiceModuloTest()
        {
            _ = gerenciador.Criar<ModuloTesteWorker>();
            ModuloTesteWorker worker = gerenciador.Obter<ModuloTesteWorker>();
            Assert.AreEqual(Abstrato.Estado.Criado, worker.Estado);
        }

        [TestMethod()]
        public void ToStringTest()
        {
            _ = gerenciador.Criar<ModuloTesteWorker>();
            ModuloTesteWorker worker = gerenciador.Obter<ModuloTesteWorker>();
            Assert.IsNotNull(worker.ToString());
        }

        [TestMethod()]
        public void DisposeTest()
        {
            _ = gerenciador.Criar<ModuloTesteWorker>();
            ModuloTesteWorker worker = gerenciador.Obter<ModuloTesteWorker>();
            Assert.AreEqual(Abstrato.Estado.Criado, worker.Estado);
            gerenciador.Remover(worker);
            Assert.AreEqual(Abstrato.Estado.Desligado, worker.Estado);
        }
    }
}