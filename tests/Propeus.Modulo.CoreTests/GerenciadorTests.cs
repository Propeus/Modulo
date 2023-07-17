using System;
using System.Diagnostics;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Attributes;
using Propeus.Modulo.Abstrato.Exceptions;
using Propeus.Modulo.Abstrato.Interfaces;

namespace Propeus.Modulo.CoreTests
{
    [Module]
    public class TesteInstanciaUnicaModule : BaseModule
    {
        public TesteInstanciaUnicaModule() : base(true)
        {
        }

        public override string ToString()
        {
            return Id;
        }
    }

    [Module]
    public class TesteInstanciaMultiplaModule : BaseModule
    {
        public TesteInstanciaMultiplaModule() : base(false)
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
        private IModuleManager gerenciador;

        [TestInitialize]
        public void Begin()
        {
            //EventoProvider.RegistrarOuvinteInformacao(TesteLog);
            //EventoProvider.RegistrarOuvinteErro(TesteLog);
            //EventoProvider.RegistrarOuvinteAviso(TesteLogAviso);
            gerenciador = Core.ModuleManagerCoreExtensions.CreateModuleManagerDefault();
        }

        private void TesteLogAviso(Type fonte, string mensagem, Exception exception)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[{DateTime.Now}] {fonte.Name}: {mensagem}");
            Console.ResetColor();
        }

        private void TesteLog(Type fonte, string mensagem, Exception exception)
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

            IModule Module = gerenciador.CreateModule<TesteInstanciaUnicaModule>();
            Assert.IsNotNull(Module);

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

            Assert.AreEqual(1, gerenciador.InitializedModules);
            _ = Assert.ThrowsException<ModuleNotFoundException>(() =>
            {
                gerenciador.RemoveModule(Module.Id);
            });


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
            for (int i = 0; i < 99; i++)
            {
                var auxm = gerenciador.CreateModule<TesteInstanciaMultiplaModule>();
                Assert.AreNotEqual(m, auxm);
                m = auxm;
            }

            Assert.AreEqual(100, gerenciador.InitializedModules);

        }


        #region Organizar depois

        public interface InterfaceSemAtributo
        {

        }

        [ModuleContract("xpto")]
        public interface InterfaceModuleInvalido
        {

        }
        [ModuleContract(typeof(OutroModuleDependenciaInterfaceValida))]
        public interface IModuleValido : IModule
        {

        }

        [ModuleContract(typeof(OutroModuleDependenciaInterfaceValida))]
        public interface IModuleInstanciaUnica : IModule
        {

        }

        [Module]
        public class ModuleInstanciaUnica : BaseModule, IModuleInstanciaUnica
        {
            public ModuleInstanciaUnica() : base(true)
            {
            }
        }

        public class ModuleInvallido
        {

        }

        public class ModuleSemAtributo : IModule
        {
            public bool IsSingleInstance { get; }
            /// <summary>
            /// Version do modelo
            /// </summary>
            public string Version { get; }
            /// <summary>
            /// Representa o estado do objeto.
            /// </summary>
            public State State { get; }
            /// <summary>
            /// Representação amigavel do ojeto. 
            /// <para>
            /// Caso seja nulo o nome da classe herdado será informado na propriedade.
            /// </para>
            /// </summary>
            public string Name { get; }
            /// <summary>
            /// Representação alfanumerica e unica do objeto.
            /// </summary>
            public string Id { get; }
            /// <summary>
            /// <see cref="Guid"/> do <see cref="System.Reflection.Assembly" /> atual
            /// </summary>
            public string ManifestId { get; }


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

            public string IdManifesto { get; }
        }


        [Module]
        public class ModuleSemConstrutor : IModule
        {
            private ModuleSemConstrutor()
            {

            }

            public bool IsSingleInstance { get; }
            public string Version { get; }
            public State State { get; }
            public string Name { get; }
            public string Id { get; }
            public string ManifestId { get; }

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

            public string IdManifesto { get; }
        }

        [Module]
        public class ModuleIntanciaUnica : BaseModule
        {
            public ModuleIntanciaUnica() : base(true)
            {
            }
        }



        [Module]
        public class ModuleDependenciaInvalida : BaseModule
        {
            public ModuleDependenciaInvalida(ModuleInvallido ModuleInvallido) : base(true)
            {
            }
        }

        [Module]
        public class ModuleDependenciaInterfaceInvalidaOpcional : BaseModule
        {
            public ModuleDependenciaInterfaceInvalidaOpcional(InterfaceModuleInvalido interfaceModuleInvalido = null) : base(false)
            {
            }
        }

        [Module]
        public class ModuleDependenciaValida : BaseModule
        {
            public ModuleDependenciaValida(ModuleDependenciaInterfaceInvalidaOpcional Module) : base(true)
            {
            }
        }

        [Module]
        public class OutroModuleDependenciaInterfaceValida : BaseModule, IModuleValido
        {
            public OutroModuleDependenciaInterfaceValida() : base(false)
            {
            }
        }

        [Module]
        public class ModuleDependenciaInterfaceValida : BaseModule
        {
            public ModuleDependenciaInterfaceValida(IModuleValido iModule) : base(false)
            {
            }
        }

        [Module]
        public class ModuleParametroInvalido : BaseModule, IModuleValido
        {
            public ModuleParametroInvalido(int a) : base(false)
            {
            }
        }

        [Module]
        public class ModuleParametroInvalidoOpcional : BaseModule, IModuleValido
        {
            public ModuleParametroInvalidoOpcional(bool instanciaUnica = false) : base(instanciaUnica)
            {
            }
        }

        [ModuleContract(default(string))]
        public interface IContratoInvalido : IModule
        {

        }

        [ModuleContract(default(Type))]
        public interface IContratoInvalidoTipo : IModule
        {

        }

        [TestMethod()]
        [TestCategory("Todos")]
        public void TodosOstestes()
        {

            Assert.IsNotNull(gerenciador.CreateModule(typeof(IModuleInstanciaUnica)));
            Assert.IsTrue(gerenciador.ExistsModule(typeof(IModuleInstanciaUnica)));

            Assert.IsNotNull(gerenciador.CreateModule(typeof(ModuleParametroInvalidoOpcional)));
            Assert.IsNotNull(gerenciador.CreateModule(typeof(ModuleDependenciaInterfaceInvalidaOpcional)));
            Assert.IsNotNull(gerenciador.CreateModule(typeof(ModuleDependenciaValida)));
            Assert.IsNotNull(gerenciador.CreateModule(typeof(ModuleDependenciaInterfaceValida)));
            Assert.IsNotNull(gerenciador.CreateModule(typeof(ModuleDependenciaInterfaceValida)).ToString());



            Assert.ThrowsException<ModuleNotFoundException>(() =>
            {
                var m = gerenciador.CreateModule(typeof(ModuleDependenciaInterfaceInvalidaOpcional));
                m.Dispose();
                gerenciador.GetModule(m.Id);
            });

            Assert.ThrowsException<ArgumentException>(() =>
            {
                gerenciador.GetModule(default(string));
            });

            Assert.ThrowsException<ModuleContractInvalidException>(() =>
            {
                gerenciador.CreateModule(typeof(IContratoInvalido));
            });

            Assert.ThrowsException<ModuleContractInvalidException>(() =>
            {
                gerenciador.CreateModule(typeof(IContratoInvalidoTipo));
            });

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                gerenciador.CreateModule(default(Type));
            });

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                gerenciador.ExistsModule(default(Type));
            });

            Assert.ThrowsException<ModuleContractNotFoundException>(() =>
            {
                gerenciador.CreateModule(typeof(InterfaceSemAtributo));
            });

            Assert.ThrowsException<ModuleTypeInvalidException>(() =>
            {
                gerenciador.CreateModule(typeof(int));
            });

            Assert.ThrowsException<ModuleTypeNotFoundException>(() =>
            {
                gerenciador.CreateModule(typeof(InterfaceModuleInvalido));
            });

            Assert.ThrowsException<ModuleTypeInvalidException>(() =>
            {
                gerenciador.CreateModule(typeof(ModuleInvallido));
            });

            Assert.ThrowsException<ModuleTypeInvalidException>(() =>
            {
                gerenciador.CreateModule(typeof(ModuleSemAtributo));
            });

            Assert.ThrowsException<ModuleBuilderAbsentException>(() =>
            {
                gerenciador.CreateModule(typeof(ModuleSemConstrutor));
            });

            Assert.ThrowsException<ModuleSingleInstanceException>(() =>
            {
                gerenciador.CreateModule(typeof(ModuleIntanciaUnica));
                gerenciador.CreateModule(typeof(ModuleIntanciaUnica));
            });

        }
        #endregion

    }
}