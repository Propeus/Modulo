using Microsoft.VisualStudio.TestTools.UnitTesting;
using Propeus.Modulo.Modelos.Interfaces;
using Propeus.Modulo.Util.Objects;

namespace Propeus.Modulo.Configuracao.Teste
{
    [TestClass]
    public class Teste
    {
        IGerenciador gerenciador;

        [TestInitialize]
        public void Inicio()
        {
            gerenciador = Core.Gerenciador.Atual.Criar<Dinamico.Gerenciador>();
        }

        [TestMethod]
        public void CarregarModulo()
        {
            var config = gerenciador.Criar<IConfiguracaoModuloContrato>();
            config["teste"] = "teste";
            Assert.IsNotNull(config["teste"]);
        }

        [TestMethod]
        public void PersistirDados()
        {
            var config = gerenciador.Criar<IConfiguracaoModuloContrato>();
            if (config["testePersistir"] is null || config["testePersistir"].IsNullOrEmpty())
            {
                config["testePersistir"] = "teste";
                gerenciador.Remover(config);
                config = gerenciador.Criar<IConfiguracaoModuloContrato>();
                Assert.IsNotNull(config["testePersistir"]);
            }
            else
            {
                Assert.IsNotNull(config["testePersistir"]);
            }
        }

        [TestMethod]
        public void InjecaoDependenciaPersistirDados()
        {
            var config = gerenciador.Criar<ISimplesModuloContrato>();
            Assert.IsNotNull(config.Funcionou());
        }

        [TestCleanup]
        public void Fim()
        {
            Core.Gerenciador.Atual.Dispose();
        }
    }
}
