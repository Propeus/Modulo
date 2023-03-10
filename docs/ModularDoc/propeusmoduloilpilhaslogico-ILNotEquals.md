# ILNotEquals `class`

## Description
!= || OpCodes.Beq_S

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph Propeus.Modulo.IL.Pilhas.Logico
  Propeus.Modulo.IL.Pilhas.Logico.ILNotEquals[[ILNotEquals]]
  Propeus.Modulo.IL.Pilhas.Logico.ILLogico[[ILLogico]]
  end
Propeus.Modulo.IL.Pilhas.Logico.ILLogico --> Propeus.Modulo.IL.Pilhas.Logico.ILNotEquals
```

## Details
### Summary
!= || OpCodes.Beq_S

### Inheritance
 - [
`ILLogico`
](./propeusmoduloilpilhaslogico-ILLogico.md)

### Constructors
#### ILNotEquals [1/2]
```csharp
public ILNotEquals(ILBuilderProxy iLBuilderProxy)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILBuilderProxy`](./propeusmoduloilproxy-ILBuilderProxy.md) | iLBuilderProxy |   |

#### ILNotEquals [2/2]
```csharp
public ILNotEquals(ILBuilderProxy iLBuilderProxy, ILLabel label)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILBuilderProxy`](./propeusmoduloilproxy-ILBuilderProxy.md) | iLBuilderProxy |   |
| [`ILLabel`](./propeusmoduloilpilhassaltos-ILLabel.md) | label |   |

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
