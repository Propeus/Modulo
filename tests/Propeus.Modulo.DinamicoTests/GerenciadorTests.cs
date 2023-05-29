using Microsoft.VisualStudio.TestTools.UnitTesting;

using Propeus.Modulo.Abstrato.Interfaces;

namespace Propeus.Modulo.DinamicoTests
{


    [TestClass()]
    public class GerenciadorTests
    {


        [TestMethod()]
        public void TodosOsTestes()
        {
            /**
             * Este gerenciador é um **modulo** porém se comporta como um gerenciador
             * 
             * Para inicializar ele é necessario passar um gerenciador "nativo"
             * **/
            using (IGerenciador gerenciador = Propeus.Modulo.Dinamico.Gerenciador.Atual(Propeus.Modulo.Core.Gerenciador.Atual))
            {
                //Note que o modulo ModuloDeExemploParaPropeusModuloDinamico **não** implementa a interface de contrato, normalmente haveria um erro de cast por parte do programa
                IInterfaceDeContratoDeExemploParaPropeusModuloDinamico modulo = gerenciador.Criar<IInterfaceDeContratoDeExemploParaPropeusModuloDinamico>();
                Assert.IsNotNull(modulo);
                Assert.IsInstanceOfType(modulo, typeof(IModulo));
                Assert.IsInstanceOfType(modulo, typeof(IInterfaceDeContratoDeExemploParaPropeusModuloDinamico));
                modulo.EscreverOlaMundo();
                //Veja em seu console o funcionamento do modulo
            }

            /**
             * Assim como o Propeus.Modulo.Core, este gerenciador não exige que tenha uma interface de contrato para que possa criar novos modulos.
             * Portanto que o modulo seja valido, ele pode ser criado diretamente no gerenciador
             * **/
            using (IGerenciador gerenciador = Propeus.Modulo.Dinamico.Gerenciador.Atual(Propeus.Modulo.Core.Gerenciador.Atual))
            {
                //Note que o modulo ModuloDeExemploParaPropeusModuloDinamico **não** implementa a interface de contrato, normalmente haveria um erro de cast por parte do programa
                ModuloDeExemploParaPropeusModuloDinamico modulo = gerenciador.Criar<ModuloDeExemploParaPropeusModuloDinamico>();
                Assert.IsNotNull(modulo);
                Assert.IsInstanceOfType(modulo, typeof(IModulo));
                Assert.IsInstanceOfType(modulo, typeof(ModuloDeExemploParaPropeusModuloDinamico));
                modulo.EscreverOlaMundo();
                //Veja em seu console o funcionamento do modulo
            }

            /**
             * Um modulo não necessariamente pode ter somente um unico contrato, cada contrato pode possuir os metodos e propriedades que serão necessarios para o seu uso
             * **/
            using (IGerenciador gerenciador = Propeus.Modulo.Dinamico.Gerenciador.Atual(Propeus.Modulo.Core.Gerenciador.Atual))
            {

                IInterfaceDeContratoDeExemploParaPropeusModuloDinamico modulo = gerenciador.Criar<IInterfaceDeContratoDeExemploParaPropeusModuloDinamico>();
                Assert.IsNotNull(modulo);
                Assert.IsInstanceOfType(modulo, typeof(IModulo));
                Assert.IsInstanceOfType(modulo, typeof(IInterfaceDeContratoDeExemploParaPropeusModuloDinamico));
                modulo.EscreverOlaMundo();

                IInterfaceDeContratoDeExemploParaPropeusModuloDinamicoComOutroMetodo modulo2 = gerenciador.Criar<IInterfaceDeContratoDeExemploParaPropeusModuloDinamicoComOutroMetodo>();
                Assert.IsNotNull(modulo2);
                Assert.IsInstanceOfType(modulo2, typeof(IModulo));
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

            /**
             * Existem cenarios onde não haverá o tipo do modulo para ser passado para o atributo ModuloContrato, neste caso é possivel utilizar
             * o campo 'nome' do atributo para obter o tipo dele.
             * */
            using (IGerenciador gerenciador = Propeus.Modulo.Dinamico.Gerenciador.Atual(Propeus.Modulo.Core.Gerenciador.Atual))
            {

                IInterfaceDeContratoDeExemploParaPropeusModuloDinamicoComOutroMetodoSemUsoDeTypeOf modulo = gerenciador.Criar<IInterfaceDeContratoDeExemploParaPropeusModuloDinamicoComOutroMetodoSemUsoDeTypeOf>();
                Assert.IsNotNull(modulo);
                Assert.IsInstanceOfType(modulo, typeof(IModulo));
                Assert.IsInstanceOfType(modulo, typeof(IInterfaceDeContratoDeExemploParaPropeusModuloDinamicoComOutroMetodoSemUsoDeTypeOf));
                modulo.EscreverOutraCoisaParaOutroContrato();
                //Veja em seu console o funcionamento do modulo
            }


            /**
             * Tanto no gerenciador do Propeus.Modulo.Core quanto no Propeus.Modulo.Dinamico existe a possibilidade de injetar dependencias durante a sua criação.
             * 
             * Existem duas formas de injeção nos gerenciadores, a obrigatoria, quando o modulo requer outro modulo e a opcional, quando o modulo pode aceitar 
             * um modulo quando estiver disponivel.
             * 
             * Para definir um modulo ocional, basta definir o parametro como opcional (tipo nome = null)
             * **/
            using (IGerenciador gerenciador = Propeus.Modulo.Dinamico.Gerenciador.Atual(Propeus.Modulo.Core.Gerenciador.Atual))
            {

                IInterfaceDeContratoDeExemploParaPropeusModuloDinamicoComDependenciaObrigatoria moduloComDependenciaObrigatoria = gerenciador.Criar<IInterfaceDeContratoDeExemploParaPropeusModuloDinamicoComDependenciaObrigatoria>();
                Assert.IsNotNull(moduloComDependenciaObrigatoria);
                Assert.IsInstanceOfType(moduloComDependenciaObrigatoria, typeof(IModulo));
                Assert.IsInstanceOfType(moduloComDependenciaObrigatoria, typeof(IInterfaceDeContratoDeExemploParaPropeusModuloDinamicoComDependenciaObrigatoria));
                moduloComDependenciaObrigatoria.EscreverOlaMundo();

                IInterfaceDeContratoDeExemploParaPropeusModuloDinamicoComDependenciaOpcional moduloComDependenciaOpcional = gerenciador.Criar<IInterfaceDeContratoDeExemploParaPropeusModuloDinamicoComDependenciaOpcional>();
                Assert.IsNotNull(moduloComDependenciaOpcional);
                Assert.IsInstanceOfType(moduloComDependenciaOpcional, typeof(IModulo));
                Assert.IsInstanceOfType(moduloComDependenciaOpcional, typeof(IInterfaceDeContratoDeExemploParaPropeusModuloDinamicoComDependenciaOpcional));
                moduloComDependenciaOpcional.EscreverOlaMundo();
                //Veja em seu console o funcionamento do modulo
            }

            /**
             * Existem casos aonde o usuario precisa criar um modulo com alguns valores já definidos.
             * 
             * Para resolver este problema o modulo pode ter opcionalmente o metodo 'void CriarInstancia(...)' onde a quantidade de parametros é definido pelo usuario.
             * 
             * Caso a quantidade e tipo de parametros forem diferentes do que foi definido no metodo, o gerenciador não irá chama-lo.
             * 
             * O usuario pode criar quantos 'CriarInstancia' quiser, portanto que nao tenha a mesma assinatura dos outros.
             * **/
            using (IGerenciadorArgumentos gerenciador = Propeus.Modulo.Dinamico.Gerenciador.Atual(Propeus.Modulo.Core.Gerenciador.Atual))
            {

                ModuloDeExemploParaPropeusModuloDinamicoComCriarInstancia modulo = gerenciador.Criar<ModuloDeExemploParaPropeusModuloDinamicoComCriarInstancia>(new object[] { "O resultado da soma é", 15, 20 });
                modulo.EscreverOlaMundo();

                ModuloDeExemploParaPropeusModuloDinamicoComCriarInstancia modulo2 = gerenciador.Criar<ModuloDeExemploParaPropeusModuloDinamicoComCriarInstancia>(new object[] { 15, 20 });
                modulo2.EscreverOlaMundo();
                //Veja em seu console o funcionamento do modulo
            }

            /**
             * Assim como o metodo 'CriarInstancia' existe o metodo 'CriarConfigurcao', este só é permitido um unico metodo, pois é considerado um metodo
             * para configurar algo que foi injetado no construtor e no metodo 'CriarInstancia'.
             * 
             * Não é obrigatorio o uso destes metodos, entretanto pode facilitar alguns fluxos de processos aonde se depende de outros modulos e/ou dados simultaneamente
             * */
            using (IGerenciadorArgumentos gerenciador = Propeus.Modulo.Dinamico.Gerenciador.Atual(Propeus.Modulo.Core.Gerenciador.Atual))
            {
                ModuloDeExemploParaPropeusModuloDinamicoComCriarInstanciaEConfiguracao modulo = gerenciador.Criar<ModuloDeExemploParaPropeusModuloDinamicoComCriarInstanciaEConfiguracao>(new object[] { 15, 20 });
                modulo.EscreverOlaMundo();
                //Veja em seu console o funcionamento do modulo
            }

            /**
             * Pode haver casos onde o nao é gerado uma interface de contrato e nem possui o tipo do modulo
             * 
             * Para este caso é prossivel criar um modulo informando somente o nome dele, entretanto este meto é mais propenso a erros humanos
             * **/
            using (IGerenciadorArgumentos gerenciador = Propeus.Modulo.Dinamico.Gerenciador.Atual(Propeus.Modulo.Core.Gerenciador.Atual))
            {
                //TODO: Consertar o provedor de tipo para nao carregar tipos ja compilados
                IModulo modulo = (ModuloDeExemploParaPropeusModuloDinamicoComCriarInstanciaEConfiguracao)gerenciador.Criar("ModuloDeExemploParaPropeusModuloDinamicoComCriarInstanciaEConfiguracao", new object[] { 15, 20 });
                Assert.IsNotNull(modulo);
                Assert.IsInstanceOfType(modulo, typeof(IModulo));
                Assert.IsInstanceOfType(modulo, typeof(ModuloDeExemploParaPropeusModuloDinamicoComCriarInstanciaEConfiguracao));

            }

            /**
             * Estes são alguns meios e funcionalidades do Propeus.Modulo.Dinamico, entretanto deve levar em consideração alguns pontos
             * 
             * 1 - Evite a criação e substituição de modulos a qualuqer momento, pois o custo de processamento é elevado por conta da construção do proxy e suas validações em tempo de execução
             * 2 - Assim como o Propeus.Modulo.Core, este gerenciador te permite uma infinidade de possibilidades com um modulo, pois este gerenciador é um modulo que gerencia outros modulos
             * aplicando a suas regras
             * 3 - Assim como o Propeus.Modulo.Core, o uso do using limita o seu tempo de vida à aquele escopo, uma vez que chega ao seu fim, o Propeus.Modulo.Core e Propeus.Modulo.Dinamico são eliminados
             * 
             * **/

            /**
             * Considerações
             * 
             * 1 - Todos os modulos utilziados aqui, estão dentro do proprio console para facilitar o entendimento do fluxo do gerenciador e permitir que debuge o que for necessario.
             * 2 - O tempo para inicializar um gerenciador vai depender da quantidade de DLLs que estiverem na pasta
             * **/
        }


    }
}