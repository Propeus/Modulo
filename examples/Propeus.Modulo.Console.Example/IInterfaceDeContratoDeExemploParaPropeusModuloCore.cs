using Propeus.Module.Abstract.Attributes;
using Propeus.Module.Abstract.Interfaces;

namespace Propeus.Modulo.Console.Example
{
    //Para criar um contrato, é necessário ter o atributo ModuloContrato e herdar de IModule
    //O atributo ModuloContrato aceita tanto tipo como o nome dele, más para o cao do Propeus.Module.Manager, recomendo utilizar o tipo
    [ModuleContract(typeof(ModuloDeExemploParaPropeusModuloCore))]
    internal interface IInterfaceDeContratoDeExemploParaPropeusModuloCore : IModule
    {
        //Crie os métodos e propriedades que o modulo deve possuir.
        //Não é necessário escrever todas as funcionalidades do modulo no contrato, somente aquilo que será utilizado.
        void EscreverOlaMundo();
    }
}
