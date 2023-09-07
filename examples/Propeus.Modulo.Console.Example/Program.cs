using Propeus.Module.Abstract.Interfaces;
using Propeus.Module.Manager;

namespace Propeus.Modulo.Console.Example
{


    internal class Program
    {
        private static void Main()
        {

            ExemploPropeusModuloCore();
            System.Console.ResetColor();

            ExemploPropeusModuloDinamico();
            System.Console.ResetColor();
        }

        private static void ExemploPropeusModuloDinamico()
        {
            System.Console.ForegroundColor = System.ConsoleColor.Yellow;
            System.Console.WriteLine("Exemplo de uso do Propeus.ModuleProxy.Dinamico");

            /**
             * Este gerenciador é um **modulo** porém se comporta como um gerenciador
             * 
             * Para inicializar ele é necessário passar um gerenciador "nativo" ou de nível superior
             * **/
            using (IModuleManager gerenciador = Propeus.Module.Manager.Dinamic.ModuleManagerExtensions.CreateModuleManager(ModuleManagerExtensions.CreateModuleManager()))
            {
                //Note que o modulo ModuloDeExemploParaPropeusModuloDinamico **não** implementa a interface de contrato, normalmente haveria um erro de cast por parte do programa
                IInterfaceDeContratoDeExemploParaPropeusModuloDinamico modulo = gerenciador.CreateModule<IInterfaceDeContratoDeExemploParaPropeusModuloDinamico>();
                modulo.EscreverOlaMundo();
                //Veja em seu console o funcionamento do modulo
            }

            /**
             * Assim como o Propeus.Module.Manager, este gerenciador não exige que tenha uma interface de contrato para que possa criar novos modulos.
             * Portanto que o modulo seja valido, ele pode ser criado diretamente no gerenciador
             * **/
            using (IModuleManager gerenciador = Propeus.Module.Manager.Dinamic.ModuleManagerExtensions.CreateModuleManager(ModuleManagerExtensions.CreateModuleManager()))
            {
                //Note que o modulo ModuloDeExemploParaPropeusModuloDinamico **não** implementa a interface de contrato, normalmente haveria um erro de cast por parte do programa
                ModuloDeExemploParaPropeusModuloDinamico modulo = gerenciador.CreateModule<ModuloDeExemploParaPropeusModuloDinamico>();
                modulo.EscreverOlaMundo();
                //Veja em seu console o funcionamento do modulo
            }

            /**
             * Um modulo não necessariamente pode ter somente um unico contrato, cada contrato pode possuir os metodos e propriedades que serão necessarios para o seu uso
             * **/
            using (IModuleManager gerenciador = Propeus.Module.Manager.Dinamic.ModuleManagerExtensions.CreateModuleManager(ModuleManagerExtensions.CreateModuleManager()))
            {

                IInterfaceDeContratoDeExemploParaPropeusModuloDinamico modulo = gerenciador.CreateModule<IInterfaceDeContratoDeExemploParaPropeusModuloDinamico>();
                modulo.EscreverOlaMundo();

                IInterfaceDeContratoDeExemploParaPropeusModuloDinamicoComOutroMetodo modulo2 = gerenciador.CreateModule<IInterfaceDeContratoDeExemploParaPropeusModuloDinamicoComOutroMetodo>();
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
            using (IModuleManager gerenciador = Propeus.Module.Manager.Dinamic.ModuleManagerExtensions.CreateModuleManager(ModuleManagerExtensions.CreateModuleManager()))
            {

                IInterfaceDeContratoDeExemploParaPropeusModuloDinamicoComOutroMetodoSemUsoDeTypeOf modulo = gerenciador.CreateModule<IInterfaceDeContratoDeExemploParaPropeusModuloDinamicoComOutroMetodoSemUsoDeTypeOf>();
                modulo.EscreverOutraCoisaParaOutroContrato();
                //Veja em seu console o funcionamento do modulo
            }


            /**
             * Tanto no gerenciador do Propeus.Module.Manager quanto no Propeus.ModuleProxy.Dinamico existe a possibilidade de injetar dependencias durante a sua criação.
             * 
             * Existem duas formas de injeção nos gerenciadores, a obrigatoria, quando o modulo requer outro modulo e a opcional, quando o modulo pode aceitar 
             * um modulo quando estiver disponivel.
             * 
             * Para definir um modulo ocional, basta definir o parametro como opcional (tipo nome = null)
             * **/
            using (IModuleManager gerenciador = Propeus.Module.Manager.Dinamic.ModuleManagerExtensions.CreateModuleManager(ModuleManagerExtensions.CreateModuleManager()))
            {

                IInterfaceDeContratoDeExemploParaPropeusModuloDinamicoComDependenciaObrigatoria moduloComDependenciaObrigatoria = gerenciador.CreateModule<IInterfaceDeContratoDeExemploParaPropeusModuloDinamicoComDependenciaObrigatoria>();
                moduloComDependenciaObrigatoria.EscreverOlaMundo();

                IInterfaceDeContratoDeExemploParaPropeusModuloDinamicoComDependenciaOpcional moduloComDependenciaOpcional = gerenciador.CreateModule<IInterfaceDeContratoDeExemploParaPropeusModuloDinamicoComDependenciaOpcional>();
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
            using (IModuleManager gerenciador = Propeus.Module.Manager.Dinamic.ModuleManagerExtensions.CreateModuleManager(ModuleManagerExtensions.CreateModuleManager()))
            {

                ModuloDeExemploParaPropeusModuloDinamicoComCriarInstancia modulo = gerenciador.CreateModule<ModuloDeExemploParaPropeusModuloDinamicoComCriarInstancia>(new object[] { "O resultado da soma é", 15, 20 });
                modulo.EscreverOlaMundo();

                ModuloDeExemploParaPropeusModuloDinamicoComCriarInstancia modulo2 = gerenciador.CreateModule<ModuloDeExemploParaPropeusModuloDinamicoComCriarInstancia>(new object[] { 15, 20 });
                modulo2.EscreverOlaMundo();
                //Veja em seu console o funcionamento do modulo
            }

            /**
             * Assim como o metodo 'CriarInstancia' existe o metodo 'CriarConfigurcao', este só é permitido um unico metodo, pois é considerado um metodo
             * para configurar algo que foi injetado no construtor e no metodo 'CriarInstancia'.
             * 
             * Não é obrigatorio o uso destes metodos, entretanto pode facilitar alguns fluxos de processos aonde se depende de outros modulos e/ou dados simultaneamente
             * */
            using (IModuleManager gerenciador = Propeus.Module.Manager.Dinamic.ModuleManagerExtensions.CreateModuleManager(ModuleManagerExtensions.CreateModuleManager()))
            {
                ModuloDeExemploParaPropeusModuloDinamicoComCriarInstanciaEConfiguracaoExample modulo = gerenciador.CreateModule<ModuloDeExemploParaPropeusModuloDinamicoComCriarInstanciaEConfiguracaoExample>(new object[] { 15, 20 });
                modulo.EscreverOlaMundo();
                //Veja em seu console o funcionamento do modulo
            }

            /**
             * Estes são alguns meios e funcionalidades do Propeus.ModuleProxy.Dinamico, entretanto deve levar em consideração alguns pontos
             * 
             * 1 - Evite a criação e substituição de modulos a qualuqer momento, pois o custo de processamento é elevado por conta da construção do proxy e suas validações em tempo de execução
             * 2 - Assim como o Propeus.Module.Manager, este gerenciador te permite uma infinidade de possibilidades com um modulo, pois este gerenciador é um modulo que gerencia outros modulos
             * aplicando a suas regras
             * 3 - Assim como o Propeus.Module.Manager, o uso do using limita o seu tempo de vida à aquele escopo, uma vez que chega ao seu fim, o Propeus.Module.Manager e Propeus.ModuleProxy.Dinamico são eliminados
             * 
             * **/

            /**
             * Considerações
             * 
             * 1 - Todos os modulos utilziados aqui, estão dentro do proprio console para facilitar o entendimento do fluxo do gerenciador e permitir que debuge o que for necessario.
             * 2 - O tempo para inicializar um gerenciador vai depender da quantidade de DLLs que estiverem na pasta
             * **/

            System.Console.WriteLine("Fim do exemplo com Propeus.ModuleProxy.Dinamico");

        }

        private static void ExemploPropeusModuloCore()
        {
            System.Console.ForegroundColor = System.ConsoleColor.Green;
            System.Console.WriteLine("Exemplo de uso do Propeus.Module.Manager");

            /**
             * Este gerenciador basicamente é uma DI em formato de modulo
             * Uma vez que termina o escopo, todos os modulos criados dentro dele são eliminados assim como o gerenciador, 
             * más ao chamar a propriedade Propeus.Module.Manager.ModuleManagerExtensions.CreateModuleManager() Uma nova instancia será criada
             * **/
            using (IModuleManager gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                IInterfaceDeContratoDeExemploParaPropeusModuloCore modulo = gerenciador.CreateModule<IInterfaceDeContratoDeExemploParaPropeusModuloCore>();
                modulo.EscreverOlaMundo();
                //Veja em seu console o funcionamento do modulo
            }

            /**
             * O gerenciador não requer obrigatoriamente uma interface de contrato.
             * Pode ser criado o modulo diretamente, portanto que siga o modelo de um.
             * **/
            using (IModuleManager gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                ModuloDeExemploParaPropeusModuloCore modulo = gerenciador.CreateModule<ModuloDeExemploParaPropeusModuloCore>();
                modulo.EscreverOlaMundo();
                //Veja em seu console o funcionamento do modulo
            }

            /**
             * É permitido também que seja criado o modulo pelo tipo (typeof), entretanto nao é recomentavel, pois pode haver
             * uma redução no desempenho já que seá necessario realizar validações em tempo de execução.
             * 
             * Pode ser utilziado o typeof(DeUmContrato) como o typeof(DeUmModulo), portanto que sigam o modelo de interface de contrato ou de modulo.
             * 
             * Neste caso, o retorno do modulo sempre será um IModule ao inves de um object ou dynamic, pois entende-se que este modulo não terá interação com o restante do programa.
             * Sim, modulos podem ser utilizados como "Workers" que podem realizar um certo tipo de trabalho e depois são descartados
             * **/
            using (IModuleManager gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                IModule modulo = gerenciador.CreateModule(typeof(ModuloDeExemploParaPropeusModuloCore));
                (modulo as ModuloDeExemploParaPropeusModuloCore).EscreverOlaMundo();
                //Veja em seu console o funcionamento do modulo
            }
            using (IModuleManager gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                IModule modulo = gerenciador.CreateModule(typeof(IInterfaceDeContratoDeExemploParaPropeusModuloCore));
                (modulo as ModuloDeExemploParaPropeusModuloCore).EscreverOlaMundo();
                //Veja em seu console o funcionamento do modulo
            }

            /**
             * Por ultimo e menos recomendável de todos.
             * 
             * É possivel criar um modulo a partir de seu nome, por isso, não importem ou criem varios modulos de mesmo nome, pois é mais facil criar um modulo com nome simples do que o
             * nome completo dele
             * 
             * Este caso só é utilizado caso não possua o tipo do modulo em seu projeto. Utilizando esta função o gerenciador busca em **todos** os assemblies em execução a procura da 
             * de **qualquer** tipo que corresponda ao nome e que seja um modulo, lembrando que está função irá lancar uma exceção caso procure um contrato.
             * 
             * Assim como o exemplo anterior, esta função te retorna um IModule
             * **/
            using (IModuleManager gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                IModule modulo = gerenciador.CreateModule("ModuloDeExemploParaPropeusModuloCore");

                //Esta linha fio adicionada para provar que o tipo informado na função anterior é o mesmo utilizado a baixo
                (modulo as ModuloDeExemploParaPropeusModuloCore).EscreverOlaMundo();
                //Veja em seu console o funcionamento do modulo
            }


            /**
             * Consderações finais
             * 
             * Não é obrigatorio criar using quando for utilizar os modulos, pois assim estará limitando o seu uso somente aquele escopo
             * Os modulos podem ser criados de divesas formas, como workers, regra de negocio ou gerenciadores como é o caso do Propeus.ModuleProxy.Dinamico
             * **/

            System.Console.WriteLine("Fim do exemplo com Propeus.Module.Manager");

        }
    }
}