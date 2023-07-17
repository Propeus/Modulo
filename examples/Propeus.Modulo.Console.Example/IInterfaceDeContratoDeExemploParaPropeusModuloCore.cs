using Propeus.Modulo.Abstrato.Attributes;
using Propeus.Modulo.Abstrato.Interfaces;

namespace Propeus.Modulo.Console.Example
{
    //Para criar um contrato, é necessario ter o atributo ModuloContrato e herdar de IModule
    //O atributo ModuloContrato aceita tanto tipo como o nome dele, más para o cao do Propeus.ModuleProxy.Core, recomendo utilizar o tipo
    [ModuleContract(typeof(ModuloDeExemploParaPropeusModuloCore))]
    internal interface IInterfaceDeContratoDeExemploParaPropeusModuloCore : IModule
    {
        //Crie os metodos e propriedades que o modulo deve possuir.
        //Não é necessario escrever todas as funcionalidades do modulo no contrato, somente aquilo que será utilizado.
        void EscreverOlaMundo();
    }
}
