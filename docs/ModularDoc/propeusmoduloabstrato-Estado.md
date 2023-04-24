# Estado `enum`

## Description
Informa o estado do modulo

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph Propeus.Modulo.Abstrato
  Propeus.Modulo.Abstrato.Estado[[Estado]]
  end
```

## Details
### Summary
Informa o estado do modulo

### Fields
#### Criado
##### Summary
Indica que o modulo foi instanciado

#### Pronto
##### Summary
Indica que o modulo foi instanciado e configurado, porem nao comecou a sua execucao

#### Inicializado
##### Summary
Define que o modulo foi inicializado com sucesso.

#### Desligado
##### Summary
Define que o modulo foi eliminado pelo gerenciador ou foi chamado o IDisposable externamente

#### Erro
##### Summary
Define que durante a execução do modulo acionado alguma Exception

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
