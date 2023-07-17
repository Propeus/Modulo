using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Attributes;

namespace Propeus.Modulo.Console.Example
{
    //Para criar um modulo, é necessario ter o atributo ModuleProxy e herdar de IModule, entretanto é recomentavel usar a classe BaseModule, pois ja foi implementado os recusos necessarios para o modulo funcionar
    //Deve ser implementado a interface de contrato para que o Propeus.ModuleProxy.Core funcione corretamente
    [Module]
    internal class ModuloDeExemploParaPropeusModuloCore : BaseModule, IInterfaceDeContratoDeExemploParaPropeusModuloCore
    {
        //O parametro instanciaUnica indica se o gerenciador pode criar uma nova instancia sempre que for solicitado ou se deve ser utilziando uma unica instancia quando houver.
        //Por padrao o valor para instanciaUnica é false
        public ModuloDeExemploParaPropeusModuloCore() : base(false)
        {
        }

        //Implemente os metodos e propriedades que o modulo deve possuir.
        public void EscreverOlaMundo()
        {
            System.Console.WriteLine("Ola mundo!");
            System.Console.WriteLine("Este é um modulo em funcionamento!");
        }

        /**Obs.:
         * Qualquer modulo, possui a liberdade de manipuar o gerenciador, sendo assim, podendo até mesmo remover outros modulos ou até este mesmo.
         * Este projeto da a liberdade de criar um novo modulo que nao precise de um IModuleManager como parametro, entretando, será necessario criar um novo nivel de gerenciador 
         * que conssiga realizar esta operação
        **/
    }

}
