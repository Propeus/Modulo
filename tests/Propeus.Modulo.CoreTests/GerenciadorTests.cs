using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Abstrato.Exceptions;
using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Core;

namespace Propeus.Modulo.CoreTests
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
            gerenciador = Gerenciador.Atual;
        }

        [TestCleanup]
        public void End()
        {
            gerenciador.Dispose();
            gerenciador = null;
        }

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
            Gerenciador.Atual.Remover(modulo);

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


        }
        [TestMethod()]
        [TestCategory("Remover")]
        public void RemoverModuloInexistente()
        {

            IModulo modulo = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
            Assert.IsNotNull(modulo);
            gerenciador.Remover(modulo.Id);

            Assert.AreEqual(0, (gerenciador as IGerenciadorDiagnostico).ModulosInicializados);
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

        //ObterInfo
        [TestMethod()]
        [TestCategory("ObterInfo")]
        public void ObterInfoModuloInstanciaUnicaPorTipo()
        {

            IModulo modulo = gerenciador.Criar<TesteInstanciaUnicaModulo>();
            Assert.IsNotNull(modulo);
            IModuloTipo modulov2 = (gerenciador as IGerenciadorInformacao).ObterInfo<TesteInstanciaUnicaModulo>();
            Assert.AreEqual(modulo, modulov2.Modulo);

        }
        [TestMethod()]
        [TestCategory("ObterInfo")]
        public void ObterInfoModuloInstanciaMultiplaPorTipo()
        {

            IModulo modulo = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
            Assert.IsNotNull(modulo);
            IModuloTipo modulov2 = (gerenciador as IGerenciadorInformacao).ObterInfo<TesteInstanciaMultiplaModulo>();
            Assert.AreEqual(modulo, modulov2.Modulo);

        }
        [TestMethod()]
        [TestCategory("ObterInfo")]
        public void ObterInfoModuloInstanciaMultipla_MultiplosModulosPorTipo()
        {

            IModulo modulo = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
            Assert.IsNotNull(modulo);
            IModulo modulov2 = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
            Assert.IsNotNull(modulov2);
            Assert.AreNotEqual(modulo, modulov2);
            IModuloTipo modulov3 = (gerenciador as IGerenciadorInformacao).ObterInfo<TesteInstanciaMultiplaModulo>();
            Assert.IsNotNull(modulov3.Modulo);
        }
        [TestMethod()]
        [TestCategory("ObterInfo")]
        public void ObterInfoModuloInstanciaUnicaPorId()
        {

            IModulo modulo = gerenciador.Criar<TesteInstanciaUnicaModulo>();
            Assert.IsNotNull(modulo);
            IModuloTipo modulov2 = (gerenciador as IGerenciadorInformacao).ObterInfo(modulo.Id);
            Assert.AreEqual(modulo, modulov2.Modulo);

        }
        [TestMethod()]
        [TestCategory("ObterInfo")]
        public void ObterInfoModuloInstanciaMultiplaPorId()
        {

            IModulo modulo = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
            Assert.IsNotNull(modulo);
            IModuloTipo modulov2 = (gerenciador as IGerenciadorInformacao).ObterInfo<TesteInstanciaMultiplaModulo>();
            Assert.AreEqual(modulo.Id, modulov2.Modulo.Id);

        }
        [TestMethod()]
        [TestCategory("ObterInfo")]
        public void ObterInfoModuloInstanciaMultipla_MultiplosModulosPorId()
        {

            IModulo modulo = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
            Assert.IsNotNull(modulo);
            IModulo modulov2 = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
            Assert.IsNotNull(modulov2);
            Assert.AreNotEqual(modulo, modulov2);
            IModuloTipo modulov3 = (gerenciador as IGerenciadorInformacao).ObterInfo(modulov2.Id);
            Assert.IsNotNull(modulov3);
            Assert.AreEqual(modulov2, modulov3.Modulo);

        }
        [TestMethod()]
        [TestCategory("ObterInfo")]
        public void ObterInfoModuloInstanciaPorIdInexistente_ArgumentException()
        {

            _ = Assert.ThrowsException<ModuloNaoEncontradoException>(() =>
            {
                _ = (gerenciador as IGerenciadorInformacao).ObterInfo(Guid.NewGuid().ToString());
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

            Assert.AreEqual(100, gerenciador.Listar().Count());

        }


        #region Organizar depois

        public interface InterfaceSemAtributo
        {

        }

        [ModuloContrato("xpto")]
        public interface InterfaceModuloInvalido
        {

        }
        [ModuloContrato(typeof(OutroModuloDependenciaInterfaceValida))]
        public interface IModuloValido : IModulo
        {

        }

        [ModuloContrato(typeof(OutroModuloDependenciaInterfaceValida))]
        public interface IModuloInstanciaUnica : IModulo
        {

        }

        [Modulo]
        public class ModuloInstanciaUnica : ModuloBase, IModuloInstanciaUnica
        {
            public ModuloInstanciaUnica() : base(true)
            {
            }
        }

        public class ModuloInvallido
        {

        }

        public class ModuloSemAtributo : IModulo
        {
            public bool InstanciaUnica { get; }
            public string Versao { get; }
            public Estado Estado { get; }
            public string Nome { get; }
            public string Id { get; }

            private bool disposedValue;

            protected virtual void Dispose(bool disposing)
            {
                if (!disposedValue)
                {
                    disposedValue = true;
                }
            }



            public void Dispose()
            {

                Dispose(disposing: true);
                GC.SuppressFinalize(this);
            }
        }


        [Modulo]
        public class ModuloSemConstrutor : IModulo
        {
            private ModuloSemConstrutor()
            {

            }

            public bool InstanciaUnica { get; }
            public string Versao { get; }
            public Estado Estado { get; }
            public string Nome { get; }
            public string Id { get; }

            private bool disposedValue;

            protected virtual void Dispose(bool disposing)
            {
                if (!disposedValue)
                {
                    disposedValue = true;
                }
            }


            public void Dispose()
            {
                // Não altere este código. Coloque o código de limpeza no método 'Dispose(bool disposing)'
                Dispose(disposing: true);
                GC.SuppressFinalize(this);
            }
        }

        [Modulo]
        public class ModuloIntanciaUnica : ModuloBase
        {
            public ModuloIntanciaUnica() : base(true)
            {
            }
        }



        [Modulo]
        public class ModuloDependenciaInvalida : ModuloBase
        {
            public ModuloDependenciaInvalida(ModuloInvallido moduloInvallido) : base(true)
            {
            }
        }

        [Modulo]
        public class ModuloDependenciaInterfaceInvalidaOpcional : ModuloBase
        {
            public ModuloDependenciaInterfaceInvalidaOpcional(InterfaceModuloInvalido interfaceModuloInvalido = null) : base(false)
            {
            }
        }

        [Modulo]
        public class ModuloDependenciaValida : ModuloBase
        {
            public ModuloDependenciaValida(ModuloDependenciaInterfaceInvalidaOpcional modulo) : base(true)
            {
            }
        }

        [Modulo]
        public class OutroModuloDependenciaInterfaceValida : ModuloBase, IModuloValido
        {
            public OutroModuloDependenciaInterfaceValida() : base(false)
            {
            }
        }

        [Modulo]
        public class ModuloDependenciaInterfaceValida : ModuloBase
        {
            public ModuloDependenciaInterfaceValida(IModuloValido iModulo) : base(false)
            {
            }
        }

        [Modulo]
        public class ModuloParametroInvalido : ModuloBase, IModuloValido
        {
            public ModuloParametroInvalido(int a) : base(false)
            {
            }
        }

        [Modulo]
        public class ModuloParametroInvalidoOpcional : ModuloBase, IModuloValido
        {
            public ModuloParametroInvalidoOpcional(bool instanciaUnica = false) : base(instanciaUnica)
            {
            }
        }

        [ModuloContrato(default(string))]
        public interface IContratoInvalido : IModulo
        {

        }

        [ModuloContrato(default(Type))]
        public interface IContratoInvalidoTipo : IModulo
        {

        }

        [TestMethod()]
        [TestCategory("Todos")]
        public void TodosOstestes()
        {

            Assert.IsNotNull(gerenciador.Criar(typeof(IModuloInstanciaUnica)));
            Assert.IsTrue(gerenciador.Existe(typeof(IModuloInstanciaUnica)));

            Assert.IsNotNull(gerenciador.Criar(typeof(ModuloParametroInvalidoOpcional)));
            Assert.IsNotNull(gerenciador.Criar(typeof(ModuloDependenciaInterfaceInvalidaOpcional)));
            Assert.IsNotNull(gerenciador.Criar(typeof(ModuloDependenciaValida)));
            Assert.IsNotNull(gerenciador.Criar(typeof(ModuloDependenciaInterfaceValida)));
            Assert.IsNotNull(gerenciador.Criar(typeof(ModuloDependenciaInterfaceValida)).ToString());



            Assert.ThrowsException<ModuloDescartadoException>(() =>
            {
                var m = gerenciador.Criar(typeof(ModuloDependenciaInterfaceInvalidaOpcional));
                m.Dispose();
                gerenciador.Obter(m.Id);
            });

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                gerenciador.Obter(default(string));
            });

            Assert.ThrowsException<ArgumentException>(() =>
            {
                gerenciador.Criar(typeof(IContratoInvalido));
            });

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                gerenciador.Criar(typeof(IContratoInvalidoTipo));
            });

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                gerenciador.Criar(default(Type));
            });

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                gerenciador.Existe(default(Type));
            });

            Assert.ThrowsException<ModuloContratoNaoEncontratoException>(() =>
            {
                gerenciador.Criar(typeof(InterfaceSemAtributo));
            });

            Assert.ThrowsException<TipoModuloInvalidoException>(() =>
            {
                gerenciador.Criar(typeof(int));
            });

            Assert.ThrowsException<TipoModuloNaoEncontradoException>(() =>
            {
                gerenciador.Criar(typeof(InterfaceModuloInvalido));
            });

            Assert.ThrowsException<TipoModuloInvalidoException>(() =>
            {
                gerenciador.Criar(typeof(ModuloInvallido));
            });

            Assert.ThrowsException<TipoModuloInvalidoException>(() =>
            {
                gerenciador.Criar(typeof(ModuloSemAtributo));
            });

            Assert.ThrowsException<ModuloConstrutorAusenteException>(() =>
            {
                gerenciador.Criar(typeof(ModuloSemConstrutor));
            });

            Assert.ThrowsException<ModuloInstanciaUnicaException>(() =>
            {
                gerenciador.Criar(typeof(ModuloIntanciaUnica));
                gerenciador.Criar(typeof(ModuloIntanciaUnica));
            });

            Assert.ThrowsException<TipoModuloInvalidoException>(() =>
            {
                gerenciador.Criar(typeof(ModuloParametroInvalido));
            });


        }
        #endregion

    }
}