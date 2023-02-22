using Microsoft.VisualStudio.TestTools.UnitTesting;

using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Dinamico;
using Propeus.Modulo.DinamicoTests.Contrato;
using Propeus.Modulo.DinamicoTests.Modulo;

namespace Propeus.Modulo.DinamicoTests
{
    [TestCategory("Dinamico.Gerenciador")]
    [TestClass]
    public class GerenciadorTeste : IDisposable
    {
        private Gerenciador gerenciador;
        string ns = "Propeus.Modulo.DinamicoTestsModels";

        [TestInitialize]
        public void GerenciadorDinamicoIniciar()
        {
#if DEBUG
            Copiar(ns, modo: "Debug");
#else
             Copiar(ns,modo:"Release");
#endif

            try
            {
                gerenciador = new Gerenciador(Gerenciador.Atual);
            }
            catch
            {
                throw;
            }

            Assert.IsNotNull(gerenciador);

        }

        [TestMethod]
        public void CriarModuloInterfaceContratoNomeadoTipo()
        {


            IModulo modulo = gerenciador.Criar(typeof(IModuloSimplesContrato));
            Assert.IsNotNull(modulo);
        }

        [TestMethod]
        public void CriarModuloInterfaceContratoNomeadGenerico()
        {

            IModuloSimplesContrato modulo = gerenciador.Criar<IModuloSimplesContrato>();
            Assert.IsNotNull(modulo);
            modulo.Teste = 10;
            Assert.AreEqual(10, modulo.Teste);

        }

        [TestMethod]
        public void CriarModuloNomeadoTipo()
        {
            IModulo modulo = gerenciador.Criar(typeof(ModuloSimplesSemContrato));
            Assert.IsNotNull(modulo);
        }

        [TestMethod]
        public void CriarModuloNomeadoGenerico()
        {
            ModuloSimplesSemContrato modulo = gerenciador.Criar<ModuloSimplesSemContrato>();
            Assert.IsNotNull(modulo);
        }

        [TestMethod]
        public void CriarTipoInstanciaUnicaMultplasVezesTipo()
        {

            IModulo result = gerenciador.Criar(typeof(ModuloSimplesSemContratoInstanciaUnica));
            Assert.IsNotNull(result);
            Assert.ThrowsException<ArgumentException>(() =>
            {
                gerenciador.Criar(typeof(ModuloSimplesSemContratoInstanciaUnica));
            });
        }

        [TestMethod]
        public void CriarTipoInstanciaUnicaMultplasVezesGenerico()
        {

            IModulo result = gerenciador.Criar(typeof(ModuloSimplesSemContratoInstanciaUnica));
            Assert.IsNotNull(result);
            Assert.ThrowsException<ArgumentException>(() =>
            {
                gerenciador.Criar(typeof(ModuloSimplesSemContratoInstanciaUnica));
            });
        }

        [TestMethod]
        public void CriarTipoInterfaceContratoNaoReferenciadoMultiplos()
        {
            IModulo modulo = gerenciador.Criar(typeof(IModuloInstanciaUnicaContrato));
            Assert.IsNotNull(modulo);
            IModulo modulo2 = gerenciador.Criar(typeof(IModuloSimplesContrato));
            Assert.IsNotNull(modulo2);
        }


        [TestMethod]
        public void ObterModuloInterfaceContratoNomeadoTipo()
        {


            _ = gerenciador.Criar(typeof(IModuloSimplesContrato));
            IModulo modulo = gerenciador.Obter(typeof(IModuloSimplesContrato));
            Assert.IsNotNull(modulo);
        }

        [TestMethod]
        public void ObterModuloInterfaceContratoNomeadoGenerico()
        {
            gerenciador.Criar<IModuloSimplesContrato>();
            IModuloSimplesContrato modulo = gerenciador.Obter<IModuloSimplesContrato>();
            Assert.IsNotNull(modulo);
            modulo.Teste = 10;
            modulo.Teste_String_GetSet = "10";
            modulo.Teste_Bool_GetSet = true;
            modulo.Teste_Decimal_GetSet = 10;
            modulo.Teste_Double_GetSet = 10.3;
            modulo.Teste_Float_GetSet = 10.2f;
            Assert.AreEqual(10, modulo.Teste);
            Assert.AreEqual("10", modulo.Teste_String_GetSet);
            Assert.AreEqual(true, modulo.Teste_Bool_GetSet);
            Assert.AreEqual(10, modulo.Teste_Decimal_GetSet);
            Assert.AreEqual(10.3, modulo.Teste_Double_GetSet);
            Assert.AreEqual(10.2f, modulo.Teste_Float_GetSet);

            Assert.AreEqual("10", modulo.Teste_String_Get);
            Assert.AreEqual(true, modulo.Teste_Bool_Get);
            Assert.AreEqual(10, modulo.Teste_Decimal_Get);
            Assert.AreEqual(10.3, modulo.Teste_Double_Get);
            Assert.AreEqual(10.2f, modulo.Teste_Float_Get);

            Assert.ThrowsException<NotImplementedException>(() =>
            {
                _ = modulo.Teste_String_GetSet_NotImplemented;
            });
            Assert.ThrowsException<NotImplementedException>(() =>
            {
                _ = modulo.Teste_Double_GetSet_NotImplemented;
            });
            Assert.ThrowsException<NotImplementedException>(() =>
            {
                _ = modulo.Teste_Float_GetSet_NotImplemented;
            });
            Assert.ThrowsException<NotImplementedException>(() =>
            {
                _ = modulo.Teste_Decimal_GetSet_NotImplemented;
            });
            Assert.ThrowsException<NotImplementedException>(() =>
            {
                _ = modulo.Teste_Bool_GetSet_NotImplemented;
            });

            Assert.ThrowsException<NotImplementedException>(() =>
            {
                _ = modulo.Teste_String_Get_NotImplemented;
            });
            Assert.ThrowsException<NotImplementedException>(() =>
            {
                _ = modulo.Teste_Double_Get_NotImplemented;
            });
            Assert.ThrowsException<NotImplementedException>(() =>
            {
                _ = modulo.Teste_Float_Get_NotImplemented;
            });
            Assert.ThrowsException<NotImplementedException>(() =>
            {
                _ = modulo.Teste_Decimal_Get_NotImplemented;
            });
            Assert.ThrowsException<NotImplementedException>(() =>
            {
                _ = modulo.Teste_Bool_Get_NotImplemented;
            });
        }

        [TestMethod]
        public void ObterModuloNomeadoTipo()
        {
            _ = gerenciador.Criar(typeof(ModuloSimplesSemContrato));
            IModulo modulo = gerenciador.Obter(typeof(ModuloSimplesSemContrato));
            Assert.IsNotNull(modulo);
        }

        [TestMethod]
        public void ObterModuloNomeadoGenerico()
        {
            _ = gerenciador.Criar<ModuloSimplesSemContrato>();
            ModuloSimplesSemContrato modulo = gerenciador.Obter<ModuloSimplesSemContrato>();
            Assert.IsNotNull(modulo);
        }

        [TestMethod]
        public void ObterTipoInterfaceContratoNaoReferenciadoMultiplos()
        {


            _ = gerenciador.Criar(typeof(IModuloInstanciaUnicaContrato));
            IModulo modulo = gerenciador.Obter(typeof(IModuloInstanciaUnicaContrato));
            Assert.IsNotNull(modulo);

            _ = gerenciador.Criar(typeof(IModuloSimplesContrato));
            IModulo modulo2 = gerenciador.Obter(typeof(IModuloSimplesContrato));
            Assert.IsNotNull(modulo2);
        }

        [TestCleanup]
        public void GerenciadorDinamicoFinalizacao()
        {
            Core.Gerenciador.Atual.Dispose();
            gerenciador = null;
        }


        private void Copiar(string modulo, string vCore = "net7.0", string modo = "Debug")
        {
            string pathDll = modulo;
            Console.WriteLine(pathDll);
            DirectoryInfo d = new DirectoryInfo(Directory.GetCurrentDirectory());
            Console.WriteLine(d.FullName);
            d = d.Parent.Parent.Parent.Parent;
            Console.WriteLine(d.FullName);
            d = d.GetDirectories().First(p => p.Name == pathDll);
            Console.WriteLine(d.FullName);
            d = d.GetDirectories().First(p => p.Name == "bin");
            Console.WriteLine(d.FullName);
            d = d.GetDirectories().First(p => p.Name == modo);
            Console.WriteLine(d.FullName);
            d = d.GetDirectories().First(p => p.Name == vCore);
            Console.WriteLine(d.FullName);
            string path = d.GetFiles($"{pathDll}.dll").First().FullName;
            Console.WriteLine(path);

            string ldir = Path.Combine(Directory.GetCurrentDirectory(), $"{pathDll}.dll");
            File.Delete(ldir);
            File.Copy(path, ldir, true);
        }

        #region IDisposable Support
        private bool disposedValue = false; // Para detectar chamadas redundantes

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (!(gerenciador is null))
                    {
                        gerenciador.Dispose();
                    }
                }

                // TODO: liberar recursos n�o gerenciados (objetos n�o gerenciados) e substituir um finalizador abaixo.
                // TODO: definir campos grandes como nulos.

                disposedValue = true;
            }
        }

        // TODO: substituir um finalizador somente se Dispose(bool disposing) acima tiver o c�digo para liberar recursos n�o gerenciados.
        // ~GerenciadorTeste()
        // {
        //   // N�o altere este c�digo. Coloque o c�digo de limpeza em Dispose(bool disposing) acima.
        //   Dispose(false);
        // }

        // C�digo adicionado para implementar corretamente o padr�o descart�vel.
        public void Dispose()
        {
            // N�o altere este c�digo. Coloque o c�digo de limpeza em Dispose(bool disposing) acima.
            Dispose(true);
            // TODO: remover marca de coment�rio da linha a seguir se o finalizador for substitu�do acima.
            GC.SuppressFinalize(this);
        }
        #endregion


    }
}
