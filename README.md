# Propeus.Modulo

Este projeto tem como objetivo facilitar o gerenciamento de modulo .NET e aplicar o conceito de DI, permitindo a inje��o de dependencia
em tempo de execu��o.

## Atualiza��es
| Data | Observa��o |
|-|-|
|15/07/2023 | Atualiza��o de objetivos|

## Status CI


### Developer
| Build                                                                                                                                                                                                                     	|                                                                                                             Sonar                                                                                                             	|
|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------	|:-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------:	|
| [![Propeus.Modulo.Core](https://github.com/Propeus/Modulo/actions/workflows/propeus-modulo-core.yml/badge.svg?branch=develop)](https://github.com/Propeus/Modulo/actions/workflows/propeus-modulo-core.yml) 	|          [![Quality gate](https://sonarcloud.io/api/project_badges/quality_gate?project=propeus-modulo-core_propeus-modulo-core)](https://sonarcloud.io/summary/new_code?id=propeus-modulo-core_propeus-modulo-core)          	|
| [![Propeus.Modulo.Dinamico](https://github.com/Propeus/Modulo/actions/workflows/propeus-modulo-dinamico.yml/badge.svg?branch=develop)](https://github.com/Propeus/Modulo/actions/workflows/propeus-modulo-dinamico.yml)	|      [![Quality gate](https://sonarcloud.io/api/project_badges/quality_gate?project=propeus-modulo-core_propeus-modulo-dinamico)](https://sonarcloud.io/summary/new_code?id=propeus-modulo-core_propeus-modulo-dinamico)      	|
| [![Propeus.Modulo.IL](https://github.com/Propeus/Modulo/actions/workflows/propeus-modulo-il.yml/badge.svg?branch=develop)](https://github.com/Propeus/Modulo/actions/workflows/propeus-modulo-il.yml)      	|            [![Quality gate](https://sonarcloud.io/api/project_badges/quality_gate?project=propeus-modulo-core_propeus-modulo-il)](https://sonarcloud.io/summary/new_code?id=propeus-modulo-core_propeus-modulo-il)            	|
| [![Propeus.Modulo.WorkerService](https://github.com/Propeus/Modulo/actions/workflows/propeus-modulo-worker-service.yml/badge.svg?branch=develop)](https://github.com/Propeus/Modulo/actions/workflows/propeus-modulo-worker-service.yml)| [![Quality gate](https://sonarcloud.io/api/project_badges/quality_gate?project=propeus-modulo-core_propeus-modulo-workerservice)](https://sonarcloud.io/summary/new_code?id=propeus-modulo-core_propeus-modulo-workerservice) 	|

### Master
| Build                                                                                                                                                                                                                     	|                                                                                                             Sonar                                                                                                             	|
|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------	|:-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------:	|
|                [![Propeus.Modulo.Core](https://github.com/Propeus/Modulo/actions/workflows/propeus-modulo-core.yml/badge.svg)](https://github.com/Propeus/Modulo/actions/workflows/propeus-modulo-core.yml)               	|          [![Quality gate](https://sonarcloud.io/api/project_badges/quality_gate?project=propeus-modulo-core_propeus-modulo-core)](https://sonarcloud.io/summary/new_code?id=propeus-modulo-core_propeus-modulo-core)          	|
|          [![Propeus.Modulo.Dinamico](https://github.com/Propeus/Modulo/actions/workflows/propeus-modulo-dinamico.yml/badge.svg)](https://github.com/Propeus/Modulo/actions/workflows/propeus-modulo-dinamico.yml)         	|      [![Quality gate](https://sonarcloud.io/api/project_badges/quality_gate?project=propeus-modulo-core_propeus-modulo-dinamico)](https://sonarcloud.io/summary/new_code?id=propeus-modulo-core_propeus-modulo-dinamico)      	|
|                   [![Propeus.Modulo.IL](https://github.com/Propeus/Modulo/actions/workflows/propeus-modulo-il.yml/badge.svg)](https://github.com/Propeus/Modulo/actions/workflows/propeus-modulo-il.yml)                  	|            [![Quality gate](https://sonarcloud.io/api/project_badges/quality_gate?project=propeus-modulo-core_propeus-modulo-il)](https://sonarcloud.io/summary/new_code?id=propeus-modulo-core_propeus-modulo-il)            	|
| [![Propeus.Modulo.WorkerService](https://github.com/Propeus/Modulo/actions/workflows/propeus-modulo-worker-service.yml/badge.svg)](https://github.com/Propeus/Modulo/actions/workflows/propeus-modulo-worker-service.yml) 	| [![Quality gate](https://sonarcloud.io/api/project_badges/quality_gate?project=propeus-modulo-core_propeus-modulo-workerservice)](https://sonarcloud.io/summary/new_code?id=propeus-modulo-core_propeus-modulo-workerservice) 	|

	
## Licen�a

Os principais projetos s�o licenciados da seguinte maneira


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
| Propeus.Modulo.Hosting       |   MIT   |

Os projetos `Propeus.Modulo.Core` e `Propeus.Modulo.Dinamico` est�o sob a licen�a `LSUNCA` para que todos possam usar de forma
**gratuita** tanto o projeto quanto as suas modificacoes e derivados.

Sendo assim qualquer empresa pode comercializar seus modulos m�s os gerenciadores e seu codigo-fonte devem ser distribuidos de forma **gratuita**,

� permitido a comercializa��o de gerenciadores que s�o construidos sem qualquer uso dos projetos `Propeus.Modulo.Core`, `Propeus.Modulo.Dinamico`,
suas modifica�oes e derivados.

\* As licen�as podem ser alteradas conforme for necessario para manter a **gratuidade** e a liberdade de usar o codigo-fonte

## Estrutura do projeto

- archived (Projetos excluidos logicamente)
- docs (Documenta��es do projeto)
- examples (Exemplos funcionais, n�o s�o testes unitarios)
- src (Codigo-fonte do projeto)
	- addons (modulos fim, ou seja, s�o criados para fazer algo, m�s nao s�o fundamentais para o funcionamento)
	- core (modulos essenciais para o funcionamento do projeto)
	- modules (modulos meio, ou seja, s�o criados para que os addons possam funcionar dentro do programa)
- tests (Testes unitarios dos projetos)

## Planos

Os proximos passos deste projeto � otimizar o codigo-fonte para ficar mais funcional e entend�vel

Ap�s este passo, os seguintes passos est�o na lista para serem executados:

1. ~~Finalizar o projeto `Propeus.Modulo.Hosting`~~
1. **Otimizar o codigo-fonte**
1. Internacionalizar os nomes de classe, interfaces, propriedades, metodos, eventos para o ingles
1. Criar exemplos mais complexos e funcionais (Jogos de RPG em console, projeto web para testar modulos etc)
1. Documentar o restante do codigo-fonte
1. Tonar o projeto como um MVP (M�nimo Produto Viavel)
1. Criar documenta��o em ingles
1. Criar modulos para aumentar a capacidade de uso do projeto (Conexoes com cache, banco de dados, comunicacao entre programas, seguran�la etc)