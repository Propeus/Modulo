using Microsoft.VisualStudio.TestTools.UnitTesting;
using Propeus.Modulo.Core;
using Propeus.Modulo.Core.Teste.ModuloSimples;
using Propeus.Modulo.Core.Teste.ModuloSimples.Contratos;
using Propeus.Modulo.Modelos.Interfaces;
using Propeus.Modulo.Teste.Interfaces;
using Propeus.Modulo.Teste.Modulos;
using System;
using System.Linq;

namespace Propeus.Modulo.Teste.Core
{
    [TestCategory("Core.Gerenciador")]
    [TestClass]
    public class Teste
    {
        private IGerenciador gerenciador;

        [TestInitialize]
        public void Inicio()
        {
            gerenciador = Gerenciador.Atual;
            Assert.IsNotNull(gerenciador);
        }

        [TestMethod]
        public void ManterVivo()
        {
            System.Threading.Tasks.Task t = gerenciador.ManterVivoAsync();
            t.Wait(TimeSpan.FromSeconds(3));
        }

        [TestMethod]
        public void CriarType()
        {
            IModulo result = gerenciador.Criar(typeof(ModuloSemInterface));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CriarTypeNulo()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                gerenciador.Criar(default(Type));
            });

        }

        [TestMethod]
        public void CriarType_Void()
        {
            Assert.ThrowsException<InvalidCastException>(() =>
            {
                gerenciador.Criar(typeof(void));
            });

        }

        [TestMethod]
        public void CriarTypeNaoModulo()
        {
            Assert.ThrowsException<InvalidCastException>(() =>
            {
                gerenciador.Criar(typeof(string));
            });

        }

        [TestMethod]
        public void CriarTypeModuloNaoMarcado()
        {
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                gerenciador.Criar(typeof(ModuloNaoMarcado));
            });

        }

        [TestMethod]
        public void CriarTypeModuloSemConstrutorPadrao()
        {
            Assert.ThrowsException<ArgumentException>(() =>
            {
                gerenciador.Criar(typeof(ModuloSemConstrutorPadrao));
            });

        }


        [TestMethod]
        public void CriarTypeInterfaceContratoNaoMarcado()
        {
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                gerenciador.Criar(typeof(IInterfaceContratoNaoMarcado));
            });

        }

        [TestMethod]
        public void CriarTypeInterfaceContratoModuloInexistente()
        {
            Assert.ThrowsException<DllNotFoundException>(() =>
            {
                gerenciador.Criar(typeof(InterfaceContratoNomeModuloErrado));
            });

        }



        [TestMethod]
        public void CriarTypeGenerico()
        {

            ModuloSemInterface result = gerenciador.Criar<ModuloSemInterface>();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CriarTypeGenericoInterfaceInstanciaUnica()
        {

            IModuloComInterfaceContrato result = gerenciador.Criar<IModuloComInterfaceContrato>();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CriarTypeInterfaceInstanciaUnica()
        {

            IModulo result = gerenciador.Criar(typeof(IModuloComInterfaceContrato));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CriarTypeGenericoInterfaceInstanciaUnicaMultiplos()
        {

            IModuloComInterfaceContrato result = gerenciador.Criar<IModuloComInterfaceContrato>();
            Assert.IsNotNull(result);
            Assert.ThrowsException<ArgumentException>(() =>
            {
                gerenciador.Criar<IModuloComInterfaceContrato>();
            });
        }

        [TestMethod]
        public void CriarTypeGenericoMultiplos()
        {

            for (int i = 0; i < 999; i++)
            {
                ModuloSemInterface result = gerenciador.Criar<ModuloSemInterface>();
                Assert.IsNotNull(result);
            }

        }

        [TestMethod]
        public void CriarTypeMultiplos()
        {

            for (int i = 0; i < 999; i++)
            {
                IModulo result = gerenciador.Criar(typeof(ModuloSemInterface));
                Assert.IsNotNull(result);
            }

        }


        [TestMethod]
        public void RemoverTipo()
        {

            IModulo result = gerenciador.Criar(typeof(ModuloSemInterface));
            Assert.IsNotNull(result);
            gerenciador.Remover(result);

        }

        [TestMethod]
        public void RemoverId()
        {

            IModulo result = gerenciador.Criar(typeof(ModuloSemInterface));
            Assert.IsNotNull(result);
            gerenciador.Remover(result.Id);
            Assert.IsTrue(result.Disposed);

        }

        [TestMethod]
        public void RemoverTipoMultiplos()
        {

            for (int i = 0; i < 999; i++)
            {
                IModulo result = gerenciador.Criar(typeof(ModuloSemInterface));
                Assert.IsNotNull(result);
                gerenciador.Remover(result);
            }
        }

        [TestMethod]
        public void RemoverTipoGenerico()
        {

            ModuloSemInterface result = gerenciador.Criar<ModuloSemInterface>();
            Assert.IsNotNull(result);
            gerenciador.Remover(result);
        }

        [TestMethod]
        public void RemoverTipoGenericoMultiplos()
        {

            for (int i = 0; i < 999; i++)
            {
                ModuloSemInterface result = gerenciador.Criar<ModuloSemInterface>();
                Assert.IsNotNull(result);
                gerenciador.Remover(result);
            }

        }

        [TestMethod]
        public void RemoverTipoInterfaceInstanciaUnica()
        {

            IModulo result = gerenciador.Criar(typeof(IModuloComInterfaceContrato));
            Assert.IsNotNull(result);
            gerenciador.Remover(result);
        }


        [TestMethod]
        public void ReiniciarTipoInterfaceInstanciaUnicaId()
        {
            IModuloComInterfaceContrato md_1 = gerenciador.Criar<IModuloComInterfaceContrato>();
            string id_1 = md_1.Id;
            IModuloComInterfaceContrato md_2 = gerenciador.Reiniciar(md_1);
            string id_2 = md_2.Id;
            Assert.AreNotEqual(id_1, id_2);
            Assert.ThrowsException<ArgumentException>(() =>
            {
                gerenciador.Obter(md_1.Id);
            });
        }

        [TestMethod]
        public void ReiniciarTipoModulo()
        {
            ModuloSemInterface md_1 = gerenciador.Criar<ModuloSemInterface>();
            string id_1 = md_1.Id;
            ModuloSemInterface md_2 = gerenciador.Reiniciar(md_1);
            string id_2 = md_2.Id;
            Assert.AreNotEqual(id_1, id_2);
            Assert.ThrowsException<ArgumentException>(() =>
            {
                gerenciador.Obter(md_1.Id);
            });
        }

        [TestMethod]
        public void ReiniciarIdModulo()
        {
            ModuloSemInterface md_1 = gerenciador.Criar<ModuloSemInterface>();
            string id_1 = md_1.Id;
            IModulo md_2 = gerenciador.Reiniciar(md_1.Id);
            string id_2 = md_2.Id;
            Assert.AreNotEqual(id_1, id_2);
            Assert.ThrowsException<ArgumentException>(() =>
            {
                gerenciador.Obter(md_1.Id);
            });
        }

        [TestMethod]
        public void ReiniciarIdModuloNew()
        {
            ModuloSemInterface md_1 = new ModuloSemInterface(gerenciador);
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                IModulo md_2 = gerenciador.Reiniciar(md_1.Id);
            });
        }

        [TestMethod]
        public void ExisteTipoModulo()
        {
            ModuloSemInterface md_1 = gerenciador.Criar<ModuloSemInterface>();
            Assert.IsTrue(gerenciador.Existe(md_1));
        }

        [TestMethod]
        public void ExisteModuloNulo()
        {
            Assert.ThrowsException<ArgumentNullException>(() => { gerenciador.Existe(default(IModulo)); });
        }

        [TestMethod]
        public void ExisteTipoModuloNulo()
        {
            Assert.ThrowsException<ArgumentNullException>(() => { gerenciador.Existe(default(Type)); });
        }

        [TestMethod]
        public void ExisteTipoInterface()
        {
            IModuloComInterfaceContrato md_1 = gerenciador.Criar<IModuloComInterfaceContrato>();
            Assert.IsTrue(gerenciador.Existe(md_1));
        }

        [TestMethod]
        public void ExisteTipoModuloId()
        {
            ModuloSemInterface md_1 = gerenciador.Criar<ModuloSemInterface>();
            Assert.IsTrue(gerenciador.Existe(md_1.Id));
        }

        [TestMethod]
        public void ExisteTipoInterfaceId()
        {
            IModuloComInterfaceContrato md_1 = gerenciador.Criar<IModuloComInterfaceContrato>();
            Assert.IsTrue(gerenciador.Existe(md_1.Id));
        }

        [TestMethod]
        public void ObterTipoInterface()
        {
            IModuloComInterfaceContrato md_1 = gerenciador.Criar<IModuloComInterfaceContrato>();
            Assert.IsNotNull(gerenciador.Obter<IModuloComInterfaceContrato>());
        }

        [TestMethod]
        public void ObterIdVazio()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                IModulo md_1 = gerenciador.Obter(string.Empty);
            });
        }

        [TestMethod]
        public void ObterIdInexistente()
        {
            Assert.ThrowsException<ArgumentException>(() =>
            {
                IModulo md_1 = gerenciador.Obter("20");
            });
        }

        [TestMethod]
        public void ObterModuloDescartado()
        {
            Assert.ThrowsException<InvalidProgramException>(() =>
            {

                IModulo md_1 = gerenciador.Criar<ModuloSemInterface>();
                var id = md_1.Id;
                md_1.Dispose();
                gerenciador.Obter(id);

            });
        }

        [TestMethod]
        public void ObterTodos()
        {
            IModuloComInterfaceContrato md_1 = gerenciador.Criar<IModuloComInterfaceContrato>();
            Assert.AreEqual(gerenciador.Listar().Count(), 1);
        }

        [TestCleanup]
        public void Finalizacao()
        {
            gerenciador.Dispose();
            gerenciador = null;
        }
    }
}
