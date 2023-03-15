using Microsoft.VisualStudio.TestTools.UnitTesting;

using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Dinamico;
using Propeus.Modulo.WorkerService;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Propeus.Modulo.WorkerService.Tests
{
    [Modulo]
    public class ModuloTesteWorker : BackgroundServiceModulo
    {
        public ModuloTesteWorker(IGerenciador gerenciador, bool instanciaUnica = false) : base(gerenciador, instanciaUnica)
        {
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                System.Console.WriteLine("Worker running at: {0}", DateTimeOffset.Now);
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
            gerenciador = new Gerenciador(Core.Gerenciador.Atual);
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
            //TODO: Adicionar 'Exception' no IModulo
            gerenciador.Criar<ModuloTesteWorker>();
            var worker = gerenciador.Obter<ModuloTesteWorker>();
            Assert.AreEqual(Abstrato.Estado.Inicializado, worker.Estado);
        }

        [TestMethod()]
        public void ToStringTest()
        {
            gerenciador.Criar<ModuloTesteWorker>();
            var worker = gerenciador.Obter<ModuloTesteWorker>();
            Assert.IsNotNull(worker.ToString());
        }

        [TestMethod()]
        public void DisposeTest()
        {
            gerenciador.Criar<ModuloTesteWorker>();
            var worker = gerenciador.Obter<ModuloTesteWorker>();
            Assert.AreEqual(Abstrato.Estado.Inicializado, worker.Estado);
            gerenciador.Remover(worker);
            Assert.AreEqual(Abstrato.Estado.Desligado, worker.Estado);
        }
    }
}