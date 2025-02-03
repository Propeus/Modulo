using System;
using System.Linq;
using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Propeus.Module.Abstract;
using Propeus.Module.Abstract.Exceptions;
using Propeus.Module.Abstract.Interfaces;
using Propeus.Module.Manager;
using Propeus.Module.ManagerTests.Contratos;
using Propeus.Module.ManagerTests.Modulos;

namespace Propeus.Module.ManagerTests
{
    [TestClass()]
    public class ModuleManagerTests
    {
        /**
 * - Metodo de extenção para obter ou criar um modulo
 * */
        [TestMethod()]
        public void ExtensionGetOrCreate_1()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {

                var module = moduleManager.CreateOrGetModule<PessoaModule>();
                var module2 = moduleManager.CreateOrGetModule<PessoaModule>();
                Assert.AreEqual(module.Id, module2.Id);
                Assert.IsNotNull(moduleManager.ToString());

            }
        }
        /**
     * - Verifica se existe um modulo por id
     * */
        [TestMethod()]
        public void ExistsModule_5()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {

                var module = moduleManager.CreateModule<PessoaModule>();
                Assert.IsTrue(moduleManager.ExistsModule(module));
                Assert.IsTrue(moduleManager.ExistsModule(module.Id));

            }
        }
        /**
       * - Verifica se existe um modulo inexistente
       * */
        [TestMethod()]
        public void ExistsModule_4()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                Assert.ThrowsException<ArgumentNullException>(() =>
                {
                    Type? type = null;
                    moduleManager.ExistsModule(type);
                });
            }
        }
        /**
       * - Verifica se existe um modulo inexistente
       * */
        [TestMethod()]
        public void ExistsModule_3()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                Assert.ThrowsException<ArgumentNullException>(() =>
                {
                    moduleManager.ExistsModule(default(IModule));
                });
            }
        }

        /**
        * - Verifica se existe um modulo por id vazio
        * */
        [TestMethod()]
        public void ExistsModule_2()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                Assert.ThrowsException<ArgumentNullException>(() =>
                {
                    Assert.IsFalse(moduleManager.ExistsModule(""));
                });
            }
        }
        /**
         * - Verifica se existe um modulo inexistente
         * */
        [TestMethod()]
        public void ExistsModule_1()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                Assert.IsFalse(moduleManager.ExistsModule("moduloInexistente"));
            }
        }

        /**
         * - Aguarda o estado inalcancavel por um periodo de tempo
         */
        [TestMethod()]
        public void WaitModule_4()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                var module = moduleManager.CreateModule<PessoaModule>();
                module.WaitModuleState(State.Ready);

            }
        }

        /**
         * - Aguarda um modulo nulo com cancelation token
         */
        [TestMethod()]
        public void WaitModule_3()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                Assert.ThrowsException<ArgumentNullException>(() =>
                {
                    default(IModule).WaitModuleState(State.Running);
                });

            }
        }
        /**
         * - Aguarda um modulo com cancelation token
         */
        [TestMethod()]
        public void WaitModule_2()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                var t = new CancellationTokenSource();
                t.CancelAfter(TimeSpan.FromSeconds(5));
                var module = moduleManager.CreateModule<PessoaModule>();
                module.WaitModuleState(State.Running, t.Token);

            }
        }
        /**
          * - Aguarda um modulo 
          */
        [TestMethod()]
        public void WaitModule_1()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                var module = moduleManager.CreateModule<PessoaModule>();
                module.WaitModuleState(State.Running);
            }
        }

        /**
         * - Cria um modulo dependendo de outro
         */
        [TestMethod()]
        public void CreateModule_35()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {

                var module = moduleManager.CreateModule<PJModule>();
                Assert.IsNotNull(module.PFModule);
                Assert.IsNotNull(module.PFModule.ModuleManager);
                Assert.AreEqual(module.PFModule.ModuleManager.Id, moduleManager.Id);

            }
        }

        /**
         * - Quando o objeto nao é classe e nem interface
         */
        [TestMethod()]
        public void CreateModule_34()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                Assert.ThrowsException<ModuleTypeInvalidException>(() =>
                {
                    var module = moduleManager.CreateModule<StrucIsNotModule>();
                });
            }
        }

        /**
         * - Quando o modulo nao implementa o atributo 
         */
        [TestMethod()]
        public void CreateModule_33()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                Assert.ThrowsException<ModuleTypeInvalidException.TypeModuleUnmarkedException>(() =>
                {
                    var module = moduleManager.CreateModule<ModuleAttributeNotImplementedModule>();
                });
            }
        }

        /**
        * - Quando o modulo nao implementa o IModule 
        */
        [TestMethod()]
        public void CreateModule_32()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                Assert.ThrowsException<ModuleTypeInvalidException.TypeModuleNotInheritedException>(() =>
                {
                    var module = moduleManager.CreateModule(typeof(ModuleNotImplemented));
                });
            }
        }

        /**
        * - Gerenciador de modulos descartado
        * */
        [TestMethod()]
        public void CreateModule_31()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                Assert.ThrowsException<ModuleManagerDisposedException>(() =>
                {
                    moduleManager.Dispose();
                    var module = moduleManager.CreateModule<StateOffModule>();
                });
            }
        }

        /**
        * - Criar modulo e forcar estado em off e tentar recuperar
        * */
        [TestMethod()]
        public void CreateModule_30()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                Assert.ThrowsException<ModuleDisposedException>(() =>
                {
                    var module = moduleManager.CreateModule<StateOffModule>();
                    module.ForceStateOff();
                    moduleManager.GetModule(module.Id);
                });
            }
        }

        /**
         * - Criar modulo interface de contrato sem anotacao
         * */
        [TestMethod()]
        public void CreateModule_29()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                Assert.ThrowsException<ModuleContractNotFoundException>(() =>
                {
                    var module = moduleManager.CreateModule<IJogoContrato>();
                });
            }
        }

        /**
        * - Criar modulo com interface de contrato por tipo
        * */
        [TestMethod()]
        public void CreateModule_28()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                Assert.ThrowsException<ModuleContractInvalidException>(() =>
                {
                    var module = moduleManager.CreateModule<IVerduraContrato>();
                });
            }
        }

        /**
          * - Criar modulo com interface de contrato por string
          * */
        [TestMethod()]
        public void CreateModule_27()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                Assert.ThrowsException<ModuleContractInvalidException>(() =>
                {
                    var module = moduleManager.CreateModule<IPlantaContrato>();
                });
            }
        }

        /**
        * - Criar modulo com interface de contrato por string
        * */
        [TestMethod()]
        public void CreateModule_26()
        {

            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                var module = moduleManager.CreateModule<IMesaContrato>();
                Assert.IsNotNull(module);
                Assert.IsNotNull(module.ToString());
                Assert.IsNotNull(module.ManifestId);
                Assert.IsTrue(moduleManager.InitializedModules > 0);
            }
        }

        /**
        * - Criar modulo com tipo nulo
        * */
        [TestMethod()]
        public void CreateModule_25()
        {

            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                Assert.ThrowsException<ModuleBuilderAbsentException>(() =>
                {
                    moduleManager.CreateModule<AnimalModule>();
                });
            }
        }

        /**
        * - Criar modulo com quantidade de parametros diferentes
        * */
        [TestMethod()]
        public void CreateModule_24()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                string NME_PESSOA = "Joao";

                var modulo = moduleManager.CreateModule<PessoaModule>(new object[] { NME_PESSOA });
                Assert.IsNotNull(modulo);
                Assert.IsInstanceOfType(modulo, typeof(PessoaModule));
                Assert.AreEqual(NME_PESSOA, modulo.Nome);
                Assert.AreEqual(0, modulo.Idade);
                Assert.IsNull(modulo.Cpf);
                Assert.AreEqual("a", modulo.Genero);
            }
        }

        /**
         * - Criar modulo com tipo nulo
         * */
        [TestMethod()]
        public void CreateModule_23()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                Assert.ThrowsException<ArgumentNullException>(() =>
                {
                    moduleManager.CreateModule(moduleType: null);
                });
            }
        }

        /**
         * - Criar modulo inexistente pelo nome
         * */
        [TestMethod()]
        public void CreateModule_22()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                Assert.ThrowsException<ModuleTypeNotFoundException>(() =>
                {
                    moduleManager.CreateModule("moduloInexistente");
                });
            }
        }

        /**
         * - Criar modulo de instancia unica com quatro parametros
         * - Criar por nameof
         * - Criar por contrato
         * */
        [TestMethod()]
        public void CreateModule_21()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                string NME_PESSOA = "Joao";
                int IDADE_PESSOA = 10;
                string CPF = "12345678912";
                string GENERO = "gato";

                var modulo = moduleManager.CreateModule(nameof(IPessoaContrato), new object[] { NME_PESSOA, IDADE_PESSOA, CPF, GENERO }) as IPessoaContrato;
                Assert.IsNotNull(modulo);
                Assert.IsInstanceOfType(modulo, typeof(PessoaModule));
                Assert.AreEqual(NME_PESSOA, modulo.Nome);
                Assert.AreEqual(IDADE_PESSOA, modulo.Idade);
                Assert.AreEqual(CPF, modulo.Cpf);
                Assert.AreEqual(GENERO, modulo.Genero);
            }
        }

        /**
         * - Criar modulo de instancia unica com quatro parametros
         * - Criar por typeof
         * - Criar por contrato
         * */
        [TestMethod()]
        public void CreateModule_20()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                string NME_PESSOA = "Joao";
                int IDADE_PESSOA = 10;
                string CPF = "12345678912";
                string GENERO = "gato";

                var modulo = moduleManager.CreateModule(typeof(IPessoaContrato), new object[] { NME_PESSOA, IDADE_PESSOA, CPF, GENERO }) as IPessoaContrato;
                Assert.IsNotNull(modulo);
                Assert.IsInstanceOfType(modulo, typeof(PessoaModule));
                Assert.AreEqual(NME_PESSOA, modulo.Nome);
                Assert.AreEqual(IDADE_PESSOA, modulo.Idade);
                Assert.AreEqual(CPF, modulo.Cpf);
                Assert.AreEqual(GENERO, modulo.Genero);
            }
        }

        /**
         * - Criar modulo de instancia unica com quatro parametros
         * - Criar por tipo
         * - Criar por contrato
         * */
        [TestMethod()]
        public void CreateModule_19()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                string NME_PESSOA = "Joao";
                int IDADE_PESSOA = 10;
                string CPF = "12345678912";
                string GENERO = "gato";

                var modulo = moduleManager.CreateModule<IPessoaContrato>(new object[] { NME_PESSOA, IDADE_PESSOA, CPF, GENERO });
                Assert.IsNotNull(modulo);
                Assert.IsInstanceOfType(modulo, typeof(PessoaModule));
                Assert.AreEqual(NME_PESSOA, modulo.Nome);
                Assert.AreEqual(IDADE_PESSOA, modulo.Idade);
                Assert.AreEqual(CPF, modulo.Cpf);
                Assert.AreEqual(GENERO, modulo.Genero);
            }
        }

        /**
         * - Criar modulo de instancia unica com dois parametros
         * - Criar por nameof
         * - Criar por contrato
         * */
        [TestMethod()]
        public void CreateModule_18()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                string NME_PESSOA = "Joao";
                int IDADE_PESSOA = 10;

                var modulo = moduleManager.CreateModule(nameof(IPessoaContrato), new object[] { NME_PESSOA, IDADE_PESSOA }) as IPessoaContrato;
                Assert.IsNotNull(modulo);
                Assert.IsInstanceOfType(modulo, typeof(PessoaModule));
                Assert.AreEqual(NME_PESSOA, modulo.Nome);
                Assert.AreEqual(IDADE_PESSOA, modulo.Idade);
            }
        }

        /**
         * - Criar modulo de instancia unica com dois parametros
         * - Criar por typeof
         * - Criar por contrato
         * */
        [TestMethod()]
        public void CreateModule_17()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                string NME_PESSOA = "Joao";
                int IDADE_PESSOA = 10;

                var modulo = moduleManager.CreateModule(typeof(IPessoaContrato), new object[] { NME_PESSOA, IDADE_PESSOA }) as IPessoaContrato;
                Assert.IsNotNull(modulo);
                Assert.IsInstanceOfType(modulo, typeof(PessoaModule));
                Assert.AreEqual(NME_PESSOA, modulo.Nome);
                Assert.AreEqual(IDADE_PESSOA, modulo.Idade);
            }
        }

        /**
         * - Criar modulo de instancia unica com dois parametros
         * - Criar por tipo
         * - Criar por contrato
         * */
        [TestMethod()]
        public void CreateModule_16()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                string NME_PESSOA = "Joao";
                int IDADE_PESSOA = 10;

                var modulo = moduleManager.CreateModule<IPessoaContrato>(new object[] { NME_PESSOA, IDADE_PESSOA });
                Assert.IsNotNull(modulo);
                Assert.IsInstanceOfType(modulo, typeof(PessoaModule));
                Assert.AreEqual(NME_PESSOA, modulo.Nome);
                Assert.AreEqual(IDADE_PESSOA, modulo.Idade);
            }
        }

        /**
         * - Criar modulo de instancia unica com quatro parametros
         * - Criar por nameof
         * */
        [TestMethod()]
        public void CreateModule_15()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                string NME_PESSOA = "Joao";
                int IDADE_PESSOA = 10;
                string CPF = "12345678912";
                string GENERO = "gato";

                var modulo = moduleManager.CreateModule(nameof(PessoaModule), new object[] { NME_PESSOA, IDADE_PESSOA, CPF, GENERO }) as PessoaModule;
                Assert.IsNotNull(modulo);
                Assert.IsInstanceOfType(modulo, typeof(PessoaModule));
                Assert.AreEqual(NME_PESSOA, modulo.Nome);
                Assert.AreEqual(IDADE_PESSOA, modulo.Idade);
                Assert.AreEqual(CPF, modulo.Cpf);
                Assert.AreEqual(GENERO, modulo.Genero);
            }
        }

        /**
         * - Criar modulo de instancia unica com quatro parametros
         * - Criar por typeof
         * */
        [TestMethod()]
        public void CreateModule_14()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                string NME_PESSOA = "Joao";
                int IDADE_PESSOA = 10;
                string CPF = "12345678912";
                string GENERO = "gato";

                var modulo = moduleManager.CreateModule(typeof(PessoaModule), new object[] { NME_PESSOA, IDADE_PESSOA, CPF, GENERO }) as PessoaModule;
                Assert.IsNotNull(modulo);
                Assert.IsInstanceOfType(modulo, typeof(PessoaModule));
                Assert.AreEqual(NME_PESSOA, modulo.Nome);
                Assert.AreEqual(IDADE_PESSOA, modulo.Idade);
                Assert.AreEqual(CPF, modulo.Cpf);
                Assert.AreEqual(GENERO, modulo.Genero);
            }
        }

        /**
         * - Criar modulo de instancia unica com quatro parametros
         * - Criar por tipo
         * */
        [TestMethod()]
        public void CreateModule_13()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                string NME_PESSOA = "Joao";
                int IDADE_PESSOA = 10;
                string CPF = "12345678912";
                string GENERO = "gato";

                var modulo = moduleManager.CreateModule<PessoaModule>(new object[] { NME_PESSOA, IDADE_PESSOA, CPF, GENERO });
                Assert.IsNotNull(modulo);
                Assert.IsInstanceOfType(modulo, typeof(PessoaModule));
                Assert.AreEqual(NME_PESSOA, modulo.Nome);
                Assert.AreEqual(IDADE_PESSOA, modulo.Idade);
                Assert.AreEqual(CPF, modulo.Cpf);
                Assert.AreEqual(GENERO, modulo.Genero);
            }
        }

        /**
         * - Criar modulo de instancia unica com dois parametros
         * - Criar por nameof
         * */
        [TestMethod()]
        public void CreateModule_12()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                string NME_PESSOA = "Joao";
                int IDADE_PESSOA = 10;

                var modulo = moduleManager.CreateModule(nameof(PessoaModule), new object[] { NME_PESSOA, IDADE_PESSOA }) as PessoaModule;
                Assert.IsNotNull(modulo);
                Assert.IsInstanceOfType(modulo, typeof(PessoaModule));
                Assert.AreEqual(NME_PESSOA, modulo.Nome);
                Assert.AreEqual(IDADE_PESSOA, modulo.Idade);
            }
        }

        /**
         * - Criar modulo de instancia unica com dois parametros
         * - Criar por typeof
         * */
        [TestMethod()]
        public void CreateModule_11()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                string NME_PESSOA = "Joao";
                int IDADE_PESSOA = 10;

                var modulo = moduleManager.CreateModule(typeof(PessoaModule), new object[] { NME_PESSOA, IDADE_PESSOA }) as PessoaModule;
                Assert.IsNotNull(modulo);
                Assert.IsInstanceOfType(modulo, typeof(PessoaModule));
                Assert.AreEqual(NME_PESSOA, modulo.Nome);
                Assert.AreEqual(IDADE_PESSOA, modulo.Idade);
            }
        }

        /**
         * - Criar modulo de instancia unica com dois parametros
         * - Criar por tipo
         * */
        [TestMethod()]
        public void CreateModule_10()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                string NME_PESSOA = "Joao";
                int IDADE_PESSOA = 10;

                var modulo = moduleManager.CreateModule<PessoaModule>(new object[] { NME_PESSOA, IDADE_PESSOA });
                Assert.IsNotNull(modulo);
                Assert.IsInstanceOfType(modulo, typeof(PessoaModule));
                Assert.AreEqual(NME_PESSOA, modulo.Nome);
                Assert.AreEqual(IDADE_PESSOA, modulo.Idade);
            }
        }

        /**
         * - Criar modulo de instancia unica com quatro parametros
         * - Criar por nameof
         * */
        [TestMethod()]
        public void CreateModule_9()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                string NME_PESSOA = "Joao";
                int IDADE_PESSOA = 10;
                string CPF = "12345678912";
                string GENERO = "gato";

                var modulo = moduleManager.CreateModule(nameof(PessoaModule), new object[] { NME_PESSOA, IDADE_PESSOA, CPF, GENERO }) as PessoaModule;
                Assert.IsNotNull(modulo);
                Assert.IsInstanceOfType(modulo, typeof(PessoaModule));
                Assert.AreEqual(NME_PESSOA, modulo.Nome);
                Assert.AreEqual(IDADE_PESSOA, modulo.Idade);
                Assert.AreEqual(CPF, modulo.Cpf);
                Assert.AreEqual(GENERO, modulo.Genero);

                Assert.ThrowsException<ModuleSingleInstanceException>(() =>
                {
                    _ = moduleManager.CreateModule(nameof(PessoaModule), new object[] { NME_PESSOA, IDADE_PESSOA, CPF, GENERO });
                });
            }
        }

        /**
         * - Criar modulo de instancia unica com quatro parametros
         * - Criar por typeof
         * */
        [TestMethod()]
        public void CreateModule_8()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                string NME_PESSOA = "Joao";
                int IDADE_PESSOA = 10;
                string CPF = "12345678912";
                string GENERO = "gato";

                var modulo = moduleManager.CreateModule(typeof(PessoaModule), new object[] { NME_PESSOA, IDADE_PESSOA, CPF, GENERO }) as PessoaModule;
                Assert.IsNotNull(modulo);
                Assert.IsInstanceOfType(modulo, typeof(PessoaModule));
                Assert.AreEqual(NME_PESSOA, modulo.Nome);
                Assert.AreEqual(IDADE_PESSOA, modulo.Idade);
                Assert.AreEqual(CPF, modulo.Cpf);
                Assert.AreEqual(GENERO, modulo.Genero);

                Assert.ThrowsException<ModuleSingleInstanceException>(() =>
                {
                    _ = moduleManager.CreateModule(typeof(PessoaModule), new object[] { NME_PESSOA, IDADE_PESSOA, CPF, GENERO });
                });
            }
        }

        /**
         * - Criar modulo de instancia unica com quatro parametros
         * - Criar por tipo
         * */
        [TestMethod()]
        public void CreateModule_7()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                string NME_PESSOA = "Joao";
                int IDADE_PESSOA = 10;
                string CPF = "12345678912";
                string GENERO = "gato";

                var modulo = moduleManager.CreateModule<PessoaModule>(new object[] { NME_PESSOA, IDADE_PESSOA, CPF, GENERO });
                Assert.IsNotNull(modulo);
                Assert.IsInstanceOfType(modulo, typeof(PessoaModule));
                Assert.AreEqual(NME_PESSOA, modulo.Nome);
                Assert.AreEqual(IDADE_PESSOA, modulo.Idade);
                Assert.AreEqual(CPF, modulo.Cpf);
                Assert.AreEqual(GENERO, modulo.Genero);

                Assert.ThrowsException<ModuleSingleInstanceException>(() =>
                {
                    _ = moduleManager.CreateModule<PessoaModule>(new object[] { NME_PESSOA, IDADE_PESSOA, CPF, GENERO });
                });
            }
        }

        /**
         * - Criar modulo de instancia unica com dois parametros
         * - Criar por nameof
         * */
        [TestMethod()]
        public void CreateModule_6()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                string NME_PESSOA = "Joao";
                int IDADE_PESSOA = 10;

                var modulo = moduleManager.CreateModule(nameof(PessoaModule), new object[] { NME_PESSOA, IDADE_PESSOA }) as PessoaModule;
                Assert.IsNotNull(modulo);
                Assert.IsInstanceOfType(modulo, typeof(PessoaModule));
                Assert.AreEqual(NME_PESSOA, modulo.Nome);
                Assert.AreEqual(IDADE_PESSOA, modulo.Idade);

                Assert.ThrowsException<ModuleSingleInstanceException>(() =>
                {
                    _ = moduleManager.CreateModule(nameof(PessoaModule), new object[] { NME_PESSOA, IDADE_PESSOA });
                });
            }
        }

        /**
         * - Criar modulo de instancia unica com dois parametros
         * - Criar por typeof
         * */
        [TestMethod()]
        public void CreateModule_5()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                string NME_PESSOA = "Joao";
                int IDADE_PESSOA = 10;

                var modulo = moduleManager.CreateModule(typeof(PessoaModule), new object[] { NME_PESSOA, IDADE_PESSOA }) as PessoaModule;
                Assert.IsNotNull(modulo);
                Assert.IsInstanceOfType(modulo, typeof(PessoaModule));
                Assert.AreEqual(NME_PESSOA, modulo.Nome);
                Assert.AreEqual(IDADE_PESSOA, modulo.Idade);

                Assert.ThrowsException<ModuleSingleInstanceException>(() =>
                {
                    _ = moduleManager.CreateModule(typeof(PessoaModule), new object[] { NME_PESSOA, IDADE_PESSOA });
                });
            }
        }

        /**
         * - Criar modulo de instancia unica com dois parametros
         * - Criar por tipo
         * */
        [TestMethod()]
        public void CreateModule_4()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                string NME_PESSOA = "Joao";
                int IDADE_PESSOA = 10;

                var modulo = moduleManager.CreateModule<PessoaModule>(new object[] { NME_PESSOA, IDADE_PESSOA });
                Assert.IsNotNull(modulo);
                Assert.IsInstanceOfType(modulo, typeof(PessoaModule));
                Assert.AreEqual(NME_PESSOA, modulo.Nome);
                Assert.AreEqual(IDADE_PESSOA, modulo.Idade);

                Assert.ThrowsException<ModuleSingleInstanceException>(() =>
                {
                    _ = moduleManager.CreateModule<PessoaModule>(new object[] { NME_PESSOA, IDADE_PESSOA });
                });
            }
        }

        /**
         * - Criar modulo de instancia unica sem parametros
         * - Forçar criar mais de um modulo de instancia unica
         * - Criar por nameof
         * */
        [TestMethod()]
        public void CreateModule_3()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                var modulo = moduleManager.CreateModule(nameof(PessoaModule)) as PessoaModule;
                Assert.IsNotNull(modulo);
                Assert.IsInstanceOfType(modulo, typeof(PessoaModule));
                Assert.ThrowsException<ModuleSingleInstanceException>(() =>
                {
                    _ = moduleManager.CreateModule(nameof(PessoaModule));
                });
                Assert.AreEqual(null, modulo.Nome);
            }
        }

        /**
         * - Criar modulo de instancia unica sem parametros
         * - Forçar criar mais de um modulo de instancia unica
         * - Criar por typeof
         * */
        [TestMethod()]
        public void CreateModule_2()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                var modulo = moduleManager.CreateModule(typeof(PessoaModule)) as PessoaModule;
                Assert.IsNotNull(modulo);
                Assert.IsInstanceOfType(modulo, typeof(PessoaModule));
                Assert.ThrowsException<ModuleSingleInstanceException>(() =>
                {
                    _ = moduleManager.CreateModule(typeof(PessoaModule));
                });
                Assert.AreEqual(null, modulo.Nome);
            }
        }

        /**
         * - Criar modulo de instancia unica sem parametros
         * - Forçar criar mais de um modulo de instancia unica
         * - Criar por tipo
         * */
        [TestMethod()]
        public void CreateModule_1()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                var modulo = moduleManager.CreateModule<PessoaModule>();
                Assert.IsNotNull(modulo);
                Assert.IsInstanceOfType(modulo, typeof(PessoaModule));
                Assert.ThrowsException<ModuleSingleInstanceException>(() =>
                {
                    _ = moduleManager.CreateModule<PessoaModule>();
                });
                Assert.AreEqual(null, modulo.Nome);
            }
        }

        [TestMethod()]
        public void RemoveModule_1()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                var modulo = moduleManager.CreateModule<PessoaModule>();
                Assert.IsNotNull(modulo);
                Assert.IsInstanceOfType(modulo, typeof(PessoaModule));

                moduleManager.RemoveModule(modulo);
                Assert.AreEqual(State.Off, modulo.State);
                Assert.ThrowsException<ModuleNotFoundException>(() =>
                {
                    var module = moduleManager.GetModule<PessoaModule>();
                });

            }
        }

        [TestMethod()]
        public void RemoveModule_2()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                PessoaModule? modulo = null;
                modulo = null;
                Assert.ThrowsException<ArgumentNullException>(() =>
                {
                    moduleManager.RemoveModule(modulo);
                });

                Assert.ThrowsException<ArgumentException>(() =>
                {
                    moduleManager.RemoveModule(string.Empty);
                });

            }
        }

        [TestMethod()]
        public void RemoveModule_3()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {

                Assert.ThrowsException<ArgumentException>(() =>
                {
                    moduleManager.RemoveModule(string.Empty);
                });

            }
        }


        [TestMethod()]
        public void GetModule_1()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                var modulo = moduleManager.CreateModule<PessoaModule>();
                Assert.IsNotNull(modulo);
                Assert.IsInstanceOfType(modulo, typeof(PessoaModule));

                var modulo_get_1 = moduleManager.GetModule(modulo.Id);
                Assert.IsNotNull(modulo_get_1);
                Assert.AreEqual(modulo.Id, modulo_get_1.Id);
            }
        }

        [TestMethod()]
        public void GetModule_2()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                var modulo = moduleManager.CreateModule<PessoaModule>();
                Assert.IsNotNull(modulo);
                Assert.IsInstanceOfType(modulo, typeof(PessoaModule));

                var modulo_get_2 = moduleManager.GetModule<PessoaModule>();
                Assert.IsNotNull(modulo_get_2);
                Assert.AreEqual(modulo_get_2.Id, modulo.Id);


            }
        }

        [TestMethod()]
        public void GetModule_3()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                var modulo = moduleManager.CreateModule<PessoaModule>();
                Assert.IsNotNull(modulo);
                Assert.IsInstanceOfType(modulo, typeof(PessoaModule));

                var modulo_get_3 = moduleManager.GetModule(modulo.GetType());
                Assert.IsNotNull(modulo_get_3);
                Assert.AreEqual(modulo_get_3.Id, modulo.Id);
            }
        }

        [TestMethod()]
        public void GetModule_4()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                Assert.ThrowsException<ArgumentException>(() =>
                {
                    _ = moduleManager.GetModule(string.Empty);
                });

            }
        }

        [TestMethod()]
        public void GetModule_5()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                var modulo = moduleManager.CreateModule<PessoaModule>();
                Assert.IsNotNull(modulo);
                Assert.IsInstanceOfType(modulo, typeof(PessoaModule));

                modulo.Dispose();

                Assert.ThrowsException<ModuleDisposedException>(() =>
                {
                    _ = moduleManager.GetModule(modulo.Id);
                });

            }
        }

        [TestMethod()]
        public void GetModule_6()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                Assert.ThrowsException<ModuleNotFoundException>(() =>
                {
                    _ = moduleManager.GetModule("Id invalido");
                });

            }
        }

        [TestMethod()]
        public void RecicleModule()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                var modulo = moduleManager.CreateModule<PessoaModule>();
                Assert.IsNotNull(modulo);
                Assert.IsInstanceOfType(modulo, typeof(PessoaModule));

                var modulo_get_3 = moduleManager.RecycleModule(modulo);
                Assert.IsNotNull(modulo_get_3);
                Assert.AreNotEqual(modulo_get_3.Id, modulo.Id);
            }
        }

        [TestMethod()]
        public void KeepAliveModule_1()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                var modulo = moduleManager.CreateModule<PessoaModule>();
                Assert.IsNotNull(modulo);
                Assert.IsInstanceOfType(modulo, typeof(PessoaModule));
                string idModulo = modulo.Id;

                moduleManager.KeepAliveModule(modulo);
                modulo = null;
                GC.Collect();
                modulo = moduleManager.GetModule<PessoaModule>();
                Assert.IsNotNull(modulo);
                Assert.AreEqual(idModulo, modulo.Id);
            }
        }

        [TestMethod()]
        public void KeepAliveModule_2()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                var modulo = moduleManager.CreateModule<PessoaModule>();
                Assert.IsNotNull(modulo);
                Assert.IsInstanceOfType(modulo, typeof(PessoaModule));

                Assert.ThrowsException<ArgumentNullException>(() =>
                {
                    moduleManager.KeepAliveModule(null);
                });

            }
        }

        [TestMethod()]
        public void KeepAliveModule_3()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                var modulo = moduleManager.CreateModule<PessoaModule>();
                Assert.IsNotNull(modulo);
                Assert.IsInstanceOfType(modulo, typeof(PessoaModule));
                modulo.Dispose();
                Assert.ThrowsException<ModuleDisposedException>(() =>
                {
                    moduleManager.KeepAliveModule(modulo);
                });
            }
        }

        [TestMethod()]
        public void KeepAliveModule_4()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                var modulo = moduleManager.CreateModule<ModuloIdErradoModule>();
                Assert.IsNotNull(modulo);
                Assert.IsInstanceOfType(modulo, typeof(ModuloIdErradoModule));
                Assert.ThrowsException<ModuleNotFoundException>(() =>
                {
                    moduleManager.KeepAliveModule(modulo);
                });
            }
        }

        [TestMethod()]
        public void ListAllModules()
        {
            using (var moduleManager = ModuleManagerExtensions.CreateModuleManager())
            {
                var modulo = moduleManager.CreateModule<ModuloIdErradoModule>();
                var modulo2 = moduleManager.CreateModule<PessoaModule>();
                var modulos = moduleManager.ListAllModules();
                Assert.IsNotNull(modulos);
                Assert.AreEqual(3, modulos.Count());


            }
        }
    }

}
