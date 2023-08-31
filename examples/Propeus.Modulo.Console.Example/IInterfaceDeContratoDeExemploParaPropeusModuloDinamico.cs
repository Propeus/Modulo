﻿using Propeus.Module.Abstract.Attributes;
using Propeus.Module.Abstract.Interfaces;

namespace Propeus.Modulo.Console.Example
{
    /**
     * Para criar um contrato, é necessário ter o atributo ModuloContrato e herdar de IModule
     * O atributo ModuloContrato aceita tanto tipo como o nome dele, más para o cao do Propeus.Module.Manager, recomendo utilizar o tipo
     * 
     * Observação, durante o uso do gerenciador dinâmico, lembre-se sempre de deixar o contrato como **public** pois por ser dinâmico haverá erro de acessibilidade 
     * durante a criação do modulo
     * **/
    [ModuleContract(typeof(ModuloDeExemploParaPropeusModuloDinamico))]
    public interface IInterfaceDeContratoDeExemploParaPropeusModuloDinamico : IModule
    {
        //Crie os métodos e propriedades que o modulo deve possuir.
        //Não é necessário escrever todas as funcionalidades do modulo no contrato, somente aquilo que será utilizado.
        void EscreverOlaMundo();
    }

    [ModuleContract(typeof(ModuloDeExemploParaPropeusModuloDinamico))]
    public interface IInterfaceDeContratoDeExemploParaPropeusModuloDinamicoComOutroMetodo : IModule
    {
        //Crie os métodos e propriedades que o modulo deve possuir.
        //Não é necessário escrever todas as funcionalidades do modulo no contrato, somente aquilo que será utilizado.
        void EscreverOutraCoisaParaOutroContrato();
    }

    /**
     * Para o caso de carregamento dinâmico, é necessário o uso do nome do modulo no atributo ModuloContrato, já que se considera que 
     * o projeto atual nao possui qualquer referencia com o projeto que implementa o modulo
     * **/
    [ModuleContract("ModuloDeExemploParaPropeusModuloDinamico")]
    public interface IInterfaceDeContratoDeExemploParaPropeusModuloDinamicoComOutroMetodoSemUsoDeTypeOf : IModule
    {
        //Crie os métodos e propriedades que o modulo deve possuir.
        //Não é necessário escrever todas as funcionalidades do modulo no contrato, somente aquilo que será utilizado.
        void EscreverOutraCoisaParaOutroContrato();
    }

    /**
     * Esta interface é para um caso onde um modulo depende opcionalmente de outro, caso tente utilizar obrigatoriamente este modulo
     * o gerenciador irá lancar uma excecao de modulo nao encontrado
     * **/
    [ModuleContract("ModuloQueNaoExiste")]
    public interface IInterfaceDeContratoDeExemploParaPropeusModuloDinamicoComModuloInexistente : IModule
    {
        //Crie os metodos e propriedades que o modulo deve possuir.
        //Não é necessario escrever todas as funcionalidades do modulo no contrato, somente aquilo que será utilizado.
        void EscreverOlaMundo();
    }

    /**
     * Esta interface é para um caso onde um modulo depende obrigatoriamente de outro, caso o modulo nao exista o gerenciador irá lancar uma excecao de modulo nao encontrado
     * **/
    [ModuleContract("ModuloDeExemploParaPropeusModuloDinamicoComDependenciaObrigatoria")]
    public interface IInterfaceDeContratoDeExemploParaPropeusModuloDinamicoComDependenciaObrigatoria : IModule
    {
        //Crie os metodos e propriedades que o modulo deve possuir.
        //Não é necessario escrever todas as funcionalidades do modulo no contrato, somente aquilo que será utilizado.
        void EscreverOlaMundo();
    }

    /**
     * Esta interface é para um caso onde um modulo depende opcionalmente de outro.
     * **/
    [ModuleContract("ModuloDeExemploParaPropeusModuloDinamicoComDependenciaOpcional")]
    public interface IInterfaceDeContratoDeExemploParaPropeusModuloDinamicoComDependenciaOpcional : IModule
    {
        //Crie os metodos e propriedades que o modulo deve possuir.
        //Não é necessario escrever todas as funcionalidades do modulo no contrato, somente aquilo que será utilizado.
        void EscreverOlaMundo();
    }
}
