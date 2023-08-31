using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Attributes;

namespace Propeus.Modulo.Console.Example
{
    //Para criar um modulo, é necessário ter o atributo ModuleProxy e herdar de IModule, entretanto é recomendável usar a classe BaseModule, pois ja foi implementado os recursos necessários para o modulo funcionar
    //Deve ser implementado a interface de contrato para que o Propeus.Modulo.Core funcione corretamente
    [Module]
    internal class ModuloDeExemploParaPropeusModuloCore : BaseModule, IInterfaceDeContratoDeExemploParaPropeusModuloCore
    {
        //O parâmetro instanciaUnica indica se o gerenciador pode criar uma nova instancia sempre que for solicitado ou se deve ser utilizando uma unica instancia quando houver.
        //Por padrão o valor para instanciaUnica é false
        public ModuloDeExemploParaPropeusModuloCore() : base()
        {
        }

        //Implemente os métodos e propriedades que o modulo deve possuir.
        public void EscreverOlaMundo()
        {
            System.Console.WriteLine("Ola mundo!");
            System.Console.WriteLine("Este é um modulo em funcionamento!");
        }

        /**Obs.:
         * Qualquer modulo, possui a liberdade de manipular o gerenciador, sendo assim, podendo até mesmo remover outros módulos ou até este mesmo.
         * Este projeto da a liberdade de criar um novo modulo que nao precise de um IModuleManager como parâmetro, entretendo, será necessário criar um novo nível de gerenciador 
         * que consiga realizar esta operação
        **/
    }

}
