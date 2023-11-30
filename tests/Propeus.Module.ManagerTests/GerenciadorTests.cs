using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Propeus.Module.Abstract.Exceptions;
using Propeus.Module.Abstract.Interfaces;
using Propeus.Module.CoreTests.Modulos;
using Propeus.Module.Manager;

namespace Propeus.Module.CoreTests
{

    [TestClass()]
    public partial class GerenciadorTests
    {


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



        //Criar
        [TestMethod()]
        [TestCategory("Criar")]
        public void CriarModuleInstanciaUnica()
        {

            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                IModule Module = gerenciador.CreateModule<TesteInstanciaUnicaModule>();
                Assert.IsNotNull(Module);
            }

        }

        [TestMethod()]
        [TestCategory("Criar")]
        public void CriarModuleInstanciaUnica_ArgumentException()
        {

            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                IModule Module = gerenciador.CreateModule<TesteInstanciaUnicaModule>();
                Assert.IsNotNull(Module);
                _ = Assert.ThrowsException<ModuleSingleInstanceException>(() =>
                {
                    Module = gerenciador.CreateModule<TesteInstanciaUnicaModule>();
                });

            }

        }
        [TestMethod()]
        [TestCategory("Criar")]
        public void CriarModuleInstanciaMultipla()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                IModule Module = gerenciador.CreateModule<TesteInstanciaMultiplaModule>();
                Assert.IsNotNull(Module);
            }

        }
        [TestMethod()]
        [TestCategory("Criar")]
        public void CriarModuleInstanciaMultipla_multiplosModules()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                IModule Module = gerenciador.CreateModule<TesteInstanciaMultiplaModule>();
                Assert.IsNotNull(Module);
                IModule Modulev2 = gerenciador.CreateModule<TesteInstanciaMultiplaModule>();
                Assert.IsNotNull(Modulev2);
                Assert.AreNotEqual(Module, Modulev2);
                Assert.AreNotEqual(Module.Id, Modulev2.Id);
            }

        }

        [TestMethod()]
        [TestCategory("Criar")]
        public void CriarModuleInstanciaUnicaPorTipo()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                IModule Module = gerenciador.CreateModule(typeof(TesteInstanciaUnicaModule));
                Assert.IsNotNull(Module);
            }

        }
        [TestMethod()]
        [TestCategory("Criar")]
        public void CriarModuleInstanciaUnicaPorTipo_ArgumentException()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                IModule Module = gerenciador.CreateModule(typeof(TesteInstanciaUnicaModule));
                Assert.IsNotNull(Module);
                _ = Assert.ThrowsException<ModuleSingleInstanceException>(() =>
                {
                    Module = gerenciador.CreateModule(typeof(TesteInstanciaUnicaModule));
                });
            }

        }
        [TestMethod()]
        [TestCategory("Criar")]
        public void CriarModuleInstanciaMultiplaPorTipo()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                IModule Module = gerenciador.CreateModule(typeof(TesteInstanciaMultiplaModule));
                Assert.IsNotNull(Module);
            }

        }
        [TestMethod()]
        [TestCategory("Criar")]
        public void CriarModuleInstanciaMultiplaPorTipo_MultiplosModules()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                IModule Module = gerenciador.CreateModule(typeof(TesteInstanciaMultiplaModule));
                Assert.IsNotNull(Module);
                IModule Modulev2 = gerenciador.CreateModule(typeof(TesteInstanciaMultiplaModule));
                Assert.IsNotNull(Modulev2);
                Assert.AreNotEqual(Module, Modulev2);
                Assert.AreNotEqual(Module.Id, Modulev2.Id);
            }

        }

        [TestMethod()]
        [TestCategory("Criar")]
        public void CriarModuleInstanciaUnicaPorNome()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                IModule Module = gerenciador.CreateModule(nameof(TesteInstanciaUnicaModule));
                Assert.IsNotNull(Module);
            }

        }
        [TestMethod()]
        [TestCategory("Criar")]
        public void CriarModuleInstanciaUnicaPorNome_ModuleInstanciaUnicaException()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                IModule Module = gerenciador.CreateModule(nameof(TesteInstanciaUnicaModule));
                Assert.IsNotNull(Module);
                _ = Assert.ThrowsException<ModuleSingleInstanceException>(() =>
                {
                    Module = gerenciador.CreateModule(nameof(TesteInstanciaUnicaModule));
                });
            }

        }
        [TestMethod()]
        [TestCategory("Criar")]
        public void CriarModuleInstanciaMultiplaPorNome()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                IModule Module = gerenciador.CreateModule(nameof(TesteInstanciaMultiplaModule));
                Assert.IsNotNull(Module);
            }

        }
        [TestMethod()]
        [TestCategory("Criar")]
        public void CriarModuleInstanciaMultiplaPorNome_MultiplosModules()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                IModule Module = gerenciador.CreateModule(nameof(TesteInstanciaMultiplaModule));
                Assert.IsNotNull(Module);
                IModule Modulev2 = gerenciador.CreateModule(nameof(TesteInstanciaMultiplaModule));
                Assert.IsNotNull(Modulev2);
                Assert.AreNotEqual(Module, Modulev2);
                Assert.AreNotEqual(Module.Id, Modulev2.Id);
            }

        }

        //Remover
        [TestMethod()]
        [TestCategory("Remover")]
        public void RemoverModuleInstanciaUnica()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                IModule Module = gerenciador.CreateModule<TesteInstanciaUnicaModule>();
                Assert.IsNotNull(Module);
                gerenciador.RemoveModule(Module);
            }

        }
        [TestMethod()]
        [TestCategory("Remover")]
        public void RemoverModuleInstanciaMultipla()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                IModule Module = gerenciador.CreateModule<TesteInstanciaMultiplaModule>();
                Assert.IsNotNull(Module);
                gerenciador.RemoveModule(Module);
            }

        }


        [TestMethod()]
        [TestCategory("Remover")]
        public void RemoverModuleInstanciaUnicaPorId()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                IModule Module = gerenciador.CreateModule<TesteInstanciaUnicaModule>();
                Assert.IsNotNull(Module);
                gerenciador.RemoveModule(Module.Id);
            }

        }
        [TestMethod()]
        [TestCategory("Remover")]
        public void RemoverModuleInstanciaMultiplaPorId()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                IModule Module = gerenciador.CreateModule<TesteInstanciaMultiplaModule>();
                Assert.IsNotNull(Module);
                gerenciador.RemoveModule(Module.Id);
            }

        }


        [TestMethod()]
        [TestCategory("Remover")]
        public void RemoverModuleInexistente()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                IModule Module = gerenciador.CreateModule<TesteInstanciaMultiplaModule>();
                Assert.IsNotNull(Module);
                gerenciador.RemoveModule(Module.Id);

                Assert.AreEqual(1, gerenciador.InitializedModules);

                gerenciador.RemoveModule(Module.Id);
            }



        }

        //Obter
        [TestMethod()]
        [TestCategory("Obter")]
        public void ObterModuleInstanciaUnicaPorTipo()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                IModule Module = gerenciador.CreateModule<TesteInstanciaUnicaModule>();
                Assert.IsNotNull(Module);
                IModule Modulev2 = gerenciador.GetModule<TesteInstanciaUnicaModule>();
                Assert.AreEqual(Module, Modulev2);
            }

        }
        [TestMethod()]
        [TestCategory("Obter")]
        public void ObterModuleInstanciaMultiplaPorTipo()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                IModule Module = gerenciador.CreateModule<TesteInstanciaMultiplaModule>();
                Assert.IsNotNull(Module);
                IModule Modulev2 = gerenciador.GetModule<TesteInstanciaMultiplaModule>();
                Assert.AreEqual(Module, Modulev2);
            }

        }
        [TestMethod()]
        [TestCategory("Obter")]
        public void ObterModuleInstanciaMultipla_MultiplosModulesPorTipo()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                IModule Module = gerenciador.CreateModule<TesteInstanciaMultiplaModule>();
                Assert.IsNotNull(Module);
                IModule Modulev2 = gerenciador.CreateModule<TesteInstanciaMultiplaModule>();
                Assert.IsNotNull(Modulev2);
                Assert.AreNotEqual(Module, Modulev2);
                IModule Modulev3 = gerenciador.GetModule<TesteInstanciaMultiplaModule>();
                Assert.IsNotNull(Modulev3);
            }

        }
        [TestMethod()]
        [TestCategory("Obter")]
        public void ObterModuleInstanciaUnicaPorId()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                IModule Module = gerenciador.CreateModule<TesteInstanciaUnicaModule>();
                Assert.IsNotNull(Module);
                IModule Modulev2 = gerenciador.GetModule(Module.Id);
                Assert.AreEqual(Module, Modulev2);
            }

        }
        [TestMethod()]
        [TestCategory("Obter")]
        public void ObterModuleInstanciaMultiplaPorId()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                IModule Module = gerenciador.CreateModule<TesteInstanciaMultiplaModule>();
                Assert.IsNotNull(Module);
                IModule Modulev2 = gerenciador.GetModule<TesteInstanciaMultiplaModule>();
                Assert.AreEqual(Module.Id, Modulev2.Id);
            }

        }
        [TestMethod()]
        [TestCategory("Obter")]
        public void ObterModuleInstanciaMultipla_MultiplosModulesPorId()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
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

        }
        [TestMethod()]
        [TestCategory("Obter")]
        public void ObterModuleInstanciaPorIdInexistente_ArgumentException()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                _ = Assert.ThrowsException<ModuleNotFoundException>(() =>
{
    _ = gerenciador.GetModule(Guid.NewGuid().ToString());
});
            }

        }


        //Existe
        [TestMethod()]
        [TestCategory("Existe")]
        public void ExisteModuleInstanciaUnicaPorInstancia()
        {

            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                IModule Module = gerenciador.CreateModule<TesteInstanciaUnicaModule>();
                Assert.IsNotNull(Module);
                Assert.IsTrue(gerenciador.ExistsModule(Module));
            }

        }
        [TestMethod()]
        [TestCategory("Existe")]
        public void ExisteModuleInstanciaMultiplaPorInstancia()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                IModule Module = gerenciador.CreateModule<TesteInstanciaMultiplaModule>();
                Assert.IsNotNull(Module);
                Assert.IsTrue(gerenciador.ExistsModule(Module));
            }

        }
        [TestMethod()]
        [TestCategory("Existe")]
        public void ExisteModuleInstanciaMultipla_MultiplosModulesPorInstancia()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                IModule Module = gerenciador.CreateModule<TesteInstanciaMultiplaModule>();
                Assert.IsNotNull(Module);
                IModule Modulev2 = gerenciador.CreateModule<TesteInstanciaMultiplaModule>();
                Assert.IsNotNull(Modulev2);
                Assert.AreNotEqual(Module, Modulev2);
                Assert.IsTrue(gerenciador.ExistsModule(Module));
                Assert.IsTrue(gerenciador.ExistsModule(Modulev2));
            }

        }
        [TestMethod()]
        [TestCategory("Existe")]
        public void ExisteModuleInstanciaUnicaPorTipo()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                IModule Module = gerenciador.CreateModule<TesteInstanciaUnicaModule>();
                Assert.IsNotNull(Module);
                Assert.IsTrue(gerenciador.ExistsModule(typeof(TesteInstanciaUnicaModule)));
            }

        }
        [TestMethod()]
        [TestCategory("Existe")]
        public void ExisteModuleInstanciaMultiplaPorTipo()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                IModule Module = gerenciador.CreateModule<TesteInstanciaMultiplaModule>();
                Assert.IsNotNull(Module);
                Assert.IsTrue(gerenciador.ExistsModule(typeof(TesteInstanciaMultiplaModule)));
            }

        }
        [TestMethod()]
        [TestCategory("Existe")]
        public void ExisteModuleInstanciaMultipla_MultiplosModulesPorTipo()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                IModule Module = gerenciador.CreateModule<TesteInstanciaMultiplaModule>();
                Assert.IsNotNull(Module);
                IModule Modulev2 = gerenciador.CreateModule<TesteInstanciaMultiplaModule>();
                Assert.IsNotNull(Modulev2);
                Assert.AreNotEqual(Module, Modulev2);
                Assert.IsTrue(gerenciador.ExistsModule(typeof(TesteInstanciaMultiplaModule)));
            }

        }
        [TestMethod()]
        [TestCategory("Existe")]
        public void ExisteModuleInstanciaUnicaPorId()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                IModule Module = gerenciador.CreateModule<TesteInstanciaUnicaModule>();
                Assert.IsNotNull(Module);
                Assert.IsTrue(gerenciador.ExistsModule(Module.Id));
            }

        }
        [TestMethod()]
        [TestCategory("Existe")]
        public void ExisteModuleInstanciaMultiplaPorId()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                IModule Module = gerenciador.CreateModule<TesteInstanciaMultiplaModule>();
                Assert.IsNotNull(Module);
                Assert.IsTrue(gerenciador.ExistsModule(Module.Id));
            }

        }
        [TestMethod()]
        [TestCategory("Existe")]
        public void ExisteModuleInstanciaMultipla_MultiplosModulesPorId()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                IModule Module = gerenciador.CreateModule<TesteInstanciaMultiplaModule>();
                Assert.IsNotNull(Module);
                IModule Modulev2 = gerenciador.CreateModule<TesteInstanciaMultiplaModule>();
                Assert.IsNotNull(Modulev2);
                Assert.AreNotEqual(Module, Modulev2);
                Assert.IsTrue(gerenciador.ExistsModule(Modulev2.Id));
            }

        }
        [TestMethod()]
        [TestCategory("Existe")]
        public void ExisteModuleInstanciaPorIdInexistente()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                Assert.IsFalse(gerenciador.ExistsModule(Guid.NewGuid().ToString()));
            }

        }

        //Reciclar
        [TestMethod()]
        [TestCategory("Reciclar")]
        public void ReiniciarModuleInstanciaUnicaPorInstancia()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                IModule Module = gerenciador.CreateModule<TesteInstanciaUnicaModule>();
                Assert.IsNotNull(Module);
                Assert.IsTrue(gerenciador.ExistsModule(Module));
                IModule Modulev2 = gerenciador.RecycleModule(Module);
                Assert.IsNotNull(Modulev2);
                Assert.IsTrue(gerenciador.ExistsModule(Modulev2));
                Assert.IsFalse(gerenciador.ExistsModule(Module));
            }

        }
        [TestMethod()]
        [TestCategory("Reciclar")]
        public void ReiniciarModuleInstanciaMultiplaPorInstancia()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                IModule Module = gerenciador.CreateModule<TesteInstanciaMultiplaModule>();
                Assert.IsNotNull(Module);
                Assert.IsTrue(gerenciador.ExistsModule(Module));
                IModule Modulev2 = gerenciador.RecycleModule(Module);
                Assert.IsNotNull(Modulev2);
                Assert.IsTrue(gerenciador.ExistsModule(Modulev2));
                Assert.IsFalse(gerenciador.ExistsModule(Module));
            }

        }
        [TestMethod()]
        [TestCategory("Reciclar")]
        public void ReiniciarModuleInstanciaUnicaPorId()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                IModule Module = gerenciador.CreateModule<TesteInstanciaUnicaModule>();
                Assert.IsNotNull(Module);
                Assert.IsTrue(gerenciador.ExistsModule(Module));
                IModule Modulev2 = gerenciador.RecycleModule(Module.Id);
                Assert.IsNotNull(Modulev2);
                Assert.IsTrue(gerenciador.ExistsModule(Modulev2));
                Assert.IsFalse(gerenciador.ExistsModule(Module));
            }

        }
        [TestMethod()]
        [TestCategory("Reciclar")]
        public void ReiniciarModuleInstanciaMultiplaPorId()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                IModule Module = gerenciador.CreateModule<TesteInstanciaMultiplaModule>();
                Assert.IsNotNull(Module);
                Assert.IsTrue(gerenciador.ExistsModule(Module));
                IModule Modulev2 = gerenciador.RecycleModule(Module.Id);
                Assert.IsNotNull(Modulev2);
                Assert.IsTrue(gerenciador.ExistsModule(Modulev2));
                Assert.IsFalse(gerenciador.ExistsModule(Module));
            }

        }
        [TestMethod()]
        [TestCategory("Reciclar")]
        public void ReiniciarModuleInstanciaPorIdInexistente_ArgumentException()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                _ = Assert.ThrowsException<ModuleNotFoundException>(() =>
{
    _ = gerenciador.RecycleModule(Guid.NewGuid().ToString());
});
            }

        }



        //Listar
        [TestMethod()]
        [TestCategory("Listar")]
        public void ListarModules_loop()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                IModule m = null;
                for (int i = 0; i < 100; i++)
                {
                    TesteInstanciaMultiplaModule auxm = gerenciador.CreateModule<TesteInstanciaMultiplaModule>();
                    Assert.AreNotEqual(m, auxm);
                    m = auxm;
                }

                Assert.AreEqual(101, gerenciador.InitializedModules);
                Assert.IsNotNull(gerenciador.ListAllModules());
            }
        }




        [TestMethod()]
        [TestCategory("Exceptions")]
        public void Teste_19()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                Assert.ThrowsException<ModuleSingleInstanceException>(() =>
                {
                    gerenciador.CreateModule(typeof(ModuleIntanciaUnica));
                    gerenciador.CreateModule(typeof(ModuleIntanciaUnica));
                });
            }
        }
        [TestMethod()]
        [TestCategory("Exceptions")]
        public void Teste_18()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                Assert.ThrowsException<ModuleBuilderAbsentException>(() =>
                {
                    gerenciador.CreateModule(typeof(ModuleSemConstrutor));
                });
            }
        }
        [TestMethod()]
        [TestCategory("Exceptions")]
        public void Teste_17()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                Assert.ThrowsException<ModuleTypeInvalidException.TypeModuleUnmarkedException>(() =>
                {
                    gerenciador.CreateModule(typeof(ModuleSemAtributo));
                });
            }
        }
        [TestMethod()]
        [TestCategory("Exceptions")]
        public void Teste_16()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                Assert.ThrowsException<ModuleTypeInvalidException.TypeModuleNotInheritedException>(() =>
                {
                    gerenciador.CreateModule(typeof(ModuleInvallido));
                });
            }
        }
        [TestMethod()]
        [TestCategory("Exceptions")]
        public void Teste_15()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                Assert.ThrowsException<ModuleTypeNotFoundException>(() =>
                {
                    gerenciador.CreateModule(typeof(IContratoModuleInvalido));
                });
            }
        }
        [TestMethod()]
        [TestCategory("Exceptions")]
        public void Teste_14()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                Assert.ThrowsException<ModuleTypeInvalidException>(() =>
                {
                    gerenciador.CreateModule(typeof(int));
                });
            }
        }
        [TestMethod()]
        [TestCategory("Exceptions")]
        public void Teste_13()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                Assert.ThrowsException<ModuleContractNotFoundException>(() =>
                {
                    gerenciador.CreateModule(typeof(InterfaceSemAtributo));
                });
            }
        }
        [TestMethod()]
        [TestCategory("Exceptions")]
        public void Teste_12()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                Assert.ThrowsException<ArgumentNullException>(() =>
                {
                    gerenciador.ExistsModule(default(Type));
                });
            }
        }
        [TestMethod()]
        [TestCategory("Exceptions")]
        public void Teste_11()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                Assert.ThrowsException<ArgumentNullException>(() =>
                {
                    gerenciador.CreateModule(default(Type));
                });
            }
        }
        [TestMethod()]
        [TestCategory("Exceptions")]
        public void Teste_10()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                Assert.ThrowsException<ModuleContractInvalidException>(() =>
                {
                    gerenciador.CreateModule(typeof(IContratoInvalidoTipo));
                });
            }
        }
        [TestMethod()]
        [TestCategory("Exceptions")]
        public void Teste_9()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                Assert.ThrowsException<ModuleContractInvalidException>(() =>
                {
                    gerenciador.CreateModule(typeof(IContratoInvalido));
                });
            }
        }
        [TestMethod()]
        [TestCategory("Exceptions")]
        public void Teste_8()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                Assert.ThrowsException<ArgumentException>(() =>
                {
                    gerenciador.GetModule(default(string));
                });
            }
        }

        [TestMethod()]
        [TestCategory("Exceptions")]
        public void Teste_7()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                Assert.ThrowsException<ModuleDisposedException>(() =>
                {
                    IModule m = gerenciador.CreateModule(typeof(ModuleDependenciaInterfaceInvalidaOpcional));
                    m.Dispose();
                    gerenciador.GetModule(m.Id);
                });
            }
        }
        [TestMethod()]
        [TestCategory("Criar")]
        public void Teste_6()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                Assert.IsNotNull(gerenciador.CreateModule(typeof(ModuleDependenciaInterfaceValida)).ToString());
            }
        }
        [TestMethod()]
        [TestCategory("Criar")]
        public void Teste_5()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                Assert.IsNotNull(gerenciador.CreateModule(typeof(ModuleDependenciaInterfaceValida)));
            }
        }
        [TestMethod()]
        [TestCategory("Criar")]
        public void Teste_4()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                Assert.IsNotNull(gerenciador.CreateModule(typeof(ModuleDependenciaValida)));
            }
        }
        [TestMethod()]
        [TestCategory("Criar")]
        public void Teste_3()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                Assert.IsNotNull(gerenciador.CreateModule(typeof(ModuleDependenciaInterfaceInvalidaOpcional)));
            }
        }
        [TestMethod()]
        [TestCategory("Criar")]
        public void Teste_2()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                Assert.IsNotNull(gerenciador.CreateModule(typeof(ModuleParametroInvalidoOpcional)));
            }
        }
        [TestMethod()]
        [TestCategory("Criar")]
        public void Teste_1()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                Assert.IsNotNull(gerenciador.CreateModule(typeof(IModuleInstanciaUnica)));
                Assert.IsTrue(gerenciador.ExistsModule(typeof(IModuleInstanciaUnica)));
            }

        }

        [TestMethod()]
        public void Testes_separar()
        {
            using (var gerenciador = ModuleManagerExtensions.CreateModuleManager())
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
}