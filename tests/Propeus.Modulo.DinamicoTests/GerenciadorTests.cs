using System;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Abstrato.Exceptions;
using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Abstrato.Modulos;
using Propeus.Modulo.Dinamico;

namespace Propeus.Modulo.DinamicoTests
{
    [Modulo]
    public class TesteInstanciaUnicaModulo : ModuloBase
    {
        public TesteInstanciaUnicaModulo() : base(true)
        {
        }

        public override string ToString()
        {
            return Id;
        }
    }

    [Modulo]
    public class TesteInstanciaMultiplaModulo : ModuloBase
    {
        public TesteInstanciaMultiplaModulo() : base(false)
        {
        }

        public override string ToString()
        {
            return Id;
        }
    }

    [TestClass()]
    public class GerenciadorTests
    {

        private IGerenciador gerenciador;

        [TestInitialize]
        public void Begin()
        {
            gerenciador = new Gerenciador(Core.Gerenciador.Atual, new GerenciadorConfiguracao() { CarregamentoRapido = true });
        }

        [TestCleanup]
        public void End()
        {
            gerenciador.Dispose();
            gerenciador = null;
            Core.Gerenciador.Atual.Dispose();
        }

        #region Proxy para Propeus.Modulo.Core.Gerenciador
        //Criar
        [TestMethod()]
        [TestCategory("Criar")]
        public void CriarModuloInstanciaUnica()
        {

            IModulo modulo = gerenciador.Criar<TesteInstanciaUnicaModulo>();
            Assert.IsNotNull(modulo);

        }

        [TestMethod()]
        [TestCategory("Criar")]
        public void CriarModuloInstanciaUnica_ArgumentException()
        {


            IModulo modulo = gerenciador.Criar<TesteInstanciaUnicaModulo>();
            Assert.IsNotNull(modulo);
            _ = Assert.ThrowsException<ModuloInstanciaUnicaException>(() =>
            {
                modulo = gerenciador.Criar<TesteInstanciaUnicaModulo>();
            });


        }
        [TestMethod()]
        [TestCategory("Criar")]
        public void CriarModuloInstanciaMultipla()
        {

            IModulo modulo = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
            Assert.IsNotNull(modulo);

        }
        [TestMethod()]
        [TestCategory("Criar")]
        public void CriarModuloInstanciaMultipla_multiplosModulos()
        {

            IModulo modulo = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
            Assert.IsNotNull(modulo);
            IModulo modulov2 = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
            Assert.IsNotNull(modulov2);
            Assert.AreNotEqual(modulo, modulov2);
            Assert.AreNotEqual(modulo.Id, modulov2.Id);

        }

        [TestMethod()]
        [TestCategory("Criar")]
        public void CriarModuloInstanciaUnicaPorTipo()
        {

            IModulo modulo = gerenciador.Criar(typeof(TesteInstanciaUnicaModulo));
            Assert.IsNotNull(modulo);

        }
        [TestMethod()]
        [TestCategory("Criar")]
        public void CriarModuloInstanciaUnicaPorTipo_ArgumentException()
        {

            IModulo modulo = gerenciador.Criar(typeof(TesteInstanciaUnicaModulo));
            Assert.IsNotNull(modulo);
            _ = Assert.ThrowsException<ModuloInstanciaUnicaException>(() =>
            {
                modulo = gerenciador.Criar(typeof(TesteInstanciaUnicaModulo));
            });

        }
        [TestMethod()]
        [TestCategory("Criar")]
        public void CriarModuloInstanciaMultiplaPorTipo()
        {

            IModulo modulo = gerenciador.Criar(typeof(TesteInstanciaMultiplaModulo));
            Assert.IsNotNull(modulo);

        }
        [TestMethod()]
        [TestCategory("Criar")]
        public void CriarModuloInstanciaMultiplaPorTipo_MultiplosModulos()
        {

            IModulo modulo = gerenciador.Criar(typeof(TesteInstanciaMultiplaModulo));
            Assert.IsNotNull(modulo);
            IModulo modulov2 = gerenciador.Criar(typeof(TesteInstanciaMultiplaModulo));
            Assert.IsNotNull(modulov2);
            Assert.AreNotEqual(modulo, modulov2);
            Assert.AreNotEqual(modulo.Id, modulov2.Id);

        }

        [TestMethod()]
        [TestCategory("Criar")]
        public void CriarModuloInstanciaUnicaPorNome()
        {

            IModulo modulo = gerenciador.Criar(nameof(TesteInstanciaUnicaModulo));
            Assert.IsNotNull(modulo);

        }
        [TestMethod()]
        [TestCategory("Criar")]
        public void CriarModuloInstanciaUnicaPorNome_ModuloInstanciaUnicaException()
        {

            IModulo modulo = gerenciador.Criar(nameof(TesteInstanciaUnicaModulo));
            Assert.IsNotNull(modulo);
            _ = Assert.ThrowsException<ModuloInstanciaUnicaException>(() =>
            {
                modulo = gerenciador.Criar(nameof(TesteInstanciaUnicaModulo));
            });

        }
        [TestMethod()]
        [TestCategory("Criar")]
        public void CriarModuloInstanciaMultiplaPorNome()
        {

            IModulo modulo = gerenciador.Criar(nameof(TesteInstanciaMultiplaModulo));
            Assert.IsNotNull(modulo);

        }
        [TestMethod()]
        [TestCategory("Criar")]
        public void CriarModuloInstanciaMultiplaPorNome_MultiplosModulos()
        {

            IModulo modulo = gerenciador.Criar(nameof(TesteInstanciaMultiplaModulo));
            Assert.IsNotNull(modulo);
            IModulo modulov2 = gerenciador.Criar(nameof(TesteInstanciaMultiplaModulo));
            Assert.IsNotNull(modulov2);
            Assert.AreNotEqual(modulo, modulov2);
            Assert.AreNotEqual(modulo.Id, modulov2.Id);

        }

        //Remover
        [TestMethod()]
        [TestCategory("Remover")]
        public void RemoverModuloInstanciaUnica()
        {

            IModulo modulo = gerenciador.Criar<TesteInstanciaUnicaModulo>();
            Assert.IsNotNull(modulo);
            gerenciador.Remover(modulo);

        }
        [TestMethod()]
        [TestCategory("Remover")]
        public void RemoverModuloInstanciaMultipla()
        {

            IModulo modulo = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
            Assert.IsNotNull(modulo);
            gerenciador.Remover(modulo);

        }
        [TestMethod()]
        [TestCategory("Remover")]
        public void RemoverModuloInstanciaUnica_loop()
        {

            for (int i = 0; i < 100; i++)
            {
                IModulo modulo = gerenciador.Criar<TesteInstanciaUnicaModulo>();
                Assert.IsNotNull(modulo);
                gerenciador.Remover(modulo);
            }

        }
        [TestMethod()]
        [TestCategory("Remover")]
        public void RemoverModuloInstanciaMultipla_loop()
        {

            for (int i = 0; i < 100; i++)
            {
                IModulo modulo = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
                Assert.IsNotNull(modulo);
                gerenciador.Remover(modulo);
            }

        }
        [TestMethod()]
        [TestCategory("Remover")]
        public void RemoverModuloInstanciaUnicaPorId()
        {

            IModulo modulo = gerenciador.Criar<TesteInstanciaUnicaModulo>();
            Assert.IsNotNull(modulo);
            gerenciador.Remover(modulo.Id);

        }
        [TestMethod()]
        [TestCategory("Remover")]
        public void RemoverModuloInstanciaMultiplaPorId()
        {

            IModulo modulo = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
            Assert.IsNotNull(modulo);
            gerenciador.Remover(modulo.Id);

        }
        [TestMethod()]
        [TestCategory("Remover")]
        public void RemoverModuloInstanciaUnicaPorId_loop()
        {

            for (int i = 0; i < 100; i++)
            {
                IModulo modulo = gerenciador.Criar<TesteInstanciaUnicaModulo>();
                Assert.IsNotNull(modulo);
                gerenciador.Remover(modulo.Id);
            }

        }
        [TestMethod()]
        [TestCategory("Remover")]
        public void RemoverModuloInstanciaMultiplaPorId_loop()
        {

            for (int i = 0; i < 100; i++)
            {
                IModulo modulo = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
                Assert.IsNotNull(modulo);
                gerenciador.Remover(modulo.Id);
            }

        }
        [TestMethod()]
        [TestCategory("Remover")]
        public void RemoverTodosModulos()
        {

            IModulo[] modulos = new IModulo[100];

            for (int i = 0; i < 100; i++)
            {
                modulos[i] = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
                Assert.IsNotNull(modulos[i]);
            }

            gerenciador.RemoverTodos();
            Assert.AreEqual(0, (gerenciador as IGerenciadorDiagnostico).ModulosInicializados);

            _ = modulos.All(m => m.Estado == Estado.Desligado);

        }
        [TestMethod()]
        [TestCategory("Remover")]
        public void RemoverModuloInexistente()
        {

            IModulo modulo = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
            Assert.IsNotNull(modulo);
            gerenciador.Remover(modulo.Id);

            Assert.AreEqual(1, (gerenciador as IGerenciadorDiagnostico).ModulosInicializados);
            _ = Assert.ThrowsException<ModuloNaoEncontradoException>(() =>
            {
                gerenciador.Remover(modulo.Id);
            });


        }

        //Obter
        [TestMethod()]
        [TestCategory("Obter")]
        public void ObterModuloInstanciaUnicaPorTipo()
        {

            IModulo modulo = gerenciador.Criar<TesteInstanciaUnicaModulo>();
            Assert.IsNotNull(modulo);
            IModulo modulov2 = gerenciador.Obter<TesteInstanciaUnicaModulo>();
            Assert.AreEqual(modulo, modulov2);

        }
        [TestMethod()]
        [TestCategory("Obter")]
        public void ObterModuloInstanciaMultiplaPorTipo()
        {

            IModulo modulo = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
            Assert.IsNotNull(modulo);
            IModulo modulov2 = gerenciador.Obter<TesteInstanciaMultiplaModulo>();
            Assert.AreEqual(modulo, modulov2);

        }
        [TestMethod()]
        [TestCategory("Obter")]
        public void ObterModuloInstanciaMultipla_MultiplosModulosPorTipo()
        {

            IModulo modulo = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
            Assert.IsNotNull(modulo);
            IModulo modulov2 = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
            Assert.IsNotNull(modulov2);
            Assert.AreNotEqual(modulo, modulov2);
            IModulo modulov3 = gerenciador.Obter<TesteInstanciaMultiplaModulo>();
            Assert.IsNotNull(modulov3);

        }
        [TestMethod()]
        [TestCategory("Obter")]
        public void ObterModuloInstanciaUnicaPorId()
        {

            IModulo modulo = gerenciador.Criar<TesteInstanciaUnicaModulo>();
            Assert.IsNotNull(modulo);
            IModulo modulov2 = gerenciador.Obter(modulo.Id);
            Assert.AreEqual(modulo, modulov2);

        }
        [TestMethod()]
        [TestCategory("Obter")]
        public void ObterModuloInstanciaMultiplaPorId()
        {

            IModulo modulo = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
            Assert.IsNotNull(modulo);
            IModulo modulov2 = gerenciador.Obter<TesteInstanciaMultiplaModulo>();
            Assert.AreEqual(modulo.Id, modulov2.Id);

        }
        [TestMethod()]
        [TestCategory("Obter")]
        public void ObterModuloInstanciaMultipla_MultiplosModulosPorId()
        {

            IModulo modulo = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
            Assert.IsNotNull(modulo);
            IModulo modulov2 = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
            Assert.IsNotNull(modulov2);
            Assert.AreNotEqual(modulo, modulov2);
            IModulo modulov3 = gerenciador.Obter(modulov2.Id);
            Assert.IsNotNull(modulov3);
            Assert.AreEqual(modulov2, modulov3);

        }
        [TestMethod()]
        [TestCategory("Obter")]
        public void ObterModuloInstanciaPorIdInexistente_ArgumentException()
        {

            _ = Assert.ThrowsException<ModuloNaoEncontradoException>(() =>
            {
                _ = gerenciador.Obter(Guid.NewGuid().ToString());
            });

        }

        //Existe
        [TestMethod()]
        [TestCategory("Existe")]
        public void ExisteModuloInstanciaUnicaPorInstancia()
        {

            IModulo modulo = gerenciador.Criar<TesteInstanciaUnicaModulo>();
            Assert.IsNotNull(modulo);
            Assert.IsTrue(gerenciador.Existe(modulo));

        }
        [TestMethod()]
        [TestCategory("Existe")]
        public void ExisteModuloInstanciaMultiplaPorInstancia()
        {

            IModulo modulo = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
            Assert.IsNotNull(modulo);
            Assert.IsTrue(gerenciador.Existe(modulo));

        }
        [TestMethod()]
        [TestCategory("Existe")]
        public void ExisteModuloInstanciaMultipla_MultiplosModulosPorInstancia()
        {

            IModulo modulo = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
            Assert.IsNotNull(modulo);
            IModulo modulov2 = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
            Assert.IsNotNull(modulov2);
            Assert.AreNotEqual(modulo, modulov2);
            Assert.IsTrue(gerenciador.Existe(modulo));
            Assert.IsTrue(gerenciador.Existe(modulov2));

        }
        [TestMethod()]
        [TestCategory("Existe")]
        public void ExisteModuloInstanciaUnicaPorTipo()
        {

            IModulo modulo = gerenciador.Criar<TesteInstanciaUnicaModulo>();
            Assert.IsNotNull(modulo);
            Assert.IsTrue(gerenciador.Existe(typeof(TesteInstanciaUnicaModulo)));

        }
        [TestMethod()]
        [TestCategory("Existe")]
        public void ExisteModuloInstanciaMultiplaPorTipo()
        {

            IModulo modulo = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
            Assert.IsNotNull(modulo);
            Assert.IsTrue(gerenciador.Existe(typeof(TesteInstanciaMultiplaModulo)));

        }
        [TestMethod()]
        [TestCategory("Existe")]
        public void ExisteModuloInstanciaMultipla_MultiplosModulosPorTipo()
        {

            IModulo modulo = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
            Assert.IsNotNull(modulo);
            IModulo modulov2 = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
            Assert.IsNotNull(modulov2);
            Assert.AreNotEqual(modulo, modulov2);
            Assert.IsTrue(gerenciador.Existe(typeof(TesteInstanciaMultiplaModulo)));

        }
        [TestMethod()]
        [TestCategory("Existe")]
        public void ExisteModuloInstanciaUnicaPorId()
        {

            IModulo modulo = gerenciador.Criar<TesteInstanciaUnicaModulo>();
            Assert.IsNotNull(modulo);
            Assert.IsTrue(gerenciador.Existe(modulo.Id));

        }
        [TestMethod()]
        [TestCategory("Existe")]
        public void ExisteModuloInstanciaMultiplaPorId()
        {

            IModulo modulo = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
            Assert.IsNotNull(modulo);
            Assert.IsTrue(gerenciador.Existe(modulo.Id));

        }
        [TestMethod()]
        [TestCategory("Existe")]
        public void ExisteModuloInstanciaMultipla_MultiplosModulosPorId()
        {

            IModulo modulo = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
            Assert.IsNotNull(modulo);
            IModulo modulov2 = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
            Assert.IsNotNull(modulov2);
            Assert.AreNotEqual(modulo, modulov2);
            Assert.IsTrue(gerenciador.Existe(modulov2.Id));

        }
        [TestMethod()]
        [TestCategory("Existe")]
        public void ExisteModuloInstanciaPorIdInexistente()
        {

            Assert.IsFalse(gerenciador.Existe(Guid.NewGuid().ToString()));

        }

        //Reciclar
        [TestMethod()]
        [TestCategory("Reciclar")]
        public void ReiniciarModuloInstanciaUnicaPorInstancia()
        {

            IModulo modulo = gerenciador.Criar<TesteInstanciaUnicaModulo>();
            Assert.IsNotNull(modulo);
            Assert.IsTrue(gerenciador.Existe(modulo));
            IModulo modulov2 = gerenciador.Reciclar(modulo);
            Assert.IsNotNull(modulov2);
            Assert.IsTrue(gerenciador.Existe(modulov2));
            Assert.IsFalse(gerenciador.Existe(modulo));

        }
        [TestMethod()]
        [TestCategory("Reciclar")]
        public void ReiniciarModuloInstanciaMultiplaPorInstancia()
        {

            IModulo modulo = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
            Assert.IsNotNull(modulo);
            Assert.IsTrue(gerenciador.Existe(modulo));
            IModulo modulov2 = gerenciador.Reciclar(modulo);
            Assert.IsNotNull(modulov2);
            Assert.IsTrue(gerenciador.Existe(modulov2));
            Assert.IsFalse(gerenciador.Existe(modulo));

        }
        [TestMethod()]
        [TestCategory("Reciclar")]
        public void ReiniciarModuloInstanciaUnicaPorId()
        {

            IModulo modulo = gerenciador.Criar<TesteInstanciaUnicaModulo>();
            Assert.IsNotNull(modulo);
            Assert.IsTrue(gerenciador.Existe(modulo));
            IModulo modulov2 = gerenciador.Reciclar(modulo.Id);
            Assert.IsNotNull(modulov2);
            Assert.IsTrue(gerenciador.Existe(modulov2));
            Assert.IsFalse(gerenciador.Existe(modulo));

        }
        [TestMethod()]
        [TestCategory("Reciclar")]
        public void ReiniciarModuloInstanciaMultiplaPorId()
        {

            IModulo modulo = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
            Assert.IsNotNull(modulo);
            Assert.IsTrue(gerenciador.Existe(modulo));
            IModulo modulov2 = gerenciador.Reciclar(modulo.Id);
            Assert.IsNotNull(modulov2);
            Assert.IsTrue(gerenciador.Existe(modulov2));
            Assert.IsFalse(gerenciador.Existe(modulo));

        }
        [TestMethod()]
        [TestCategory("Reciclar")]
        public void ReiniciarModuloInstanciaPorIdInexistente_ArgumentException()
        {

            _ = Assert.ThrowsException<ModuloNaoEncontradoException>(() =>
            {
                _ = gerenciador.Reciclar(Guid.NewGuid().ToString());
            });

        }

        //Listar
        [TestMethod()]
        [TestCategory("Listar")]
        public void ListarModulos_loop()
        {

            for (int i = 0; i < 100; i++)
            {
                _ = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
            }

            Assert.AreEqual(101, gerenciador.Listar().Count());

        }
        #endregion

        [TestMethod]
        [TestCategory("Runtime")]
        public void ExecutarModuloEmTempoDeExecucao()
        {
            CarregarModuloDLL("Propeus.Modulo.DinamicoTests.ModuloSoma");

            gerenciador.Dispose();
            gerenciador = null;
            gerenciador = new Gerenciador(Core.Gerenciador.Atual, new GerenciadorConfiguracao() { CarregamentoRapido = false });

            IModuloCalculadoraContrato CalculadoraSoma = (gerenciador as IGerenciadorArgumentos).Criar<IModuloCalculadoraContrato>(new object[] { 1, "Ola mundo" });
            Assert.AreEqual(2, CalculadoraSoma.Calcular(1, 1));
            DescarregarModuloDLL("Propeus.Modulo.DinamicoTests.ModuloSoma");
        }


        private void CarregarModuloDLL(string nomeProjeto)
        {
            File.Copy($"../../../../{nomeProjeto}/bin/Debug/net7.0/{nomeProjeto}.dll", $"./{nomeProjeto}.dll", true);
        }

        private void DescarregarModuloDLL(string nomeProjeto)
        {
            File.Delete($"./{nomeProjeto}.dll");
        }
    }


    [ModuloContrato("ModuloCalculo")]
    public interface IModuloCalculadoraContrato : IModulo
    {
        int Calcular(int a, int b);
    }
}