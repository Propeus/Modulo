using Microsoft.VisualStudio.TestTools.UnitTesting;

using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Core;
using Propeus.Modulo.Core.Exceptions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Propeus.Modulo.Core.Tests
{
    [Modulo]
    public class TesteInstanciaUnicaModulo : ModuloBase
    {
        //TODO: Existe uma falha no construtor onde o valor defult esta vindo diferente do que foi definido no parametro
        public TesteInstanciaUnicaModulo(IGerenciador gerenciador) : base(gerenciador, true)
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
        public TesteInstanciaMultiplaModulo(IGerenciador gerenciador) : base(gerenciador, false)
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

        //Criar
        [TestMethod()]
        [TestCategory("Criar")]
        public void CriarModuloInstanciaUnica()
        {
            using (IGerenciador gerenciador = Gerenciador.Atual)
            {
                IModulo modulo = gerenciador.Criar<TesteInstanciaUnicaModulo>();
                Assert.IsNotNull(modulo);
            }
        }
        [TestMethod()]
        [TestCategory("Criar")]
        public void CriarModuloInstanciaUnica_ArgumentException()
        {
            using (IGerenciador gerenciador = Gerenciador.Atual)
            {
                {
                    IModulo modulo = gerenciador.Criar<TesteInstanciaUnicaModulo>();
                    Assert.IsNotNull(modulo);
                    Assert.ThrowsException<ModuloInstanciaUnicaException>(() =>
                    {
                        modulo = gerenciador.Criar<TesteInstanciaUnicaModulo>();
                    });
                }
            }
        }
        [TestMethod()]
        [TestCategory("Criar")]
        public void CriarModuloInstanciaMultipla()
        {
            using (IGerenciador gerenciador = Gerenciador.Atual)
            {
                IModulo modulo = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
                Assert.IsNotNull(modulo);
            }
        }
        [TestMethod()]
        [TestCategory("Criar")]
        public void CriarModuloInstanciaMultipla_multiplosModulos()
        {
            using (IGerenciador gerenciador = Gerenciador.Atual)
            {
                IModulo modulo = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
                Assert.IsNotNull(modulo);
                IModulo modulov2 = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
                Assert.IsNotNull(modulov2);
                Assert.AreNotEqual(modulo, modulov2);
                Assert.AreNotEqual(modulo.Id, modulov2.Id);
            }
        }

        [TestMethod()]
        [TestCategory("Criar")]
        public void CriarModuloInstanciaUnicaPorTipo()
        {
            using (IGerenciador gerenciador = Gerenciador.Atual)
            {
                IModulo modulo = gerenciador.Criar(typeof(TesteInstanciaUnicaModulo));
                Assert.IsNotNull(modulo);
            }
        }
        [TestMethod()]
        [TestCategory("Criar")]
        public void CriarModuloInstanciaUnicaPorTipo_ArgumentException()
        {
            using (IGerenciador gerenciador = Gerenciador.Atual)
            {
                {
                    IModulo modulo = gerenciador.Criar(typeof(TesteInstanciaUnicaModulo));
                    Assert.IsNotNull(modulo);
                    Assert.ThrowsException<ModuloInstanciaUnicaException>(() =>
                    {
                        modulo = gerenciador.Criar(typeof(TesteInstanciaUnicaModulo));
                    });
                }
            }
        }
        [TestMethod()]
        [TestCategory("Criar")]
        public void CriarModuloInstanciaMultiplaPorTipo()
        {
            using (IGerenciador gerenciador = Gerenciador.Atual)
            {
                IModulo modulo = gerenciador.Criar(typeof(TesteInstanciaMultiplaModulo));
                Assert.IsNotNull(modulo);
            }
        }
        [TestMethod()]
        [TestCategory("Criar")]
        public void CriarModuloInstanciaMultiplaPorTipo_MultiplosModulos()
        {
            using (IGerenciador gerenciador = Gerenciador.Atual)
            {
                IModulo modulo = gerenciador.Criar(typeof(TesteInstanciaMultiplaModulo));
                Assert.IsNotNull(modulo);
                IModulo modulov2 = gerenciador.Criar(typeof(TesteInstanciaMultiplaModulo));
                Assert.IsNotNull(modulov2);
                Assert.AreNotEqual(modulo, modulov2);
                Assert.AreNotEqual(modulo.Id, modulov2.Id);
            }
        }

        [TestMethod()]
        [TestCategory("Criar")]
        public void CriarModuloInstanciaUnicaPorNome()
        {
            using (IGerenciador gerenciador = Gerenciador.Atual)
            {
                IModulo modulo = gerenciador.Criar(nameof(TesteInstanciaUnicaModulo));
                Assert.IsNotNull(modulo);
            }
        }
        [TestMethod()]
        [TestCategory("Criar")]
        public void CriarModuloInstanciaUnicaPorNome_ModuloInstanciaUnicaException()
        {
            using (IGerenciador gerenciador = Gerenciador.Atual)
            {
                {
                    IModulo modulo = gerenciador.Criar(nameof(TesteInstanciaUnicaModulo));
                    Assert.IsNotNull(modulo);
                    Assert.ThrowsException<ModuloInstanciaUnicaException>(() =>
                    {
                        modulo = gerenciador.Criar(nameof(TesteInstanciaUnicaModulo));
                    });
                }
            }
        }
        [TestMethod()]
        [TestCategory("Criar")]
        public void CriarModuloInstanciaMultiplaPorNome()
        {
            using (IGerenciador gerenciador = Gerenciador.Atual)
            {
                IModulo modulo = gerenciador.Criar(nameof(TesteInstanciaMultiplaModulo));
                Assert.IsNotNull(modulo);
            }
        }
        [TestMethod()]
        [TestCategory("Criar")]
        public void CriarModuloInstanciaMultiplaPorNome_MultiplosModulos()
        {
            using (IGerenciador gerenciador = Gerenciador.Atual)
            {
                IModulo modulo = gerenciador.Criar(nameof(TesteInstanciaMultiplaModulo));
                Assert.IsNotNull(modulo);
                IModulo modulov2 = gerenciador.Criar(nameof(TesteInstanciaMultiplaModulo));
                Assert.IsNotNull(modulov2);
                Assert.AreNotEqual(modulo, modulov2);
                Assert.AreNotEqual(modulo.Id, modulov2.Id);
            }
        }

        //Remover
        [TestMethod()]
        [TestCategory("Remover")]
        public void RemoverModuloInstanciaUnica()
        {
            using (IGerenciador gerenciador = Gerenciador.Atual)
            {
                IModulo modulo = gerenciador.Criar<TesteInstanciaUnicaModulo>();
                Assert.IsNotNull(modulo);
                Gerenciador.Atual.Remover(modulo);
            }
        }
        [TestMethod()]
        [TestCategory("Remover")]
        public void RemoverModuloInstanciaMultipla()
        {
            using (IGerenciador gerenciador = Gerenciador.Atual)
            {
                IModulo modulo = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
                Assert.IsNotNull(modulo);
                gerenciador.Remover(modulo);
            }
        }
        [TestMethod()]
        [TestCategory("Remover")]
        public void RemoverModuloInstanciaUnica_loop()
        {
            using (IGerenciador gerenciador = Gerenciador.Atual)
            {
                for (int i = 0; i < 100; i++)
                {
                    IModulo modulo = gerenciador.Criar<TesteInstanciaUnicaModulo>();
                    Assert.IsNotNull(modulo);
                    gerenciador.Remover(modulo);
                }
            }
        }
        [TestMethod()]
        [TestCategory("Remover")]
        public void RemoverModuloInstanciaMultipla_loop()
        {
            using (IGerenciador gerenciador = Gerenciador.Atual)
            {
                for (int i = 0; i < 100; i++)
                {
                    IModulo modulo = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
                    Assert.IsNotNull(modulo);
                    gerenciador.Remover(modulo);
                }
            }
        }
        [TestMethod()]
        [TestCategory("Remover")]
        public void RemoverModuloInstanciaUnicaPorId()
        {
            using (IGerenciador gerenciador = Gerenciador.Atual)
            {
                IModulo modulo = gerenciador.Criar<TesteInstanciaUnicaModulo>();
                Assert.IsNotNull(modulo);
                gerenciador.Remover(modulo.Id);
            }
        }
        [TestMethod()]
        [TestCategory("Remover")]
        public void RemoverModuloInstanciaMultiplaPorId()
        {
            using (IGerenciador gerenciador = Gerenciador.Atual)
            {
                IModulo modulo = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
                Assert.IsNotNull(modulo);
                gerenciador.Remover(modulo.Id);
            }
        }
        [TestMethod()]
        [TestCategory("Remover")]
        public void RemoverModuloInstanciaUnicaPorId_loop()
        {
            using (IGerenciador gerenciador = Gerenciador.Atual)
            {
                for (int i = 0; i < 100; i++)
                {
                    IModulo modulo = gerenciador.Criar<TesteInstanciaUnicaModulo>();
                    Assert.IsNotNull(modulo);
                    gerenciador.Remover(modulo.Id);
                }
            }
        }
        [TestMethod()]
        [TestCategory("Remover")]
        public void RemoverModuloInstanciaMultiplaPorId_loop()
        {
            using (IGerenciador gerenciador = Gerenciador.Atual)
            {
                for (int i = 0; i < 100; i++)
                {
                    IModulo modulo = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
                    Assert.IsNotNull(modulo);
                    gerenciador.Remover(modulo.Id);
                }
            }
        }
        [TestMethod()]
        [TestCategory("Remover")]
        public void RemoverTodosModulos()
        {
            using (IGerenciador gerenciador = Gerenciador.Atual)
            {
                IModulo[] modulos = new IModulo[100];

                for (int i = 0; i < 100; i++)
                {
                    modulos[i] = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
                    Assert.IsNotNull(modulos[i]);
                }

                gerenciador.RemoverTodos();
                Assert.AreEqual(0, (gerenciador as IGerenciadorDiagnostico).ModulosInicializados);

                modulos.All(m => m.Estado == Estado.Desligado);
            }
        }
        [TestMethod()]
        [TestCategory("Remover")]
        public void RemoverModuloInexistente()
        {
            using (IGerenciador gerenciador = Gerenciador.Atual)
            {
                IModulo modulo = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
                Assert.IsNotNull(modulo);
                gerenciador.Remover(modulo.Id);

                Assert.AreEqual(0, (gerenciador as IGerenciadorDiagnostico).ModulosInicializados);
                Assert.ThrowsException<ModuloNaoEncontradoException>(() =>
                {
                    gerenciador.Remover(modulo.Id);
                });

            }
        }

        //Obter
        [TestMethod()]
        [TestCategory("Obter")]
        public void ObterModuloInstanciaUnicaPorTipo()
        {
            using (IGerenciador gerenciador = Gerenciador.Atual)
            {
                IModulo modulo = gerenciador.Criar<TesteInstanciaUnicaModulo>();
                Assert.IsNotNull(modulo);
                IModulo modulov2 = gerenciador.Obter<TesteInstanciaUnicaModulo>();
                Assert.AreEqual(modulo, modulov2);
            }
        }
        [TestMethod()]
        [TestCategory("Obter")]
        public void ObterModuloInstanciaMultiplaPorTipo()
        {
            using (IGerenciador gerenciador = Gerenciador.Atual)
            {
                IModulo modulo = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
                Assert.IsNotNull(modulo);
                IModulo modulov2 = gerenciador.Obter<TesteInstanciaMultiplaModulo>();
                Assert.AreEqual(modulo, modulov2);
            }
        }
        [TestMethod()]
        [TestCategory("Obter")]
        public void ObterModuloInstanciaMultipla_MultiplosModulosPorTipo()
        {
            using (IGerenciador gerenciador = Gerenciador.Atual)
            {
                IModulo modulo = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
                Assert.IsNotNull(modulo);
                IModulo modulov2 = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
                Assert.IsNotNull(modulov2);
                Assert.AreNotEqual(modulo, modulov2);
                IModulo modulov3 = gerenciador.Obter<TesteInstanciaMultiplaModulo>();
                Assert.IsNotNull(modulov3);
            }
        }
        [TestMethod()]
        [TestCategory("Obter")]
        public void ObterModuloInstanciaUnicaPorId()
        {
            using (IGerenciador gerenciador = Gerenciador.Atual)
            {
                IModulo modulo = gerenciador.Criar<TesteInstanciaUnicaModulo>();
                Assert.IsNotNull(modulo);
                IModulo modulov2 = gerenciador.Obter(modulo.Id);
                Assert.AreEqual(modulo, modulov2);
            }
        }
        [TestMethod()]
        [TestCategory("Obter")]
        public void ObterModuloInstanciaMultiplaPorId()
        {
            using (IGerenciador gerenciador = Gerenciador.Atual)
            {
                IModulo modulo = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
                Assert.IsNotNull(modulo);
                IModulo modulov2 = gerenciador.Obter<TesteInstanciaMultiplaModulo>();
                Assert.AreEqual(modulo.Id, modulov2.Id);
            }
        }
        [TestMethod()]
        [TestCategory("Obter")]
        public void ObterModuloInstanciaMultipla_MultiplosModulosPorId()
        {
            using (IGerenciador gerenciador = Gerenciador.Atual)
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
        }
        [TestMethod()]
        [TestCategory("Obter")]
        public void ObterModuloInstanciaPorIdInexistente_ArgumentException()
        {
            using (IGerenciador gerenciador = Gerenciador.Atual)
            {
                Assert.ThrowsException<ModuloNaoEncontradoException>(() =>
                {
                    IModulo modulo = gerenciador.Obter(Guid.NewGuid().ToString());
                });
            }
        }

        //Existe
        [TestMethod()]
        [TestCategory("Existe")]
        public void ExisteModuloInstanciaUnicaPorInstancia()
        {
            using (IGerenciador gerenciador = Gerenciador.Atual)
            {
                IModulo modulo = gerenciador.Criar<TesteInstanciaUnicaModulo>();
                Assert.IsNotNull(modulo);
                Assert.IsTrue(gerenciador.Existe(modulo));
            }
        }
        [TestMethod()]
        [TestCategory("Existe")]
        public void ExisteModuloInstanciaMultiplaPorInstancia()
        {
            using (IGerenciador gerenciador = Gerenciador.Atual)
            {
                IModulo modulo = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
                Assert.IsNotNull(modulo);
                Assert.IsTrue(gerenciador.Existe(modulo));
            }
        }
        [TestMethod()]
        [TestCategory("Existe")]
        public void ExisteModuloInstanciaMultipla_MultiplosModulosPorInstancia()
        {
            using (IGerenciador gerenciador = Gerenciador.Atual)
            {
                IModulo modulo = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
                Assert.IsNotNull(modulo);
                IModulo modulov2 = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
                Assert.IsNotNull(modulov2);
                Assert.AreNotEqual(modulo, modulov2);
                Assert.IsTrue(gerenciador.Existe(modulo));
                Assert.IsTrue(gerenciador.Existe(modulov2));
            }
        }
        [TestMethod()]
        [TestCategory("Existe")]
        public void ExisteModuloInstanciaUnicaPorTipo()
        {
            using (IGerenciador gerenciador = Gerenciador.Atual)
            {
                IModulo modulo = gerenciador.Criar<TesteInstanciaUnicaModulo>();
                Assert.IsNotNull(modulo);
                Assert.IsTrue(gerenciador.Existe(typeof(TesteInstanciaUnicaModulo)));
            }
        }
        [TestMethod()]
        [TestCategory("Existe")]
        public void ExisteModuloInstanciaMultiplaPorTipo()
        {
            using (IGerenciador gerenciador = Gerenciador.Atual)
            {
                IModulo modulo = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
                Assert.IsNotNull(modulo);
                Assert.IsTrue(gerenciador.Existe(typeof(TesteInstanciaMultiplaModulo)));
            }
        }
        [TestMethod()]
        [TestCategory("Existe")]
        public void ExisteModuloInstanciaMultipla_MultiplosModulosPorTipo()
        {
            using (IGerenciador gerenciador = Gerenciador.Atual)
            {
                IModulo modulo = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
                Assert.IsNotNull(modulo);
                IModulo modulov2 = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
                Assert.IsNotNull(modulov2);
                Assert.AreNotEqual(modulo, modulov2);
                Assert.IsTrue(gerenciador.Existe(typeof(TesteInstanciaMultiplaModulo)));
            }
        }
        [TestMethod()]
        [TestCategory("Existe")]
        public void ExisteModuloInstanciaUnicaPorId()
        {
            using (IGerenciador gerenciador = Gerenciador.Atual)
            {
                IModulo modulo = gerenciador.Criar<TesteInstanciaUnicaModulo>();
                Assert.IsNotNull(modulo);
                Assert.IsTrue(gerenciador.Existe(modulo.Id));
            }
        }
        [TestMethod()]
        [TestCategory("Existe")]
        public void ExisteModuloInstanciaMultiplaPorId()
        {
            using (IGerenciador gerenciador = Gerenciador.Atual)
            {
                IModulo modulo = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
                Assert.IsNotNull(modulo);
                Assert.IsTrue(gerenciador.Existe(modulo.Id));
            }
        }
        [TestMethod()]
        [TestCategory("Existe")]
        public void ExisteModuloInstanciaMultipla_MultiplosModulosPorId()
        {
            using (IGerenciador gerenciador = Gerenciador.Atual)
            {
                IModulo modulo = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
                Assert.IsNotNull(modulo);
                IModulo modulov2 = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
                Assert.IsNotNull(modulov2);
                Assert.AreNotEqual(modulo, modulov2);
                Assert.IsTrue(gerenciador.Existe(modulov2.Id));
            }
        }
        [TestMethod()]
        [TestCategory("Existe")]
        public void ExisteModuloInstanciaPorIdInexistente()
        {
            using (IGerenciador gerenciador = Gerenciador.Atual)
            {
                Assert.IsFalse(gerenciador.Existe(Guid.NewGuid().ToString()));
            }
        }

        //Reiniciar
        [TestMethod()]
        [TestCategory("Reiniciar")]
        public void ReiniciarModuloInstanciaUnicaPorInstancia()
        {
            using (IGerenciador gerenciador = Gerenciador.Atual)
            {
                IModulo modulo = gerenciador.Criar<TesteInstanciaUnicaModulo>();
                Assert.IsNotNull(modulo);
                Assert.IsTrue(gerenciador.Existe(modulo));
                IModulo modulov2 = gerenciador.Reiniciar(modulo);
                Assert.IsNotNull(modulov2);
                Assert.IsTrue(gerenciador.Existe(modulov2));
                Assert.IsFalse(gerenciador.Existe(modulo));
            }
        }
        [TestMethod()]
        [TestCategory("Reiniciar")]
        public void ReiniciarModuloInstanciaMultiplaPorInstancia()
        {
            using (IGerenciador gerenciador = Gerenciador.Atual)
            {
                IModulo modulo = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
                Assert.IsNotNull(modulo);
                Assert.IsTrue(gerenciador.Existe(modulo));
                IModulo modulov2 = gerenciador.Reiniciar(modulo);
                Assert.IsNotNull(modulov2);
                Assert.IsTrue(gerenciador.Existe(modulov2));
                Assert.IsFalse(gerenciador.Existe(modulo));
            }
        }
        [TestMethod()]
        [TestCategory("Reiniciar")]
        public void ReiniciarModuloInstanciaUnicaPorId()
        {
            using (IGerenciador gerenciador = Gerenciador.Atual)
            {
                IModulo modulo = gerenciador.Criar<TesteInstanciaUnicaModulo>();
                Assert.IsNotNull(modulo);
                Assert.IsTrue(gerenciador.Existe(modulo));
                IModulo modulov2 = gerenciador.Reiniciar(modulo.Id);
                Assert.IsNotNull(modulov2);
                Assert.IsTrue(gerenciador.Existe(modulov2));
                Assert.IsFalse(gerenciador.Existe(modulo));
            }
        }
        [TestMethod()]
        [TestCategory("Reiniciar")]
        public void ReiniciarModuloInstanciaMultiplaPorId()
        {
            using (IGerenciador gerenciador = Gerenciador.Atual)
            {
                IModulo modulo = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
                Assert.IsNotNull(modulo);
                Assert.IsTrue(gerenciador.Existe(modulo));
                IModulo modulov2 = gerenciador.Reiniciar(modulo.Id);
                Assert.IsNotNull(modulov2);
                Assert.IsTrue(gerenciador.Existe(modulov2));
                Assert.IsFalse(gerenciador.Existe(modulo));
            }
        }
        [TestMethod()]
        [TestCategory("Reiniciar")]
        public void ReiniciarModuloInstanciaPorIdInexistente_ArgumentException()
        {
            using (IGerenciador gerenciador = Gerenciador.Atual)
            {
                Assert.ThrowsException<ModuloNaoEncontradoException>(() =>
                {
                    IModulo modulov2 = gerenciador.Reiniciar(Guid.NewGuid().ToString());
                });
            }
        }

        //Listar
        [TestMethod()]
        [TestCategory("Listar")]
        public void ListarModulos_loop()
        {
            using (IGerenciador gerenciador = Gerenciador.Atual)
            {
                for (int i = 0; i < 100; i++)
                {
                    _ = gerenciador.Criar<TesteInstanciaMultiplaModulo>();
                }

                Assert.AreEqual(100,gerenciador.Listar().Count());
            }
        }
    }
}