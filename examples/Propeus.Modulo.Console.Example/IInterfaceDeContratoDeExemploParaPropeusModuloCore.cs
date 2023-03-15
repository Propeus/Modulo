using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Abstrato.Interfaces;

namespace Propeus.Modulo.Console.Example
{
    //Para criar um contrato, é necessario ter o atributo ModuloContrato e herdar de IModulo
    //O atributo ModuloContrato aceita tanto tipo como o nome dele, más para o cao do Propeus.Modulo.Core, recomendo utilizar o tipo
    [ModuloContrato(typeof(ModuloDeExemploParaPropeusModuloCore))]
    internal interface IInterfaceDeContratoDeExemploParaPropeusModuloCore : IModulo
    {
        //Crie os metodos e propriedades que o modulo deve possuir.
        //Não é necessario escrever todas as funcionalidades do modulo no contrato, somente aquilo que será utilizado.
        void EscreverOlaMundo();
    }
}
