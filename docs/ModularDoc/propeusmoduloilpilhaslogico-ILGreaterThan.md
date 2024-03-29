# ILGreaterThan `class`

## Description
&gt; || OpCodes.Ble_Un_S || OpCodes.Cgt

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph Propeus.Modulo.IL.Pilhas.Logico
  Propeus.Modulo.IL.Pilhas.Logico.ILGreaterThan[[ILGreaterThan]]
  Propeus.Modulo.IL.Pilhas.Logico.ILLogico[[ILLogico]]
  end
Propeus.Modulo.IL.Pilhas.Logico.ILLogico --> Propeus.Modulo.IL.Pilhas.Logico.ILGreaterThan
```

## Details
### Summary
&gt; || OpCodes.Ble_Un_S || OpCodes.Cgt

### Inheritance
 - [
`ILLogico`
](./propeusmoduloilpilhaslogico-ILLogico.md)

### Constructors
#### ILGreaterThan [1/2]
```csharp
public ILGreaterThan(ILBuilderProxy iLBuilderProxy)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILBuilderProxy`](./propeusmoduloilproxy-ILBuilderProxy.md) | iLBuilderProxy |   |

#### ILGreaterThan [2/2]
```csharp
public ILGreaterThan(ILBuilderProxy iLBuilderProxy, ILLabel label)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILBuilderProxy`](./propeusmoduloilproxy-ILBuilderProxy.md) | iLBuilderProxy |   |
| [`ILLabel`](./propeusmoduloilpilhassaltos-ILLabel.md) | label |   |

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
