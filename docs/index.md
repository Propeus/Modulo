# Propeus.Modulo

Este projeto tem como objetivo facilitar o gerenciamento de modulo .NET e aplicar o conceito de DI, permitindo a injeção de dependencia
em tempo de execuçao.

## Atualizações
| Data | Observação |
|-|-|
|15/07/2023 | Atualização de objetivos|
|19/07/2023 | Adiciona status do sonar|
|23/07/2023 | Adiciona documentação detalhada e github page|
## Status CI

### Sonar
|Projeto                |Qualidade                                                                                                                                                                                                                                |Segurança                                                                                                                                                                                                                               |Manutenção                                                                                                                                                                                                                                  |Confiabilidade                                                                                               																																   |
|-----------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
|Propeus.Modulo.Core    |[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=propeus-modulo-core_propeus-modulo-core&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=propeus-modulo-core_propeus-modulo-core)        |[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=propeus-modulo-core_propeus-modulo-core&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=propeus-modulo-core_propeus-modulo-core)        |[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=propeus-modulo-core_propeus-modulo-core&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=propeus-modulo-core_propeus-modulo-core)        |[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=propeus-modulo-core_propeus-modulo-core&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=propeus-modulo-core_propeus-modulo-core)        |
|Propeus.Modulo.Dinamico|[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=propeus-modulo-core_propeus-modulo-dinamico&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=propeus-modulo-core_propeus-modulo-dinamico)|[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=propeus-modulo-core_propeus-modulo-dinamico&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=propeus-modulo-core_propeus-modulo-dinamico)|[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=propeus-modulo-core_propeus-modulo-dinamico&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=propeus-modulo-core_propeus-modulo-dinamico)|[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=propeus-modulo-core_propeus-modulo-dinamico&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=propeus-modulo-core_propeus-modulo-dinamico)|
|Propeus.Modulo.IL      |[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=propeus-modulo-core_propeus-modulo-il&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=propeus-modulo-core_propeus-modulo-il)            |[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=propeus-modulo-core_propeus-modulo-il&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=propeus-modulo-core_propeus-modulo-il)            |[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=propeus-modulo-core_propeus-modulo-il&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=propeus-modulo-core_propeus-modulo-il)            |[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=propeus-modulo-core_propeus-modulo-il&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=propeus-modulo-core_propeus-modulo-il)            |

### Build      
|Developer																																																								  |Master																																																							 |
|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| [![Propeus.Modulo.Core](https://github.com/Propeus/Modulo/actions/workflows/propeus-modulo-core.yml/badge.svg?branch=develop)](https://github.com/Propeus/Modulo/actions/workflows/propeus-modulo-core.yml)                	          | [![Propeus.Modulo.Core](https://github.com/Propeus/Modulo/actions/workflows/propeus-modulo-core.yml/badge.svg)](https://github.com/Propeus/Modulo/actions/workflows/propeus-modulo-core.yml)               	                     |
| [![Propeus.Modulo.Dinamico](https://github.com/Propeus/Modulo/actions/workflows/propeus-modulo-dinamico.yml/badge.svg?branch=develop)](https://github.com/Propeus/Modulo/actions/workflows/propeus-modulo-dinamico.yml)		          | [![Propeus.Modulo.Dinamico](https://github.com/Propeus/Modulo/actions/workflows/propeus-modulo-dinamico.yml/badge.svg)](https://github.com/Propeus/Modulo/actions/workflows/propeus-modulo-dinamico.yml)         	             |
| [![Propeus.Modulo.IL](https://github.com/Propeus/Modulo/actions/workflows/propeus-modulo-il.yml/badge.svg?branch=develop)](https://github.com/Propeus/Modulo/actions/workflows/propeus-modulo-il.yml)                      	          | [![Propeus.Modulo.IL](https://github.com/Propeus/Modulo/actions/workflows/propeus-modulo-il.yml/badge.svg)](https://github.com/Propeus/Modulo/actions/workflows/propeus-modulo-il.yml)                  	                     |
| [![Propeus.Modulo.WorkerService](https://github.com/Propeus/Modulo/actions/workflows/propeus-modulo-worker-service.yml/badge.svg?branch=develop)](https://github.com/Propeus/Modulo/actions/workflows/propeus-modulo-worker-service.yml)| [![Propeus.Modulo.WorkerService](https://github.com/Propeus/Modulo/actions/workflows/propeus-modulo-worker-service.yml/badge.svg)](https://github.com/Propeus/Modulo/actions/workflows/propeus-modulo-worker-service.yml) 	     |
	
## Licença

Os principais projetos são licenciados da seguinte maneira


|            Projeto           | Licenca |
|:----------------------------:|:-------:|
| Propeus.Modulo.Util          |   MIT   |
| Propeus.Modulo.Abstrato      |   MIT   |
| Propeus.Modulo.Core          |   MIT   |
| Propeus.Modulo.Dinamico      |   MIT   |
| Propeus.Modulo.IL            |   MIT   |
| Propeus.Modulo.WorkerService |   MIT   |
| Propeus.Modulo.Console       |   MIT   |
| Propeus.Modulo.CLI           |   MIT   |
| Propeus.Modulo.Hosting       |   MIT   |

## Estrutura do projeto
```bash
.
├── archived 
├── docs /
│   ├── _site
│   ├── api
│   ├── CODE_OF_CONDUCT.md
│   └── README.md
├── examples 
├── src /
│   ├── addons
│   ├── core 
│   └── modules
└── tests 
```
## Planos

Os proximos passos deste projeto é **otimizar o codigo-fonte** para ficar mais funcional e entendivel

Após este passo, os seguintes passos estão na lista para serem executados:

1. ~~Finalizar o projeto `Propeus.Modulo.Hosting`~~
1. **Otimizar o codigo-fonte**
1. Internacionalizar os nomes de classe, interfaces, propriedades, metodos, eventos para o ingles
1. Criar exemplos mais complexos e funcionais (Jogos de RPG em console, projeto web para testar modulos etc)
1. Documentar o restante do codigo-fonte
1. Tonar o projeto como um MVP (Mínimo Produto Viavel)
1. Criar documentação em ingles
1. Criar modulos para aumentar a capacidade de uso do projeto (Conexoes com cache, banco de dados, comunicacao entre programas, segurança etc)

Caso queira saber mais detalhes acesse [o projeto](https://propeus.github.io/Modulo/).

