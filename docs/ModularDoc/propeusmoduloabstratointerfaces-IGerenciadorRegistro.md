# IGerenciadorRegistro `interface`

## Description
Modelo base para registrar os modulos no gerenciador

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph Propeus.Modulo.Abstrato.Interfaces
  Propeus.Modulo.Abstrato.Interfaces.IGerenciadorRegistro[[IGerenciadorRegistro]]
  class Propeus.Modulo.Abstrato.Interfaces.IGerenciadorRegistro interfaceStyle;
  end
```

## Members
### Methods
#### Public  methods
| Returns | Name |
| --- | --- |
| `void` | [`Registrar`](#registrar)([`IModulo`](./propeusmoduloabstratointerfaces-IModulo.md) modulo)<br>Registra o modulo no gerenciador |

## Details
### Summary
Modelo base para registrar os modulos no gerenciador

### Methods
#### Registrar
```csharp
public void Registrar(IModulo modulo)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`IModulo`](./propeusmoduloabstratointerfaces-IModulo.md) | modulo | Instancia do modulo |

##### Summary
Registra o modulo no gerenciador

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
