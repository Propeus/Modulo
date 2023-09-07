using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Propeus.Module.Abstract.Interfaces;
using Propeus.Module.Manager.Dinamic.Modules;

namespace Propeus.Modulo.DinamicoTests
{


    [TestClass()]
    public class GerenciadorTests
    {

        public IModuleManager GetModuleManager()
        {
            return Module.Manager.Dinamic.ModuleManagerExtensions.CreateModuleManager(Module.Manager.ModuleManagerExtensions.CreateModuleManager());

        }

        /**
        * Estes são alguns meios e funcionalidades do Propeus.Module.Manager.Dinamic, entretanto deve levar em consideração alguns pontos
        * 
        * 1 - Evite a criação e substituição de modulos a qualuqer momento, pois o custo de processamento é elevado por conta da construção do proxy e suas validações em tempo de execução
        * 2 - Assim como o Propeus.Module.Manager, este gerenciador te permite uma infinidade de possibilidades com um modulo, pois este gerenciador é um modulo que gerencia outros modulos
        * aplicando a suas regras
        * 3 - Assim como o Propeus.Module.Manager, o uso do using limita o seu tempo de vida à aquele escopo, uma vez que chega ao seu fim, o Propeus.Module.Manager e Propeus.Module.Manager.Dinamic são eliminados
        * 
        * **/

        /**
         * Considerações
         * 
         * 1 - Todos os modulos utilziados aqui, estão dentro do proprio console para facilitar o entendimento do fluxo do gerenciador e permitir que debuge o que for necessario.
         * 2 - O tempo para inicializar um gerenciador vai depender da quantidade de DLLs que estiverem na pasta
         * **/


        [TestMethod()]
        public void Teste_11()
        {
            using (IModuleManager gerenciador = GetModuleManager())
            {
                var module = gerenciador.CreateModule<ModuleDeExemploParaParametrosCustomizados>(new object[] { 10, 12 });
                Assert.IsNotNull(module);
            }


        }

        [TestMethod()]
        public void Teste_10()
        {
            using (IModuleManager gerenciador = GetModuleManager())
            {
                ListenerModule listerner = gerenciador.CreateModule<ListenerModule>();
                Assert.IsNotNull(listerner);
                listerner.SetOnLoadModule((obj) => { });
                listerner.SetOnRebuildModule((obj) => { });
                listerner.SetOnUnloadModule((obj) => { });
                var module = gerenciador.CreateModule<IInterfaceDeContratoDeExemploParaPropeusModuloDinamicoComOutroMetodoSemUsoDeTypeOf>();
                gerenciador.KeepAliveModule(module);
                Assert.IsNotNull(listerner.GetAllModules().ToList());

            }


        }

        [TestMethod()]
        public void Teste_9()
        {
            using (IModuleManager gerenciador = GetModuleManager())
            {
                Assert.ThrowsException<ArgumentNullException>(() => { gerenciador.CreateModule(default(Type)); });

            }

            using (IModuleManager gerenciador = GetModuleManager())
            {
                Assert.ThrowsException<ArgumentNullException>(() => { gerenciador.CreateModule(default(Type), Array.Empty<object>()); });
                Assert.IsNotNull(gerenciador.CreateModule(typeof(ModuloDeExemploParaPropeusModuloDinamicoComCriarInstanciaEConfiguracao), Array.Empty<object>()));
                var module = gerenciador.CreateModule(typeof(ModuloDeExemploParaPropeusModuloDinamicoComCriarInstanciaEConfiguracao), Array.Empty<object>());
                gerenciador.RemoveModule(module);
                module = gerenciador.CreateModule(typeof(ModuloDeExemploParaPropeusModuloDinamicoComCriarInstanciaEConfiguracao), Array.Empty<object>());
                gerenciador.RemoveModule(module.Id);
                Assert.IsNotNull(gerenciador.ListAllModules());
            }
        }

        [TestMethod()]
        public void Teste_8()
        {
            /**
                         * Pode haver casos onde o nao é gerado uma interface de contrato e nem possui o tipo do modulo
                         * 
                         * Para este caso é prossivel criar um modulo informando somente o nome dele, entretanto este meto é mais propenso a erros humanos
                         * **/
            using (IModuleManager gerenciador = GetModuleManager())
            {
                IModule modulo = (ModuloDeExemploParaPropeusModuloDinamicoComCriarInstanciaEConfiguracao)gerenciador.CreateModule("ModuloDeExemploParaPropeusModuloDinamicoComCriarInstanciaEConfiguracao", new object[] { 15, 20 });
                Assert.IsNotNull(modulo);
                Assert.IsInstanceOfType(modulo, typeof(IModule));
                Assert.IsInstanceOfType(modulo, typeof(ModuloDeExemploParaPropeusModuloDinamicoComCriarInstanciaEConfiguracao));

            }
        }
        [TestMethod()]
        public void Teste_7()
        {
            /**
             * Assim como o metodo 'CriarInstancia' existe o metodo 'CriarConfigurcao', este só é permitido um unico metodo, pois é considerado um metodo
             * para configurar algo que foi injetado no construtor e no metodo 'CriarInstancia'.
             * 
             * Não é obrigatorio o uso destes metodos, entretanto pode facilitar alguns fluxos de processos aonde se depende de outros modulos e/ou dados simultaneamente
             * */
            using (IModuleManager gerenciador = GetModuleManager())
            {
                ModuloDeExemploParaPropeusModuloDinamicoComCriarInstanciaEConfiguracao modulo = gerenciador.CreateModule<ModuloDeExemploParaPropeusModuloDinamicoComCriarInstanciaEConfiguracao>(new object[] { 15, 20 });
                modulo.EscreverOlaMundo();
                //Veja em seu console o funcionamento do modulo
            }
        }
        [TestMethod()]
        public void Teste_6()
        {
            /**
             * Existem casos aonde o usuario precisa criar um modulo com alguns valores já definidos.
             * 
             * Para resolver este problema o modulo pode ter opcionalmente o metodo 'void CriarInstancia(...)' onde a quantidade de parametros é definido pelo usuario.
             * 
             * Caso a quantidade e tipo de parametros forem diferentes do que foi definido no metodo, o gerenciador não irá chama-lo.
             * 
             * O usuario pode criar quantos 'CriarInstancia' quiser, portanto que nao tenha a mesma assinatura dos outros.
             * **/
            using (IModuleManager gerenciador = GetModuleManager())
            {

                ModuloDeExemploParaPropeusModuloDinamicoComCriarInstancia modulo = gerenciador.CreateModule<ModuloDeExemploParaPropeusModuloDinamicoComCriarInstancia>(new object[] { "O resultado da soma é", 15, 20 });
                modulo.EscreverOlaMundo();

                ModuloDeExemploParaPropeusModuloDinamicoComCriarInstancia modulo2 = gerenciador.CreateModule<ModuloDeExemploParaPropeusModuloDinamicoComCriarInstancia>(new object[] { 15, 20 });
                modulo2.EscreverOlaMundo();
                //Veja em seu console o funcionamento do modulo
            }
        }
        [TestMethod()]
        public void Teste_5()
        {
            /**
             * Tanto no gerenciador do Propeus.Module.Manager quanto no Propeus.Module.Manager.Dinamic existe a possibilidade de injetar dependencias durante a sua criação.
             * 
             * Existem duas formas de injeção nos gerenciadores, a obrigatoria, quando o modulo requer outro modulo e a opcional, quando o modulo pode aceitar 
             * um modulo quando estiver disponivel.
             * 
             * Para definir um modulo ocional, basta definir o parametro como opcional (tipo nome = null)
             * **/
            using (IModuleManager gerenciador = GetModuleManager())
            {

                IInterfaceDeContratoDeExemploParaPropeusModuloDinamicoComDependenciaObrigatoria moduloComDependenciaObrigatoria = gerenciador.CreateModule<IInterfaceDeContratoDeExemploParaPropeusModuloDinamicoComDependenciaObrigatoria>();
                Assert.IsNotNull(moduloComDependenciaObrigatoria);
                Assert.IsInstanceOfType(moduloComDependenciaObrigatoria, typeof(IModule));
                Assert.IsInstanceOfType(moduloComDependenciaObrigatoria, typeof(IInterfaceDeContratoDeExemploParaPropeusModuloDinamicoComDependenciaObrigatoria));
                moduloComDependenciaObrigatoria.EscreverOlaMundo();

                IInterfaceDeContratoDeExemploParaPropeusModuloDinamicoComDependenciaOpcional moduloComDependenciaOpcional = gerenciador.CreateModule<IInterfaceDeContratoDeExemploParaPropeusModuloDinamicoComDependenciaOpcional>();
                Assert.IsNotNull(moduloComDependenciaOpcional);
                Assert.IsInstanceOfType(moduloComDependenciaOpcional, typeof(IModule));
                Assert.IsInstanceOfType(moduloComDependenciaOpcional, typeof(IInterfaceDeContratoDeExemploParaPropeusModuloDinamicoComDependenciaOpcional));
                moduloComDependenciaOpcional.EscreverOlaMundo();
                //Veja em seu console o funcionamento do modulo
            }
        }
        [TestMethod()]
        public void Teste_4()
        {
            /**
             * Existem cenarios onde não haverá o tipo do modulo para ser passado para o atributo ModuloContrato, neste caso é possivel utilizar
             * o campo 'nome' do atributo para obter o tipo dele.
             * */
            using (IModuleManager gerenciador = GetModuleManager())
            {

                IInterfaceDeContratoDeExemploParaPropeusModuloDinamicoComOutroMetodoSemUsoDeTypeOf modulo = gerenciador.CreateModule<IInterfaceDeContratoDeExemploParaPropeusModuloDinamicoComOutroMetodoSemUsoDeTypeOf>();
                Assert.IsNotNull(modulo);
                Assert.IsInstanceOfType(modulo, typeof(IModule));
                Assert.IsInstanceOfType(modulo, typeof(IInterfaceDeContratoDeExemploParaPropeusModuloDinamicoComOutroMetodoSemUsoDeTypeOf));
                modulo.EscreverOutraCoisaParaOutroContrato();
                //Veja em seu console o funcionamento do modulo
            }
        }
        [TestMethod()]
        public void Teste_3()
        {
            /**
                         * Um modulo não necessariamente pode ter somente um unico contrato, cada contrato pode possuir os metodos e propriedades que serão necessarios para o seu uso
                         * **/
            using (IModuleManager gerenciador = GetModuleManager())
            {

                IInterfaceDeContratoDeExemploParaPropeusModuloDinamico modulo = gerenciador.CreateModule<IInterfaceDeContratoDeExemploParaPropeusModuloDinamico>();
                Assert.IsNotNull(modulo);
                Assert.IsInstanceOfType(modulo, typeof(IModule));
                Assert.IsInstanceOfType(modulo, typeof(IInterfaceDeContratoDeExemploParaPropeusModuloDinamico));
                modulo.EscreverOlaMundo();

                IInterfaceDeContratoDeExemploParaPropeusModuloDinamicoComOutroMetodo modulo2 = gerenciador.CreateModule<IInterfaceDeContratoDeExemploParaPropeusModuloDinamicoComOutroMetodo>();
                Assert.IsNotNull(modulo2);
                Assert.IsInstanceOfType(modulo2, typeof(IModule));
                Assert.IsInstanceOfType(modulo2, typeof(IInterfaceDeContratoDeExemploParaPropeusModuloDinamicoComOutroMetodo));
                Assert.IsInstanceOfType(modulo2, typeof(IInterfaceDeContratoDeExemploParaPropeusModuloDinamico));
                modulo2.EscreverOutraCoisaParaOutroContrato();
                //Veja em seu console o funcionamento do modulo

                /** 
                 * Apesar de implementarem o mesmo tipo, a variavel 'modulo' não pode ser do tipo IInterfaceDeContratoDeExemploParaPropeusModuloDinamicoComOutroMetodo **porém** o 
                 * contrario pode ser feito, pois a cada novo contrato, um novo tipo é gerado implementado os contratos anteriores.
                 * 
                 * Também não é possivel converter as classes de contrato para o tipo do modulo, pois neste contexto, é criado uma classe de proxy com o tipo do modulo
                 * **/
            }
        }
        [TestMethod()]
        public void Teste_2()
        {
            /**
             * Assim como o Propeus.Module.Manager, este gerenciador não exige que tenha uma interface de contrato para que possa criar novos modulos.
             * Portanto que o modulo seja valido, ele pode ser criado diretamente no gerenciador
             * **/
            using (IModuleManager gerenciador = GetModuleManager())
            {
                //Note que o modulo ModuloDeExemploParaPropeusModuloDinamico **não** implementa a interface de contrato, normalmente haveria um erro de cast por parte do programa
                ModuloDeExemploParaPropeusModuloDinamico modulo = gerenciador.CreateModule<ModuloDeExemploParaPropeusModuloDinamico>();
                Assert.IsNotNull(modulo);
                Assert.IsInstanceOfType(modulo, typeof(IModule));
                Assert.IsInstanceOfType(modulo, typeof(ModuloDeExemploParaPropeusModuloDinamico));
                modulo.EscreverOlaMundo();
                //Veja em seu console o funcionamento do modulo
            }
        }
        [TestMethod()]
        public void Teste_1()
        {
            /**
             * Este gerenciador é um **modulo** porém se comporta como um gerenciador
             * 
             * Para inicializar ele é necessario passar um gerenciador "nativo"
             * **/
            using (IModuleManager gerenciador = GetModuleManager())
            {
                //Note que o modulo ModuloDeExemploParaPropeusModuloDinamico **não** implementa a interface de contrato, normalmente haveria um erro de cast por parte do programa
                IInterfaceDeContratoDeExemploParaPropeusModuloDinamico modulo = gerenciador.CreateModule<IInterfaceDeContratoDeExemploParaPropeusModuloDinamico>();
                Assert.IsNotNull(modulo);
                Assert.IsInstanceOfType(modulo, typeof(IModule));
                Assert.IsInstanceOfType(modulo, typeof(IInterfaceDeContratoDeExemploParaPropeusModuloDinamico));
                modulo.EscreverOlaMundo();
                //Veja em seu console o funcionamento do modulo
            }
        }
    }
}