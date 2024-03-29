# IGerenciadorInformacao `interface`

## Description
Modelo base para obter informações dos modulos

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph Propeus.Modulo.Abstrato.Interfaces
  Propeus.Modulo.Abstrato.Interfaces.IGerenciadorInformacao[[IGerenciadorInformacao]]
  class Propeus.Modulo.Abstrato.Interfaces.IGerenciadorInformacao interfaceStyle;
  end
```

## Members
### Methods
#### Public  methods
| Returns | Name |
| --- | --- |
| [`IModuloTipo`](./propeusmoduloabstratointerfaces-IModuloTipo.md) | [`ObterInfo`](#obterinfo-13)(`...`) |

## Details
### Summary
Modelo base para obter informações dos modulos

### Methods
#### ObterInfo [1/3]
```csharp
public IModuloTipo ObterInfo<T>()
where T : IModulo
```

#### ObterInfo [2/3]
```csharp
public IModuloTipo ObterInfo(Type type)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `Type` | type | Qualquer tipo herdado de [IModulo](./propeusmoduloabstratointerfaces-IModulo.md) |

##### Summary
Obtem o [IModuloTipo](./propeusmoduloabstratointerfaces-IModuloTipo.md) de `type` caso exista 



##### Returns
[IModuloTipo](./propeusmoduloabstratointerfaces-IModuloTipo.md)

#### ObterInfo [3/3]
```csharp
public IModuloTipo ObterInfo(string id)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `string` | id | Identificação unica do modulo |

##### Summary
Obtem  o [IModuloTipo](./propeusmoduloabstratointerfaces-IModuloTipo.md) do modulo pelo id

##### Returns
[IModuloTipo](./propeusmoduloabstratointerfaces-IModuloTipo.md)

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
