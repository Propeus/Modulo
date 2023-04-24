using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Abstrato.Interfaces;

namespace Propeus.Modulo.DinamicoTests
{
    /**
      * Para criar um contrato, é necessario ter o atributo ModuloContrato e herdar de IModulo
      * O atributo ModuloContrato aceita tanto tipo como o nome dele, más para o cao do Propeus.Modulo.Core, recomendo utilizar o tipo
      * 
      * Observação, durante o uso do gerenciador dinamico, lembre-se sempre de deixar o contrato como **public** pois por ser dinamico haverá erro de assecibilidade 
      * durante a criação do modulo
      * **/
    [ModuloContrato(typeof(ModuloDeExemploParaPropeusModuloDinamico))]
    public interface IInterfaceDeContratoDeExemploParaPropeusModuloDinamico : IModulo
    {
        //Crie os metodos e propriedades que o modulo deve possuir.
        //Não é necessario escrever todas as funcionalidades do modulo no contrato, somente aquilo que será utilizado.
        void EscreverOlaMundo();
    }

    [ModuloContrato(typeof(ModuloDeExemploParaPropeusModuloDinamico))]
    public interface IInterfaceDeContratoDeExemploParaPropeusModuloDinamicoComOutroMetodo : IModulo
    {
        //Crie os metodos e propriedades que o modulo deve possuir.
        //Não é necessario escrever todas as funcionalidades do modulo no contrato, somente aquilo que será utilizado.
        void EscreverOutraCoisaParaOutroContrato();
    }

    /**
     * Para o caso de carregamento dinamico, é necessario o uso do nome do modulo no atributo ModuloContrato, já que se considera que 
     * o projeto atual nao possui qualquer referencia com o projeto que implementa o modulo
     * **/
    [ModuloContrato("ModuloDeExemploParaPropeusModuloDinamico")]
    public interface IInterfaceDeContratoDeExemploParaPropeusModuloDinamicoComOutroMetodoSemUsoDeTypeOf : IModulo
    {
        //Crie os metodos e propriedades que o modulo deve possuir.
        //Não é necessario escrever todas as funcionalidades do modulo no contrato, somente aquilo que será utilizado.
        void EscreverOutraCoisaParaOutroContrato();
    }

    /**
     * Esta interface é para um caso onde um modulo depende opcionalmente de outro, caso tente utilizar obrigatoriamente este modulo
     * o gerenciador irá lancar uma excecao de modulo nao encontrado
     * **/
    [ModuloContrato("ModuloQueNaoExiste")]
    public interface IInterfaceDeContratoDeExemploParaPropeusModuloDinamicoComModuloInexistente : IModulo
    {
        //Crie os metodos e propriedades que o modulo deve possuir.
        //Não é necessario escrever todas as funcionalidades do modulo no contrato, somente aquilo que será utilizado.
        void EscreverOlaMundo();
    }

    /**
     * Esta interface é para um caso onde um modulo depende obrigatoriamente de outro, caso o modulo nao exista o gerenciador irá lancar uma excecao de modulo nao encontrado
     * **/
    [ModuloContrato("ModuloDeExemploParaPropeusModuloDinamicoComDependenciaObrigatoria")]
    public interface IInterfaceDeContratoDeExemploParaPropeusModuloDinamicoComDependenciaObrigatoria : IModulo
    {
        //Crie os metodos e propriedades que o modulo deve possuir.
        //Não é necessario escrever todas as funcionalidades do modulo no contrato, somente aquilo que será utilizado.
        void EscreverOlaMundo();
    }

    /**
     * Esta interface é para um caso onde um modulo depende opcionalmente de outro.
     * **/
    [ModuloContrato("ModuloDeExemploParaPropeusModuloDinamicoComDependenciaOpcional")]
    public interface IInterfaceDeContratoDeExemploParaPropeusModuloDinamicoComDependenciaOpcional : IModulo
    {
        //Crie os metodos e propriedades que o modulo deve possuir.
        //Não é necessario escrever todas as funcionalidades do modulo no contrato, somente aquilo que será utilizado.
        void EscreverOlaMundo();
    }
}
