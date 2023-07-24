using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Propeus.Modulo.Abstrato.Exceptions;
using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.CoreTests.Modulos;
using Propeus.Modulo.Core;

namespace Propeus.Modulo.CoreTests
{

    [TestClass()]
    public partial class GerenciadorTests
    {
        public IModuleManager gerenciador;

        [TestInitialize]
        public void Begin()
        {
            //EventoProvider.RegistrarOuvinteInformacao(TesteLog);
            //EventoProvider.RegistrarOuvinteErro(TesteLog);
            //EventoProvider.RegistrarOuvinteAviso(TesteLogAviso);
            gerenciador = ModuleManagerExtensions.CreateModuleManager();
        }

        public void TesteLogAviso(Type fonte, string mensagem, Exception exception)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[{DateTime.Now}] {fonte.Name}: {mensagem}");
            Console.ResetColor();
        }

        public void TesteLog(Type fonte, string mensagem, Exception exception)
        {
            if (exception == null)
            {

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[{DateTime.Now}] {fonte.Name}: {mensagem}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[{DateTime.Now}] {fonte.Name}: {mensagem} | {exception.Message}");
            }
            Console.ResetColor();
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
        public void CriarModuleInstanciaUnica()
        {

            using (gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                IModule Module = gerenciador.CreateModule<TesteInstanciaUnicaModule>();
                Assert.IsNotNull(Module); 
            }

        }

        [TestMethod()]
        [TestCategory("Criar")]
        public void CriarModuleInstanciaUnica_ArgumentException()
        {


            IModule Module = gerenciador.CreateModule<TesteInstanciaUnicaModule>();
            Assert.IsNotNull(Module);
            _ = Assert.ThrowsException<ModuleSingleInstanceException>(() =>
            {
                Module = gerenciador.CreateModule<TesteInstanciaUnicaModule>();
            });


        }
        [TestMethod()]
        [TestCategory("Criar")]
        public void CriarModuleInstanciaMultipla()
        {

            IModule Module = gerenciador.CreateModule<TesteInstanciaMultiplaModule>();
            Assert.IsNotNull(Module);

        }
        [TestMethod()]
        [TestCategory("Criar")]
        public void CriarModuleInstanciaMultipla_multiplosModules()
        {

            IModule Module = gerenciador.CreateModule<TesteInstanciaMultiplaModule>();
            Assert.IsNotNull(Module);
            IModule Modulev2 = gerenciador.CreateModule<TesteInstanciaMultiplaModule>();
            Assert.IsNotNull(Modulev2);
            Assert.AreNotEqual(Module, Modulev2);
            Assert.AreNotEqual(Module.Id, Modulev2.Id);

        }

        [TestMethod()]
        [TestCategory("Criar")]
        public void CriarModuleInstanciaUnicaPorTipo()
        {

            IModule Module = gerenciador.CreateModule(typeof(TesteInstanciaUnicaModule));
            Assert.IsNotNull(Module);

        }
        [TestMethod()]
        [TestCategory("Criar")]
        public void CriarModuleInstanciaUnicaPorTipo_ArgumentException()
        {

            IModule Module = gerenciador.CreateModule(typeof(TesteInstanciaUnicaModule));
            Assert.IsNotNull(Module);
            _ = Assert.ThrowsException<ModuleSingleInstanceException>(() =>
            {
                Module = gerenciador.CreateModule(typeof(TesteInstanciaUnicaModule));
            });

        }
        [TestMethod()]
        [TestCategory("Criar")]
        public void CriarModuleInstanciaMultiplaPorTipo()
        {

            IModule Module = gerenciador.CreateModule(typeof(TesteInstanciaMultiplaModule));
            Assert.IsNotNull(Module);

        }
        [TestMethod()]
        [TestCategory("Criar")]
        public void CriarModuleInstanciaMultiplaPorTipo_MultiplosModules()
        {

            IModule Module = gerenciador.CreateModule(typeof(TesteInstanciaMultiplaModule));
            Assert.IsNotNull(Module);
            IModule Modulev2 = gerenciador.CreateModule(typeof(TesteInstanciaMultiplaModule));
            Assert.IsNotNull(Modulev2);
            Assert.AreNotEqual(Module, Modulev2);
            Assert.AreNotEqual(Module.Id, Modulev2.Id);

        }

        [TestMethod()]
        [TestCategory("Criar")]
        public void CriarModuleInstanciaUnicaPorNome()
        {

            IModule Module = gerenciador.CreateModule(nameof(TesteInstanciaUnicaModule));
            Assert.IsNotNull(Module);

        }
        [TestMethod()]
        [TestCategory("Criar")]
        public void CriarModuleInstanciaUnicaPorNome_ModuleInstanciaUnicaException()
        {

            IModule Module = gerenciador.CreateModule(nameof(TesteInstanciaUnicaModule));
            Assert.IsNotNull(Module);
            _ = Assert.ThrowsException<ModuleSingleInstanceException>(() =>
            {
                Module = gerenciador.CreateModule(nameof(TesteInstanciaUnicaModule));
            });

        }
        [TestMethod()]
        [TestCategory("Criar")]
        public void CriarModuleInstanciaMultiplaPorNome()
        {

            IModule Module = gerenciador.CreateModule(nameof(TesteInstanciaMultiplaModule));
            Assert.IsNotNull(Module);

        }
        [TestMethod()]
        [TestCategory("Criar")]
        public void CriarModuleInstanciaMultiplaPorNome_MultiplosModules()
        {

            IModule Module = gerenciador.CreateModule(nameof(TesteInstanciaMultiplaModule));
            Assert.IsNotNull(Module);
            IModule Modulev2 = gerenciador.CreateModule(nameof(TesteInstanciaMultiplaModule));
            Assert.IsNotNull(Modulev2);
            Assert.AreNotEqual(Module, Modulev2);
            Assert.AreNotEqual(Module.Id, Modulev2.Id);

        }

        //Remover
        [TestMethod()]
        [TestCategory("Remover")]
        public void RemoverModuleInstanciaUnica()
        {

            IModule Module = gerenciador.CreateModule<TesteInstanciaUnicaModule>();
            Assert.IsNotNull(Module);
            gerenciador.RemoveModule(Module);

        }
        [TestMethod()]
        [TestCategory("Remover")]
        public void RemoverModuleInstanciaMultipla()
        {

            IModule Module = gerenciador.CreateModule<TesteInstanciaMultiplaModule>();
            Assert.IsNotNull(Module);
            gerenciador.RemoveModule(Module);

        }


        [TestMethod()]
        [TestCategory("Remover")]
        public void RemoverModuleInstanciaUnicaPorId()
        {

            IModule Module = gerenciador.CreateModule<TesteInstanciaUnicaModule>();
            Assert.IsNotNull(Module);
            gerenciador.RemoveModule(Module.Id);

        }
        [TestMethod()]
        [TestCategory("Remover")]
        public void RemoverModuleInstanciaMultiplaPorId()
        {

            IModule Module = gerenciador.CreateModule<TesteInstanciaMultiplaModule>();
            Assert.IsNotNull(Module);
            gerenciador.RemoveModule(Module.Id);

        }

        [TestMethod()]
        [TestCategory("Remover")]
        public void RemoverTodosModules()
        {
            IModule[] Modules = new IModule[100];

            for (int i = 0; i < 100; i++)
            {
                Modules[i] = gerenciador.CreateModule<TesteInstanciaMultiplaModule>();
                Assert.IsNotNull(Modules[i]);
            }

            gerenciador.RemoveAllModules();
            Assert.AreEqual(0, gerenciador.InitializedModules);
        }
        [TestMethod()]
        [TestCategory("Remover")]
        public void RemoverModuleInexistente()
        {

            IModule Module = gerenciador.CreateModule<TesteInstanciaMultiplaModule>();
            Assert.IsNotNull(Module);
            gerenciador.RemoveModule(Module.Id);

            Assert.AreEqual(0, gerenciador.InitializedModules);

            gerenciador.RemoveModule(Module.Id);



        }

        //Obter
        [TestMethod()]
        [TestCategory("Obter")]
        public void ObterModuleInstanciaUnicaPorTipo()
        {

            IModule Module = gerenciador.CreateModule<TesteInstanciaUnicaModule>();
            Assert.IsNotNull(Module);
            IModule Modulev2 = gerenciador.GetModule<TesteInstanciaUnicaModule>();
            Assert.AreEqual(Module, Modulev2);

        }
        [TestMethod()]
        [TestCategory("Obter")]
        public void ObterModuleInstanciaMultiplaPorTipo()
        {

            IModule Module = gerenciador.CreateModule<TesteInstanciaMultiplaModule>();
            Assert.IsNotNull(Module);
            IModule Modulev2 = gerenciador.GetModule<TesteInstanciaMultiplaModule>();
            Assert.AreEqual(Module, Modulev2);

        }
        [TestMethod()]
        [TestCategory("Obter")]
        public void ObterModuleInstanciaMultipla_MultiplosModulesPorTipo()
        {

            IModule Module = gerenciador.CreateModule<TesteInstanciaMultiplaModule>();
            Assert.IsNotNull(Module);
            IModule Modulev2 = gerenciador.CreateModule<TesteInstanciaMultiplaModule>();
            Assert.IsNotNull(Modulev2);
            Assert.AreNotEqual(Module, Modulev2);
            IModule Modulev3 = gerenciador.GetModule<TesteInstanciaMultiplaModule>();
            Assert.IsNotNull(Modulev3);

        }
        [TestMethod()]
        [TestCategory("Obter")]
        public void ObterModuleInstanciaUnicaPorId()
        {

            IModule Module = gerenciador.CreateModule<TesteInstanciaUnicaModule>();
            Assert.IsNotNull(Module);
            IModule Modulev2 = gerenciador.GetModule(Module.Id);
            Assert.AreEqual(Module, Modulev2);

        }
        [TestMethod()]
        [TestCategory("Obter")]
        public void ObterModuleInstanciaMultiplaPorId()
        {

            IModule Module = gerenciador.CreateModule<TesteInstanciaMultiplaModule>();
            Assert.IsNotNull(Module);
            IModule Modulev2 = gerenciador.GetModule<TesteInstanciaMultiplaModule>();
            Assert.AreEqual(Module.Id, Modulev2.Id);

        }
        [TestMethod()]
        [TestCategory("Obter")]
        public void ObterModuleInstanciaMultipla_MultiplosModulesPorId()
        {

            IModule Module = gerenciador.CreateModule<TesteInstanciaMultiplaModule>();
            Assert.IsNotNull(Module);
            IModule Modulev2 = gerenciador.CreateModule<TesteInstanciaMultiplaModule>();
            Assert.IsNotNull(Modulev2);
            Assert.AreNotEqual(Module, Modulev2);
            IModule Modulev3 = gerenciador.GetModule(Modulev2.Id);
            Assert.IsNotNull(Modulev3);
            Assert.AreEqual(Modulev2, Modulev3);

        }
        [TestMethod()]
        [TestCategory("Obter")]
        public void ObterModuleInstanciaPorIdInexistente_ArgumentException()
        {

            _ = Assert.ThrowsException<ModuleNotFoundException>(() =>
            {
                _ = gerenciador.GetModule(Guid.NewGuid().ToString());
            });

        }


        //Existe
        [TestMethod()]
        [TestCategory("Existe")]
        public void ExisteModuleInstanciaUnicaPorInstancia()
        {

            IModule Module = gerenciador.CreateModule<TesteInstanciaUnicaModule>();
            Assert.IsNotNull(Module);
            Assert.IsTrue(gerenciador.ExistsModule(Module));

        }
        [TestMethod()]
        [TestCategory("Existe")]
        public void ExisteModuleInstanciaMultiplaPorInstancia()
        {

            IModule Module = gerenciador.CreateModule<TesteInstanciaMultiplaModule>();
            Assert.IsNotNull(Module);
            Assert.IsTrue(gerenciador.ExistsModule(Module));

        }
        [TestMethod()]
        [TestCategory("Existe")]
        public void ExisteModuleInstanciaMultipla_MultiplosModulesPorInstancia()
        {

            IModule Module = gerenciador.CreateModule<TesteInstanciaMultiplaModule>();
            Assert.IsNotNull(Module);
            IModule Modulev2 = gerenciador.CreateModule<TesteInstanciaMultiplaModule>();
            Assert.IsNotNull(Modulev2);
            Assert.AreNotEqual(Module, Modulev2);
            Assert.IsTrue(gerenciador.ExistsModule(Module));
            Assert.IsTrue(gerenciador.ExistsModule(Modulev2));

        }
        [TestMethod()]
        [TestCategory("Existe")]
        public void ExisteModuleInstanciaUnicaPorTipo()
        {

            IModule Module = gerenciador.CreateModule<TesteInstanciaUnicaModule>();
            Assert.IsNotNull(Module);
            Assert.IsTrue(gerenciador.ExistsModule(typeof(TesteInstanciaUnicaModule)));

        }
        [TestMethod()]
        [TestCategory("Existe")]
        public void ExisteModuleInstanciaMultiplaPorTipo()
        {

            IModule Module = gerenciador.CreateModule<TesteInstanciaMultiplaModule>();
            Assert.IsNotNull(Module);
            Assert.IsTrue(gerenciador.ExistsModule(typeof(TesteInstanciaMultiplaModule)));

        }
        [TestMethod()]
        [TestCategory("Existe")]
        public void ExisteModuleInstanciaMultipla_MultiplosModulesPorTipo()
        {

            IModule Module = gerenciador.CreateModule<TesteInstanciaMultiplaModule>();
            Assert.IsNotNull(Module);
            IModule Modulev2 = gerenciador.CreateModule<TesteInstanciaMultiplaModule>();
            Assert.IsNotNull(Modulev2);
            Assert.AreNotEqual(Module, Modulev2);
            Assert.IsTrue(gerenciador.ExistsModule(typeof(TesteInstanciaMultiplaModule)));

        }
        [TestMethod()]
        [TestCategory("Existe")]
        public void ExisteModuleInstanciaUnicaPorId()
        {

            IModule Module = gerenciador.CreateModule<TesteInstanciaUnicaModule>();
            Assert.IsNotNull(Module);
            Assert.IsTrue(gerenciador.ExistsModule(Module.Id));

        }
        [TestMethod()]
        [TestCategory("Existe")]
        public void ExisteModuleInstanciaMultiplaPorId()
        {

            IModule Module = gerenciador.CreateModule<TesteInstanciaMultiplaModule>();
            Assert.IsNotNull(Module);
            Assert.IsTrue(gerenciador.ExistsModule(Module.Id));

        }
        [TestMethod()]
        [TestCategory("Existe")]
        public void ExisteModuleInstanciaMultipla_MultiplosModulesPorId()
        {

            IModule Module = gerenciador.CreateModule<TesteInstanciaMultiplaModule>();
            Assert.IsNotNull(Module);
            IModule Modulev2 = gerenciador.CreateModule<TesteInstanciaMultiplaModule>();
            Assert.IsNotNull(Modulev2);
            Assert.AreNotEqual(Module, Modulev2);
            Assert.IsTrue(gerenciador.ExistsModule(Modulev2.Id));

        }
        [TestMethod()]
        [TestCategory("Existe")]
        public void ExisteModuleInstanciaPorIdInexistente()
        {

            Assert.IsFalse(gerenciador.ExistsModule(Guid.NewGuid().ToString()));

        }

        //Reciclar
        [TestMethod()]
        [TestCategory("Reciclar")]
        public void ReiniciarModuleInstanciaUnicaPorInstancia()
        {

            IModule Module = gerenciador.CreateModule<TesteInstanciaUnicaModule>();
            Assert.IsNotNull(Module);
            Assert.IsTrue(gerenciador.ExistsModule(Module));
            IModule Modulev2 = gerenciador.RecycleModule(Module);
            Assert.IsNotNull(Modulev2);
            Assert.IsTrue(gerenciador.ExistsModule(Modulev2));
            Assert.IsFalse(gerenciador.ExistsModule(Module));

        }
        [TestMethod()]
        [TestCategory("Reciclar")]
        public void ReiniciarModuleInstanciaMultiplaPorInstancia()
        {

            IModule Module = gerenciador.CreateModule<TesteInstanciaMultiplaModule>();
            Assert.IsNotNull(Module);
            Assert.IsTrue(gerenciador.ExistsModule(Module));
            IModule Modulev2 = gerenciador.RecycleModule(Module);
            Assert.IsNotNull(Modulev2);
            Assert.IsTrue(gerenciador.ExistsModule(Modulev2));
            Assert.IsFalse(gerenciador.ExistsModule(Module));

        }
        [TestMethod()]
        [TestCategory("Reciclar")]
        public void ReiniciarModuleInstanciaUnicaPorId()
        {

            IModule Module = gerenciador.CreateModule<TesteInstanciaUnicaModule>();
            Assert.IsNotNull(Module);
            Assert.IsTrue(gerenciador.ExistsModule(Module));
            IModule Modulev2 = gerenciador.RecycleModule(Module.Id);
            Assert.IsNotNull(Modulev2);
            Assert.IsTrue(gerenciador.ExistsModule(Modulev2));
            Assert.IsFalse(gerenciador.ExistsModule(Module));

        }
        [TestMethod()]
        [TestCategory("Reciclar")]
        public void ReiniciarModuleInstanciaMultiplaPorId()
        {

            IModule Module = gerenciador.CreateModule<TesteInstanciaMultiplaModule>();
            Assert.IsNotNull(Module);
            Assert.IsTrue(gerenciador.ExistsModule(Module));
            IModule Modulev2 = gerenciador.RecycleModule(Module.Id);
            Assert.IsNotNull(Modulev2);
            Assert.IsTrue(gerenciador.ExistsModule(Modulev2));
            Assert.IsFalse(gerenciador.ExistsModule(Module));

        }
        [TestMethod()]
        [TestCategory("Reciclar")]
        public void ReiniciarModuleInstanciaPorIdInexistente_ArgumentException()
        {

            _ = Assert.ThrowsException<ModuleNotFoundException>(() =>
            {
                _ = gerenciador.RecycleModule(Guid.NewGuid().ToString());
            });

        }



        //Listar
        [TestMethod()]
        [TestCategory("Listar")]
        public void ListarModules_loop()
        {
            IModule m = null;
            for (int i = 0; i < 100; i++)
            {
                TesteInstanciaMultiplaModule auxm = gerenciador.CreateModule<TesteInstanciaMultiplaModule>();
                Assert.AreNotEqual(m, auxm);
                m = auxm;
            }

            Assert.AreEqual(100, gerenciador.InitializedModules);
            Assert.IsNotNull(gerenciador.ListAllModules());
        }




        [TestMethod()]
        [TestCategory("Exceptions")]
        public void Teste_19()
        {
            Assert.ThrowsException<ModuleSingleInstanceException>(() =>
            {
                gerenciador.CreateModule(typeof(ModuleIntanciaUnica));
                gerenciador.CreateModule(typeof(ModuleIntanciaUnica));
            });
        }
        [TestMethod()]
        [TestCategory("Exceptions")]
        public void Teste_18()
        {
            Assert.ThrowsException<ModuleBuilderAbsentException>(() =>
            {
                gerenciador.CreateModule(typeof(ModuleSemConstrutor));
            });
        }
        [TestMethod()]
        [TestCategory("Exceptions")]
        public void Teste_17()
        {
            Assert.ThrowsException<ModuleTypeInvalidException>(() =>
            {
                gerenciador.CreateModule(typeof(ModuleSemAtributo));
            });
        }
        [TestMethod()]
        [TestCategory("Exceptions")]
        public void Teste_16()
        {
            Assert.ThrowsException<ModuleTypeInvalidException>(() =>
            {
                gerenciador.CreateModule(typeof(ModuleInvallido));
            });
        }
        [TestMethod()]
        [TestCategory("Exceptions")]
        public void Teste_15()
        {
            Assert.ThrowsException<ModuleTypeNotFoundException>(() =>
            {
                gerenciador.CreateModule(typeof(IContratoModuleInvalido));
            });
        }
        [TestMethod()]
        [TestCategory("Exceptions")]
        public void Teste_14()
        {
            Assert.ThrowsException<ModuleTypeInvalidException>(() =>
            {
                gerenciador.CreateModule(typeof(int));
            });
        }
        [TestMethod()]
        [TestCategory("Exceptions")]
        public void Teste_13()
        {
            Assert.ThrowsException<ModuleContractNotFoundException>(() =>
            {
                gerenciador.CreateModule(typeof(InterfaceSemAtributo));
            });
        }
        [TestMethod()]
        [TestCategory("Exceptions")]
        public void Teste_12()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                gerenciador.ExistsModule(default(Type));
            });
        }
        [TestMethod()]
        [TestCategory("Exceptions")]
        public void Teste_11()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                gerenciador.CreateModule(default(Type));
            });
        }
        [TestMethod()]
        [TestCategory("Exceptions")]
        public void Teste_10()
        {
            Assert.ThrowsException<ModuleContractInvalidException>(() =>
            {
                gerenciador.CreateModule(typeof(IContratoInvalidoTipo));
            });
        }
        [TestMethod()]
        [TestCategory("Exceptions")]
        public void Teste_9()
        {
            Assert.ThrowsException<ModuleContractInvalidException>(() =>
            {
                gerenciador.CreateModule(typeof(IContratoInvalido));
            });
        }
        [TestMethod()]
        [TestCategory("Exceptions")]
        public void Teste_8()
        {
            Assert.ThrowsException<ArgumentException>(() =>
            {
                gerenciador.GetModule(default(string));
            });
        }

        [TestMethod()]
        [TestCategory("Exceptions")]
        public void Teste_7()
        {
            Assert.ThrowsException<ModuleDisposedException>(() =>
            {
                IModule m = gerenciador.CreateModule(typeof(ModuleDependenciaInterfaceInvalidaOpcional));
                m.Dispose();
                gerenciador.GetModule(m.Id);
            });
        }
        [TestMethod()]
        [TestCategory("Criar")]
        public void Teste_6()
        {
            Assert.IsNotNull(gerenciador.CreateModule(typeof(ModuleDependenciaInterfaceValida)).ToString());
        }
        [TestMethod()]
        [TestCategory("Criar")]
        public void Teste_5()
        {
            Assert.IsNotNull(gerenciador.CreateModule(typeof(ModuleDependenciaInterfaceValida)));
        }
        [TestMethod()]
        [TestCategory("Criar")]
        public void Teste_4()
        {
            Assert.IsNotNull(gerenciador.CreateModule(typeof(ModuleDependenciaValida)));
        }
        [TestMethod()]
        [TestCategory("Criar")]
        public void Teste_3()
        {
            Assert.IsNotNull(gerenciador.CreateModule(typeof(ModuleDependenciaInterfaceInvalidaOpcional)));
        }
        [TestMethod()]
        [TestCategory("Criar")]
        public void Teste_2()
        {
            Assert.IsNotNull(gerenciador.CreateModule(typeof(ModuleParametroInvalidoOpcional)));
        }
        [TestMethod()]
        [TestCategory("Criar")]
        public void Teste_1()
        {
            Assert.IsNotNull(gerenciador.CreateModule(typeof(IModuleInstanciaUnica)));
            Assert.IsTrue(gerenciador.ExistsModule(typeof(IModuleInstanciaUnica)));
        }

        [TestMethod()]
        public void Testes_separar()
        {
            //Verifica se nao existe modulo
            Assert.IsFalse(gerenciador.ExistsModule(typeof(ModuleDependenciaInvalida)));
            //Verifica tipo invalido
            Assert.IsFalse(gerenciador.ExistsModule(typeof(IContratoModuleInvalido)));
            //Verifica valor invalido
            Assert.ThrowsException<ArgumentNullException>(() => { gerenciador.ExistsModule(""); });
            //Verifica valor invalido
            Assert.ThrowsException<ArgumentException>(() => { gerenciador.RemoveModule(""); });
            //Obtem instancia que nunca criou
            Assert.ThrowsException<ModuleNotFoundException>(() => { gerenciador.GetModule(typeof(ModuleDependenciaInvalida)); });
            //Tem que dar um jeito de testar isso aqui
            Assert.IsNotNull(gerenciador.CreateOrGetModule<TesteInstanciaMultiplaModule>());
            Assert.IsNotNull(gerenciador.CreateOrGetModule<TesteInstanciaMultiplaModule>());
            Assert.IsNotNull(gerenciador.CreateOrGetModule<TesteInstanciaMultiplaModule>().ToString());
            Assert.IsNotNull(gerenciador.ToString());

            //Assert.IsNotNull(gerenciador.CreateOrGetModule<TaskJobModule>().ToString());
            //Assert.IsNotNull(gerenciador.CreateOrGetModule<TaskJobModule>().ToStringView());
        }
    }
}