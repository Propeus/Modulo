# Propeus.Modulo

[![.NET](https://github.com/Propeus/Modulo/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Propeus/Modulo/actions/workflows/dotnet.yml)

Este projeto tem como objetivo criar e gerenciar módulos .NET dinamicamente, permitindo que sejam modificados, melhorados ou removidos em tempo de execução, sem a necessidade de realizar um CI/CD ou mesmo paralisar um sistema para realizar o deploy. Em poucas palavras, este projeto tem como objetivo ser um "Docker" para .NET.

O projeto não requer instalação, pois é adicionado como componente e inicializado junto com a aplicação web, console ou worker service.

## Licença

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

### Porque alguns projetos possuem licença diferente?
A minha intenção com este projeto e permitir a livre concorrência, por este motivo eu desejo que qualquer um possa criar, distribuir e vender seus módulos, entretanto o núcleo que gerencia eles (Propeus.Modulo.Core e Propeus.Modulo.Dinamico) e suas modificações devem ser distribuídos de forma totalmente gratuita.

## Como Contribuir

Para contribuir com este projeto, siga as instruções abaixo:

1. Faça um fork deste repositório
2. Crie uma branch para suas alterações (`git checkout -b feature/nome-da-feature`)
3. Faça suas alterações e adicione seus commits (`git commit -am 'Adicionando uma nova feature'`)
4. Faça um push para a branch (`git push origin feature/nome-da-feature`)
5. Abra um pull request para a branch `master` deste repositório

## Estado do Projeto

Este projeto está em fase de MVP (Minimum Viable Product) e pode ser testado usando os projetos na pasta "examples". O projeto segue a seguinte estrutura de pastas:

 - src (Código-fonte) 
 - examples (Exemplos funcionais) 
 - test (Projetos de teste unitário) 
 - docs (Documentação do projeto)

Estamos trabalhando para desenvolver o projeto e testá-lo completamente antes de disponibilizá-lo para uso. Fique à vontade para contribuir e nos ajudar a torná-lo uma realidade!

## Exemplos
Os exemplos podem ser encontrados dentro da pasta 'example', nele você encontrara exemplos de uso em console e workservice

## Proximos passos
Estes são os próximos passos do projeto

 1. Adicionar integração com a Injeção de dependência do .NET
 2. Criar integracao com projetos ASP.NET
 3. Adicionar o projeto no nuget.org
 4. Padronizar linguagem do codigo para o ingles (classes, metodos, documentacoes etc)
 5. Criar novos testes para o projeto Propeus.Modulo.IL 
 6. Alterar forma como e gerado os módulos dinamicamente (de proxy para espelhamento da classe) 
