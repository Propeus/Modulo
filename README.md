# Propeus.Modulo

[![.NET](https://github.com/Propeus/Modulo/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Propeus/Modulo/actions/workflows/dotnet.yml)

Este projeto tem como objetivo criar e gerenciar m�dulos .NET dinamicamente, permitindo que sejam modificados, melhorados ou removidos em tempo de execu��o, sem a necessidade de realizar um CI/CD ou mesmo paralisar um sistema para realizar o deploy. Em poucas palavras, este projeto tem como objetivo ser um "Docker" para .NET.

O projeto n�o requer instala��o, pois � adicionado como componente e inicializado junto com a aplica��o web, console ou worker service.

## Licen�a

|            Projeto           | Licenca |
|:----------------------------:|:-------:|
| Propeus.Modulo.Util          |   MIT   |
| Propeus.Modulo.Abstrato      |   MIT   |
| Propeus.Modulo.Core          |  LSUNCA |
| Propeus.Modulo.Dinamico      |  LSUNCA |
| Propeus.Modulo.IL            |   MIT   |
| Propeus.Modulo.WorkerService |   MIT   |
| Propeus.Modulo.Console       |   MIT   |
| Propeus.Modulo.CLI           |   MIT   |

### Porque alguns projetos possuem licen�a diferente?
A minha inten��o com este projeto e permitir a livre concorr�ncia, por este motivo eu desejo que qualquer um possa criar, distribuir e vender seus m�dulos, entretanto o n�cleo que gerencia eles (Propeus.Modulo.Core e Propeus.Modulo.Dinamico) e suas modifica��es devem ser distribu�dos de forma totalmente gratuita.

## Como Contribuir

Para contribuir com este projeto, siga as instru��es abaixo:

1. Fa�a um fork deste reposit�rio
2. Crie uma branch para suas altera��es (`git checkout -b feature/nome-da-feature`)
3. Fa�a suas altera��es e adicione seus commits (`git commit -am 'Adicionando uma nova feature'`)
4. Fa�a um push para a branch (`git push origin feature/nome-da-feature`)
5. Abra um pull request para a branch `master` deste reposit�rio

## Estado do Projeto

Este projeto est� em fase de MVP (Minimum Viable Product) e pode ser testado usando os projetos na pasta "examples". O projeto segue a seguinte estrutura de pastas:

 - src (C�digo-fonte) 
 - examples (Exemplos funcionais) 
 - test (Projetos de teste unit�rio) 
 - docs (Documenta��o do projeto)

Estamos trabalhando para desenvolver o projeto e test�-lo completamente antes de disponibiliz�-lo para uso. Fique � vontade para contribuir e nos ajudar a torn�-lo uma realidade!

## Exemplos
Os exemplos podem ser encontrados dentro da pasta 'example', nele voc� encontrara exemplos de uso em console e workservice

## Proximos passos
Estes s�o os pr�ximos passos do projeto

 1. Adicionar integra��o com a Inje��o de depend�ncia do .NET
 2. Criar integracao com projetos ASP.NET
 3. Adicionar o projeto no nuget.org
 4. Padronizar linguagem do codigo para o ingles (classes, metodos, documentacoes etc)
 5. Criar novos testes para o projeto Propeus.Modulo.IL 
 6. Alterar forma como e gerado os m�dulos dinamicamente (de proxy para espelhamento da classe) 
